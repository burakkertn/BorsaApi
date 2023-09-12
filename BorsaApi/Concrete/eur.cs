using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class eur
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("eur")]
        public double EUR { get; set; }

    
    }
}
