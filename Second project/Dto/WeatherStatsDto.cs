namespace Second_project.Dto
{
    public class WeatherStatsDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public double? MinTemperature { get; set; } 
        public double? MaxWindSpeed { get; set; }
        public DateTime LastTimeUpdated { get; set; }
    }
}
