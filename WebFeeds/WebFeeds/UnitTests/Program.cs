#region WebFeeds License
/*---------------------------------------------------------------------------------*\

	WebFeeds distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2006-2008 Stephen M. McKamey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.

\*---------------------------------------------------------------------------------*/
#endregion WebFeeds License

using System;
using System.IO;

using WebFeeds.Feeds;
using WebFeeds.Feeds.Extensions;
using WebFeeds.Feeds.Rdf;

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

					#region DublinCore test

					if (feed is RdfFeed)
					{
						DublinCore dc = new DublinCore();
						((RdfFeed)feed).Channel.FillExtensions(dc);

						foreach (DublinCore.TermName term in dc.Terms)
						{
							Console.WriteLine("DublinCore {0}: {1}", term, dc[term]);
						}
					}

					#endregion DublinCore test

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
