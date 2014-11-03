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

            // GET: Hooks
        [Route("{id?}", Name = "detail")]
        public ActionResult Index(string id, string inspect)
        {
            if(String.IsNullOrEmpty(id)) return View();

            //todo: make sure the id exists
            if (this.InInspectMode()) {return this.InspectView(id);}

            return this.CaptureRequest(id);
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
            return this.View("Inspect", Db[id]);
        }

        private ActionResult CaptureRequest(string id)
        {
            var rh = new RequestHook
                         {
                             CreatedUtc = DateTime.UtcNow,
                             Headers = this.Request.Headers,
                             FormValues = this.Request.Form,
                             QueryStringValues = this.Request.QueryString,
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

            Db.AddOrUpdate(id, s => new List<RequestHook>(),
                (s, list) =>
                    {
                        list.Add(rh);
                        return list;
                    });

            ViewData.Model = id;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("new"), Route("hooks/new")]
        public ActionResult New()
        {
            var id = new HookId().ToString();
            Db[id] = new List<RequestHook>();
            return this.Redirect(string.Format("/{0}?inspect", id));
        }

        private ConcurrentDictionary<string, List<RequestHook>> Db
        {
            get
            {
                return memorydb ?? (memorydb = new ConcurrentDictionary<string, List<RequestHook>>());
            }
        }
    }
}