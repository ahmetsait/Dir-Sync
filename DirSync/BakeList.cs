using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirSync
{
	public class BakeList
	{
		public List<Tuple<DirectoryInfo, DirectoryInfo, SyncAction>> directories;
		public List<Tuple<FileInfo, FileInfo, SyncAction>> files;

		public BakeList()
		{
			directories = new List<Tuple<DirectoryInfo, DirectoryInfo, SyncAction>>(64);
			files = new List<Tuple<FileInfo, FileInfo, SyncAction>>(256);
		}
	}
}
