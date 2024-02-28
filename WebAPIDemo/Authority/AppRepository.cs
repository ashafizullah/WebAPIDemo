using System;
namespace WebAPIDemo.Authority
{
	public class AppRepository
	{
		private static List<Application> _applications = new List<Application>()
		{
			new Application
			{
				ApplicationId = 1,
				ApplicationName = "MVCWebApp",
				ClientId = "1239810283012803102839012839012",
				Secret = "123-91820-3-1293-12",
				Scopes = "read,write,delete"
			}
		};

		public static Application? GetApplicationByClientId(string clientId)
		{
			return _applications.FirstOrDefault(x => x.ClientId == clientId);
		}
	}
}

