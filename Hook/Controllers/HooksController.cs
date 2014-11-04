using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hook.Controllers
{
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net;
    using System.Net.Http;

    using Hook.Models;
    public class HooksController : Controller
    {
        private static ConcurrentDictionary<string, List<RequestHook>> memorydb;

        private IHookRepo repo = new MongoHookRepo();

            // GET: Hooks
        [Route("{id?}", Name = "detail")]
        public ActionResult Index(string id, string inspect)
        {
            if(String.IsNullOrEmpty(id)) return View();

            //todo: make sure the id exists
            var hook = repo.Get(id);
            if(hook == null) return new HttpNotFoundResult();

            if (this.InInspectMode()) {return this.InspectView(hook);}

            return this.CaptureRequest(hook);
        }

        private bool InInspectMode()
        {
            if (Request.QueryString.Count == 0) return false;
            var qs = Request.QueryString[0];
            if (qs == null) return false;
            return (qs.Equals("inspect", StringComparison.InvariantCultureIgnoreCase));
        }

        private ViewResult InspectView(Hook hook)
        {
            ViewBag.Id = hook.Id;
            ViewBag.Url = Url.RouteUrl("detail", new { hook.Id }, "http");
            return this.View("Inspect", hook.Requests); //todo: interface for retrieving hook by id
        }

        private ActionResult CaptureRequest(Hook hook)
        {
            var rh = new RequestHook
                         {
                             CreatedUtc = DateTime.UtcNow,
                             Headers = this.Request.Headers.ToKeyValuePairs(),
                             FormValues = this.Request.Form.ToKeyValuePairs(),
                             QueryStringValues = this.Request.QueryString.ToKeyValuePairs(),
                             Method = this.Request.HttpMethod,
                             IpAddress =
                                 this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                                 ?? this.Request.UserHostAddress,
                             Url = this.Request.Url,
                             Bytes = this.Request.TotalBytes
                         };

            if (Request.RequestType == "POST")
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    rh.RawBody = reader.ReadToEnd();
                }
            }

            //todo: make sure we only store up-to 20 requests per hook
            hook.Requests.Add(rh);
            repo.Save(hook);

            ViewData.Model = hook.Id;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("new"), Route("hooks/new")]
        public ActionResult New()
        {
            var id = new HookId().ToString();
            repo.Create(id);
            return this.Redirect(string.Format("/{0}?inspect", id));
        }
    }
}