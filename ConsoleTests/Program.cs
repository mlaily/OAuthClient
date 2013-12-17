using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using OAuth;

namespace ConsoleTests
{
	partial class Program
	{
		static void Main(string[] args)
		{
			//application already authorized
			var oauth = new OAuthClient(new ClientCredentials(consumerKey, secretKey), new TokenCredentials(tokenIdentifier, tokenSharedSecret));
			var request = oauth.CreateAuthenticatedRequest("http://api.tumblr.com/v2/user/info");
			string x;
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
				{
					x = sr.ReadToEnd();
				}
			}

			//full authorization process
			var oauthProcess = new OAuthProcess(new ClientCredentials(consumerKey, secretKey),
				"http://www.tumblr.com/oauth/request_token",
				"http://www.tumblr.com/oauth/authorize",
				"http://www.tumblr.com/oauth/access_token");
			oauthProcess.RequestTemporaryCredentials();
			var authorizationRequestUri = oauthProcess.GetAuthorizationUri();
			//the user must visit the authorization uri and accept the application to continue.
			//they are then redirected to the callback uri, with the required oauth parameters provided by the server.
			string verifier = oauthProcess.ParseVerifierFromAuthorizedCallbackUri("http://authorizedCallbackUri");
			//alternatively, if the server directly provides a verifier (i.e twitter pin) to the user, it can be used here.
			var tokenCredentials = oauthProcess.RequestTokenCredentials(verifier);

			oauth = new OAuthClient(oauthProcess.ClientCredentials, tokenCredentials);

			request = (HttpWebRequest)HttpWebRequest.Create("http://api.tumblr.com/v2/user/info");
			oauth.MakeRequestAuthenticated(request);
			string y;
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
				{
					y = sr.ReadToEnd();
				}
			}
		}
	}
}
