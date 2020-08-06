namespace PlantGrower.Services
{
    using PlantGrower.Models;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using System.Net.Mail;

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

        public async Task Create(Plant plant)
        {
            await _plants.InsertOneAsync(plant).ConfigureAwait(false);
            
            return;
        }
    }
}
