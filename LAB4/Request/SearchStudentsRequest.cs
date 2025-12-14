using lab4_2.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace lab4_2.ServerRequest
{
    public class SearchStudentsRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        public async Task<List<Student>> SearchByName(string name)
        {
            return await SendSearchRequest(new { command = "getprofilesbyname", name = name });
        }

        public async Task<List<Student>> SearchByRating(double rating)
        {
            return await SendSearchRequest(new
                { command = "getprofilesbyrating", Rating = rating.ToString(CultureInfo.InvariantCulture) });
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
                    
                    Console.WriteLine(response);

                    try
                    {
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject.success)
                        {
                            return responseObject.profiles;
                        }
                        else
                        {
                            MessageBox.Show(responseObject.message);
                            return new List<Student>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Обробка помилок парсингу
                        Console.WriteLine($"Error parsing response: {ex.Message}");
                        return new List<Student>();
                    }
                }
            }
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Student> profiles { get; set; }
        }
    }
}