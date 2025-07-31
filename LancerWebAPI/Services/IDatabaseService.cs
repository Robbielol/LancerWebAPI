namespace LancerWebAPI.Services
{
    public interface IDatabaseService
    {
        public Task Create<T>(string query);
        public Task Read<T>(string query);
        public Task Update<T>(string query);
        public Task Delete<T>(string query);
    }
}
