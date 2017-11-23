using System.Collections.Generic;
using System.IO;

namespace DirSync
{
	public class SyncList
	{
		public IList<Pair<DirectoryInfo>> dirPairs;
		public IList<Pair<FileInfo>> filePairs;
		
		public SyncList()
		{
			dirPairs = new List<Pair<DirectoryInfo>>(64);
			filePairs = new List<Pair<FileInfo>>(256);
		}
	}
}
