using AuthandProductCRUD.Model;
using MongoDB.Driver;

namespace AuthandProductCRUD.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        // Get user by username and password
        public User GetUser(string username, string password)
        {
            return _users.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
        }

        // Add new user to the database
        public void AddUser(User user)
        {
            _users.InsertOne(user);
        }
    }
}