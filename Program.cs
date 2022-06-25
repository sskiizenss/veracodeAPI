﻿using System;
using System.Net; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace veracodeAPI
{
	public class Program
	{
		//private const string AuthorizationHeader = "Authorization";
		//private const string AuthorizationHeader = "'Content-Type': 'application/json'";
		private const string AuthorizationHeader = "Authorization";
        private const string ApiId = "c4b040564dcbaeff63962baf74799fb6";
        private const string ApiKey = "250d32e49d3876d84ca8c3c24b972ca567507d177a2c5b563a10b6b41baa64831cb5df1f59652fb1fc9763d77cbe61dc5974b9cc36b3e5951c00c29a690aa23e";

		//Pour utiliser les API XML
		//const string urlBase = "analysiscenter.veracode.eu";
		//Pour utiliser les API REST
		const string urlBase = "api.veracode.eu";

		public static void Main(string[] args)
		{
			try
			{
				getAppList();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Press any key to continue.");
				Console.ReadKey();
			}
		}

		private static JObject convertToJSON(string stringToTransform)
		{
			JObject json = JObject.Parse(stringToTransform);
			return json;
		}

		private static void getAppList()
		{
			//Pour utiliser les API XML
			//const string urlPath = "/api/5.0/getapplist.do";
			//Pour utiliser les API REST
			const string urlPath = "/appsec/v1/applications/?page=0&size=50";

            const string httpVerb = "GET";
			string urlParams = string.Empty;

			using var httpClient = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, "https://"+urlBase+urlPath);

            var authorization = HmacAuthHeader.HmacSha256.CalculateAuthorizationHeader(ApiId, ApiKey, urlBase, urlPath, urlParams, httpVerb);

			request.Headers.Add(AuthorizationHeader, authorization);
			//request.Headers.Add("Content-Type", "application/json");

            var response = httpClient.Send(request);
			var reader = new StreamReader(response.Content.ReadAsStream());
			var responseBody = reader.ReadToEnd();

            Console.WriteLine(responseBody);
		}
	}
}