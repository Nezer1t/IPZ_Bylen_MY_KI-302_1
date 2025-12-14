using System.Text.Json;
using System.Windows;
using lab4_2.Entity;

namespace lab2_2.Request
{
    public class GetStudentDetailedInfo
    {
        // Метод для отримання детальної інформації про студента (імітація відповіді сервера)
        public async Task<StudentDetailedInfo> FetchStudentDetailedInfo(string userName)
        {
            await Task.Delay(500); // Симуляція затримки мережі

            // Створюємо фіктивну відповідь, якби вона прийшла з сервера
            var simulatedResponse = new
            {
                success = true,
                message = "Profile fetched successfully.",
                profile = new StudentDetailedInfo
                {
                    Username = userName,
                    Name = "Іван Петренко",
                    Rating = 4.75,
                    University = "Львівська політехніка",
                    Specialization = "Комп'ютерні науки",
                    Email = "ivan.petrenko@example.com"
                }
            };

            // Серіалізуємо і одразу десеріалізуємо як у справжньому коді
            string response = JsonSerializer.Serialize(simulatedResponse);
            Console.WriteLine(response);

            try
            {
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                if (responseObject?.success == true)
                {
                    return responseObject.profile;
                }
                else
                {
                    MessageBox.Show(responseObject?.message ?? "Unknown error");
                    return null;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return null;
            }
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public StudentDetailedInfo profile { get; set; }
        }
    }
}
