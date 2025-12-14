using lab4_2.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace lab4_2.ServerRequest
{
    public class SearchStudentsRequest
    {
        // Список студентів для симуляції (фіктивна база даних)
        private readonly List<Student> _allStudents = new List<Student>
        {
            new Student { Name = "Іван Петренко", Username = "ivan.petrenko", Rating = 4.75 },
            new Student { Name = "Олена Коваль", Username = "olena.koval", Rating = 4.62 },
            new Student { Name = "Марко Шевченко", Username = "marko.shevchenko", Rating = 4.90 },
            new Student { Name = "Софія Романюк", Username = "sofia.romanyuk", Rating = 4.55 },
            new Student { Name = "Артем Сокіл", Username = "artem.sokil", Rating = 4.33 }
        };

        public async Task<List<Student>> SearchByName(string name)
        {
            return await SendSearchRequest(new { command = "getprofilesbyname", name = name });
        }

        public async Task<List<Student>> SearchByRating(double rating)
        {
            return await SendSearchRequest(new { command = "getprofilesbyrating", Rating = rating.ToString(CultureInfo.InvariantCulture) });
        }

        public async Task<List<Student>> SearchByUniversity(string university)
        {
            return await SendSearchRequest(new { command = "getprofilesbyuniversity", university = university });
        }

        public async Task<List<Student>> SearchBySpecialty(string specialty)
        {
            return await SendSearchRequest(new { command = "getprofilesbyspecialization", specialization = specialty });
        }

        private async Task<List<Student>> SendSearchRequest(object requestModel)
        {
            await Task.Delay(400); // Симуляція затримки

            string requestJson = JsonSerializer.Serialize(requestModel);
            Console.WriteLine($"Simulated request: {requestJson}");

            // Усі результати будуть успішними
            var simulatedResponse = new
            {
                success = true,
                message = "Search completed successfully.",
                profiles = FilterStudents(requestModel)
            };

            string response = JsonSerializer.Serialize(simulatedResponse);
            Console.WriteLine($"Simulated response: {response}");

            try
            {
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                if (responseObject?.success == true)
                {
                    return responseObject.profiles;
                }

                MessageBox.Show(responseObject?.message ?? "Unknown error");
                return new List<Student>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return new List<Student>();
            }
        }

        // Простий фільтр для імітації пошуку
        private List<Student> FilterStudents(object requestModel)
        {
            string json = JsonSerializer.Serialize(requestModel);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("name", out var nameProp))
            {
                string name = nameProp.GetString()?.ToLower() ?? "";
                return _allStudents.Where(s => s.Name.ToLower().Contains(name)).ToList();
            }

            if (root.TryGetProperty("Rating", out var ratingProp))
            {
                if (double.TryParse(ratingProp.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var rating))
                    return _allStudents.Where(s => s.Rating >= rating).ToList();
            }

            if (root.TryGetProperty("university", out _))
            {
                // У нас немає університетів у Student, тому просто повернемо все
                return _allStudents;
            }

            if (root.TryGetProperty("specialization", out _))
            {
                // Аналогічно, просто повернемо все
                return _allStudents;
            }

            return _allStudents;
        }

        // Клас-обгортка відповіді
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Student> profiles { get; set; }
        }
    }
}
