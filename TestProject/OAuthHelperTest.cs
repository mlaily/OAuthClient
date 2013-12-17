using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using OAuth;

namespace TestProject
{


	/// <summary>
	///This is a test class for OAuthTest and is intended
	///to contain all OAuthTest Unit Tests
	///</summary>
	[TestClass()]
	public class OAuthHelperTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for UrlEncode
		///</summary>
		[TestMethod()]
		[DeploymentItem("OAuthClient.dll")]
		public void UrlEncodeTest()
		{
			string value = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~ @&\uD55C";
			string expected = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~%20%40%26%ED%95%9C";
			string actual;
			actual = OAuthHelper_Accessor.OAuthPercentEncode(value);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for GetBaseStringUri
		///</summary>
		[TestMethod()]
		[DeploymentItem("OAuthClient.dll")]
		public void GetBaseStringUriTest()
		{
			var uri = new Uri("htTP://eXAmple.Com/r%20v/ù/X%0A?id=123");
			string expected = "http://example.com/r%20v/%C3%B9/X%0A";
			string actual;
			actual = OAuthHelper_Accessor.GetBaseStringUri(uri);
			Assert.AreEqual(expected, actual);

			uri = new Uri("https://www.example.net:8080/?q=1");
			expected = "https://www.example.net:8080/";
			actual = OAuthHelper_Accessor.GetBaseStringUri(uri);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for GenerateTimeStamp
		///</summary>
		[TestMethod()]
		[DeploymentItem("OAuthClient.dll")]
		public void GenerateTimeStampTest()
		{
			string actual;
			actual = OAuthHelper_Accessor.GenerateUTCTimestamp();
			bool match = System.Text.RegularExpressions.Regex.IsMatch(actual, "^[0-9]+$");
			Assert.IsTrue(match);
		}

		/// <summary>
		///A test for GetNormalizedParameters
		///</summary>
		[TestMethod()]
		[DeploymentItem("OAuthClient.dll")]
		public void GetNormalizedParametersTest()
		{
			Uri requestUri = new Uri("http://example.com/request?b5=%3D%253D&a3=a&c%40=&a2=r%20b&c2&a3=2+q");
			var oauth_consumer_key = new QueryParameter(OAuthParameter.ConsumerKey, "9djdj82h48djs9d2");
			var oauth_token = new QueryParameter(OAuthParameter.Token, "kkk9d7dh3k39sjv7");
			var oauth_timestamp = new QueryParameter(OAuthParameter.Timestamp, "137131201");
			var oauth_signature_method = new QueryParameter(OAuthParameter.SignatureMethod, "HMAC-SHA1");
			var oauth_nonce = new QueryParameter(OAuthParameter.Nonce, "7d8f3e4a");
			string expected = "a2=r%20b&a3=2%20q&a3=a&b5=%3D%253D&c%40=&c2=&oauth_consumer_key=9djdj82h48djs9d2&oauth_nonce=7d8f3e4a&oauth_signature_method=HMAC-SHA1&oauth_timestamp=137131201&oauth_token=kkk9d7dh3k39sjv7";
			var parameters = OAuthHelper_Accessor.GetParametersFromUri(requestUri);
			parameters.Add(oauth_consumer_key);
			parameters.Add(oauth_token);
			parameters.Add(oauth_timestamp);
			parameters.Add(oauth_signature_method);
			parameters.Add(oauth_nonce);
			var actual = OAuthHelper_Accessor.NormalizeParameters(parameters);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for GetSignatureBaseString
		///</summary>
		[TestMethod()]
		[DeploymentItem("OAuthClient.dll")]
		public void GetSignatureBaseStringTest()
		{
			Uri requestUri = new Uri("http://example.com/request?b5=%3D%253D&a3=a&c%40=&a2=r%20b");
			string body = "c2&a3=2+q";
			var parsedBody = OAuthHelper.ParseParameters(body);
			var oauth_consumer_key = new QueryParameter(OAuthParameter.ConsumerKey, "9djdj82h48djs9d2");
			var oauth_token = new QueryParameter(OAuthParameter.Token, "kkk9d7dh3k39sjv7");
			var oauth_timestamp = new QueryParameter(OAuthParameter.Timestamp, "137131201");
			var oauth_signature_method = new QueryParameter(OAuthParameter.SignatureMethod, "HMAC-SHA1");
			var oauth_nonce = new QueryParameter(OAuthParameter.Nonce, "7d8f3e4a");
			var parametersList = parsedBody;
			parametersList.Add(oauth_consumer_key);
			parametersList.Add(oauth_token);
			parametersList.Add(oauth_timestamp);
			parametersList.Add(oauth_signature_method);
			parametersList.Add(oauth_nonce);
			string expected = "POST&http%3A%2F%2Fexample.com%2Frequest&a2%3Dr%2520b%26a3%3D2%2520q%26a3%3Da%26b5%3D%253D%25253D%26c%2540%3D%26c2%3D%26oauth_consumer_key%3D9djdj82h48djs9d2%26oauth_nonce%3D7d8f3e4a%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D137131201%26oauth_token%3Dkkk9d7dh3k39sjv7";
			string actual;
			actual = OAuthHelper_Accessor.GetSignatureBaseString("POST", requestUri, parametersList);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for GetAuthorizationHeaderValue
		///</summary>
		[TestMethod()]
		public void GetAuthorizationHeaderValueTest()
		{
			var parameters = new List<QueryParameter>();
			parameters.Add(new QueryParameter("realm", "Example"));
			parameters.Add(new QueryParameter("oauth_consumer_key", "0685bd9184jfhq22"));
			parameters.Add(new QueryParameter("oauth_token", "ad180jjd733klru7"));

			string expected = @"OAuth realm=""Example"",oauth_consumer_key=""0685bd9184jfhq22"",oauth_token=""ad180jjd733klru7""";
			string actual;
			actual = OAuthHelper.GetAuthorizationHeaderValue(parameters);
			Assert.AreEqual(expected, actual);
		}
	}
}
