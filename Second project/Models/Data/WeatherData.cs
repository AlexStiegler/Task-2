using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Second_project.Models.Data
{
    public class WeatherData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Country { get; set; }
        public string City { get; set; }

        public double WindSpeed { get; set; }

        public double Temperature { get; set; }

        public double Clouds { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
