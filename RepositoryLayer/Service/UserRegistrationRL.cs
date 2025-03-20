using System.Linq;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLayer.Model;

namespace RepositoryLayer.Service
{
    public class UserRegistrationRL : IUserRegistrationRL
    {
        private readonly GreetingAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserRegistrationRL(GreetingAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool RegisterUser(RegistrationDTO registrationDTO)
        {
            if (_context.Users.Any(u => u.Email == registrationDTO.Email))
                return false;

            var user = new UserEntity
            {
                Username = registrationDTO.Username,
                Email = registrationDTO.Email,
                PasswordHash = registrationDTO.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public UserEntity LoginUser(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDTO.Email);
            Console.WriteLine("this is the rl for login "+user);
            if (user == null)
                return null;


            return user;
        }
        public UserEntity GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return null;

            return user;
        }
        public bool UpdateUser(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == resetPasswordDTO.Email);

                if (existingUser == null)
                {
                    return false;
                }

                existingUser.PasswordHash = resetPasswordDTO.NewPassword;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public bool SaveResetToken(string email, string token)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return false;
                }

                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(10);

                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reset token: {ex.Message}");
                return false;
            }
        }

    }
}
