using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Net.C4D.Mongodb.Transactions.Mongo
{
    public class Entity
    {
        [BsonId]
        [JsonIgnore]
        public virtual ObjectId _id { get; set; }
    }
}