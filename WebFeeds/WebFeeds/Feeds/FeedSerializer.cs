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
using System.Net;
using System.Xml;
using System.Xml.Serialization;

using WebFeeds.Feeds.Atom;
using WebFeeds.Feeds.Rss;
using WebFeeds.Feeds.Rdf;
using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Utility for serialization
	/// </summary>
	public static class FeedSerializer
	{
		#region Serialization Methods

		/*
		 * From MSDN:
		 * 
		 * To increase performance, the XML serialization infrastructure dynamically generates
		 * assemblies to serialize and deserialize specified types. The infrastructure finds and
		 * reuses those assemblies. This behavior occurs only when using the following constructors:
		 * 
		 *		System.Xml.Serialization.XmlSerializer(Type) 
		 *		System.Xml.Serialization.XmlSerializer(Type,String) 
		 * 
		 * If you use any of the other constructors, multiple versions of the same assembly are generated
		 * and never unloaded, resulting in a memory leak and poor performance. The simplest solution is
		 * to use one of the two constructors above. Otherwise, you must cache the assemblies in a Hashtable,
		 * as shown in the following example.
		 */

		public static IWebFeed DeserializeXml(string url, int timeout)
		{
			WebRequest request = WebRequest.Create(url);
			if (request is HttpWebRequest)
			{
				((HttpWebRequest)request).AllowAutoRedirect = true;
				((HttpWebRequest)request).UserAgent = "WebFeeds/1.0";
			}
			request.Timeout = timeout;

			using (WebResponse response = request.GetResponse())
			{
				return FeedSerializer.DeserializeXml(response.GetResponseStream());
			}
		}

		public static IWebFeed DeserializeXml(Stream input)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			settings.IgnoreProcessingInstructions = true;

			using (XmlReader reader = XmlReader.Create(input, settings))
			{
				reader.MoveToContent();

				Type type = FeedSerializer.GetFeedType(reader.NamespaceURI, reader.LocalName);

				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(reader) as IWebFeed;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="feed"></param>
		/// <param name="output"></param>
		/// <param name="xsltUrl"></param>
		/// <remarks>
		/// PrettyPrint defaults to true for Debug builds, false for Release builds
		/// </remarks>
		public static void SerializeXml(IWebFeed feed, Stream output, string xsltUrl)
		{
			bool prettyPrint =
#if DEBUG
				true;
#else
				false;
#endif
			FeedSerializer.SerializeXml(feed, output, xsltUrl, prettyPrint);
		}

		public static void SerializeXml(IWebFeed feed, Stream output, string xsltUrl, bool prettyPrint)
		{
			// setup document formatting, make human readable
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CheckCharacters = true;
			settings.CloseOutput = true;
			settings.ConformanceLevel = ConformanceLevel.Document;
			settings.Encoding = System.Text.Encoding.UTF8;
			if (prettyPrint)
			{
				settings.Indent = true;
				settings.IndentChars = "\t";
			}
			else
			{
				settings.Indent = false;
				settings.NewLineChars = String.Empty;
			}
			settings.NewLineHandling = NewLineHandling.Replace;

			XmlWriter writer = XmlWriter.Create(output, settings);

			if (!String.IsNullOrEmpty(xsltUrl))
			{
				// add a stylesheet for browser viewing
				// render the XSLT processor instruction.
				writer.WriteProcessingInstruction(
					"xml-stylesheet",
					String.Format("type=\"text/xsl\" href=\"{0}\" version=\"1.0\"", xsltUrl));
			}

			// get all namespaces
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			feed.AddNamespaces(namespaces);

			// serialize feed
			XmlSerializer serializer = new XmlSerializer(feed.GetType());
			serializer.Serialize(writer, feed, namespaces);
		}

		#endregion Serialization Methods

		#region Utility Methods

		public static Type GetFeedType(string namespaceUri, string rootElement)
		{
			switch (rootElement)
			{
				case AtomFeed.RootElement:
				{
					switch (namespaceUri)
					{
						case AtomFeed03.Namespace:
						{
							return typeof(AtomFeed03);
						}
						case AtomFeed10.Namespace:
						{
							return typeof(AtomFeed10);
						}
					}
					break;
				}
				case RssFeed.RootElement:
				{
					return typeof(RssFeed);
				}
				case RdfFeed.RootElement:
				{
					return typeof(RdfFeed);
				}
			}

			return typeof(Object);
		}

		#endregion Utility Methods
	}
}
