namespace Javascript.ViewEngines
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Microsoft.ClearScript.V8;

    public class Host
    {
        private readonly V8ScriptEngine _context;
        private readonly string _scriptPath;
        public List<JsEngine> Engines { get; private set; }

        public Host(V8ScriptEngine context, string scriptPath)
        {
            _context = context;
            _scriptPath = scriptPath;
            this.Engines = new List<JsEngine>();
        }

        public void add(dynamic engine)
        {
            string name = engine.name;

            var extensions = new List<string>();
            for (var i = 0; i < engine.extensions.length; i++)
            {
                extensions.Add((string)engine.extensions[i]);
            }

            var requires = new List<string>();
            for (var i = 0; i < engine.requires.length; i++)
            {
                requires.Add((string)engine.requires[i]);
            }

            //var template = (JsEngine.TemplateDelegate)this._context.Evaluate(engine.template);
            var template = engine.template;

            var source = "var window = this;\r\n" + string.Join(";", requires.Select(r => File.ReadAllText(HostingEnvironment.MapPath(Path.Combine(this._scriptPath, r)))));

            this.Engines.Add(new JsEngine(name, extensions, requires, template, source));
        }
    }
}