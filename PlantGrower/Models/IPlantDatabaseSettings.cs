namespace PlantGrower.Models
{
    public interface IPlantDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
