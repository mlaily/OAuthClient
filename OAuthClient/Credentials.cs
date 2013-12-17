using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth
{
	/// <summary>
	/// Represents a set of credentials, made of an identifier and a shared-secret.
	/// </summary>
	public abstract class Credentials
	{
		public string Identifier { get; protected set; }
		public string SharedSecret { get; protected set; }

		public Credentials(string identifier, string sharedSecret)
		{
			this.Identifier = identifier;
			this.SharedSecret = sharedSecret;
		}
	}

	public class TemporaryCredentials : Credentials
	{
		public TemporaryCredentials(string identifier, string sharedSecret) : base(identifier, sharedSecret) { }
	}
	public class ClientCredentials : Credentials
	{
		public ClientCredentials(string identifier, string sharedSecret) : base(identifier, sharedSecret) { }
	}
	public class TokenCredentials : Credentials
	{
		public TokenCredentials(string identifier, string sharedSecret) : base(identifier, sharedSecret) { }
	}
}
