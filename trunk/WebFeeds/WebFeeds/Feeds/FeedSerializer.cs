using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

using WebFeeds.Feeds.Atom;
using WebFeeds.Feeds.Rss;
using WebFeeds.Feeds.Rdf;
using WebFeeds.Feeds.Modules;

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
				//serializer.UnknownElement += new XmlElementEventHandler(serializer_UnknownElement);
				//serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
				return serializer.Deserialize(reader) as IWebFeed;
			}
		}

		//private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		//{
		//    XmlSerializer serializer = sender as XmlSerializer;

		//    if (DublinCore.IsDublinCore(e.Attr))
		//    {
		//        return;
		//    }

		//    return;
		//}

		//private static void serializer_UnknownElement(object sender, XmlElementEventArgs e)
		//{
		//    XmlSerializer serializer = sender as XmlSerializer;

		//    if (DublinCore.IsDublinCore(e.Element))
		//    {
		//        return;
		//    }

		//    return;
		//}

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
