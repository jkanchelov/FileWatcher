using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Example
{
	class ExampleTray : Form
	{
		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;
		private Thread watcherProcess;

		private System.ComponentModel.IContainer components;

		public ExampleTray()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Exit", OnExit);

			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExampleTray));
			trayIcon = new NotifyIcon();

			trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("Example.Icon")));
			trayIcon.Text = "Example";
			trayIcon.ContextMenu = trayMenu;

			trayIcon.Visible = true;
		}


		private void OnExit(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		protected override void OnLoad(EventArgs e)
		{
			Visible = false;
			ShowInTaskbar = false;

			watcherProcess = new Thread(() => new WatcherExample(Directory.GetCurrentDirectory() + @"\targetFolder"));
			watcherProcess.Start();

			base.OnLoad(e);
		}
	}
}
