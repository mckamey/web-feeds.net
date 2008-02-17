using System;
using System.IO;

using WebFeeds.Feeds;

namespace WebFeeds
{
	class Program
	{
		#region Constants

		private const int Timeout = 5000;
		private const string UnitTestFolder = @"UnitTests\\";
		private const string OutputFolder = @"Output\\";

		#endregion Constants

		#region Program Entry

		static void Main(string[] args)
		{
			string[] unitTests = Directory.GetFiles(UnitTestFolder, "*.xml", SearchOption.AllDirectories);
			if (Directory.Exists(OutputFolder))
			{
				try
				{
					Directory.Delete(OutputFolder, true);
				}
				catch { }
			}
			Directory.CreateDirectory(OutputFolder);

			foreach (string unitTest in unitTests)
			{
				try
				{
					string path = Path.GetFullPath(unitTest);
					IWebFeed feed = FeedSerializer.DeserializeXml(path, Timeout);
					using (Stream output = File.OpenWrite(unitTest.Replace(UnitTestFolder, OutputFolder)))
					{
						output.SetLength(0L);
						FeedSerializer.SerializeXml(feed, output, null);
					}
				}
				catch (Exception ex)
				{
					File.WriteAllText(unitTest.Replace(UnitTestFolder, OutputFolder), ex.ToString());
				}
			}
		}

		#endregion Program Entry
	}
}
