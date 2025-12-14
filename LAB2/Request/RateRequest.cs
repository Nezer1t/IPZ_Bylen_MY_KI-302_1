using System.Text.Json;
using System.Windows;

namespace lab2_2.Request
{
    public class RateRequest
    {
        // Симуляція успішного виконання запиту на оцінювання
        public async Task<bool> RateAsync(string username, string speciality, string rating)
        {
            await Task.Delay(400); // Симулюємо мережеву затримку

            // Створюємо фіктивну відповідь сервера
            var simulatedResponse = new
            {
                success = true,
                message = $"Rating {rating} for speciality '{speciality}' by user '{username}' saved successfully."
            };

            // Серіалізація у JSON для імітації справжнього обміну
            string response = JsonSerializer.Serialize(simulatedResponse);
            Console.WriteLine(response);

            try
            {
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                if (responseObject?.success == true)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(responseObject?.message ?? "Unknown error");
                    return false;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return false;
            }
        }

        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}