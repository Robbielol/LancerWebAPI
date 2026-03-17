namespace LancerWebAPI.Services
{
    public interface IDatabaseService
    {
        public Task Create<T>(IEnumerable<T> list);
        public Task<IEnumerable<T>> Read<T>(string filter);
        public Task Update<T>(string query);
        public Task Delete<T>(string query);
    }
}
