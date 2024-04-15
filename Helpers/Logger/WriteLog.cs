using Microsoft.Extensions.Configuration;

namespace NS_callLogService.Util
{
	public class LogWrite
	{
		private static IConfiguration _configuration;
		private string folderPath = string.Empty;
		private string subfolderPath = string.Empty;
		private string fileName = string.Empty;
		private string filePath = string.Empty;

		public LogWrite(IConfiguration configuration)
		{
			_configuration = configuration;

			if(_configuration is not null)
			{
				setupLogDirectories(_configuration);
			}
		}
		public void WriteLog(string Message)
		{
			try
			{
				if (!string.IsNullOrEmpty(Message))
				{
					Console.WriteLine(Message);
					
					using (StreamWriter sw = new StreamWriter(filePath, true))
					{
						sw.WriteLine($"[{DateTime.Now.ToString()}] : {Message}");
						sw.Flush();
					};
				}
				else
				{
					throw new Exception("Empty message string in while writing log");
				}
			}
			catch (Exception e)
			{
				/*if (Environment.OSVersion.ToString().Contains("Windows"))
                {
                    string mac = Environment.MachineName;
                    string message = $"----{DateTime.Now.ToString()}------\n {e}";
                    using EventLog eventLog = new EventLog("Application");
                    eventLog.Source = "Lakshya_WS_RC_CallRecordings";
                    eventLog.WriteEntry(message, EventLogEntryType.Error);
                }*/
				Console.Out.WriteLine($"FATAL ERROR: {e}");
				Console.WriteLine($"FATAL ERROR: {e}");
				Environment.ExitCode = -1;
			}
		}

		private void setupLogDirectories(IConfiguration _configuration)
		{
			try
			{
				// Set up the Log Folders from Config files
				folderPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetSection("LogFolderName").Value);
				subfolderPath = Path.Join(folderPath, DateTime.Now.ToString("yyyy_MM_dd"));
				fileName = _configuration.GetSection("CallLogServiceLogFileName").Value.Replace("{date}", DateTime.Now.ToString("yyyy_MMM_dd"));

				filePath = Path.Join(subfolderPath, fileName);
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
				if (!Directory.Exists(subfolderPath))
				{
					Directory.CreateDirectory(subfolderPath);
				}
			}
			catch (Exception e)
			{
				Console.Out.WriteLine($"ERROR while creating log folder setup : {e}");
				Console.WriteLine($"FATAL ERROR: {e}");
			}
		}
	}
}
