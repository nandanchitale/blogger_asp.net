using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Helpers.Logging
{
	public static class FileLoggerExtensions
	{
		public static ILoggerFactory AddFileLogger(this ILoggerFactory factory, IHttpContextAccessor accessor,
										  IConfiguration configuration, IWebHostEnvironment webHostEnvironment, Func<string, LogLevel, bool> filter = null)
		{
			FileLoggerOptions fileLoggerOptions = new();
			configuration.GetSection("Logging").GetSection("LogFile").GetSection("Options").Bind(fileLoggerOptions);
			factory.AddProvider(new FileLoggerProvider(filter, accessor, fileLoggerOptions, webHostEnvironment));
			return factory;
		}
	}
}
