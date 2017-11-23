using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirSync
{
	internal sealed class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public static readonly string exeDir = Path.GetDirectoryName(Application.ExecutablePath),
			appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			configPath = Path.Combine(appData, "DirSync\\path.ini");
	}
}
