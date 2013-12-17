using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace OAuth
{
	public class OAuthProcess
	{
		public string TemporaryCredentialsRequestEndpoint { get; protected set; }
		public string ResourceOwnerAuthorizationEndpoint { get; protected set; }
		public string TokenRequestEndpoint { get; protected set; }

		public ClientCredentials ClientCredentials { get; protected set; }
		public TemporaryCredentials TemporaryCredentials { get; protected set; }

		/// <summary>
		/// The default is HMAC_SHA1.
		/// </summary>
		public SignatureMethod SignatureMethod { get; set; }

		public OAuthProcess(ClientCredentials clientCredentials, string temporaryCredentialsRequestEndpoint, string resourceOwnerAuthorizationEndpoint, string tokenRequestEndpoint)
		{
			this.SignatureMethod = SignatureMethod.HMAC_SHA1;
			this.ClientCredentials = clientCredentials;
			this.TemporaryCredentialsRequestEndpoint = temporaryCredentialsRequestEndpoint;
			this.ResourceOwnerAuthorizationEndpoint = resourceOwnerAuthorizationEndpoint;
			this.TokenRequestEndpoint = tokenRequestEndpoint;
		}

		/// <summary>
		/// Request the temporary credential, using the client credentials of this instance and the provided callback uri.
		/// The obtained temporary credentials are stored in this instance and returned.
		/// </summary>
		public TemporaryCredentials RequestTemporaryCredentials(string callbackUri = "oob")
		{
			HttpWebRequest request = Util.CreateRequest(Util.HttpMethod.POST, this.TemporaryCredentialsRequestEndpoint);
			var oAuthParameters = OAuthHelper.GetQueryParameters(this.ClientCredentials.Identifier, this.SignatureMethod,
				new QueryParameter(OAuthParameter.Callback, callbackUri));
			//add the oauth parameters to the request. the token secret does not exist yet so it's empty.
			OAuthHelper.MakeRequestAuthenticated(request, oAuthParameters, this.ClientCredentials.SharedSecret, "");

			string responseString;
			Util.TryGetResponse(request, out responseString);

			var parsed = OAuthHelper.ParseParameters(responseString);
			//oauth_callback_confirmed MUST be present and set to "true". The parameter is used to differentiate from previous versions of the protocol.
			if (parsed.Any(x => x.Name == OAuthParameter.CallbackConfirmed.ToStringValue() && x.Value.ToLowerInvariant() == "true"))
			{
				var identifier = parsed.Single(x => x.Name == OAuthParameter.Token.ToStringValue()).Value;
				var sharedSecret = parsed.Single(x => x.Name == OAuthParameter.TokenSecret.ToStringValue()).Value;
				this.TemporaryCredentials = new TemporaryCredentials(identifier, sharedSecret);
				return this.TemporaryCredentials;
			}
			else
			{
				throw new Exception("Unexpected response from the server. \"oauth_callback_confirmed\" parameter not found!");
			}
		}

		/// <summary>
		/// Return an uri formed by the combination of the ResourceOwnerAuthorizationEndpoint and the temporary credentials.
		/// call RequestTemporaryCredentials() beforehand to obtain the temporary credentials.
		/// (if provided, the temporaryCredentials argument is used over the temporary credentials of this instance)
		/// </summary>
		/// <returns></returns>
		public string GetAuthorizationUri(TemporaryCredentials temporaryCredentials = null)
		{
			if (temporaryCredentials == null) temporaryCredentials = this.TemporaryCredentials;
			if (temporaryCredentials == null)
				throw new ArgumentNullException("temporaryCredentials", "The temporaryCredentials argument is null, and this.TemporaryCredentials is also null!");

			return string.Format("{0}?{1}={2}",
				this.ResourceOwnerAuthorizationEndpoint, OAuthParameter.Token.ToStringValue(), temporaryCredentials.Identifier);
		}

		/// <summary>
		/// Parse the provided authorized callback uri to extract the verifier provided by the server.
		/// If you already have a verifier (PIN code, etc...), use the RequestTokenCredentials() method directly.
		/// </summary>
		public string ParseVerifierFromAuthorizedCallbackUri(string authorizedCallbackUri)
		{
			var parsedCallbackUri = OAuthHelper.GetParametersFromUri(new Uri(authorizedCallbackUri));
			var verifier = parsedCallbackUri.SingleOrDefault(x => x.Name == OAuthParameter.Verifier.ToStringValue()).Value;
			if (verifier == null)
			{
				throw new Exception("verifier parameter not found in the url!");
			}
			return verifier;
		}

		/// <summary>
		/// Final step to the OAuth authorization process.
		/// Make a request to the TokenRequestEndpoint to finally get the token credentials.
		/// (if provided, the temporaryCredentials argument is used over the temporary credentials of this instance)
		/// </summary>
		public TokenCredentials RequestTokenCredentials(string verifier, TemporaryCredentials temporaryCredentials = null)
		{
			if (temporaryCredentials == null) temporaryCredentials = this.TemporaryCredentials;
			if (temporaryCredentials == null)
				throw new ArgumentNullException("temporaryCredentials", "The temporaryCredentials argument is null, and this.TemporaryCredentials is also null!");

			HttpWebRequest request = Util.CreateRequest(Util.HttpMethod.POST, this.TokenRequestEndpoint);
			var oAuthParameters = OAuthHelper.GetQueryParameters(this.ClientCredentials.Identifier, this.SignatureMethod,
				new QueryParameter(OAuthParameter.Token, temporaryCredentials.Identifier),
				new QueryParameter(OAuthParameter.Verifier, verifier));

			OAuthHelper.MakeRequestAuthenticated(request, oAuthParameters, this.ClientCredentials.SharedSecret, temporaryCredentials.SharedSecret);

			string responseString;
			Util.TryGetResponse(request, out responseString);

			var parsed = OAuthHelper.ParseParameters(responseString);
			var identifier = parsed.Single(x => x.Name == OAuthParameter.Token.ToStringValue()).Value;
			var sharedSecret = parsed.Single(x => x.Name == OAuthParameter.TokenSecret.ToStringValue()).Value;
			return new TokenCredentials(identifier, sharedSecret);
		}
	}

	public class OAuthClient
	{
		public ClientCredentials ClientCredentials { get; protected set; }

		public TokenCredentials TokenCredentials { get; protected set; }

		/// <summary>
		/// The default is HMAC_SHA1.
		/// </summary>
		public SignatureMethod SignatureMethod { get; set; }

		public OAuthClient(ClientCredentials clientCredentials, TokenCredentials tokenCredentials)
		{
			this.SignatureMethod = OAuth.SignatureMethod.HMAC_SHA1;
			this.ClientCredentials = clientCredentials;
			this.TokenCredentials = tokenCredentials;
		}

		/// <summary>
		/// Use this method to append the OAuth headers to the request to make it authenticated.
		/// </summary>
		public void MakeRequestAuthenticated(HttpWebRequest request)
		{
			var oAuthParameters = OAuthHelper.GetQueryParameters(
				this.ClientCredentials.Identifier, this.SignatureMethod,
				new QueryParameter(OAuthParameter.Token, this.TokenCredentials.Identifier));
			OAuthHelper.MakeRequestAuthenticated(
				request, oAuthParameters, ClientCredentials.SharedSecret, TokenCredentials.SharedSecret);
		}
		/// <summary>
		/// Create a new request, pre-authenticated.
		/// </summary>
		public HttpWebRequest CreateAuthenticatedRequest(string uri, string method = "GET")
		{
			HttpWebRequest request = Util.CreateRequest(method, uri);
			MakeRequestAuthenticated(request);
			return request;
		}
	}
}
