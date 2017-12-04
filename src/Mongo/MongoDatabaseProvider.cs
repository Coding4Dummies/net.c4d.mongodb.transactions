using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TenLi.Api.DataAccess.Mongo
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase Create();
    }

    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly IConfigurationRoot _configuration;

        public MongoDatabaseProvider(IConfigurationRoot configuration)
        {
            _configuration = configuration;

            // Set up MongoDB conventions
            var pack = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }

        public IMongoDatabase Create()
        {
            var client = new MongoClient(Connection.ToMongoUrl());
            string databaseName = Connection.DatabaseName;
            return client.GetDatabase(databaseName);
        }

        public MongoUrlBuilder Connection
        {
            get { return new MongoUrlBuilder(_configuration["MongoDB:ConnectionString"]); }
        }
    }
}
