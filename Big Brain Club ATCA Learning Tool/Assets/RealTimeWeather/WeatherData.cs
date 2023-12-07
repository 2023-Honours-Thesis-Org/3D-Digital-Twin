using System.Collections.Generic;

namespace Weather {
    [System.Serializable]
    public class WeatherData
    {
        
        public Coord coord ;
        public List<Weather> weather ;
        public string stations ;
        public Main main ;
        public int visibility ;
        public Wind wind ;
        public Clouds clouds ;
        public int dt ;
        public Sys sys ;
        public int timezone ;
        public int id ;
        public string name ;
        public int cod ;

        public WeatherData() : this(null, null, "", null, 0, null, null, 0, null, 0, 0, "", 0)
        {
            // empty class
        }

        public WeatherData(Coord coord, List<Weather> weather, string stations, Main main, int visibility, Wind wind, Clouds clouds, int dt, Sys sys, int timezone, int id, string name, int cod)
        {
            this.coord = coord;
            this.weather = weather;
            this.stations = stations;
            this.main = main;
            this.visibility = visibility;
            this.wind = wind;
            this.clouds = clouds;
            this.dt = dt;
            this.sys = sys;
            this.timezone = timezone;
            this.id = id;
            this.name = name;
            this.cod = cod;
        } 
    }

    [System.Serializable]
    public class Coord
    {
        public double lon ;
        public double lat ;

        public Coord(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        }
    }

    [System.Serializable]
    public class Weather
    {
        public int id ;
        public string main ;
        public string description ;
        public string icon ;

        public Weather(int id, string main, string description, string icon)
        {
            this.id = id;
            this.main = main;
            this.description = description;
            this.icon = icon;
        }
    }

    [System.Serializable]
    public class Main 
    {
        public float temp ;
        public float feels_like ;
        public float temp_min ;
        public float temp_max ;
        public int pressure ;
        public int humidity ;

        public Main(float temp, float feels_like, float temp_min, float temp_max, int pressure, int humidity)
        {
            this.temp = temp;
            this.feels_like = feels_like;
            this.temp_min = temp_min;
            this.temp_max = temp_max;
            this.pressure = pressure;
            this.humidity = humidity;
        }
    }
    
    [System.Serializable]
    public class Wind
    {
        public float speed ;
        public int deg ;
        public float gust ;

        public Wind(float speed, int deg, float gust)
        {
            this.speed = speed;
            this.deg = deg;
            this.gust = gust;
        }
    }

    [System.Serializable]
    public class Clouds
    {
        public int all ;

        public Clouds(int all) 
        {
            this.all = all;
        }
    }

    [System.Serializable]
    public class Sys
    {
        public int type ;
        public int id ;
        public string country ;
        public int sunrise ;
        public int sunset ;

        public Sys(int type, int id, string country, int sunrise, int sunset)
        {
            this.type = type;
            this.id = id;
            this.country = country;
            this.sunrise = sunrise;
            this.sunset = sunset;
        }
    }
}