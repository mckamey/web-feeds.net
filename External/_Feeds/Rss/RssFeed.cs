using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Rss
{
	/// <summary>
	/// RSS 2.0 Root
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	[XmlRoot(RssFeed.ElementName, Namespace=RssFeed.Namespace)]
	public class RssFeed
	{
		#region Constants

		public const string RssSpecificationUrl = "http://blogs.law.harvard.edu/tech/rss";
		private const string ElementName = "rss";
		private const string Namespace = "";

		#endregion Constants

		#region Fields

		private RssChannel channel = null;
		private Version version = new Version(2,0);

		#endregion Fields

		#region Init

		public RssFeed()
		{
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("channel")]
		public RssChannel Channel
		{
			get
			{
				if (this.channel == null)
					this.channel = new RssChannel();

				return this.channel;
			}
			set { this.channel = value; }
		}

		[XmlAttribute("version")]
		public string Version
		{
			get { return this.version.ToString(); }
			set { this.version = new Version(value); }
		}

		#endregion Properties
	}
}
