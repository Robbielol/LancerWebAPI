namespace LancerWebAPI.Services
{
    public interface IDatabaseService
    {
        public Task Create<T>(string query);
        public Task<IEnumerable<T>> Read<T>(string query);
        public Task Update<T>(string query);
        public Task Delete<T>(string query);
    }
}
