using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class usd
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("usd")]
        public double USD { get; set; }

    }
}
