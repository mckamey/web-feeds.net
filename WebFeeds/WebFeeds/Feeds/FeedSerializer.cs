using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

using WebFeeds.Feeds.Atom;
using WebFeeds.Feeds.Rss;
//using WebFeeds.Feeds.Rdf;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Utility for serialization
	/// </summary>
	public static class FeedSerializer
	{
		#region Serialization Methods

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
				return DeserializeXml(response.GetResponseStream()) ;
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

				Type type = GetFeedType(reader.NamespaceURI, reader.LocalName);

				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(reader) as IWebFeed;
			}
		}

		public static void SerializeXml(IWebFeed feed, Stream output, string xsltUrl)
		{
			// setup document formatting, make human readable
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CheckCharacters = true;
			settings.CloseOutput = true;
			settings.ConformanceLevel = ConformanceLevel.Document;
			settings.Encoding = System.Text.Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			XmlWriter writer = XmlWriter.Create(output, settings);

			if (!String.IsNullOrEmpty(xsltUrl))
			{
				// add a stylesheet for browser viewing
				// render the XSLT processor instruction.
				writer.WriteProcessingInstruction(
					"xml-stylesheet",
					String.Format("type=\"text/xsl\" href=\"{0}\" version=\"1.0\"", xsltUrl));
			}

			// write out feed
			XmlSerializer serializer = new XmlSerializer(feed.GetType());
			serializer.Serialize(writer, feed);
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
				//case RdfFeed.RootElement:
				//{
				//    return typeof(RdfFeed);
				//}
			}

			return typeof(Object);
		}

		#endregion Utility Methods
	}
}