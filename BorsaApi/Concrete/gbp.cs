using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class gbp
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("gbp")]
        public double GBP { get; set; }

    
    }
}
