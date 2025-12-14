using System.Globalization;
using lab4_2.Entity;
using System.Text.Json;
using System.Windows;

namespace lab4_2.Request
{
    public class UpdateInfoRequest
    {
        // Імітація запиту оновлення даних студента
        public async Task<bool> Update(string username, StudentDetailedInfo detailedInfo)
        {
            try
            {
                await Task.Delay(400); // Симуляція затримки мережі

                // Додаємо username для узгодження з реальним форматом
                detailedInfo.Username = username;

                // Формуємо фіктивну JSON-відповідь від "сервера"
                var simulatedResponse = new
                {
                    success = true,
                    message = $"Profile for '{detailedInfo.Username}' updated successfully.",
                };

                // Серіалізація у JSON, як у справжньому запиті
                string response = JsonSerializer.Serialize(simulatedResponse);
                Console.WriteLine($"Simulated request for {detailedInfo.Username}");
                Console.WriteLine(response);

                // Імітація десеріалізації
                var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);

                if (responseObject?.success == true)
                {
                    return true; // Імітація успішного оновлення
                }
                else
                {
                    MessageBox.Show(responseObject?.message ?? "Update failed.");
                    return false;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing simulated response: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        // Клас для обгортки відповіді
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
