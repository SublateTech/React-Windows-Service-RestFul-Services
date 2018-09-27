using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TcpLib;
using System.Text;
using System.IO.Ports;

namespace TcpServerDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm: System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnClose = new System.Windows.Forms.Button();
            this.txtTrama = new System.Windows.Forms.TextBox();
            this.b_send = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_IP = new System.Windows.Forms.RadioButton();
            this.rbSerial = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(422, 80);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtTrama
            // 
            this.txtTrama.Location = new System.Drawing.Point(22, 15);
            this.txtTrama.Name = "txtTrama";
            this.txtTrama.Size = new System.Drawing.Size(454, 20);
            this.txtTrama.TabIndex = 1;
            // 
            // b_send
            // 
            this.b_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_send.Location = new System.Drawing.Point(22, 41);
            this.b_send.Name = "b_send";
            this.b_send.Size = new System.Drawing.Size(75, 23);
            this.b_send.TabIndex = 2;
            this.b_send.Text = "Send";
            this.b_send.Click += new System.EventHandler(this.b_send_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSerial);
            this.groupBox1.Controls.Add(this.rb_IP);
            this.groupBox1.Location = new System.Drawing.Point(155, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 56);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vía";
            // 
            // rb_IP
            // 
            this.rb_IP.AutoSize = true;
            this.rb_IP.Checked = true;
            this.rb_IP.Location = new System.Drawing.Point(35, 11);
            this.rb_IP.Name = "rb_IP";
            this.rb_IP.Size = new System.Drawing.Size(61, 17);
            this.rb_IP.TabIndex = 0;
            this.rb_IP.TabStop = true;
            this.rb_IP.Text = "TCP/IP";
            this.rb_IP.UseVisualStyleBackColor = true;
            // 
            // rbSerial
            // 
            this.rbSerial.AutoSize = true;
            this.rbSerial.Location = new System.Drawing.Point(35, 31);
            this.rbSerial.Name = "rbSerial";
            this.rbSerial.Size = new System.Drawing.Size(51, 17);
            this.rbSerial.TabIndex = 1;
            this.rbSerial.Text = "Serial";
            this.rbSerial.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(506, 110);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.b_send);
            this.Controls.Add(this.txtTrama);
            this.Controls.Add(this.btnClose);
            this.Name = "MainForm";
            this.Text = "TcpServer";
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}


		private TcpServer Server;
		private System.Windows.Forms.Button btnClose;
        private TextBox txtTrama;
        private Button b_send;
        private GroupBox groupBox1;
        private RadioButton rbSerial;
        private RadioButton rb_IP;
        private TarificadorServiceProvider Provider;
        

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			Provider = new TarificadorServiceProvider();
			Server = new TcpServer(Provider, 2300);
			Server.Start();
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void MainForm_Closed(object sender, System.EventArgs e)
		{
			Server.Stop();
		}

        private void b_send_Click(object sender, EventArgs e)
        {
            String _receivedStr = txtTrama.Text + "\n";
            if (rb_IP.Checked)
            {
                if (Server._connections.Count == 0)
                    MessageBox.Show("No tiene conexiones activas.");
                else
                {
                    TcpLib.ConnectionState cs = (TcpLib.ConnectionState)Server._connections[Server._connections.Count - 1];
                    cs.Write(Encoding.UTF8.GetBytes(_receivedStr), 0,
                        _receivedStr.Length);
                    _receivedStr = "";
                }
            }
            if (rbSerial.Checked)
            {
                SerialPort port = new SerialPort("COM5");

                port.BaudRate = 1200;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.RtsEnable = true;

                port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                port.Open();

                // Write a string
                port.Write(_receivedStr);

                // Write a set of bytes
                //port.Write(new byte[] { 0x0A, 0xE2, 0xFF }, 0, 3);

                port.Close();
            }
        }

        private  void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.WriteLine("Data Received:");
                Console.Write(indata);
            }



        }
}
