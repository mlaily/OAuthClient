using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace OAuth
{
	internal static class Util
	{

		/// <summary>
		/// Supported http methods.
		/// The string representations of the enum can be used.
		/// </summary>
		internal static class HttpMethod
		{
			internal const string POST = "POST";
			internal const string GET = "GET";
		}

		/// <summary>
		/// Utility method avoiding the hassle of creating a new HttpWebRequest and setting some default values by hand...
		/// </summary>
		internal static HttpWebRequest CreateRequest(string method, string uri)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
			request.Method = method;
			request.ServicePoint.Expect100Continue = false;
			return request;
		}

		/// <summary>
		/// Utility method allowing to easily get the response from a HttpWebRequest.
		/// The body is returned via the out responseBody parameter.
		/// If an exception occurs, responseBody is populated if possible, then the exception is thrown.
		/// </summary>
		internal static HttpWebResponse TryGetResponse(HttpWebRequest request, out string responseBody, int contentLength = 0)
		{
			HttpWebResponse response = null;
			try
			{
				request.ContentLength = contentLength;
				response = (HttpWebResponse)request.GetResponse();
				using (response)
				using (var sr = new StreamReader(response.GetResponseStream()))
				{
					responseBody = sr.ReadToEnd();
				}
				return response;
			}
			catch (WebException ex)
			{
				using (var sr = new StreamReader(ex.Response.GetResponseStream()))
				{
					responseBody = sr.ReadToEnd();
				}
				throw new Exception(string.Format("WebException caught. Response body:\n{0}", responseBody), ex);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
