using System.Text.Json;
using System.Windows;

namespace lab4_2.Request
{
    public class StudentLogin
    {
        // Імітація запиту логіну без підключення до сервера
        public async Task<bool> LoginAsync(string username, string password)
        {
            await Task.Delay(400); // Симуляція мережевої затримки

            // Фіктивна відповідь від "сервера"
            var simulatedResponse = new
            {
                success = true,
                message = $"User '{username}' logged in successfully."
            };

            // Серіалізуємо відповідь у JSON (для повної імітації)
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
                    MessageBox.Show(responseObject?.message ?? "Login failed.");
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