Implementation based on the rfc5849, which provides an
informational documentation of OAuth Core 1.0 Revision A.

The original community specification used a somewhat different
terminology which maps as follows to the rfc:

	New RFC Terms		|	Old community specification terms
-------------------------------------------------------------
client					|	Consumer
server					|	Service Provider
resource owner			|	User
client credentials		|	Consumer Key and Secret
temporary credentials	|	Request Token and Secret
token credentials		|	Access Token and Secret

credentials: combination of an identifier and a shared-secret.

02/2013 - Melvyn Laïly.