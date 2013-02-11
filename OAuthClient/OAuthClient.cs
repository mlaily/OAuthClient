using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth.Enums;
using System.Net;

namespace OAuth
{
	public class OAuthClient
	{

		public OAuthCredentials ClientCredentials { get; protected set; }

		public OAuthCredentials TemporaryCredentials { get; protected set; }

		public OAuthCredentials TokenCredentials { get; protected set; }

		public string TemporaryCredentialsRequestEndpoint { get; protected set; }

		public string ResourceOwnerAuthorizationEndpoint { get; protected set; }

		public string TokenRequestEndpoint { get; protected set; }

		/// <summary>
		/// The default is HMAC_SHA1.
		/// </summary>
		public SignatureMethod SignatureMethod { get; set; }

		/// <summary>
		/// Used internally when requesting the token credentials from the authorized callback uri.
		/// </summary>
		protected string Verifier { get; set; }

		/// <summary>
		/// Supported http methods.
		/// The string representations of the enum are used.
		/// </summary>
		private enum HttpMethod
		{
			POST,
			GET,
		}

		public OAuthClient()
		{
			this.SignatureMethod = SignatureMethod.HMAC_SHA1;
		}

		public OAuthClient(
			string clientIdentifier,
			string clientSharedSecred,
			string temporaryCredentialsRequestEndpoint,
			string resourceOwnerAuthorizationEndpoint,
			string tokenRequestEndpoint)
			: this()
		{
			this.ClientCredentials = new OAuthCredentials(clientIdentifier, clientSharedSecred);
			this.TemporaryCredentialsRequestEndpoint = temporaryCredentialsRequestEndpoint;
			this.ResourceOwnerAuthorizationEndpoint = resourceOwnerAuthorizationEndpoint;
			this.TokenRequestEndpoint = tokenRequestEndpoint;
		}

		/// <summary>
		/// Utility method avoiding the hassle of creating a new HttpWebRequest and setting some default values by hand...
		/// </summary>
		/// <param name="method"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		private static HttpWebRequest CreateRequest(HttpMethod method, string uri)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
			request.Method = method.ToString();
			request.ServicePoint.Expect100Continue = false;
			return request;
		}

		/// <summary>
		/// Utility method allowing to easily get the response from a HttpWebRequest.
		/// The body is returned via the out responseBody parameter.
		/// If an exception occurs, responseBody is populated if possible, then the exception is thrown.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="responseBody"></param>
		/// <returns></returns>
		private static HttpWebResponse TryGetResponse(HttpWebRequest request, out string responseBody)
		{
			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
				using (response)
				using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
				{
					responseBody = sr.ReadToEnd();
				}
			}
			catch (WebException ex)
			{
				using (ex.Response)
				using (var sr = new System.IO.StreamReader(ex.Response.GetResponseStream()))
				{
					responseBody = sr.ReadToEnd();
				}
				throw;
			}
			catch (Exception)
			{
				throw;
			}
			return response;
		}

		/// <summary>
		/// First step to the authorization process.
		/// Make a request to the TemporaryCredentialsRequestEndpoint, then store the resulting temporary credentials in this instance.
		/// </summary>
		public void RequestTemporaryCredentials()
		{
			HttpWebRequest request = CreateRequest(HttpMethod.POST, this.TemporaryCredentialsRequestEndpoint);
			//TODO: should allow the user to set the callback uri here.
			//From the rfc:
			//An absolute URI back to which the server will
			//redirect the resource owner when the Resource Owner
			//Authorization step (Section 2.2) is completed.  If
			//the client is unable to receive callbacks or a
			//callback URI has been established via other means,
			//the parameter value MUST be set to "oob" (case
			//sensitive), to indicate an out-of-band
			//configuration.
			var oAuthParameters = OAuthHelper.GetTemporaryCredentialsRequestParameters(ClientCredentials.Identifier, SignatureMethod.HMAC_SHA1);
			OAuthHelper.MakeRequestAuthenticated(request, oAuthParameters, ClientCredentials.SharedSecret, "");

			string responseString;
			TryGetResponse(request, out responseString);

			var parsed = OAuthHelper.ParseParameters(responseString);
			//oauth_callback_confirmed MUST be present and set to "true". The parameter is used to differentiate from previous versions of the protocol.
			if (parsed.Any(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.CallbackConfirmed] && x.Value.ToLowerInvariant() == "true"))
			{
				var identifier = parsed.Single(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.Token]).Value;
				var sharedSecret = parsed.Single(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.TokenSecret]).Value;
				this.TemporaryCredentials = new OAuthCredentials(identifier, sharedSecret);
			}
			else
			{
				throw new Exception("Unexpected response from the server.");
			}
		}

		/// <summary>
		/// Return an uri formed by the combination of the ResourceOwnerAuthorizationEndpoint and the temporary credentials.
		/// RequestTemporaryCredentials() must be called beforehand.
		/// </summary>
		/// <returns></returns>
		public string GetAuthorizationUri()
		{
			if (this.TemporaryCredentials == null)
			{
				throw new Exception("The RequestTemporaryCredentials() method must be called first!");
			}
			return string.Format("{0}?{1}={2}", this.ResourceOwnerAuthorizationEndpoint, OAuthHelper.OAuthParametersNames[OAuthParameter.Token], this.TemporaryCredentials.Identifier);
		}

		/// <summary>
		/// Final step to the OAuth authorization process.
		/// Parse the provided authorized callback uri to extract the credentials provided by the server,
		/// then make a request to the TokenRequestEndpoint to finally get the token credentials, and store them in this instance.
		/// </summary>
		/// <param name="authorizedCallbackUri"></param>
		public void RequestTokenCredentials(string authorizedCallbackUri)
		{
			var parsedCallbackUri = OAuthHelper.GetParametersFromUri(new Uri(authorizedCallbackUri));
			if (!parsedCallbackUri.Any(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.Token] && x.Value == this.TemporaryCredentials.Identifier))
			{
				throw new Exception("The temporary identifier in the url does not match the one in this instance!");
			}
			this.Verifier = parsedCallbackUri.SingleOrDefault(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.Verifier]).Value;
			if (this.Verifier == null)
			{
				throw new Exception("Verifier parameter not found in the url!");
			}

			HttpWebRequest request = CreateRequest(HttpMethod.POST, this.TokenRequestEndpoint);
			var oAuthParameters = OAuthHelper.GetTokenCredentialsRequestParameters(this.ClientCredentials.Identifier, SignatureMethod.HMAC_SHA1, this.TemporaryCredentials.Identifier, this.Verifier);

			OAuthHelper.MakeRequestAuthenticated(request, oAuthParameters, ClientCredentials.SharedSecret, TemporaryCredentials.SharedSecret);

			string responseString;
			TryGetResponse(request, out responseString);

			var parsed = OAuthHelper.ParseParameters(responseString);
			var identifier = parsed.Single(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.Token]).Value;
			var sharedSecret = parsed.Single(x => x.Name == OAuthHelper.OAuthParametersNames[OAuthParameter.TokenSecret]).Value;
			this.TokenCredentials = new OAuthCredentials(identifier, sharedSecret);
		}

		/// <summary>
		/// Once this instance has obtained the needed token credentials,
		/// use this method to append the OAuth headers to the request to make it authenticated.
		/// </summary>
		/// <param name="request"></param>
		public void MakeRequestAuthenticated(HttpWebRequest request)
		{
			var oAuthParameters = OAuthHelper.GetTokenCredentialsParameters(this.ClientCredentials.Identifier, SignatureMethod.HMAC_SHA1, this.TokenCredentials.Identifier);
			OAuthHelper.MakeRequestAuthenticated(request, oAuthParameters, ClientCredentials.SharedSecret, TokenCredentials.SharedSecret);
		}

	}
}
