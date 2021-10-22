using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Data.Mongo.Collections
{
    public abstract class MongoBaseDocument
    {
        /// <summary>
        /// Id > String
        /// </summary>
        [BsonId]
        [JsonProperty(Order = 1)]
        [BsonElement(Order = 0)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
