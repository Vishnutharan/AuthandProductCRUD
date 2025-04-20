using AuthandProductCRUD.Model;
using MongoDB.Driver;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; 

namespace AuthandProductCRUD.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly ILogger<UserService> _logger; 

        public UserService(IConfiguration configuration, ILogger<UserService> logger) 
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
            _logger = logger; 
        }

        public User GetUser(string username, string password)
        {
            _logger.LogInformation($"GetUser called with username: {username}, password: {password}");
            var user = _users.Find(u => u.Username == username).FirstOrDefault();
            if (user != null && user.Password != null && password != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    _logger.LogInformation("Password verified successfully.");
                    return user;
                }
                else
                {
                    _logger.LogInformation("Password verification failed.");
                }
            }
            else
            {
                _logger.LogInformation("User is null or password check failed.");
            }
            return null;
        }

        public void AddUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _users.InsertOne(user);
        }

        public User GetUserByUsername(string username)
        {
            return _users.Find(u => u.Username == username).FirstOrDefault();
        }
    }
}