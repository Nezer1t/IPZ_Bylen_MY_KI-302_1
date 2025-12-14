using lab3_2.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_2.api.Services;

public class UserRatingsService
{
    private readonly AppDbContext _context;

    public UserRatingsService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddRating(string username, string speciality, int rating)
    {
        try
        {
            // Перевірка, чи користувач з таким юзернеймом існує
            var userExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            if (userExists == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            // Перевірка, чи спеціальність не є порожньою
            if (string.IsNullOrWhiteSpace(speciality))
            {
                throw new ArgumentException("Speciality cannot be empty.");
            }

            // Перевірка діапазону оцінки
            if (rating < 1 || rating > 100)
            {
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 100.");
            }

            // Перевірка, чи вже існує оцінка для даного юзернейму та спеціальності
            var existingRating = await _context.UserRatings
                .FirstOrDefaultAsync(r => r.Username == username && r.Speciality == speciality);

            if (existingRating != null)
            {
                // Оновлення існуючої оцінки
                existingRating.Rating = rating;
                _context.UserRatings.Update(existingRating);
            }
            else
            {
                // Додавання нової оцінки
                var userRating = new UserRatings
                {
                    Username = username,
                    Speciality = speciality,
                    Rating = rating
                };

                _context.UserRatings.Add(userRating);
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error: {ex.Message}");
        }
    }

}
