using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BorsaApi.Concrete
{
    public class che
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("che")]
        public double CHE { get; set; }

    
    }
}
