namespace UnityServerWFA
{
    partial class ServerForm
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
            this.lblHeaderInfo = new System.Windows.Forms.Label();
            this.lblHeaderWarning = new System.Windows.Forms.Label();
            this.lblHeaderError = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnClientConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeaderInfo
            // 
            this.lblHeaderInfo.AutoSize = true;
            this.lblHeaderInfo.Location = new System.Drawing.Point(110, 97);
            this.lblHeaderInfo.Name = "lblHeaderInfo";
            this.lblHeaderInfo.Size = new System.Drawing.Size(46, 13);
            this.lblHeaderInfo.TabIndex = 0;
            this.lblHeaderInfo.Text = "Info Log";
            // 
            // lblHeaderWarning
            // 
            this.lblHeaderWarning.AutoSize = true;
            this.lblHeaderWarning.Location = new System.Drawing.Point(110, 139);
            this.lblHeaderWarning.Name = "lblHeaderWarning";
            this.lblHeaderWarning.Size = new System.Drawing.Size(68, 13);
            this.lblHeaderWarning.TabIndex = 1;
            this.lblHeaderWarning.Text = "Warning Log";
            // 
            // lblHeaderError
            // 
            this.lblHeaderError.AutoSize = true;
            this.lblHeaderError.Location = new System.Drawing.Point(110, 184);
            this.lblHeaderError.Name = "lblHeaderError";
            this.lblHeaderError.Size = new System.Drawing.Size(50, 13);
            this.lblHeaderError.TabIndex = 2;
            this.lblHeaderError.Text = "Error Log";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(205, 97);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(36, 13);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "Empty";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(205, 139);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(36, 13);
            this.lblWarning.TabIndex = 4;
            this.lblWarning.Text = "Empty";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(205, 184);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(36, 13);
            this.lblError.TabIndex = 5;
            this.lblError.Text = "Empty";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(113, 39);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 6;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(208, 39);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(75, 23);
            this.btnStopServer.TabIndex = 7;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // btnClientConnect
            // 
            this.btnClientConnect.Location = new System.Drawing.Point(319, 38);
            this.btnClientConnect.Name = "btnClientConnect";
            this.btnClientConnect.Size = new System.Drawing.Size(84, 23);
            this.btnClientConnect.TabIndex = 8;
            this.btnClientConnect.Text = "Client Connect";
            this.btnClientConnect.UseVisualStyleBackColor = true;
            this.btnClientConnect.Click += new System.EventHandler(this.btnClientConnect_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClientConnect);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblHeaderError);
            this.Controls.Add(this.lblHeaderWarning);
            this.Controls.Add(this.lblHeaderInfo);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeaderInfo;
        private System.Windows.Forms.Label lblHeaderWarning;
        private System.Windows.Forms.Label lblHeaderError;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnClientConnect;
    }
}