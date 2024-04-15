namespace WeatherDataGenerator.Models
{
    internal class WeatherData
    {
        public string? DayOfWeek { get; set; }
        public string? Weather { get; set; }
        public double LowestTemp { get; set; }
        public double HighestTemp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public Wind Wind { get; set; }
        public double Visibility { get; set; }
        public double Clouds { get; set; }
    }

    internal class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
    }
}
