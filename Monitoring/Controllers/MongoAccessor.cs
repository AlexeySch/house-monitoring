using System;
using System.Collections;
using System.Security.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Monitoring.Controllers
{
    public static class MongoAccessor
    {

        public static Sensors GetState()
        {
            var client = GetMongoClient();
            var col = client.GetDatabase("iot").GetCollection<BsonDocument>("deviceData");
            var s = new Sensors();

            s.Humidity_Inside = (double)FindLast(col, "Humidity_Inside");
            s.Temperature_Inside = (double)FindLast(col, "Temperature_Inside");
            s.Power_Status = (string) FindLast(col, "Power_Status"); 
            s.Power_Voltage= (double)FindLast(col, "Power_Voltage");
            return s;
        }

        private static object FindLast(IMongoCollection<BsonDocument> collection, string item)
        {
            var sort = Builders<BsonDocument>.Sort.Descending("timestamp");
            return BsonTypeMapper.MapToDotNetValue(collection.Find(Builders<BsonDocument>.Filter.Eq("item", item)).Sort(sort).FirstOrDefault()["value"]);
        }

        public static Array LoadTimeSeries(string deviceName)
        {
            MongoClient mongoClient = GetMongoClient();
            var dataCollection = mongoClient.GetDatabase("iot").GetCollection<BsonDocument>("deviceData");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("item", deviceName) & filterBuilder.Gt("timestamp", DateTime.Today.AddDays(-14));
            var projection = Builders<BsonDocument>.Projection.Include("timestamp").Include("value").Exclude("_id");
            var documents = dataCollection.Find(filter).Project(projection);
            Array data = ConvertToArray(documents, "timestamp", "value");
            return data;

        }

        private static MongoClient GetMongoClient()
        {
            string connectionString =@"mongodb://35.156.115.201:27017";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            return mongoClient;
        }

        private static Array ConvertToArray(IFindFluent<BsonDocument, BsonDocument> documents, string timestampField,
    string valueField)
        {
            var result = new ArrayList();
            var cursor = documents.ToCursor();

            foreach (var document in documents.ToEnumerable())
            {
                result.Add(new object[] { document[timestampField].AsBsonDateTime.MillisecondsSinceEpoch, BsonTypeMapper.MapToDotNetValue(document[valueField]) });
            }
            return result.ToArray();
        }

    }
}