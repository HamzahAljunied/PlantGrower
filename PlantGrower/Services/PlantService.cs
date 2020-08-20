namespace PlantGrower.Services
{
    using PlantGrower.Models;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using MongoDB.Driver.Linq;
    using System;
    using Microsoft.VisualBasic;
    using MongoDB.Bson;

    public class PlantService
    {
        private readonly IMongoCollection<Plant> _plants;

        public PlantService(IPlantDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _plants = database.GetCollection<Plant>(nameof(Plant));
        }

        public async Task<List<Plant>> Get()
        {
            var plants = await _plants.FindAsync(book => true).ConfigureAwait(false);
            return plants.ToList();            
        }

        public async Task<List<Plant>> Get(IQueryCollection queries)
        {
            var filter = GetFilter(queries);

            var plants = await _plants.FindAsync(filter);            
            return plants.ToList();
        }

        public async Task Create(Plant plant)
        {
            await _plants.InsertOneAsync(plant).ConfigureAwait(false);
            
            return;
        }

        public async Task Update(Plant plant, IQueryCollection queries)
        {
            var filter = GetFilter(queries);                 
            var res = await _plants.FindOneAndReplaceAsync(filter, plant).ConfigureAwait(false);

            return;
        }

        public async Task<DeleteResult> Delete(IQueryCollection queries)
        {
            var filter = GetFilter(queries);
            var delResult = _plants.DeleteManyAsync(filter);

            return await delResult.ConfigureAwait(false);
        }

        private FilterDefinition<Plant> GetFilter(IQueryCollection queries)
        {
            var builder = Builders<Plant>.Filter;
            var filter = builder.Empty;
            var props = Type.GetType("PlantGrower.Models.Plant").GetProperties();

            foreach (var keyPair in queries)
            {
                if (keyPair.Key.ToLower().EndsWith("_lt"))
                {
                    var bsonKey = keyPair.Key.Split('_')[0];
                    var queryType = props.First(prop => prop.Name.ToLower() == bsonKey.ToLower());
                    filter &= builder.Lte(bsonKey, Convert.ChangeType(keyPair.Value.ToString(), queryType.PropertyType));
                }
                else if (keyPair.Key.ToLower().EndsWith("_gt"))
                {
                    var bsonKey = keyPair.Key.Split('_')[0];
                    var queryType = props.First(prop => prop.Name.ToLower() == bsonKey.ToLower());
                    filter &= builder.Gte(bsonKey, Convert.ChangeType(keyPair.Value.ToString(), queryType.PropertyType));
                }
                else
                {
                    var bsonKey = keyPair.Key;
                    var queryType = props.First(prop => prop.Name.ToLower() == bsonKey.ToLower());
                    filter &= builder.Eq(bsonKey, Convert.ChangeType(keyPair.Value.ToString(), queryType.PropertyType));
                }
            }

            return filter;
        }
    }
}
