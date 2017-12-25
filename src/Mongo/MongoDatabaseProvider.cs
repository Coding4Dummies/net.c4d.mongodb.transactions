using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Net.C4D.Mongodb.Transactions.Mongo
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase Create();
    }

    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly string _connectionString;
        public MongoDatabaseProvider(string conncectionString)
        {

            _connectionString = conncectionString;

            // Set up MongoDB conventions
            var pack = new ConventionPack {
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfNullConvention(true)
            };

            ConventionRegistry.Register("MongoDb Conventions", pack, t => true);
        }

        public IMongoDatabase Create()
        {
            var mongoUrlBuilder = new MongoUrlBuilder(_connectionString);
            var client = new MongoClient(mongoUrlBuilder.ToMongoUrl());
            string databaseName = mongoUrlBuilder.DatabaseName;

            return client.GetDatabase(databaseName);
        }
    }
}