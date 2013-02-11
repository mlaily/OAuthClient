using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth.Enums
{
	/// <summary>
	/// The textual representation of each parameter can be retrieved using
	/// the OAuthHelper.OAuthSignatureMethods dictionary.
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
	/// the OAuthHelper.OAuthParametersNames dictionary.
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
}
