using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class jpy
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("jpy")]
        public double JPY { get; set; }

    
    }
}
