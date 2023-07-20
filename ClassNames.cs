using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongo1
{
    public class ClassNames
    {

        [BsonId]
        public ObjectId Id { get; set; }

        //[BsonElement("name")]
        public String ClassName { get; set; }
        public String Namespace { get; set; }

    }
}
