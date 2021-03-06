﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;

namespace OAuth
{
	public static partial class OAuthHelper
	{
		/// <summary>
		/// revision a.
		/// http://tools.ietf.org/html/rfc5849
		/// </summary>
		public const string VERSION = "1.0";

		/// <summary>
		/// Add the provided parameters to the header of the provided request.
		/// </summary>
		public static void MakeRequestAuthenticated(HttpWebRequest request, IEnumerable<QueryParameter> parameters, string clientSecret, string tokenSecret)
		{
			QueryParameter signatureMethodParameter = null;
			SignatureMethod signatureMethod;
			try
			{
				signatureMethodParameter = parameters.Single(x => x.Name == OAuthParameter.SignatureMethod.ToStringValue());
				signatureMethod = OAuthSignatureMethods.Single(x => x.Value == signatureMethodParameter.Value).Key;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Signature method not found in the parameters.", ex);
			}
			var parametersList = parameters.ToList();
			var signatureBase = OAuthHelper.GetSignatureBaseString(request.Method, request.Address, parametersList);
			var signature = OAuthHelper.GetSignature(signatureMethod, signatureBase, clientSecret, tokenSecret);
			parametersList.Add(new QueryParameter(OAuthParameter.Signature, signature));
			request.Headers.Add(HttpRequestHeader.Authorization, OAuthHelper.GetAuthorizationHeaderValue(parametersList));
		}

		public static string GetAuthorizationHeaderValue(IEnumerable<QueryParameter> parameters)
		{
			StringBuilder result = new StringBuilder();
			result.Append("OAuth ");
			foreach (var parameter in parameters)
			{
				if (parameter != parameters.First())
				{
					result.Append(",");
				}
				result.AppendFormat("{0}=\"{1}\"", OAuthPercentEncode(parameter.Name), OAuthPercentEncode(parameter.Value));
			}
			return result.ToString();
		}

		public static IEnumerable<QueryParameter> GetQueryParameters(string clientIdentifier, SignatureMethod signatureMethod, params QueryParameter[] other)
		{
			List<QueryParameter> result = new List<QueryParameter>();
			result.Add(new QueryParameter(OAuthParameter.Version, VERSION));
			result.Add(new QueryParameter(OAuthParameter.ConsumerKey, clientIdentifier));
			result.Add(new QueryParameter(OAuthParameter.SignatureMethod, OAuthSignatureMethods[signatureMethod]));

			switch (signatureMethod)
			{
				case SignatureMethod.PLAINTEXT:
					break;
				case SignatureMethod.HMAC_SHA1:
					result.Add(new QueryParameter(OAuthParameter.Timestamp, OAuthHelper.GenerateUTCTimestamp()));
					result.Add(new QueryParameter(OAuthParameter.Nonce, OAuthHelper.GenerateNonce()));
					break;
				case SignatureMethod.RSA_SHA1:
				default:
					throw new NotImplementedException();
			}

			foreach (var parameter in other)
			{
				result.Add(parameter);
			}
			return result;
		}

		public static string GetSignatureBaseString(string httpMethod, Uri requestUri, IEnumerable<QueryParameter> oAuthParams)
		{
			StringBuilder signatureBaseString = new StringBuilder();
			string baseStringUri = GetBaseStringUri(requestUri);
			List<QueryParameter> parameters = GetParametersFromUri(requestUri);

			if (oAuthParams != null)
			{
				//must be in their decoded form
				parameters.AddRange(oAuthParams);
			}

			signatureBaseString.Append(OAuthPercentEncode(httpMethod));
			signatureBaseString.Append('&');
			signatureBaseString.Append(OAuthPercentEncode(baseStringUri));
			signatureBaseString.Append('&');
			signatureBaseString.Append(OAuthPercentEncode(NormalizeParameters(parameters)));
			return signatureBaseString.ToString();
		}

		/// <summary>
		/// Get properly formatted base uri
		/// </summary>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		public static string GetBaseStringUri(Uri requestUri)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}://", requestUri.Scheme.ToLowerInvariant());
			sb.Append(requestUri.Host.ToLowerInvariant());
			if (!requestUri.IsDefaultPort)
			{
				sb.AppendFormat(":{0}", requestUri.Port);
			}
			//System.Uri seems to url encode properly in upper case
			sb.Append(requestUri.AbsolutePath);
			return sb.ToString();
		}

		/// <summary>
		/// Get decoded parameters.
		/// </summary>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		/// <remarks>
		/// does not take into account other parameters than those in the uri
		/// http://tools.ietf.org/html/rfc5849#section-3.4.1.3.1
		/// </remarks>
		public static List<QueryParameter> GetParametersFromUri(Uri requestUri)
		{
			//urlencoded at this point
			string queryParameters = requestUri.Query;

			if (queryParameters.StartsWith("?"))
			{
				queryParameters = queryParameters.Remove(0, 1);
			}
			List<QueryParameter> parameters = ParseParameters(queryParameters);
			return parameters;
		}

		/// <summary>
		/// parse a string of encoded name-value pairs,
		/// the name separated from the value by a '=' char,
		/// the parameters separated from each over by a '&amp;' char.
		/// (i.e: application/x-www-form-urlencoded mime type)
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static List<QueryParameter> ParseParameters(string query)
		{
			List<QueryParameter> parameters = new List<QueryParameter>();
			if (!string.IsNullOrEmpty(query))
			{
				string[] splittedQuery = query.Split('&');
				foreach (string nameValuePair in splittedQuery)
				{
					if (!string.IsNullOrEmpty(nameValuePair))
					{
						string name;
						if (nameValuePair.Contains('='))
						{
							string[] splittedPair = nameValuePair.Split('=');
							name = UrlDecoder.UrlDecode(splittedPair[0]);
							string value = UrlDecoder.UrlDecode(splittedPair[1]);
							parameters.Add(new QueryParameter(name, value));
						}
						else
						{
							name = UrlDecoder.UrlDecode(nameValuePair);
							parameters.Add(new QueryParameter(name, ""));
						}

					}
				}
			}
			return parameters;
		}

		private static string NormalizeParameters(IEnumerable<QueryParameter> parameters)
		{
			List<QueryParameter> encodedParameters = new List<QueryParameter>();
			foreach (var item in parameters)
			{
				encodedParameters.Add(
					new QueryParameter(
						OAuthPercentEncode(item.Name),
						OAuthPercentEncode(item.Value)));
			}
			encodedParameters.Sort(new QueryParameterComparer());
			return string.Join("&", encodedParameters.Select(x => x.Encode()));
		}

		public static string GetSignature(SignatureMethod method, string signatureBaseString, string clientSecret, string tokenSecret)
		{
			switch (method)
			{
				case SignatureMethod.PLAINTEXT:
					return string.Format("{0}&{1}", OAuthPercentEncode(clientSecret), OAuthPercentEncode(tokenSecret));
				case SignatureMethod.HMAC_SHA1:
					string key = string.Format("{0}&{1}", OAuthPercentEncode(clientSecret), OAuthPercentEncode(tokenSecret));
					HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(key));
					byte[] digest = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString));
					return Convert.ToBase64String(digest);
				case SignatureMethod.RSA_SHA1:
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// http://tools.ietf.org/html/rfc5849#section-3.6
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string OAuthPercentEncode(string value)
		{
			byte[] valueUTF8Bytes = System.Text.Encoding.UTF8.GetBytes(value);
			StringBuilder result = new StringBuilder();
			foreach (byte symbol in valueUTF8Bytes)
			{
				if ((symbol >= 'a' && symbol <= 'z') ||
					(symbol >= 'A' && symbol <= 'Z') ||
					(symbol >= '0' && symbol <= '9') ||
					symbol == '-' ||
					symbol == '.' ||
					symbol == '_' ||
					symbol == '~')
				{
					//must not be encoded
					result.Append((char)symbol);
				}
				else
				{
					//encoded upper case
					result.AppendFormat("%{0:X2}", symbol);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Get the string representation of the current utc time as a unix timestamp.
		/// </summary>
		/// <returns></returns>
		public static string GenerateUTCTimestamp()
		{
			// Default implementation of UNIX time of the current UTC time
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}

		public static string GenerateNonce()
		{
			//uses CoCreateGuid() win32 function, which should be a good cryptographic random source.
			return Guid.NewGuid().ToString();
		}
	}
}