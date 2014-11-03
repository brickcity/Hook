using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hook.Controllers
{
    using Hook.Models;
    public class HooksController : Controller
    {
        // GET: Hooks
        [Route("{id?}", Name = "detail")]
        public ActionResult Index(string id, string inspect)
        {
            if(String.IsNullOrEmpty(id)) return View();

            //todo: make sure the id exists
            if (this.InInspectMode()) return this.InspectView(id);

            return this.Detail(id);
        }

        private bool InInspectMode()
        {
            if (Request.QueryString.Count == 0) return false;
            var qs = Request.QueryString[0];
            if (qs == null) return false;
            return (qs.Equals("inspect", StringComparison.InvariantCultureIgnoreCase));
        }

        private ViewResult InspectView(string id)
        {
            ViewBag.Id = id;
            ViewBag.Url = Url.RouteUrl("detail", new { id }, "http");
            return this.View("Inspect");
        }

        private ViewResult Detail(string id)
        {
            
            ViewData.Model = id;
            return this.View("Detail");
        }

        [Route("new"), Route("hooks/new")]
        public ActionResult New()
        {
            var id = new HookId().ToString();
            return this.Redirect(string.Format("/{0}?inspect", id));
        }
    }
}