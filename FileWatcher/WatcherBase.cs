using FileWatcherLib.Events;
using System.IO;
using System.Threading;

namespace FileWatcherLib
{
	public abstract class WatcherBase
	{
		private FilesEventHandler fileHandler;
		private Thread checkForReadyFilesThread;

		private string rootDirectory;

		public WatcherBase(string rootDirectory)
		{
			RootDirectory = rootDirectory;

			Init();
		}

		public string RootDirectory
		{
			get { return rootDirectory; }
			set { rootDirectory = value; }
		}

		protected Thread CheckForReadyFilesThread
		{
			get { return checkForReadyFilesThread; }
			set { checkForReadyFilesThread = value; }
		}

		protected FilesEventHandler FileHandler
		{
			get { return fileHandler; }
			set { fileHandler = value; }
		}

		protected void Init()
		{
			RootDirectory = rootDirectory;
			FileWatcher watcher = new FileWatcher(rootDirectory);
			watcher.OnFileChanged += OnFileChanged;

			fileHandler = new FilesEventHandler();
			fileHandler.OnFileReady += OnFileReady;

			CheckForReadyFilesThread = new Thread(new ThreadStart(fileHandler.CheckForReadyFilesThread));
			CheckForReadyFilesThread.Start();
		}

		protected virtual void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
		{
			if (ShouldIgnore(eventArgs))
			{
				return;
			}

			FileHandler.AddFile(eventArgs.FullPath);
			CheckForReadyFilesThread.Interrupt();
		}

		protected virtual bool ShouldIgnore(FileSystemEventArgs eventArgs)
		{
			bool isDir = (File.GetAttributes(eventArgs.FullPath) & FileAttributes.Directory) == FileAttributes.Directory;

			if (isDir)
			{
				return true;
			}

			return false;
		}

		protected abstract void OnFileReady(object sender, FileReadyEventArgs e);
	}
}
