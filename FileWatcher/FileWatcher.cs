using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;

namespace FileWatcherLib
{
	public class FileWatcher : IDisposable
	{
		private static List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
		private static bool isWorking;

		public delegate void FileChanged(object sender, FileSystemEventArgs e);
		public event FileChanged OnFileChanged;

		private string rootDir;
		private FileSystemWatcher watcher;


		public FileWatcher(string watcherDirectory)
		{
			IsWorking = true;

			rootDir = watcherDirectory;
			startWatching();
		}

		public FileWatcher(string watcherDirectory, string watchFilter)
		{
			rootDir = watcherDirectory;
			startWatching();

			WatchExtensions = watchFilter;
		}

		public string WatchExtensions
		{
			get { return Watcher.Filter; }

			set
			{
				if (value[0] != '.' || value.Length < 2)
				{
					throw new InvalidDataException("Invalid file extension");
				}

				Watcher.Filter = value;
			}
		}

		public static bool IsWorking
		{
			get { return isWorking; }

			set
			{
				isWorking = value;
				UpdateWatchersStatus(IsWorking);
			}
		}

		public FileSystemWatcher Watcher { get; set; }

		public void Dispose()
		{
			rootDir = null;

			if (watchers.Contains(Watcher))
			{
				watchers.Remove(Watcher);
			}

			Watcher.Dispose();
			Watcher = null;
		}

		private static void UpdateWatchersStatus(bool isWorking)
		{
			foreach (FileSystemWatcher watcher in watchers)
			{
				watcher.EnableRaisingEvents = isWorking;
			}
		}

		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void startWatching()
		{
			Watcher = new FileSystemWatcher(rootDir);
			Watcher.InternalBufferSize = 2621440;
			Watcher.EnableRaisingEvents = true;
			Watcher.IncludeSubdirectories = true;
			Watcher.NotifyFilter = NotifyFilters.Size; //| NotifyFilters.FileName | NotifyFilters.Size;

			watchers.Add(Watcher);

			if (WatchExtensions != null)
			{
				Watcher.Filter = WatchExtensions;
			}

			Watcher.Changed += new FileSystemEventHandler(FileChange);
		}

		private void FileChange(object sender, FileSystemEventArgs e)
		{
			OnFileChanged(this, e);
		}
	}
}
