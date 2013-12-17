namespace RegisterTokenUtility
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtRequestTokenUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtAuthorizeUrl = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAccessTokenUrl = new System.Windows.Forms.TextBox();
			this.btnRequestTemporaryCredentials = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txtAuthorizationRequestUrl = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtPinOrCallbackUrl = new System.Windows.Forms.TextBox();
			this.btnRequestTokenCredentials = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTokenIdentifier = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtSharedSecret = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtClientIdentifier = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtClientSharedSecret = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtCallbackUrl = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtRequestTokenUrl
			// 
			this.txtRequestTokenUrl.Location = new System.Drawing.Point(12, 25);
			this.txtRequestTokenUrl.Name = "txtRequestTokenUrl";
			this.txtRequestTokenUrl.Size = new System.Drawing.Size(446, 20);
			this.txtRequestTokenUrl.TabIndex = 0;
			this.txtRequestTokenUrl.Text = "http://www.tumblr.com/oauth/request_token";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(276, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Temporary Credentials Request URI (Request Token Uri)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(242, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Resource Owner Authorization URI (Authorize Uri)";
			// 
			// txtAuthorizeUrl
			// 
			this.txtAuthorizeUrl.Location = new System.Drawing.Point(12, 64);
			this.txtAuthorizeUrl.Name = "txtAuthorizeUrl";
			this.txtAuthorizeUrl.Size = new System.Drawing.Size(446, 20);
			this.txtAuthorizeUrl.TabIndex = 2;
			this.txtAuthorizeUrl.Text = "http://www.tumblr.com/oauth/authorize";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 87);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(197, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Token Request URI (Access Token Uri)";
			// 
			// txtAccessTokenUrl
			// 
			this.txtAccessTokenUrl.Location = new System.Drawing.Point(12, 103);
			this.txtAccessTokenUrl.Name = "txtAccessTokenUrl";
			this.txtAccessTokenUrl.Size = new System.Drawing.Size(446, 20);
			this.txtAccessTokenUrl.TabIndex = 4;
			this.txtAccessTokenUrl.Text = "http://www.tumblr.com/oauth/access_token\r\n";
			// 
			// btnRequestTemporaryCredentials
			// 
			this.btnRequestTemporaryCredentials.Location = new System.Drawing.Point(284, 274);
			this.btnRequestTemporaryCredentials.Name = "btnRequestTemporaryCredentials";
			this.btnRequestTemporaryCredentials.Size = new System.Drawing.Size(177, 23);
			this.btnRequestTemporaryCredentials.TabIndex = 11;
			this.btnRequestTemporaryCredentials.Text = "Request Temporary Credentials";
			this.btnRequestTemporaryCredentials.UseVisualStyleBackColor = true;
			this.btnRequestTemporaryCredentials.Click += new System.EventHandler(this.btnRequestTemporaryCredentials_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 303);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(127, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Authorization Request Uri";
			// 
			// txtAuthorizationRequestUrl
			// 
			this.txtAuthorizationRequestUrl.Location = new System.Drawing.Point(12, 319);
			this.txtAuthorizationRequestUrl.Name = "txtAuthorizationRequestUrl";
			this.txtAuthorizationRequestUrl.ReadOnly = true;
			this.txtAuthorizationRequestUrl.Size = new System.Drawing.Size(446, 20);
			this.txtAuthorizationRequestUrl.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 351);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(87, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "PIN/Callback Uri";
			// 
			// txtPinOrCallbackUrl
			// 
			this.txtPinOrCallbackUrl.Location = new System.Drawing.Point(12, 367);
			this.txtPinOrCallbackUrl.Name = "txtPinOrCallbackUrl";
			this.txtPinOrCallbackUrl.Size = new System.Drawing.Size(446, 20);
			this.txtPinOrCallbackUrl.TabIndex = 15;
			// 
			// btnRequestTokenCredentials
			// 
			this.btnRequestTokenCredentials.Location = new System.Drawing.Point(281, 398);
			this.btnRequestTokenCredentials.Name = "btnRequestTokenCredentials";
			this.btnRequestTokenCredentials.Size = new System.Drawing.Size(177, 23);
			this.btnRequestTokenCredentials.TabIndex = 16;
			this.btnRequestTokenCredentials.Text = "Request Token Credentials";
			this.btnRequestTokenCredentials.UseVisualStyleBackColor = true;
			this.btnRequestTokenCredentials.Click += new System.EventHandler(this.btnRequestTokenCredentials_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 430);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(155, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "Token Identifier/Access Token";
			// 
			// txtTokenIdentifier
			// 
			this.txtTokenIdentifier.Location = new System.Drawing.Point(12, 446);
			this.txtTokenIdentifier.Name = "txtTokenIdentifier";
			this.txtTokenIdentifier.ReadOnly = true;
			this.txtTokenIdentifier.Size = new System.Drawing.Size(446, 20);
			this.txtTokenIdentifier.TabIndex = 18;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 469);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(183, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Token Shared Secret/Access Secret";
			// 
			// txtSharedSecret
			// 
			this.txtSharedSecret.Location = new System.Drawing.Point(9, 485);
			this.txtSharedSecret.Name = "txtSharedSecret";
			this.txtSharedSecret.ReadOnly = true;
			this.txtSharedSecret.Size = new System.Drawing.Size(446, 20);
			this.txtSharedSecret.TabIndex = 20;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 188);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(190, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "Client Identifier/Consumer Key/Api Key";
			// 
			// txtClientIdentifier
			// 
			this.txtClientIdentifier.Location = new System.Drawing.Point(12, 204);
			this.txtClientIdentifier.Name = "txtClientIdentifier";
			this.txtClientIdentifier.Size = new System.Drawing.Size(446, 20);
			this.txtClientIdentifier.TabIndex = 8;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 227);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(244, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Client Shared Secret/Consumer Secret/Api Secret";
			// 
			// txtClientSharedSecret
			// 
			this.txtClientSharedSecret.Location = new System.Drawing.Point(12, 243);
			this.txtClientSharedSecret.Name = "txtClientSharedSecret";
			this.txtClientSharedSecret.Size = new System.Drawing.Size(446, 20);
			this.txtClientSharedSecret.TabIndex = 10;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 138);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(218, 13);
			this.label10.TabIndex = 5;
			this.label10.Text = "Callback Uri (set to \"oob\" if it does not apply)";
			// 
			// txtCallbackUrl
			// 
			this.txtCallbackUrl.Location = new System.Drawing.Point(12, 154);
			this.txtCallbackUrl.Name = "txtCallbackUrl";
			this.txtCallbackUrl.Size = new System.Drawing.Size(446, 20);
			this.txtCallbackUrl.TabIndex = 6;
			this.txtCallbackUrl.Text = "oob";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 518);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtCallbackUrl);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtClientSharedSecret);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtClientIdentifier);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtSharedSecret);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtTokenIdentifier);
			this.Controls.Add(this.btnRequestTokenCredentials);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtPinOrCallbackUrl);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtAuthorizationRequestUrl);
			this.Controls.Add(this.btnRequestTemporaryCredentials);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtAccessTokenUrl);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtAuthorizeUrl);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtRequestTokenUrl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Register Token Utility";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtRequestTokenUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtAuthorizeUrl;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAccessTokenUrl;
		private System.Windows.Forms.Button btnRequestTemporaryCredentials;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtAuthorizationRequestUrl;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtPinOrCallbackUrl;
		private System.Windows.Forms.Button btnRequestTokenCredentials;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtTokenIdentifier;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtSharedSecret;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtClientIdentifier;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtClientSharedSecret;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtCallbackUrl;
	}
}

