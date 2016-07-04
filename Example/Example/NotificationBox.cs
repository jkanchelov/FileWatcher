using System;
using System.Windows.Forms;

namespace Example
{
	public partial class NotificationBox : Form
	{
		private System.ComponentModel.IContainer components = null;

		private RichTextBox textBox;
		private string formText;

		private Timer timer;
		private int closeTime;


		public NotificationBox(string text,int closingTime = 5000)
		{
			formText = text;
			closeTime = closingTime;

			InitializeComponent();
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.textBox.Font = new System.Drawing.Font("Migdebuli", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)), true);
			this.textBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.textBox.Location = new System.Drawing.Point(1, 1);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(784, 253);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "Example";
			// 
			// NotificationForm
			// 
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ClientSize = new System.Drawing.Size(784, 252);
			this.Controls.Add(this.textBox);
			this.Name = "NotificationForm";
			this.Text = "NotificationBox";
			this.Load += new System.EventHandler(this.MessageForm_Load);
			this.ResumeLayout(false);

		}

		private void MessageForm_Load(object sender, EventArgs e)
		{
			textBox.Text = formText;

			this.Focus();
			this.Activate();

			this.timer.Enabled = true;
			this.timer.Interval = closeTime;
			this.timer.Tick += new EventHandler(this.timer1_Tick);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
