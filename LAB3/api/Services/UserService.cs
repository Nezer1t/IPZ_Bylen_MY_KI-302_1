using System.Text.RegularExpressions;
using lab3_2.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_2.api.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterUser(string username, string email, string password)
    {
        try
        {
            // Перевірка, чи існує користувач з таким юзернеймом
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                throw new InvalidOperationException("User already exists.");
            }

            // Перевірка валідності юзернейму
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                throw new ArgumentException("Username must be at least 3 characters long.");
            }

            // Перевірка валідності електронної пошти
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Перевірка валідності пароля
            if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit) || !password.Any(char.IsPunctuation))
            {
                throw new ArgumentException("Password must be at least 8 characters long, contain upper and lower case letters, numbers, and special characters.");
            }

            // Створення нового користувача
            var user = new User
            {
                Username = username,
                Email = email,
                Password = password
            };

            // Додавання користувача в контекст і збереження змін
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }


    public async Task<bool> LoginUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Username and password must not be empty.");
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && user.Password == password)
        {
            return true;
        }

        return false;
    }
}