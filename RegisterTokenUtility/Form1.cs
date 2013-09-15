using OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterTokenUtility
{
	public partial class Form1 : Form
	{

		private OAuthClient oauth;

		public Form1()
		{
			InitializeComponent();
		}

		private void btnRequestTemporaryCredentials_Click(object sender, EventArgs e)
		{
			try
			{
				oauth = new OAuthClient(
					txtClientIdentifier.Text.Trim(),
					txtClientSharedSecret.Text.Trim(),
					txtRequestTokenUrl.Text.Trim(),
					txtAuthorizeUrl.Text.Trim(),
					txtAccessTokenUrl.Text.Trim());
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occured while creating an OAuth object. you must provide valid informations!", "Error");
			}

			try
			{
				oauth.RequestTemporaryCredentials(txtCallbackUrl.Text.Trim());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "An error occured while requesting temporary credentials!");
			}

			try
			{
				txtAuthorizationRequestUrl.Text = oauth.GetAuthorizationUri();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occured while requesting the authorization uri!\n" + ex.ToString(), "Error");
			}
		}

		private void btnRequestTokenCredentials_Click(object sender, EventArgs e)
		{
			try
			{
				oauth.RequestTokenCredentials(txtPinOrCallbackUrl.Text.Trim());
				txtTokenIdentifier.Text = oauth.TokenCredentials.Identifier;
				txtSharedSecret.Text = oauth.TokenCredentials.SharedSecret;
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occured while requesting the final token credentials!\n" + ex.ToString(), "Error");
			}
		}
	}
}
