using LancerWebAPI.Database;
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

        public void RegisterUser()
        {

        }

        public async Task<bool> CheckIfUserExists(string email)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            return existingUser != null;
        }

    }
}
