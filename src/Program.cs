using System;
using Microsoft.Extensions.Configuration;
using Net.C4D.MongodbProvider;

namespace Net.C4D.Mongodb.Transactions {
    class Program {
        static void Main (string[] args) {
            IConfiguration config = new ConfigurationBuilder ()
                .AddJsonFile ("appsettings.json", true, true)
                .Build ();

            var mongodbProvider = new MongoDatabaseProvider (config["ConnectionStrings:DefaultConnetcion"]);

        }
    }
}