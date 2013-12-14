namespace Javascript.ViewEngines
{
    using System.IO;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Microsoft.ClearScript.V8;

    public class JavascriptView : IView, IViewDataContainer
    {
        private readonly string _viewPath;
        private readonly JsEngine _engine;
        private readonly V8ScriptEngine _context;

        public JavascriptView(string viewPath, JsEngine engine, V8ScriptEngine context)
        {
            _viewPath = viewPath;
            _engine = engine;
            _context = context;
        }

        #region IView Members

        internal Stream LoadStream()
        {
            return VirtualPathProvider.OpenFile(_viewPath);
        }
        #endregion

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            var sb = new StringBuilder();

            string template;
            dynamic model = null;

            using (var context = new V8ScriptEngine())
            {
                using (var reader = new StreamReader(LoadStream()))
                {
                    template = reader.ReadToEnd();
                }

                if (viewContext != null && viewContext.ViewData.Model != null)
                {
                    model = viewContext.ViewData.Model;
                }

                writer.Write(this._engine.Template(template, model, viewContext.RequestContext, viewContext.RequestContext.HttpContext.User, viewContext.Controller.ControllerContext, viewContext.ViewData));
            }
        }

        public ViewDataDictionary ViewData { get; set; }
    }

    public class ConsoleLogger
    {
        public void log(dynamic what)
        {
            
        }
    }
}
