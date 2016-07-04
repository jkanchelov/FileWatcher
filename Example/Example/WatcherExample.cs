using FileWatcherLib;
using FileWatcherLib.Events;

namespace Example
{
	class WatcherExample : WatcherBase
	{
		public WatcherExample(string rootDirectory) : base(rootDirectory)
		{
		}

		protected override void OnFileReady(object sender, FileReadyEventArgs e)
		{
			CallMessageForm(string.Format("File changed : {0}\nFilePath: {1}",e.FileName,e.FileFullPath));
		}

		private void CallMessageForm(string text, int closeTime = 5000)
		{
			using (var messageForm = new NotificationBox(text, closeTime))
			{
				messageForm.TopMost = true;
				messageForm.BringToFront();
				messageForm.ShowDialog();
				messageForm.Focus();
				messageForm.Activate();
			}
		}
	}
}
