using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Second_project.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getMaxWindSpeedByCountryAndCityProcedure = @"
               CREATE PROCEDURE GetMaxWindSpeedByCountry
               AS
               BEGIN
                   SELECT
                       w1.Country,
                       w1.City,
                       w1.LastUpdateTime,
                       MAX(w1.WindSpeed) as MaxWindSpeed
                   FROM
                       WeatherData w1
                   INNER JOIN (
                       SELECT
                           Country,
                           MAX(WindSpeed) as MaxWindSpeed
                       FROM
                           WeatherData
                       GROUP BY
                           Country
                   ) w2 ON w1.Country = w2.Country AND w1.WindSpeed = w2.MaxWindSpeed
                   GROUP BY
                       w1.Country,
                       w1.City,
                       w1.LastUpdateTime
               END
            ";

            var getMinTemperatureByCountryAndCityProcedure = @"
               CREATE PROCEDURE GetWeatherStatsWithoutParameters
               AS
               BEGIN
                   SELECT
                       w1.Country,
                       w1.City,
                       w1.LastUpdateTime,
                       MIN(w1.Temperature) as MinTemperature
                   FROM
                       WeatherData w1
                   INNER JOIN (
                       SELECT
                           Country,
                           MIN(Temperature) as MinTemperature
                       FROM
                           WeatherData
                       GROUP BY
                           Country
                   ) w2 ON w1.Country = w2.Country AND w1.Temperature = w2.MinTemperature
                   GROUP BY
                       w1.Country,
                       w1.City,
                       w1.LastUpdateTime
               END
            ";

            migrationBuilder.Sql(getMaxWindSpeedByCountryAndCityProcedure);
            migrationBuilder.Sql(getMinTemperatureByCountryAndCityProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetMaxWindSpeedByCountry");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetWeatherStatsWithoutParameters");
        }
    }

}
