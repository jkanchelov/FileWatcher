using FileWatcherLib.Events;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace FileWatcherLib
{
	public class FilesEventHandler
	{
		private int TIME_WAITING_TO_SYNC_EVENTS = 250;

		public delegate void FileReady(object sender, FileReadyEventArgs e);
		public event FileReady OnFileReady;

		private HashSet<string> filesToHandle;
		private bool isWorking;

		System.Timers.Timer timer;
		private bool isTimerStarted;
		private bool isTimerWaitingForEventsCompleted;

		private string fileFullPath;

		public FilesEventHandler()
		{
			filesToHandle = new HashSet<string>();
			isTimerStarted = false;
			isTimerWaitingForEventsCompleted = false;

			IsWorking = false;
		}

		public bool IsWorking
		{
			private get { return isWorking; }

			set { isWorking = value; }
		}

		public int TIME_WAITING_FILES
		{
			get { return TIME_WAITING_TO_SYNC_EVENTS; }

			set { TIME_WAITING_TO_SYNC_EVENTS = value; }
		}

		public void CheckForReadyFilesThread()
		{
			while (true)
			{
				if (IsWorking)
				{
					if (isTimerWaitingForEventsCompleted)
					{
						HandleFiles();
					}
				}
				else
				{
					try { Thread.Sleep(Timeout.Infinite); } catch { }
				}
			}
		}

		public void AddFile(string fileName)
		{
			//changing the state to working
			IsWorking = true; 
			
			filesToHandle.Add(fileName);

			//if there ain't a timer to raise event
			if (isTimerStarted == false)
			{
				isTimerStarted = true;
				isTimerWaitingForEventsCompleted = false;

				timer = new System.Timers.Timer(TIME_WAITING_FILES);
				timer.Elapsed += new ElapsedEventHandler(onTimerComplete);
				timer.Enabled = true;
			}
		}

		private void onTimerComplete(object sender, ElapsedEventArgs e)
		{
			timer.Dispose();
			isTimerStarted = false;
			isTimerWaitingForEventsCompleted = true;
		}

		private void HandleFiles()
		{
			if (filesToHandle.Count != 0)
			{
				//get the set as array
				string[] setArray = new string[filesToHandle.Count];
				filesToHandle.CopyTo(setArray);

				//get the first value of the set and then delete it from the set
				fileFullPath = setArray[0];
				string fileName = fileFullPath.Substring(fileFullPath.LastIndexOf("\\") + 1);
				filesToHandle.Remove(fileFullPath);

				//dispatch fileReady event with the selected file
				FileReadyEventArgs readyItem = new FileReadyEventArgs(fileName,fileFullPath);
				OnFileReady(this,readyItem);
			}
			else
			{
				//there are no more files to handle
				IsWorking = false;
			}
		}
	}
}
