﻿using System;
using System.Net; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace veracodeAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				//Console.ReadLine();

				bool showMenuType = true;
				while(showMenuType)
				{
					Console.Clear();
					Console.WriteLine("XML or REST ?");
					Console.Write("\r\nSelect an option: ");
					switch(Console.ReadLine())
					{
						case "XML":
							showMenuType = MainMenuXML();
							break;
						case "REST":
							showMenuType = MainMenuRest();
							break;
						default:
							Console.WriteLine("Bye !");
							showMenuType = false;
							break;
					}
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey();
				}
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

		private static bool MainMenuXML()
		{
			apiActionXml apiXml = new apiActionXml();

			bool showMenuAction = true;
			while(showMenuAction)
			{
				Console.Clear();
				Console.WriteLine("Choose an option");
				Console.WriteLine("1) GetAppList");
				Console.WriteLine("2) ");
				Console.WriteLine("3) ");
				Console.Write("\r\nSelect an option: ");

				switch(Console.ReadLine())
				{
					case "1":
						apiXml.getAppList();
						showMenuAction = true;
						break;
					case "2":
						showMenuAction = true;
						break;
					case "3":
						showMenuAction = true;
						break;
					default:
						showMenuAction = false;
						break;
				}
				if(showMenuAction == true)
				{
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey();
				}
			}
			return true;
		}

		private static bool MainMenuRest()
		{
			apiActionRest apiRest = new apiActionRest();


			bool showMenuAction = true;
			while(showMenuAction)
			{
				Console.Clear();
				Console.WriteLine("Choose an option");
				Console.WriteLine("1) GetAppList");
				Console.WriteLine("2) GetAppListTag (Disabled)");
				Console.WriteLine("3) GetAppListByComplianceStatus");
				Console.WriteLine("4) GetAppListByCustomFields");
				Console.WriteLine("5) GetAppListbyLastPolicyEvaluation (Disabled)");
				Console.WriteLine("6) GetIdOfProject");
				Console.WriteLine("7) ViewPipelineScanDetails");
				Console.WriteLine("8) viewApplicationDetails");
				Console.Write("\r\nSelect an option: ");

				switch(Console.ReadLine())
				{
					case "1":
						apiRest.getAppList();
						showMenuAction = true;
						break;
					case "2":
						apiRest.getAppListTag();
						showMenuAction = true;
						break;
					case "3":
						apiRest.getAppListByComplianceStatus(null);
						showMenuAction = true;
						break;
					case "4":
						apiRest.getAppListByCustomFields(null, null);
						showMenuAction = true;
						break;
					case "5":
						apiRest.getAppListbyLastPolicyEvaluation();
						showMenuAction = true;
						break;
					case "6":
						apiRest.getIdOfProjects();
						showMenuAction = true;
						break;
					case "7":
						apiRest.viewPipelineScanDetails(null);
						showMenuAction = true;
						break;
					case "8":
						apiRest.viewApplicationDetails(null);
						showMenuAction = true;
						break;
					default:
						showMenuAction = false;
						break;
				}
				if(showMenuAction == true)
				{
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey();
				}
			}
			return true;
		}
	}
}