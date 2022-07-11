using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace veracodeAPI
{
    public class apiActionRest
    {
        private const string AuthorizationHeader = "Authorization";
        private const string urlBase = "api.veracode.eu";
        private string urlParams = string.Empty;
        private const string ApiId = "APIID";
        private const string ApiKey = "APIKEY";

        //Permet de récupérer une la liste des applications
        public string getAppList()
        {
            string response = makeAction("/appsec/v1/applications/?page=0&size=50", "GET");
            Console.WriteLine(response);

            return response;
        }

        // ! Doesn't work ATM
        public void getAppListTag()
        {
            //Console.Write("Tags (separated by ','):");
            //string[] tags = Console.ReadLine();
            string response = makeAction("/appsec/v1/applications?tags=test", "GET");
            Console.WriteLine(response);
        }

        //Permet de récupérer la liste des Apps par Status de compliance
        public void getAppListByComplianceStatus(string? complianceStatus)
        {
            if(complianceStatus == null)
            {
                do{
                    Console.Write("Compliance status you want (PASSED, DID_NOT_PASS, NOT_ASSESSED): ");
                    complianceStatus = Console.ReadLine();
                }while((complianceStatus != "PASSED") && (complianceStatus != "DID_NOT_PASS") && (complianceStatus != "NOT_ASSESSED"));
            }
            string response = makeAction("/appsec/v1/applications?policy_compliance="+complianceStatus, "GET");
            Console.WriteLine(response);
        }


        //Permet de récupérer la liste des apps possédant un customtag et la valeur associée
        public void getAppListByCustomFields(string? custom_field_name, string? custom_field_value)
        {
            if(custom_field_name == null && custom_field_value == null)
            {
                Console.Write("Custom_field_name: ");
                custom_field_name = Console.ReadLine();
                Console.Write("Custom_field_name: ");
                custom_field_value = Console.ReadLine();

                string response = makeAction("/appsec/v1/applications?custom_field_names="+custom_field_name+"&custom_field_values="+custom_field_value, "GET");
                Console.WriteLine(response);
            }
        }

        // ! Ne fonctionne pas
        public void getAppListbyLastPolicyEvaluation()
        {
            //yyyy-mm-ddThh:mm:ss.000Z
            string date = "2022-06-25T00:00:00.000Z";
            string response = makeAction("/appsec/v1/applications?policy_compliance_checked_after="+date, "GET");
            Console.WriteLine(response);
        }

        ///////////////////////////////////////////////////
        // * Permet de récupérer les ID des projets
        ///////////////////////////////////////////////////
        public void getIdOfProjects()
        {
            string jsonAppList = getAppList();   
            dynamic data = JObject.Parse(jsonAppList);

            for(int x=0 ; x < data._embedded.applications.Count ; x++)
            {
                Console.Write(data._embedded.applications[x].profile.name+": ");
                Console.WriteLine(data._embedded.applications[x].id);
            }
        }

        ///////////////////////////////////////////////////
        // * Permet de récupérer l'ID d'un projet en fonction de son nom
        ///////////////////////////////////////////////////
        public void getIdOfProject(string? idProjet)
        {

        }

        ///////////////////////////////////////////////////
        // * Permet d'afficher le détail des vulnérabilités d'un scan
        // ! Ne fonctionne pas, impossible de récupérer les données de l'API sur un scan prédéfini
        ///////////////////////////////////////////////////
        public void viewPipelineScanDetails(string? scan_id)
        {
            Console.Write("Scan Id: ");
            scan_id = Console.ReadLine();

            //string response = makeAction("/pipeline_scan/v1/scans/"+scan_id, "GET");
            //string response = makeAction("/appsec/v2/applications/"+scan_id+"/findings?scan_type=STATIC", "GET");
            string response = makeAction("/appsec/v2/applications/"+scan_id+"/findings?include_annot=TRUE", "GET");
            Console.WriteLine(response);
        }

        ///////////////////////////////////////////////////
        // * Permet d'afficher les détails d'une application
        ///////////////////////////////////////////////////
        public void viewApplicationDetails(string? project_id)
        {
            Console.Write("Application id: ");
            project_id = Console.ReadLine();

            string response = makeAction("/appsec/v1/applications/?legacy_id="+project_id, "GET");
            Console.WriteLine(response);
        }

        ///////////////////////////////////////////////////
        // * Effectue la requête veracode et récupére le résultat
        ///////////////////////////////////////////////////
        private string makeAction(string urlPath, string httpVerb)
        {
            string urlParams = string.Empty;
            using var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, "https://"+urlBase+urlPath);

            var authorization = HmacAuthHeader.HmacSha256.CalculateAuthorizationHeader(ApiId, ApiKey, urlBase, urlPath, urlParams, httpVerb);

			request.Headers.Add(AuthorizationHeader, authorization);

            var response = httpClient.Send(request);
			var reader = new StreamReader(response.Content.ReadAsStream());
			var responseBody = reader.ReadToEnd();

            return responseBody;
        }

        //Permet de travailler sur le JSON sur son format string initial
        private void transformJSON(string initStringJson)
        {
            /*JObject json = JObject.Parse(initStringJson);
            foreach(var e in json)
            {
                Console.WriteLine(e);
            }*/
            
            /*using var resp = JsonDocument.Parse(initStringJson);

            var  root = resp.RootElement;

            var elems = root.EnumerateArray();

            elems.MoveNext();

            Console.WriteLine(elems.Current);*/

            JsonNode forecastNode = JsonNode.Parse(initStringJson)!;

            var options = new JsonSerializerOptions { WriteIndented = true};
            //Console.WriteLine(forecastNode!.ToJsonString(options));

            JsonNode aNode = forecastNode!["id"]!;
            Console.WriteLine($"JSON={aNode.ToJsonString()}");



        }
    }
}