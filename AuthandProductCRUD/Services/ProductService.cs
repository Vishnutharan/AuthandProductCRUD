using AuthandProductCRUD.Model;
using MongoDB.Driver;

namespace AuthandProductCRUD.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _products = database.GetCollection<Product>("Products");
        }

        public List<Product> GetAll() => _products.Find(_ => true).ToList();
        public Product GetById(string id) => _products.Find(p => p.Id == id).FirstOrDefault();
        public void Create(Product product) => _products.InsertOne(product);
        public void Update(string id, Product productIn) =>
            _products.ReplaceOne(p => p.Id == id, productIn);
        public void Delete(string id) => _products.DeleteOne(p => p.Id == id);
    }
}