using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirSync
{
	public struct Pair<T>
	{
		public T Item1, Item2;

		public Pair(T item1, T item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}
	}
}
