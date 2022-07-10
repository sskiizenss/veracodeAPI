namespace veracodeAPI
{
    public class apiActionXml
    {
        private const string AuthorizationHeader = "Authorization";
        private const string urlBase = "analysiscenter.veracode.eu";
        private string urlParams = string.Empty;
        private const string ApiId = "c4b040564dcbaeff63962baf74799fb6";
        private const string ApiKey = "250d32e49d3876d84ca8c3c24b972ca567507d177a2c5b563a10b6b41baa64831cb5df1f59652fb1fc9763d77cbe61dc5974b9cc36b3e5951c00c29a690aa23e";

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
    }
}