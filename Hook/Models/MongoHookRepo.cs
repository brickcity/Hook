using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class MongoHookRepo : IHookRepo
    {
        private const string DatabaseName = "HookDb";
        private const string CollectionName = "Hooks";
        private const string ConnectionString = "mongodb://localhost:27017";

        private MongoDatabase database;

        private MongoCollection<Hook> hooksCollection;
        public MongoHookRepo()
        {
            var client = new MongoClient(ConnectionString);
            var server = client.GetServer();

            database = server.GetDatabase(DatabaseName);

            if (!database.CollectionExists(CollectionName))
            {
                database.CreateCollection(CollectionName);
            }
            hooksCollection = database.GetCollection<Hook>(CollectionName);
        }

        public void Create(string id)
        {
            hooksCollection.Save(new Hook() { Id = id });
        }

        public Hook Get(string id)
        {
            return hooksCollection.AsQueryable().SingleOrDefault(h => h.Id == id);
        }

        public void Save(Hook hook)
        {
            var existing = this.Get(hook.Id);
            if (existing != null)
            {
                existing.Requests = hook.Requests;
                hooksCollection.Save(existing);
                return;
            }

            hooksCollection.Save(hook);
        }
    }
}