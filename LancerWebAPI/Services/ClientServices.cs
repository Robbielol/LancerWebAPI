namespace LancerWebAPI.Services
{
    public class ClientServices
    {
        public ClientServices() { }


        public List<WebsiteModel> GetAllPlaces(string location, string query, double distance)
        {
            Data = DBService.GetData(location, query, distance);
            
            return new List<WebsiteModel>();
        }
    }
}
