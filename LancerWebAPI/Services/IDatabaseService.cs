namespace LancerWebAPI.Services
{
    public interface IDatabaseService
    {
        public Task Create<T>(string query);
        public void Read<T>(string query);
        public void Update<T>(string query);
        public void Delete<T>(string query);
    }
}
