using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab4_2.Entity;

namespace lab4_2.ServerRequest;

public class GetStudents
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    // Метод для отримання списку студентів через TCP
    public async Task<List<Student>> FetchStudentsAsync()
    {
        var requestModel = new
        {
            command = "getallprofiles"
        };

        var request = JsonSerializer.Serialize(requestModel);
        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                try
                {
                    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                    using (var memoryStream = new MemoryStream())
                    {
                        var buffer = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await memoryStream.WriteAsync(buffer, 0, bytesRead);
                        }

                        string response = Encoding.UTF8.GetString(memoryStream.ToArray());
                        
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject.success)
                        {
                            return responseObject.profiles;
                        }
                        
                        MessageBox.Show(responseObject.message);
                        return new List<Student>();
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
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