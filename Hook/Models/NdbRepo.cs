using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    using NDatabase;
    using NDatabase.Api;

    public class NdbRepo : IHookRepo
    {
        private IOdb Database()
        {
            var dataDir = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var dbFile = System.IO.Path.Combine(dataDir, "hooks.db");
            return OdbFactory.Open(dbFile);
        }

        public void Create(string id)
        {
            this.Save(new Hook() { Id = id });
        }

        public Hook Get(string id)
        {
            using (var db = this.Database())
            {
                return db.AsQueryable<Hook>().SingleOrDefault(t => t.Id == id);
            }
        }

        public void Save(Hook hook)
        {
            using (var db = this.Database())
            {
                var existingObj = db.AsQueryable<Hook>().SingleOrDefault(t => t.Id == hook.Id);
                if (existingObj != null) db.Delete(existingObj);

                db.Store(hook);
            }
        }
    }
}