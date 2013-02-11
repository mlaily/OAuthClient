This is my .Net implementation of OAuth.
I strictly followed the rfc5848, and everything that is implemented
should be done the right way (regarding the rfc)...
Some parts are, and will probably stay, not implemented.
(RSA signature...)

This implementation seems quite usable to me, though it would
probably need a bit more of some proof testing...

Though I like my code as it is, some refactoring would also be a good thing.

---

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