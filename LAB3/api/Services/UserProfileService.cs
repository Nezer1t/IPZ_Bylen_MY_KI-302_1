using System.Text.RegularExpressions;
using lab3_2.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_2.api.Services;

public class UserProfileService
    {
        private readonly AppDbContext _context;

        public UserProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> UpsertUserProfile(UserProfile model)
        {
            // Перевірка, чи юзернейм існує в таблиці користувачів
            var userExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            if (userExists == null)
            {
                throw new InvalidOperationException("Username does not exist.");
            }

            // Перевірка валідності даних
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            if (model.Name.Length < 5 || model.Name.Length > 50)
            {
                throw new ArgumentException("Name must be between 5 and 50 characters.");
            }


            if (string.IsNullOrWhiteSpace(model.University))
            {
                throw new ArgumentException("University cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model.Specialization))
            {
                throw new ArgumentException("Specialization cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new ArgumentException("Email cannot be empty.");
            }
            
            // Перевірка діапазону оцінки
            if (model.Rating < 1 || model.Rating > 100)
            {
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 100.");
            }

            // Перевірка формату електронної пошти
            if (!Regex.IsMatch(model.Email, @"^[^@\s]+@lpnu\.com$"))
            {
                throw new ArgumentException("Email must be in a valid format and end with lpnu.com.");
            }

            // Перевірка, чи профіль з таким юзернеймом уже існує
            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (existingProfile != null)
            {
                // Оновлення даних профілю
                existingProfile.Name = model.Name;
                existingProfile.University = model.University;
                existingProfile.Specialization = model.Specialization;
                existingProfile.Email = model.Email;
                existingProfile.Rating = model.Rating;
                _context.UserProfiles.Update(existingProfile);
            }
            else
            {
                // Додавання нового профілю
                _context.UserProfiles.Add(model);
            }

            await _context.SaveChangesAsync();
            return model;
        }


        public async Task<IEnumerable<UserProfile>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> GetUserProfileByName(string userName)
        {
            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Username == userName);

            if (userProfile == null)
            {
                return new UserProfile { Username = userName };
            }

            return userProfile;
        }
        
        public async Task<IEnumerable<UserProfile>> GetUserProfilesByName(string name)
        {
            return await _context.UserProfiles
                .Where(u => u.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfilesByRating(double rating)
        {
            return await _context.UserProfiles
                .Where(u => u.Rating >= rating)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfilesByUniversity(string university)
        {
            return await _context.UserProfiles
                .Where(u => u.University.Contains(university))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfilesBySpecialization(string specialization)
        {
            return await _context.UserProfiles
                .Where(u => u.Specialization.Contains(specialization))
                .ToListAsync();
        }

    }
