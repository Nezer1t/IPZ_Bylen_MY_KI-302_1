using System.Text.Json;
using System.Windows;
using lab4_2.Entity;

namespace lab4_2.ServerRequest
{
    public class GetStudents
    {
        // ІМІТАЦІЯ: повертаємо статичні дані, не підключаючись до сервера
        public async Task<List<Student>> FetchStudentsAsync()
        {
            await Task.Delay(400); // невелика затримка для правдоподібності

            // Фіктивна відповідь у форматі, подібному до реального сервера
            var simulatedResponse = new
            {
                success = true,
                message = "Profiles fetched successfully.",
                profiles = new List<Student>
                {
                    new Student { Name = "Іван Петренко",   Username = "ivan.petrenko",   Rating = 4.75 },
                    new Student { Name = "Олена Коваль",    Username = "olena.koval",     Rating = 4.62 },
                    new Student { Name = "Марко Шевченко",  Username = "marko.shevchenko",Rating = 4.90 },
                    new Student { Name = "Софія Романюк",   Username = "sofia.romanyuk",  Rating = 4.55 },
                    new Student { Name = "Артем Сокіл",     Username = "artem.sokil",     Rating = 4.33 }
                }
            };

            // Серіалізуємо/десеріалізуємо як у «справжньому» запиті
            string response = JsonSerializer.Serialize(simulatedResponse);
            Console.WriteLine(response);

            try
            {
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                if (responseObject?.success == true && responseObject.profiles != null)
                {
                    return responseObject.profiles;
                }

                // Залишено для сумісності зі структурою оригіналу
                MessageBox.Show(responseObject?.message ?? "Unknown error");
                return new List<Student>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return new List<Student>();
            }
        }

        // Клас-обгортка відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Student> profiles { get; set; }
        }
    }
}
