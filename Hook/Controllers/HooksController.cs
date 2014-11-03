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
        [Route("{id?}")]
        public ActionResult Index(string id)
        {
            if(String.IsNullOrEmpty(id)) return View();

            return this.Detail(id);
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
            return RedirectToAction("Index", new { Id = id });
        }
    }
}