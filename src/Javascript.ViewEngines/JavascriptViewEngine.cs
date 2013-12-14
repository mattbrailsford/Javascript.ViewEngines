namespace Javascript.ViewEngines
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Microsoft.ClearScript;
    using Microsoft.ClearScript.V8;

    public class JavascriptViewEngine : IViewEngine
    {
        private string[] _searchLocations;
        private readonly V8ScriptEngine _context;
        private readonly IDictionary<string, JsEngine> _extensions;
        private string _enginePath;
        private string _scriptsPath;

        #region IViewEngine Members

        public JavascriptViewEngine(string enginePath = "~/scripts/engines.js", params string[] paths)
        {
            this._context = new V8ScriptEngine();
            this._extensions = new Dictionary<string, JsEngine>();

            //ensure engine path is not null
            this._enginePath = enginePath;
            this._scriptsPath = enginePath.Substring(0, enginePath.LastIndexOf("/"));
            //find engines

            var host = new Host(this._context, this._scriptsPath);
            this._context.AllowReflection = true;
            this._context.AddHostObject("engines", HostItemFlags.GlobalMembers, host);
            this._context.AddHostType("Template", typeof(JsEngine.TemplateDelegate));

            //should i set up a file watcher?
            //should i look in other paths for an engine.js?
            //how should this be configurable?
            using (var stream = new StreamReader(HostingEnvironment.MapPath(enginePath)))
            {
                this._context.Execute(stream.ReadToEnd());
            }

            host.Engines.ForEach(e => this._context.Execute(e.Source));

            BuildSearchLocations(host.Engines, paths);
        }

        private void BuildSearchLocations(IEnumerable<JsEngine> engines, params string[] paths)
        {
            var searchLocations = new List<string>();

            foreach (var engine in engines)
            {
                foreach (var extension in engine.Extensions)
                {
                    searchLocations.Add("~/Views/{1}/{0}." + extension);
                    searchLocations.Add("~/Views/Shared/{0}." + extension);
                    if (paths != null)
                    {
                        foreach (string path in paths)
                        {
                            searchLocations.Add(path + "." + extension);
                        }
                    }
                    this._extensions.Add(extension.ToUpperInvariant(), engine);
                }
            }

            _searchLocations = searchLocations.ToArray();
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            //grab the current controller from the route data
            string controllerName = null;
            if (controllerContext != null)
            {
                controllerName = controllerContext.RouteData.GetRequiredString("controller");
            }

            //for proper error handling we need to return a list of locations we attempted to search for the view
            string[] searchedLocations;

            var result = FindView(partialViewName, controllerName, out searchedLocations);
            if (result != null)
            {
                return result;
            }

            //we couldn't find the view
            return new ViewEngineResult(searchedLocations);
        }

        private ViewEngineResult FindView(string viewName, string controllerName, out string[] searchedLocations)
        {
            ViewEngineResult viewEngineResult = null;

            //get the actual path of the view - returns null if none is found
            string viewPath = FindPath(viewName, controllerName, out searchedLocations);

            if (viewPath != null)
            {
                var view = GetView(viewPath);

                {
                    viewEngineResult = new ViewEngineResult(view, this);
                }
            }

            return viewEngineResult;
        }

        private JavascriptView GetView(string viewPath)
        {
            var engine = GetViewEngineFromExtension(viewPath);

            return new JavascriptView(viewPath, engine, this._context);
        }

        private JsEngine GetViewEngineFromExtension(string viewPath)
        {
            string extension = viewPath.Substring(viewPath.LastIndexOf(".") + 1).ToUpperInvariant();
            if (this._extensions.ContainsKey(extension))
            {
                return this._extensions[extension];
            }

            throw new Exception("engine not found");
        }

        private string FindPath(string viewName, string controllerName, out string[] searchedLocations)
        {
            searchedLocations = new string[_searchLocations.Length];

            for (int i = 0; i < _searchLocations.Length; i++)
            {
                string virtualPath = string.Format(_searchLocations[i], viewName, controllerName);

                searchedLocations[i] = virtualPath;

                //check the active VirtualPathProvider if the file exists
                if (HostingEnvironment.VirtualPathProvider.FileExists(virtualPath))
                {
                    //add it to cache - not currently implemented
                    return HostingEnvironment.VirtualPathProvider.GetFile(virtualPath).VirtualPath;
                }
            }

            return null;
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return FindPartialView(controllerContext, viewName, useCache);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view) { }

        #endregion
    }
}
