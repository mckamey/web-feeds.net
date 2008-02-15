using System;
using System.Xml.Serialization;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Feed interface
	/// </summary>
	public interface IWebFeed
	{
		/// <summary>
		/// Gets the MIME Type designation for the feed
		/// </summary>
		string MimeType { get; }

		/// <summary>
		/// Gets the default namespace URI for the feed
		/// </summary>
		XmlSerializerNamespaces Namespaces { get; }
	}
}
