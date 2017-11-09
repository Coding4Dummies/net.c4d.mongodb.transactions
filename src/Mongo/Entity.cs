using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace TenLi.Api.DataAccess.Mongo
{
    public class Entity
	{
		[BsonId]
		[JsonIgnore]
		public virtual ObjectId _id { get; set; }
	}
}
