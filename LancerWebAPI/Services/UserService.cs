using LancerWebAPI.Database;
using LancerWebAPI.DTOs;
using LancerWebAPI.Models;

namespace LancerWebAPI.Services
{

    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(RegisterRequestUserDto registerRequest)
        {
            // Password Encryption  
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            // Build the full database model from the tiny DTO
            var newUser = new UserModel
            {
                Email = registerRequest.Email,
                PasswordHash = hashedPassword,
                DateCreated = DateTime.UtcNow,
                Role = "Standard"
                // Id is left null so MongoDB automatically generates it!
            };

            // 4. Save to the database
            await _userRepository.InsertOneAsync(newUser);
        }

        public async Task<bool> CheckIfUserExists(string email)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            return existingUser != null;
        }

    }
}
