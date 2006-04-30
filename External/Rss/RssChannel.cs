using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Rss
{
	/// <summary>
	/// RSS 2.0 Channel
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	[Serializable]
	public class RssChannel
	{
		#region Fields

		private List<RssItem> items = new List<RssItem>();

		// required
		private string title = null;
		private Uri link = null;
		private string description = null;

		// optional
		private System.Globalization.CultureInfo language = System.Globalization.CultureInfo.InvariantCulture;
		private string copyright = null;
		private RssEmail managingEditor = null;
		private RssEmail webMaster = null;
		private DateTime pubDate = DateTime.MinValue;
		private string pubDate_Rfc822 = null;
		private DateTime lastBuildDate = DateTime.MinValue;
		private string lastBuildDate_Rfc822 = null;
		private List<RssCategory> categories = new List<RssCategory>();
		private string generator = null;
		private string docs = null;
		private RssCloud cloud = null;
		private int ttl = Int32.MinValue;
		private RssImage image = null;
		private string rating = null;
		private RssTextInput textInput = null;
		private RssSkipHours skipHours = null;
		private RssSkipDays skipDays = null;

		#endregion Fields

		#region Init

		public RssChannel() { }

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

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

		[DefaultValue(null)]
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		[DefaultValue("")]
		[XmlElement("language")]
		public string Language
		{
			get { return this.language.Name; }
			set { this.language = System.Globalization.CultureInfo.GetCultureInfo(value); }
		}

		[DefaultValue(null)]
		[XmlElement("copyright")]
		public string Copyright
		{
			get { return this.copyright; }
			set { this.copyright = value; }
		}

		/// <summary>
		/// Gets and sets the managing editor of the channel.
		/// </summary>
		[XmlElement("managingEditor")]
		public RssEmail ManagingEditor
		{
			get
			{
				if (this.managingEditor == null)
					this.managingEditor = new RssEmail();

				return this.managingEditor;
			}
			set { this.managingEditor = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool ManagingEditorSpecified
		{
			get { return (this.managingEditor != null && !this.managingEditor.IsEmpty()); }
			set { }
		}

		/// <summary>
		/// Gets and sets the webMaster of the channel.
		/// </summary>
		[XmlElement("webMaster")]
		public RssEmail WebMaster
		{
			get
			{
				if (this.webMaster == null)
					this.webMaster = new RssEmail();

				return this.webMaster;
			}
			set { this.webMaster = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool WebMasterSpecified
		{
			get { return (this.webMaster != null && !this.webMaster.IsEmpty()); }
			set { }
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

		[XmlIgnore]
		public DateTime LastBuildDate
		{
			get { return this.lastBuildDate; }
			set { this.lastBuildDate = value; }
		}

		/// <summary>
		/// Gets and sets the lastBuildDate using RFC-822 date format.  For serialization purposes only, use the LastBuildDate property instead.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("lastBuildDate")]
		public string LastBuildDate_Rfc822
		{
			get
			{
				if (this.lastBuildDate == DateTime.MinValue)
					return this.lastBuildDate_Rfc822;

				return this.lastBuildDate.ToString("R");
			}
			set
			{
				if (DateTime.TryParse(value, out this.lastBuildDate))
				{
					this.lastBuildDate_Rfc822 = null;
					return;
				}

				this.lastBuildDate_Rfc822 = value;
			}
		}

		[XmlElement("category")]
		public List<RssCategory> Categories
		{
			get { return this.categories; }
			set { this.categories = value; }
		}

		[DefaultValue(null)]
		[XmlElement("generator")]
		public string Generator
		{
			get { return this.generator; }
			set { this.generator = value; }
		}

		[DefaultValue(null)]
		[XmlElement("docs")]
		public string Docs
		{
			get { return this.docs; }
			set { this.docs = value; }
		}

		[DefaultValue(null)]
		[XmlElement("cloud")]
		public RssCloud Cloud
		{
			get
			{
				if (this.cloud == null)
					this.cloud = new RssCloud();

				return this.cloud;
			}
			set { this.cloud = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool CloudSpecified
		{
			get { return (this.cloud != null && !this.cloud.IsEmpty()); }
			set { }
		}

		[DefaultValue(Int32.MinValue)]
		[XmlElement("ttl")]
		public int Ttl
		{
			get { return this.ttl; }
			set
			{
				if (value < 0)
					this.ttl = Int32.MinValue;
				else
					this.ttl = value;
			}
		}

		[DefaultValue(null)]
		[XmlElement("image")]
		public RssImage Image
		{
			get
			{
				if (this.image == null)
					this.image = new RssImage();

				return this.image;
			}
			set { this.image = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool ImageSpecified
		{
			get { return (this.image != null && !this.image.IsEmpty()); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("rating")]
		public string Rating
		{
			get { return this.rating; }
			set { this.rating = value; }
		}

		[XmlElement("textInput")]
		public RssTextInput TextInput
		{
			get
			{
				if (this.textInput == null)
					this.textInput = new RssTextInput();

				return this.textInput;
			}
			set { this.textInput = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool TextInputSpecified
		{
			get { return (this.textInput != null && !this.textInput.IsEmpty()); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("skipHours")]
		public RssSkipHours SkipHours
		{
			get
			{
				if (this.skipHours == null)
					this.skipHours = new RssSkipHours();

				return this.skipHours;
			}
			set { this.skipHours = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool SkipHoursSpecified
		{
			get { return (this.skipHours != null && !this.skipHours.IsEmpty()); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("skipDays")]
		public RssSkipDays SkipDays
		{
			get
			{
				if (this.skipDays == null)
					this.skipDays = new RssSkipDays();

				return this.skipDays;
			}
			set { this.skipDays = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool SkipDaysSpecified
		{
			get { return (this.skipDays != null && !this.skipDays.IsEmpty()); }
			set { }
		}

		[XmlElement("item")]
		public List<RssItem> Items
		{
			get { return this.items; }
			set { this.items = value; }
		}

		[XmlIgnore]
		public RssItem this[int index]
		{
			get { return this.items[index]; }
			set { this.items[index] = value; }
		}

		#endregion Properties
	}
}