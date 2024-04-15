using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Claims;

namespace Helpers.Logging
{
	public class FileLogger : ILogger
	{
		protected readonly FileLoggerProvider _LoggerFileProvider;

		private string _categoryName;
		private Func<string, LogLevel, bool> _filter;
		private readonly IHttpContextAccessor _accessor;
		public FileLogger([NotNull] FileLoggerProvider LoggerFileProvider, string categoryName, Func<string, LogLevel, bool> filter, IHttpContextAccessor accessor)
		{
			_LoggerFileProvider = LoggerFileProvider;
			_categoryName = categoryName;
			_filter = filter;
			_accessor = accessor;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return (_filter == null || _filter(_categoryName, logLevel));
			//return logLevel != LogLevel.None;
		}
		// formatter delegate just prints the Exception Message(e.Message)


		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			try
			{
				if (!IsEnabled(logLevel))
				{
					return;
				}

				var fileName = GetCurrentFileName(_LoggerFileProvider.Options.FilePath.Replace("{date}", DateTime.Now.ToString("yyyyMMdd")), _LoggerFileProvider.Options.FolderPath);
				var fullFilePath = Path.Join(_LoggerFileProvider.Options.FolderPath, fileName); // Get the full log file path. Seperated by day.
				var objects = getObjects(state);
				JObject jsonObject = new()
				{
					["Timestamp"] = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:f"),
					["Loglevel"] = logLevel.ToString()
				};
				LogSessionData(jsonObject);
				jsonObject["Message"] = formatter(state, exception);
				LogParameters(objects, jsonObject);
				LogException(exception, jsonObject);

				//var logRecord = string.Format("{0} [{1}] {2} \nLine Number:{3}\nInner Exception: {4} \nStack Trace: {5} \n Target Site:{6}\n----------------Entire Exception:---------\n{7}\n",
				//    "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "]", 
				//    logLevel.ToString(), 
				//    formatter(state, exception),
				//    lineNumber,
				//    innerException, 
				//    ExceptionStackTrace,
				//    TargetSite,
				//    Exception); // Format the log entry.

				// Write the log entry to the text file.
				using var streamWriter = new StreamWriter(fullFilePath, true);
				streamWriter.WriteLine(jsonObject.ToString() + ",");
			}
			catch (Exception e)
			{
				if (Environment.OSVersion.ToString().Contains("Windows"))
				{
					string mac = Environment.MachineName;
					string message = $"----{DateTime.Now.ToString()}------\n {e}";
					using EventLog eventLog = new EventLog("Application");
					eventLog.Source = "Application";
					eventLog.WriteEntry(message, EventLogEntryType.Information);
				}
			}
		}

		private static void LogParameters(object[] objects, JObject jsonObject)
		{
			if (objects != null)
			{
				if (objects.Length > 0)
				{
					JArray objectsArray = new();
					foreach (var o in objects)
					{
						objectsArray.Add(JToken.FromObject(o));
					}
					jsonObject["Parameters passed:"] = objectsArray;
				}
			}
		}

		private void LogSessionData(JObject jsonObject)
		{
			if (_accessor.HttpContext != null) // you should check HttpContext 
			{
				string session_user = _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
				string session_user_name = _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
				if (session_user != null)
				{
					jsonObject["Session User"] = $"{session_user_name} ({session_user})";
				}
				jsonObject["Path"] = _accessor.HttpContext.Request.Path.ToString();
			}
		}

		private static void LogException(Exception exception, JObject jsonObject)
		{
			if (exception != null)
			{
				var Exception = exception.ToString();
				var TargetSite = exception.TargetSite.ToString();
				var innerException = exception.InnerException != null ? exception.InnerException.ToString() : "No inner exception generated";
				var ExceptionStackTrace = exception.StackTrace != null ? exception.StackTrace : "No stack trace was generated";

				var st = new StackTrace(exception, true);
				// Get the top stack frame
				var frame = st.GetFrame(st.FrameCount - 1);
				// Get the line number from the stack frame
				var lineNumber = frame.GetFileLineNumber().ToString();
				jsonObject["Line number"] = lineNumber;
				jsonObject["Inner Exception"] = exception.InnerException != null ? exception.InnerException.ToString() : "No inner exception generated";
				jsonObject["Stack Trace"] = exception.StackTrace != null ? exception.StackTrace : "No stack trace was generated";
				jsonObject["Entire Exception"] = Exception;
			}
		}

		private static object[] getObjects<TState>(TState state)
		{
			FieldInfo[] fieldsInfo = state.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo props = fieldsInfo.FirstOrDefault(o => o.Name == "_values");
			return (object[])props?.GetValue(state);
		}

		/// <summary>
		/// This function returns new file name if the existing file name's size has exceeded a limit. (Creating a Rolling File)
		/// </summary>
		/// <param name="originalFileName"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public string GetCurrentFileName(string originalFileName, string path)
		{
			string fileName = originalFileName;
			string ext = Path.GetExtension(Path.Join(path, originalFileName));
			string InitialFileName = Path.GetFileNameWithoutExtension(Path.Join(path, originalFileName));
			int cnt = 0;
			do
			{
				if (File.Exists(Path.Join(path, fileName)))
				{
					var fileSize = (new FileInfo(Path.Join(path, fileName))).Length;
					if (fileSize > _LoggerFileProvider.Options.MaxFileSize && _LoggerFileProvider.Options.RollingFile == true)
					{
						cnt++;
						fileName = $"{InitialFileName}_{cnt}{ext}";
					}
					else
						break;
				}
				else
					break;
			} while (true);
			return fileName;
		}
	}

}

