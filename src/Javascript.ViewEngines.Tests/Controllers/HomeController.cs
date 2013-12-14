namespace Javascript.ViewEngines.Tests.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                string[] engines = { "haml", "handlebars", "jade", "mustache", "resig", "underscore", "vash" };
                return View("Home", engines);
            }

            return View(id, new
                {
                    title = "A new view engine.",
                    message = string.Format("Welcome to {0}{1}, rendered on the server!", id.Substring(0, 1).ToUpper(), id.Substring(1))
                });
        }
    }
}
