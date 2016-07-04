using System;

namespace FileWatcherLib.Events
{
	public class FileReadyEventArgs : EventArgs
	{
		string fileName = string.Empty;
		string fileFullPath = string.Empty;

		public FileReadyEventArgs(string fileName)
		{
			FileName = fileName;
		}

		public FileReadyEventArgs(string fileName, string fileFullPath) : this(fileName)
		{
			FileFullPath = fileFullPath;
		}

		public string FileName
		{
			get { return fileName; }

			private set
			{
				fileName = value;
			}
		}

		public string FileFullPath
		{
			get { return fileFullPath; }
			set { fileFullPath = value; }
		}
	}
}
