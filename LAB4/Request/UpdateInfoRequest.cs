using System.Globalization;
using lab4_2.Entity;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace lab4_2.Request
{
    public class UpdateInfoRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        public async Task<bool> Update(string username, StudentDetailedInfo detailedInfo)
        {
            try
            {
                // Додати юзернейм до об'єкта, який буде відправлено на сервер
                detailedInfo.Username = username;

                // Створити об'єкт запиту без обгортки studentProfile
                var requestModel = new
                {
                    command = "upsert",
                    Username = detailedInfo.Username,
                    Name = detailedInfo.Name,
                    University = detailedInfo.University,
                    Specialization = detailedInfo.Specialization,
                    Email = detailedInfo.Email,
                    Rating = detailedInfo.Rating.ToString(CultureInfo.InvariantCulture)
                };

                var request = JsonSerializer.Serialize(requestModel);
                var bytesToSend = Encoding.UTF8.GetBytes(request);

                using (var client = new TcpClient(ServerAddress, ServerPort))
                {
                    using (var stream = client.GetStream())
                    {
                        // Відправка запиту на сервер
                        await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                        // Отримання відповіді від сервера
                        var buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Обробка відповіді
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject.success)
                        {
                            return true; // Оновлення успішне
                        }
                        else
                        {
                            MessageBox.Show(responseObject.message);
                            return false; // Оновлення не вдалося
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false; // У випадку помилки
            }
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
