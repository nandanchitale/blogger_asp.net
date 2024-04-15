namespace Helpers.Logging
{
	public class FileLoggerOptions
	{
		public virtual string FilePath { get; set; }

		public virtual string FolderPath { get; set; }

		public virtual bool RollingFile { get; set; }

		public virtual long MaxFileSize { get; set; }
	}
}
