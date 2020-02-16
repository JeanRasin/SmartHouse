﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHouse.Domain.Core
{
    public class MongoBaseModel
    {
        [BsonId]
       // [BsonRepresentation(BsonType.ObjectId)] todo: error!!!
        public string Id { get; set; }
        [BsonElement("categoryName")]
        [BsonRepresentation(BsonType.String)]
        public string CategoryName { get; set; }
    }
}
