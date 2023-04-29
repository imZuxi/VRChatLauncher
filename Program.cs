using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using BestHTTP.JSON;
using Microsoft.Win32;

namespace launch
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Main(string[] args)
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\EasyAntiCheat_EOS", "ProductsInstalled", "null");
			object obj;
			if (((Dictionary<string, object>)Json.Decode(File.ReadAllText(baseDirectory + "\\EasyAntiCheat\\Settings.json"))).TryGetValue("productid", out obj))
			{
				string text2 = obj as string;
				if (text2 != null)
				{
					if (!ServiceController.GetServices().Any((ServiceController x) => x.ServiceName == "EasyAntiCheat_EOS") || text == null || !text.Contains(text2))
					{
						Process process = new Process();
						process.StartInfo.FileName = baseDirectory + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe";
						process.StartInfo.WorkingDirectory = baseDirectory;
						process.StartInfo.Arguments = "install " + text2;
						process.Start();
						process.WaitForExit();
					}
                    string ProcessStartARGS = string.Join(" ", args);
                    Process StartEACProcess = new Process();
                    StartEACProcess.StartInfo.FileName = "start_protected_game.exe";
                    StartEACProcess.StartInfo.WorkingDirectory = baseDirectory;
                    StartEACProcess.StartInfo.Arguments = ProcessStartARGS;
                    StartEACProcess.Start();

                    return; 
					IntPtr? intPtr = null;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					foreach (string text3 in args)
					{
						if (text3.StartsWith("--affinity="))
						{
							string[] array = text3.Split(new char[]
							{
								'='
							});
							if (array.Length == 2)
							{
								string text4 = array[1].Trim(new char[]
								{
									'"'
								}).Trim();
								flag3 = true;
								if (text4.StartsWith("0x"))
								{
									text4 = text4.Substring(2);
								}
								ulong value;
								if (ulong.TryParse(text4, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
								{
									intPtr = new IntPtr?((IntPtr)((long)value));
									break;
								}
							}
						}
					}
					if (!flag3)
					{
						string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp/VRChat/VRChat");
						string text5 = null;
						if (File.Exists(Path.Combine(path, "settings_com.amplitude")))
						{
							try
							{
								text5 = (string)((Dictionary<string, object>)Json.Decode(File.ReadAllText(Path.Combine(path, "settings_com.amplitude"))))["com.amplitude_userId"];
							}
							catch (Exception)
							{
							}
						}
						if (text5 != null)
						{
							HttpClient httpClient = new HttpClient();
							string str = "client-bD50OiwphEUxwPTBQbUUIwhSTTDfCGdB";
							httpClient.DefaultRequestHeaders.Add("Authorization", "Api-Key " + str);
							Task<HttpResponseMessage> async = httpClient.GetAsync("https://api.lab.amplitude.com/v1/vardata?&user_id=" + "ANON" + "&flag_key=ct-processor-affinity");
							if (async.Wait(2000))
							{
								HttpResponseMessage result = async.Result;
								if (result.IsSuccessStatusCode)
								{
									Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Decode(result.Content.ReadAsStringAsync().Result);
									try
									{
										flag = ((string)((Dictionary<string, object>)dictionary["ct-processor-affinity"])["key"] != "control");
										flag2 = ((string)((Dictionary<string, object>)dictionary["ct-processor-affinity"])["key"] == "second-ccx-affinity");
									}
									catch (Exception)
									{
									}
								}
							}
						}
						object value2 = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0\\").GetValue("ProcessorNameString");
						if (value2.ToString().Contains("X3D"))
						{
							flag2 = false;
						}
						if (value2 != null && flag)
						{
							if (value2.ToString().Contains("AMD Ryzen 5 1600 ") || value2.ToString().Contains("AMD Ryzen 5 1600X ") || value2.ToString().Contains("AMD Ryzen Threadripper 1920X ") || value2.ToString().Contains("AMD Ryzen 5 1600 (AF) ") || value2.ToString().Contains("AMD Ryzen 5 2600E ") || value2.ToString().Contains("AMD Ryzen 5 2600 ") || value2.ToString().Contains("AMD Ryzen 5 2600X ") || value2.ToString().Contains("AMD Ryzen Threadripper 2920X ") || value2.ToString().Contains("AMD Ryzen Threadripper 2970WX ") || value2.ToString().Contains("AMD Ryzen 5 3500 ") || value2.ToString().Contains("AMD Ryzen 5 3500X ") || value2.ToString().Contains("AMD Ryzen 5 3600 ") || value2.ToString().Contains("AMD Ryzen 5 3600X ") || value2.ToString().Contains("AMD Ryzen 5 3600XT ") || value2.ToString().Contains("AMD Ryzen 9 3900 ") || value2.ToString().Contains("AMD Ryzen 9 3900X ") || value2.ToString().Contains("AMD Ryzen 9 3900XT ") || value2.ToString().Contains("AMD Ryzen Threadripper 3960X ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 3945WX ") || value2.ToString().Contains("AMD Ryzen 5 4600GE ") || value2.ToString().Contains("AMD Ryzen 5 4600G ") || value2.ToString().Contains("AMD Ryzen 5 4500U ") || value2.ToString().Contains("AMD Ryzen 5 4600U ") || value2.ToString().Contains("AMD Ryzen 5 4680U ") || value2.ToString().Contains("AMD Ryzen 5 4600HS ") || value2.ToString().Contains("AMD Ryzen 5 4600H ") || value2.ToString().Contains("AMD Ryzen 5 5500U "))
							{
								intPtr = new IntPtr?(new IntPtr(63));
								if (flag2)
								{
									intPtr = new IntPtr?(new IntPtr(intPtr.Value.ToInt64() << 6));
								}
							}
							else if (value2.ToString().Contains("AMD Ryzen 7 1700 ") || value2.ToString().Contains("AMD Ryzen 7 1700X ") || value2.ToString().Contains("AMD Ryzen 7 1800X ") || value2.ToString().Contains("AMD Ryzen Threadripper 1900X ") || value2.ToString().Contains("AMD Ryzen Threadripper 1950X ") || value2.ToString().Contains("AMD Ryzen 7 2700E ") || value2.ToString().Contains("AMD Ryzen 7 2700 ") || value2.ToString().Contains("AMD Ryzen 7 2700X ") || value2.ToString().Contains("AMD Ryzen Threadripper 2950X ") || value2.ToString().Contains("AMD Ryzen Threadripper 2990WX ") || value2.ToString().Contains("AMD Ryzen 7 3700X ") || value2.ToString().Contains("AMD Ryzen 7 3800X ") || value2.ToString().Contains("AMD Ryzen 7 3800XT ") || value2.ToString().Contains("AMD Ryzen 9 3950X ") || value2.ToString().Contains("AMD Ryzen Threadripper 3970X ") || value2.ToString().Contains("AMD Ryzen Threadripper 3990X ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 3955WX ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 3975WX ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 3995WX ") || value2.ToString().Contains("AMD Ryzen 7 4700GE ") || value2.ToString().Contains("AMD Ryzen 7 4700G ") || value2.ToString().Contains("AMD Ryzen 7 4700U ") || value2.ToString().Contains("AMD Ryzen 7 4800U ") || value2.ToString().Contains("AMD Ryzen 7 4800H ") || value2.ToString().Contains("AMD Ryzen 7 4800HS ") || value2.ToString().Contains("AMD Ryzen 7 4980U ") || value2.ToString().Contains("AMD Ryzen 9 4900HS ") || value2.ToString().Contains("AMD Ryzen 9 4900H ") || value2.ToString().Contains("AMD Ryzen 7 5700U ") || value2.ToString().Contains("AMD Ryzen 3 7330U ") || value2.ToString().Contains("AMD Ryzen 3 7335U "))
							{
								intPtr = new IntPtr?(new IntPtr(255));
								if (flag2)
								{
									intPtr = new IntPtr?(new IntPtr(intPtr.Value.ToInt64() << 8));
								}
							}
							else if (value2.ToString().Contains("AMD Ryzen 9 5900 ") || value2.ToString().Contains("AMD Ryzen 9 5900X ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 5945WX ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 5965WX ") || value2.ToString().Contains("AMD Ryzen 9 7900 ") || value2.ToString().Contains("AMD Ryzen 9 7900X ") || value2.ToString().Contains("AMD Ryzen 9 7900X3D ") || value2.ToString().Contains("AMD Ryzen 9 7845HX "))
							{
								intPtr = new IntPtr?(new IntPtr(4095));
								if (flag2)
								{
									intPtr = new IntPtr?(new IntPtr(intPtr.Value.ToInt64() << 12));
								}
							}
							else if (value2.ToString().Contains("AMD Ryzen 9 5950X ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 5995WX ") || value2.ToString().Contains("AMD Ryzen 9 7950X ") || value2.ToString().Contains("AMD Ryzen 9 7950X3D ") || value2.ToString().Contains("AMD Ryzen 9 7950HX ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 5955WX ") || value2.ToString().Contains("AMD Ryzen Threadripper PRO 5975WX "))
							{
								intPtr = new IntPtr?(new IntPtr(65535));
								if (flag2)
								{
									intPtr = new IntPtr?(new IntPtr(intPtr.Value.ToInt64() << 16));
								}
							}
						}
					}
					string text6 = string.Join(" ", args);
					if (flag3)
					{
						text6 += " --user-affinity";
					}
					else if (intPtr != null)
					{
						text6 = text6 + " --affinity=" + intPtr.Value.ToString("X");
					}
					Process process2 = new Process();
					process2.StartInfo.FileName = "start_protected_game.exe";
					process2.StartInfo.WorkingDirectory = baseDirectory;
					process2.StartInfo.Arguments = text6;
					process2.Start();
					try
					{
						if (intPtr != null)
						{
							process2.ProcessorAffinity = intPtr.Value;
						}
					}
					catch (Win32Exception)
					{
					}
					return;
				}
			}
			throw new Exception("ProductID parse error");
		}
	}
}
