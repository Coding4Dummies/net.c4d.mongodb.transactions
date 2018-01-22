using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Net.C4D.Mongodb.Transactions.Commands;

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

            // Get all types that implements ICommand
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsClass && type.GetInterfaces().Contains(typeof(ICommand)));

            // Register for Bson Deserialization
            foreach (var type in types)
            {
                BsonClassMap.LookupClassMap(type);
            }
        }

        public IMongoDatabase Create()
        {
            var mongoUrlBuilder = new MongoUrlBuilder(_connectionString);
            var client = new MongoClient(mongoUrlBuilder.ToMongoUrl());
            string databaseName = mongoUrlBuilder.DatabaseName;

            return client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetMongoCollection<T>()
        {
            return Create().GetCollection<T>(Inflector.Pluralize(typeof(T).Name));
        }

    }
}