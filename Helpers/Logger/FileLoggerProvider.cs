using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Helpers.Logging
{
	[ProviderAlias("LogFile")]
	public class FileLoggerProvider : ILoggerProvider
	{
		private readonly Func<string, LogLevel, bool> _filter;
		private readonly IHttpContextAccessor _accessor;
		public readonly FileLoggerOptions Options;
		public readonly IWebHostEnvironment _webHostEnvironment;

		public FileLoggerProvider(Func<string, LogLevel, bool> filter, IHttpContextAccessor accessor, FileLoggerOptions _options, IWebHostEnvironment webHostEnvironment)
		{
			_filter = filter;
			_accessor = accessor;
			_webHostEnvironment = webHostEnvironment;
			Options = _options;
			Options.FolderPath = _webHostEnvironment.ContentRootPath + "\\" + Options.FolderPath;
			if (!Directory.Exists(Options.FolderPath))
			{
				Directory.CreateDirectory(Options.FolderPath);
			}
		}
		public ILogger CreateLogger(string categoryName)
		{
			return new FileLogger(this, categoryName, _filter, _accessor);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
