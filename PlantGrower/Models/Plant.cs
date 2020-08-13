namespace PlantGrower.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Plant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]        
        public string Name { get; set; }
        
        [BsonElement("germinationDays")]
        public double GerminationDays { get; set; }
    }
}
