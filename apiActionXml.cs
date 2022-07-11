namespace veracodeAPI
{
    public class apiActionXml
    {
        private const string AuthorizationHeader = "Authorization";
        private const string urlBase = "analysiscenter.veracode.eu";
        private string urlParams = string.Empty;
        private const string ApiId = "APIID";
        private const string ApiKey = "APIKEY";

        //Permet de récupérer une la liste des applications
        public void getAppList()
        {
            const string urlPath = "/api/5.0/getapplist.do";
            const string httpVerb = "GET";
			string urlParams = string.Empty;

			using var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, "https://"+urlBase+urlPath);

            var authorization = HmacAuthHeader.HmacSha256.CalculateAuthorizationHeader(ApiId, ApiKey, urlBase, urlPath, urlParams, httpVerb);

			request.Headers.Add(AuthorizationHeader, authorization);

            var response = httpClient.Send(request);
			var reader = new StreamReader(response.Content.ReadAsStream());
			var responseBody = reader.ReadToEnd();

            Console.WriteLine(responseBody);
        }

        public void getScanDetail()
        {
            const string urlPath = "/api/5.0/getapplist.do";
            const string httpVerb = "GET";
			string urlParams = string.Empty;

			using var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, "https://"+urlBase+urlPath);

            var authorization = HmacAuthHeader.HmacSha256.CalculateAuthorizationHeader(ApiId, ApiKey, urlBase, urlPath, urlParams, httpVerb);

			request.Headers.Add(AuthorizationHeader, authorization);

            var response = httpClient.Send(request);
			var reader = new StreamReader(response.Content.ReadAsStream());
			var responseBody = reader.ReadToEnd();

            Console.WriteLine(responseBody);
        }
    }
}