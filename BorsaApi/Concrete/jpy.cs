using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class jpy
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("jpy")]
        public double JPY { get; set; }

    
    }
}
