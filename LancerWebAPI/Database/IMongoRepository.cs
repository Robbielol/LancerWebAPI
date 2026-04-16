namespace LancerWebAPI.Database
{
    public interface IMongoRepository<T> where T : class
    {
        public Task InsertManyAsync(IEnumerable<T> docs);
        public Task InsertOneAsync(T doc);

        public Task<IEnumerable<T>> GetAllAsync();
    }
}
