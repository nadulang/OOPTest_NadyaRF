using System;
using System.Collections.Generic;
using System.IO;

namespace OOPTest_NadyaRF
{
    public class LogClass
    {
		public interface ILog
		{
			DateTime Date { get; set; }
			string Message { get; set; }
		}
		public static class Log_1
		{
			public static List<Log> errorLog = new List<Log>();
			public static void SaveAllLog()
			{
				var lines = new List<string>();
				foreach (var X in errorLog)
				{
					lines.Add($"{X.Date} INFO: {X.Message}");
				}
				File.WriteAllLines(@"/Users/gigaming/Documents/Nadya RF/Bootcamp_Refactory/OOPTest_NadyaRF/OOPTest_NadyaRF/app.log", lines);
			}
			public static void PopulateLog(string msg)
			{
				DateTime date = DateTime.Now;
				errorLog.Add(new Log { Date = date, Message = msg });
			}
		}
		public class Log : ILog
		{
			public DateTime Date { get; set; }
			public string Message { get; set; }
		}
	}
}
    

