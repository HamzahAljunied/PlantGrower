namespace PlantGrower.Models
{
    public class PlantDatabaseSettings : IPlantDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
