// WeatherService.js
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7142/weatherdata';

const WeatherService = {
  getMinTemperatures: async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/min-temperatures`);
      return response.data;
    } catch (error) {
      console.error('Error fetching minimum temperature data:', error);
      throw error;
    }
  },

  getMaxWindSpeeds: async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/max-wind-speeds`);
      return response.data;
    } catch (error) {
      console.error('Error fetching maximum wind speed data:', error);
      throw error;
    }
  },

  getTrendData: async (country, city) => {
    try {
      const response = await axios.get(`${API_BASE_URL}/trend/${country}/${city}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching trend data for ${country}, ${city}:`, error);
      throw error;
    }
  },

  // If you have a general endpoint for all weather data:
  getAllWeatherData: async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching all weather data:', error);
      throw error;
    }
  },
};

export default WeatherService;
