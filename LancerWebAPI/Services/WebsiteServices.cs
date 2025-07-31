namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {
        public WebsiteServices() { }


        public async Task<IEnumerable<WebsiteModel>> GetAllPlaces(string location, string query, double distance)
        {
            //Check if similiar query in in DB
            IDatabaseService service = new DatabaseService();

            await service.Read<WebsiteModel>(query, collectionName);

            //Return if true if not continue

            //Go to GoogleAPI and Get Data

            //Send data to DB

            //Return data
            
            return new List<WebsiteModel>();
        }
    }
}
