using System.Text.Json;
using System.Windows;

namespace lab4_2.Request
{
    public class StudentRegister
    {
        // Імітація запиту на реєстрацію
        public async Task<bool> RegisterAsync(string username, string password, string email)
        {
            await Task.Delay(400); // Симулюємо невелику затримку, як при запиті

            // Фіктивна JSON-відповідь, як від справжнього сервера
            var simulatedResponse = new
            {
                success = true,
                message = $"User '{username}' registered successfully with email '{email}'."
            };

            // Серіалізація для повної симуляції процесу
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
                    MessageBox.Show(responseObject?.message ?? "Registration failed.");
                    return false;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}");
                return false;
            }
        }

        // Обгортка відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}