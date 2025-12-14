using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab4_2.Entity;

namespace lab2_2.Request
{
    public class GetStudentDetailedInfo
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для отримання детальної інформації про студента через TCP
        public async Task<StudentDetailedInfo> FetchStudentDetailedInfo(string userName)
        {
            StudentDetailedInfo studentInfo = null;

            var requestModel = new
            {
                command = "getprofilebyname",
                username = userName
            };

            var request = JsonSerializer.Serialize(requestModel);
            var bytesToSend = Encoding.UTF8.GetBytes(request);

            try
            {
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

                        Console.WriteLine(response);

                        // Десеріалізація отриманого JSON
                        try
                        {
                            var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                            if (responseObject?.success == true)
                            {
                                studentInfo = responseObject.profile;
                            }
                            else
                            {
                                MessageBox.Show(responseObject.message);
                            }
                        }
                        catch (JsonException ex)
                        {
                            MessageBox.Show($"Error parsing response: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }

            return studentInfo;
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
