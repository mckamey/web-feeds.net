using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Feeds.Rss
{
	/// <summary>
	/// RSS 2.0 Item
	///		http://blogs.law.harvard.edu/tech/rss#hrelementsOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssItem
	{
		#region Fields

		// required
		private string title = null;
		private Uri link = null;
		private string description = null;

		//optional
		private RssEmail author = null;
		private List<RssCategory> categories = new List<RssCategory>();
		private Uri comments = null;
		private RssEnclosure enclosure = null;
		private RssGuid guid = null;
		private DateTime pubDate = DateTime.MinValue;
		private string pubDate_Rfc822 = null;
		private RssSource source = null;

		#endregion Fields

		#region Init

		public RssItem() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the title of the item.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Gets and sets the url of the item.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("link")]
		public string Link
		{
			get
			{
				if (this.link == null)
					return null;

				return this.link.AbsoluteUri;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.link = null;
					return;
				}

				this.link = new Uri(value, UriKind.Absolute);
			}
		}

		/// <summary>
		/// Gets and sets the description of the item.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		/// <summary>
		/// Gets and sets the author of the item.
		/// </summary>
		[XmlElement("author")]
		public RssEmail Author
		{
			get
			{
				if (this.author == null)
					this.author = new RssEmail();

				return this.author;
			}
			set { this.author = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool AuthorSpecified
		{
			get { return (this.author != null && !this.author.IsEmpty()); }
			set { }
		}

		[XmlElement("category")]
		public List<RssCategory> Categories
		{
			get { return this.categories; }
			set { this.categories = value; }
		}

		/// <summary>
		/// Gets and sets comments about the item.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("comments")]
		public string Comments
		{
			get
			{
				if (this.comments == null)
					return null;

				return this.comments.AbsoluteUri;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.comments = null;
					return;
				}

				this.comments = new Uri(value, UriKind.Absolute);
			}
		}

		[XmlElement("enclosure")]
		public RssEnclosure Enclosure
		{
			get
			{
				if (this.enclosure == null)
					this.enclosure = new RssEnclosure();

				return this.enclosure;
			}
			set { this.enclosure = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool EnclosureSpecified
		{
			get { return (this.enclosure != null && !this.enclosure.IsEmpty()); }
			set { }
		}

		[XmlElement("guid")]
		public RssGuid Guid
		{
			get
			{
				if (this.guid == null)
					this.guid = new RssGuid();

				return this.guid;
			}
			set { this.guid = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool GuidSpecified
		{
			get { return (this.guid != null && !this.guid.IsEmpty()); }
			set {  }
		}

		[XmlIgnore]
		public DateTime PubDate
		{
			get { return this.pubDate; }
			set { this.pubDate = value; }
		}

		/// <summary>
		/// Gets and sets the pubDate using RFC-822 date format.  For serialization purposes only, use the PubDate property instead.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("pubDate")]
		public string PubDate_Rfc822
		{
			get
			{
				if (this.pubDate == DateTime.MinValue)
					return this.pubDate_Rfc822;

				return this.pubDate.ToString("R");
			}
			set
			{
				if (DateTime.TryParse(value, out this.pubDate))
				{
					this.pubDate_Rfc822 = null;
					return;
				}

				this.pubDate_Rfc822 = value;
			}
		}

		[DefaultValue(null)]
		[XmlElement("source")]
		public RssSource Source
		{
			get { return this.source; }
			set { this.source = value; }
		}

		#endregion Properties
	}
}
