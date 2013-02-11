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
			var oauth = new OAuthClient(consumerKey, secretKey, tokenIdentifier, tokenSharedSecret);
			var request = (HttpWebRequest)HttpWebRequest.Create("http://api.tumblr.com/v2/user/info");
			oauth.MakeRequestAuthenticated(request);
			string x;
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
				{
					x = sr.ReadToEnd();
				}
			}

			//full authorization process
			oauth = new OAuthClient(consumerKey, secretKey,
				"http://www.tumblr.com/oauth/request_token",
				"http://www.tumblr.com/oauth/authorize",
				"http://www.tumblr.com/oauth/access_token");
			oauth.RequestTemporaryCredentials();
			var authorizationRequestUri = oauth.GetAuthorizationUri();
			//the user must visit the authorization uri and accept the application to continue.
			//they are then redirected to the callback uri, with the required oauth parameters provided by the server.
			string authorizedCallBackUri = "authorized (verified) callback uri from the server...";
			oauth.RequestTokenCredentials(authorizedCallBackUri);

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
