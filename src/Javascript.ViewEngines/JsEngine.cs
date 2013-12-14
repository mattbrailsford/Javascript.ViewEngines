namespace Javascript.ViewEngines
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.ClearScript.V8;

    public class JsEngine
    {
        private readonly List<string> _extensions;
        private readonly List<string> _requires;
        public string Name { get; private set; }
        public string Source { get; private set; }
        public IEnumerable<string> Extensions { get { return _extensions; } }
        public IEnumerable<string> Requires { get { return _requires; } }
        public TemplateDelegate Template { get; private set; }

        public delegate string TemplateDelegate(string template, object model, RequestContext requestContext, IPrincipal user, ControllerContext controllerContext, ViewDataDictionary viewData);

        public JsEngine(string name, List<string> extensions, List<string> requires, TemplateDelegate template, string source)
        {
            this.Name = name;
            this._extensions = extensions;
            this._requires = requires;
            this.Source = source;
            this.Template = template;
        }
    }
}
