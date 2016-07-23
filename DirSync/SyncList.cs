using System.Collections.Generic;
using System.IO;

namespace DirSync
{
	public class SyncList
	{
		public IList<Pair<FileInfo>> filePairs;
		public IList<Pair<DirectoryInfo>> dirPairs;
		
		public SyncList()
		{
			filePairs = new List<Pair<FileInfo>>(256);
			dirPairs = new List<Pair<DirectoryInfo>>(64);
		}
	}
}
