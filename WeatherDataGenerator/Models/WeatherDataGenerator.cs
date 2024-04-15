using System.Text.Json;

namespace WeatherDataGenerator.Models
{
    internal class WeatherDataGenerator
    {
        private static WeatherData[] weeklyWeather; // Przeniesiona deklaracja na poziom klasy

        public static void GenerateWeatherData()
        {
            string[] weather = { "Deszcz", "Słonecznie", "Śnieg", "Mgliście", "Burza", "Lekkie zachmurzenie", "Pochmurno", "Mżawka" };
            string[] day_of_week = { "poniedzialek", "wtorek", "sroda", "czwartek", "piatek", "sobota", "niedziela" };
            string[] months = { "styczen", "luty", "marzec", "kwiecien", "maj", "czerwiec", "lipiec", "sierpien", "wrzesien", "pazdziernik", "listopad", "grudzien" };

            Random rnd = new Random();

            Console.WriteLine("Podaj miesiąc (np. styczen, luty, ...(bez polskich znakow)):");
            string monthInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(monthInput))
            {
                Console.WriteLine("Nie ma takiego miesiaca! Sprobuj ponownie");
                return;
            }

            if (Array.IndexOf(months, monthInput) != -1)
            {
                int monthIndex = Array.IndexOf(months, monthInput) + 1;
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, monthIndex);

                weeklyWeather = new WeatherData[daysInMonth]; // Inicjalizacja tablicy

                for (int i = 0; i < daysInMonth; i++)
                {
                    WeatherData data = new WeatherData();
                    data.DayOfWeek = day_of_week[new Random().Next(day_of_week.Length)];
                    data.Weather = weather[new Random().Next(weather.Length)];
                    data.LowestTemp = GenerateTemperature(monthIndex, true);
                    data.HighestTemp = GenerateTemperature(monthIndex, false);
                    data.Pressure = rnd.Next(980, 1020);
                    data.Humidity = rnd.Next(50, 100);
                    data.Wind = new Wind { Speed = rnd.NextDouble() * 20, Deg = rnd.Next(0, 360) };
                    data.Visibility = rnd.NextDouble() * 100;
                    data.Clouds = rnd.NextDouble() * 100;

                    weeklyWeather[i] = data;
                }

                string fileName = $"pogoda_{monthInput}.json";
                string json = JsonSerializer.Serialize(weeklyWeather, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fileName, json);

                Console.WriteLine($"Dane pogodowe zostały zapisane do pliku {fileName}");
            }
            else
            {
                Console.WriteLine("Niepoprawny miesiąc.");
                return;
            }

            Console.WriteLine("Wybierz dzień tygodnia do wyświetlenia (np. poniedzialek, wtorek, ... (bez polskich znakow)):");
            string chosenDay = Console.ReadLine()?.ToLower();

            // Czy dzien tygodnia jest poprawny
            if (Array.IndexOf(day_of_week, chosenDay) == -1)
            {
                Console.WriteLine("Nie ma takiego dnia tygodnia! Sprobuj ponownie");
                return;
            }

            // Odczytaj dane pogodowe z pliku JSON
            string jsonFromFile = File.ReadAllText($"pogoda_{monthInput}.json");
            WeatherData[] weeklyWeatherFromFile = JsonSerializer.Deserialize<WeatherData[]>(jsonFromFile);

            foreach (var dayWeather in weeklyWeatherFromFile)
            {
                if (dayWeather.DayOfWeek == chosenDay)
                {
                    Console.WriteLine($"Pogoda na {dayWeather.DayOfWeek}:");
                    Console.WriteLine($"- Pogoda: {dayWeather.Weather}");
                    Console.WriteLine($"- Najniższa temperatura: {dayWeather.LowestTemp}°C");
                    Console.WriteLine($"- Najwyższa temperatura: {dayWeather.HighestTemp}°C");
                    Console.WriteLine($"- Ciśnienie: {dayWeather.Pressure} hPa");
                    Console.WriteLine($"- Wilgotność: {dayWeather.Humidity}%");
                    Console.WriteLine($"- Prędkość wiatru: {dayWeather.Wind.Speed} km/h");
                    Console.WriteLine($"- Kierunek wiatru: {dayWeather.Wind.Deg}°");
                    Console.WriteLine($"- Widoczność: {dayWeather.Visibility}%");
                    Console.WriteLine($"- Zachmurzenie: {dayWeather.Clouds}%");
                    break;
                }
            }

            Console.ReadLine();
        }

        static double GenerateTemperature(int month, bool isLowestTemp)
        {
            Random rnd = new Random();
            double baseTemp = 0;

            switch (month)
            {
                case 12:
                case 1:
                case 2: // grudzień, styczeń, luty
                    baseTemp = isLowestTemp ? -10 : 5;
                    break;
                case 6:
                case 7:
                case 8: // czerwiec, lipiec, sierpień
                    baseTemp = isLowestTemp ? 15 : 25;
                    break;
                case 3:
                case 4:
                case 5: // marzec, kwiecień, maj
                    baseTemp = isLowestTemp ? 5 : 15;
                    break;
                default:
                    baseTemp = isLowestTemp ? 0 : 20;
                    break;
            }

            return baseTemp + (rnd.NextDouble() * 10);
        }
    }
}

