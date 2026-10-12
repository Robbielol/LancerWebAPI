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

        public async Task<UserModel> LoginUserAsync(LoginRequestUserDto request)
        {
            // Check if a user with this email even exists
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return null; // User not found
            }

            // Compare the plain text password from the React form against the database hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash);

            if (!isPasswordValid)
            {
                return null; // Invalid password
            }

            // If both pass, return the user model back to the controller
            return existingUser;
        }

    }
}
