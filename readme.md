"# Rbac.Auth.RemoteApi" 

   if a MVC controller is run at http://localhost:5000, and provide a POST API by Url http://localhost:5000/Identity/Permission/GetUser
   
   then we can create the DemoWrapper class.


    //
    //
    public class DemoWrapper : RemoteApiWrapper
    {
        private static DemoWrapper _instance = null;

        private DemoWrapper(string domain) : base(domain)
        { }

        public static DemoWrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DemoWrapper(“http://localhost:5000”);
                }

                return _instance;
            }
        }

        public async Task<User> GetUser(HttpRequest request, Guid userId)
        {
            List<KeyValuePair<string, string>> paramArray = new List<KeyValuePair<string, string>>();

            paramArray.Add(new KeyValuePair<string, string>("userId", userId.ToString()));

            var baseUrl = $"/Identity/Permission/GetUser";
            var json = await PostRequestAsync(request, baseUrl, paramArray);
            var data = JsonConvert.DeserializeObject<User>(json);
            return data;
        }
    }
	
	
	In our funtion can call the remote api by
	
	try
	{
		var user = DemoWrapper.Instance.GetUser(httpRequest, userId);
		...
	}
	catch(Exception ex)
	{
	}