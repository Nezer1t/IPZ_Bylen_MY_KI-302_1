using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using lab3_2.api.Models;
using lab3_2.api.Services;

public class TcpServer
{
    private readonly UserService _userService;
    private readonly UserProfileService _userProfileService;
    private readonly UserRatingsService _userRatingsService;
    private readonly ILogger _logger;

    public TcpServer(UserService userService, UserProfileService userProfileService, UserRatingsService userRatingsService, ILogger<TcpServer> logger)
    {
        _userService = userService;
        _userProfileService = userProfileService;
        _userRatingsService = userRatingsService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (client)
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine(request);

            try
            {
                // Десеріалізація запиту
                var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(request);
                if (requestData == null)
                {
                    var response = new { message = "Invalid command." };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                    return;
                }

                var command = requestData["command"].ToLower();
                if (command == "register")
                {
                    bool registerSuccess = await _userService.RegisterUser(requestData["username"],
                        requestData["email"],
                        requestData["password"]);
                    var response = new
                    {
                        success = registerSuccess,
                        message = registerSuccess ? "User created successfully." : "Invalid username or password."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "login")
                {
                    bool loginSuccess = await _userService.LoginUser(requestData["username"], requestData["password"]);
                    var response = new
                    {
                        success = loginSuccess,
                        message = loginSuccess ? "Login successful." : "Invalid username or password."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "upsert")
                {
                    var userProfile = new UserProfile
                    {
                        Username = requestData["Username"],
                        Name = requestData["Name"],
                        University = requestData["University"],
                        Specialization = requestData["Specialization"],
                        Email = requestData["Email"],
                        Rating = double.Parse(requestData["Rating"], CultureInfo.InvariantCulture)
                    };

                    var result = await _userProfileService.UpsertUserProfile(userProfile);
                    var response = new
                        { success = true, message = "UserProfile updated successfully.", profile = result };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                // Обробка команди "getprofilebyname" для отримання профілю за іменем користувача
                else if (command == "getprofilebyname")
                {
                    var profile = await _userProfileService.GetUserProfileByName(requestData["username"]);
                    var response = new { success = true, profile };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                // Обробка команди "getallprofiles" для отримання всіх профілів
                else if (command == "getallprofiles")
                {
                    var profiles = await _userProfileService.GetUserProfiles();
                    var response = new { success = true, profiles };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getprofilesbyname")
                {
                    var profiles = await _userProfileService.GetUserProfilesByName(requestData["name"]);
                    var response = new { success = true, profiles };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getprofilesbyrating")
                {
                    // Переконайтеся, що тут ви правильно парсите rating
                    double rating = double.Parse(requestData["Rating"], CultureInfo.InvariantCulture);
                    var profiles = await _userProfileService.GetUserProfilesByRating(rating);
                    var response = new { success = true, profiles };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getprofilesbyuniversity")
                {
                    var profiles = await _userProfileService.GetUserProfilesByUniversity(requestData["university"]);
                    var response = new { success = true, profiles };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "getprofilesbyspecialization")
                {
                    var profiles =
                        await _userProfileService.GetUserProfilesBySpecialization(requestData["specialization"]);
                    var response = new { success = true, profiles };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else if (command == "addrating")
                {
                    var username = requestData["Username"];
                    var speciality = requestData["Speciality"];
                    int rating = int.Parse(requestData["Rating"], CultureInfo.InvariantCulture);

                    bool success = await _userRatingsService.AddRating(username, speciality, rating);
                    var response = new
                    {
                        success = success,
                        message = success ? "Rating added successfully." : "Failed to add rating."
                    };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
                else
                {
                    var response = new {success = false,  message = "Invalid command." };
                    var jsonResponse = JsonSerializer.Serialize(response);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
                }
            }
            catch (Exception ex)
            {
                var response = new {success = false,  message = $"{ex.Message}" };
                var jsonResponse = JsonSerializer.Serialize(response);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(jsonResponse));
            }
        }
    }
}