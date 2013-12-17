using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth
{
	/// <summary>
	/// The textual representation of each parameter can be retrieved using
	/// the OAuthHelper.GetValue extension method.
	/// </summary>
	public enum SignatureMethod
	{
		PLAINTEXT,
		HMAC_SHA1,
		RSA_SHA1,
	}

	/// <summary>
	/// Convenient list of well known OAuth parameters.
	/// The textual representation of each parameter can be retrieved using
	/// the OAuthHelper.GetValue extension method.
	/// </summary>
	public enum OAuthParameter
	{
		ConsumerKey,
		Callback,
		Version,
		SignatureMethod,
		Signature,
		Timestamp,
		Nonce,
		Token,
		TokenSecret,
		CallbackConfirmed,
		Verifier,
	}

	public static partial class OAuthHelper
	{

		private static readonly Dictionary<OAuthParameter, string> OAuthParametersNames = new Dictionary<OAuthParameter, string>()
		{
			{OAuthParameter.ConsumerKey, "oauth_consumer_key"},
			{OAuthParameter.Callback, "oauth_callback"},
			{OAuthParameter.Version,"oauth_version"},
			{OAuthParameter.SignatureMethod,"oauth_signature_method"},
			{OAuthParameter.Signature, "oauth_signature"},
			{OAuthParameter.Timestamp, "oauth_timestamp"},
			{OAuthParameter.Nonce, "oauth_nonce"},
			{OAuthParameter.Token,"oauth_token"},
			{OAuthParameter.TokenSecret, "oauth_token_secret"},
			{OAuthParameter.CallbackConfirmed, "oauth_callback_confirmed"},
			{OAuthParameter.Verifier, "oauth_verifier"},
		};
		public static string ToStringValue(this OAuthParameter value)
		{
			if (Enum.IsDefined(typeof(OAuthParameter), value)) return OAuthParametersNames[value];
			else throw new NotImplementedException();
		}

		private static readonly Dictionary<SignatureMethod, string> OAuthSignatureMethods = new Dictionary<SignatureMethod, string>()
		{
			{SignatureMethod.PLAINTEXT, "PLAINTEXT"},
			{SignatureMethod.RSA_SHA1, "RSA-SHA1"},
			{SignatureMethod.HMAC_SHA1, "HMAC-SHA1"},
		};
		public static string ToStringValue(this SignatureMethod value)
		{
			if (Enum.IsDefined(typeof(SignatureMethod), value)) return OAuthSignatureMethods[value];
			else throw new NotImplementedException();
		}

	}
}
