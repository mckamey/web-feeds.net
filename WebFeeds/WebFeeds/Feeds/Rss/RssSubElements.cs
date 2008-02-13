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
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Rss
{
	#region RssCategory

	/// <summary>
	/// RSS 2.0 Category
	///		http://blogs.law.harvard.edu/tech/rss#ltcategorygtSubelementOfLtitemgt
	///		http://blogs.law.harvard.edu/tech/rss#syndic8
	/// </summary>
	[Serializable]
	public class RssCategory
	{
		#region Fields

		private string domain = null;
		private string value = null;

		#endregion Fields

		#region Init

		public RssCategory() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets a string that identifies a categorization taxonomy (url).
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("domain")]
		public string Domain
		{
			get { return this.domain; }
			set { this.domain = value; }
		}

		/// <summary>
		/// Gets and sets a slash-delimited string which identifies a hierarchic location in the indicated taxonomy.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties
	}

	#endregion RssCategory

	#region RssCloud

	/// <summary>
	/// RSS 2.0 Cloud
	///		http://blogs.law.harvard.edu/tech/rss#ltcloudgtSubelementOfLtchannelgt
	///		http://blogs.law.harvard.edu/tech/soapMeetsRss#rsscloudInterface
	/// </summary>
	[Serializable]
	public class RssCloud
	{
		#region Fields

		private string domain = null;
		private int port = Int32.MinValue;
		private string path = null;
		private string registerProcedure = null;
		private string protocol = null;

		#endregion Fields

		#region Init

		public RssCloud() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the domain.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("domain")]
		public string Domain
		{
			get { return this.domain; }
			set { this.domain = value; }
		}

		/// <summary>
		/// Gets and sets the port.
		/// </summary>
		[DefaultValue(Int32.MinValue)]
		[XmlAttribute("port")]
		public int Port
		{
			get { return this.port; }
			set
			{
				if (value <= 0)
					this.port = Int32.MinValue;
				else
					this.port = value;
			}
		}

		/// <summary>
		/// Gets and sets the path.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("path")]
		public string Path
		{
			get { return this.path; }
			set { this.path = value; }
		}

		/// <summary>
		/// Gets and sets the registerProcedure.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("registerProcedure")]
		public string RegisterProcedure
		{
			get { return this.registerProcedure; }
			set { this.registerProcedure = value; }
		}

		/// <summary>
		/// Gets and sets the protocol.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("protocol")]
		public string Protocol
		{
			get { return this.protocol; }
			set { this.protocol = value; }
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Domain) &&
				this.Port <= 0 &&
				String.IsNullOrEmpty(this.Path) &&
				String.IsNullOrEmpty(this.RegisterProcedure) &&
				String.IsNullOrEmpty(this.Protocol);
		}

		#endregion Methods
	}

	#endregion RssCloud

	#region RssEmail

	/// <summary>
	/// RSS 2.0 Email
	///		http://blogs.law.harvard.edu/tech/rss#ltauthorgtSubelementOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssEmail
	{
		#region Constants

		private const string EmailFormat = "{0} ({1})";

		#endregion Constants

		#region Fields

		private string name = null;
		private string email = null;

		#endregion Fields

		#region Init

		public RssEmail() { }

		#endregion Init

		#region Properties

		[XmlIgnore]
		public string Email
		{
			get { return this.email; }
			set { this.email = value; }
		}

		[XmlIgnore]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		[XmlText]
		public string Value
		{
			get
			{
				if (String.IsNullOrEmpty(this.name))
					return this.email;

				if (String.IsNullOrEmpty(this.email))
					return this.name;

				return String.Format(RssEmail.EmailFormat, this.email.Trim(), this.name.Trim());
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.name = this.email = null;
					return;
				}

				int start = value.IndexOf("(");
				int end = value.LastIndexOf(")");
				if (end <= start || start < 0 || end < 0)
				{
					this.name = value;
					this.email = null;
					return;
				}

				this.name = value.Substring(start+1, end-start-1);
				this.email = value.Substring(0, start);
			}
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Email) &&
				String.IsNullOrEmpty(this.Name);
		}

		#endregion Methods
	}

	#endregion RssEmail

	#region RssEnclosure

	/// <summary>
	/// RSS 2.0 Enclosure
	///		http://blogs.law.harvard.edu/tech/rss#ltenclosuregtSubelementOfLtitemgt
	///		http://www.thetwowayweb.com/payloadsforrss
	///		http://www.reallysimplesyndication.com/discuss/msgReader$221
	/// </summary>
	[Serializable]
	public class RssEnclosure
	{
		#region Fields

		private Uri url = null;
		private long length = 0;
		private string type = null;

		#endregion Fields

		#region Init

		public RssEnclosure() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the URL where the enclosure is located.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("url")]
		public string Url
		{
			get
			{
				if (this.url == null)
					return null;

				return this.url.AbsoluteUri;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.url = null;
					return;
				}

				this.url = new Uri(value, UriKind.Absolute);
			}
		}

		/// <summary>
		/// Gets and sets the length of the enclosure in bytes.
		/// </summary>
		[XmlAttribute("length")]
		public long Length
		{
			get { return this.length; }
			set
			{
				if (value < 0L)
					this.length = 0L;
				else
					this.length = value;
			}
		}

		/// <summary>
		/// Gets and sets the MIME type for the resource.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("type")]
		public string Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return (this.Length <= 0) &&
				String.IsNullOrEmpty(this.Url) &&
				String.IsNullOrEmpty(this.Type);
		}

		#endregion Methods
	}

	#endregion RssEnclosure

	#region RssGuid

	/// <summary>
	/// RSS 2.0 Guid
	///		http://blogs.law.harvard.edu/tech/rss#ltguidgtSubelementOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssGuid
	{
		#region Fields

		private bool isPermaLink = false;
		private string value = null;

		#endregion Fields

		#region Init

		public RssGuid() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets if the url is permanent.
		/// </summary>
		[DefaultValue(false)]
		[XmlAttribute("isPermaLink")]
		public bool IsPermaLink
		{
			get { return this.isPermaLink; }
			set { this.isPermaLink = value; }
		}

		/// <summary>
		/// Gets and sets the globally unique identifier, may be an url or other unique string.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Value);
		}

		#endregion Methods
	}

	#endregion RssGuid

	#region RssImage

	/// <summary>
	/// RSS 2.0 Image
	///		http://blogs.law.harvard.edu/tech/rss#ltimagegtSubelementOfLtchannelgt
	/// </summary>
	[Serializable]
	public class RssImage
	{
		#region Fields

		// required
		private Uri url = null;
		private string title = null;
		private Uri link = null;

		//optional
		private int width = Int32.MinValue;
		private int height = Int32.MinValue;

		#endregion Fields

		#region Init

		public RssImage() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the url to which the image is linked.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("url")]
		public string Url
		{
			get
			{
				if (this.url == null)
					return null;

				return this.url.AbsoluteUri;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.url = null;
					return;
				}

				this.url = new Uri(value, UriKind.Absolute);
			}
		}

		/// <summary>
		/// Gets and sets the title of the image (alternate text).
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Gets and sets the url to which the image is linked.
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
		/// Gets and sets the width of the image.
		/// </summary>
		[DefaultValue(Int32.MinValue)]
		[XmlElement("width")]
		public int Width
		{
			get { return this.width; }
			set
			{
				if (value <= 0)
					this.width = Int32.MinValue;
				else
					this.width = value;
			}
		}

		/// <summary>
		/// Gets and sets the height of the image.
		/// </summary>
		[DefaultValue(Int32.MinValue)]
		[XmlElement("height")]
		public int Height
		{
			get { return this.height; }
			set
			{
				if (value <= 0)
					this.height = Int32.MinValue;
				else
					this.height = value;
			}
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Url) &&
				String.IsNullOrEmpty(this.Title) &&
				String.IsNullOrEmpty(this.Link) &&
				this.Width <= 0 ||
				this.Height <= 0;
		}

		#endregion Methods
	}

	#endregion RssImage

	#region RssSkipDays

	/// <summary>
	/// RSS 2.0 SkipDays
	///		http://blogs.law.harvard.edu/tech/skipHoursDays
	/// </summary>
	[Serializable]
	public class RssSkipDays
	{
		#region Constants

		private static readonly int[] DayMasks = new int[7];
		private const int EmptyDays = 0x0;

		#endregion Constants

		#region Fields

		private BitVector32 days = new BitVector32(EmptyDays);

		#endregion Fields

		#region Init

		static RssSkipDays()
		{
			int i = (int)DayOfWeek.Sunday;
			RssSkipDays.DayMasks[i] = BitVector32.CreateMask(0);
			for (i++; i<=(int)DayOfWeek.Saturday; i++)
			{
				RssSkipDays.DayMasks[i] = BitVector32.CreateMask(RssSkipDays.DayMasks[i-1]);
			}
		}

		public RssSkipDays() { }

		#endregion Init

		#region Properties

		[XmlIgnore]
		public bool this[DayOfWeek day]
		{
			get { return this.days[RssSkipDays.DayMasks[(int)day]]; }
			set { this.days[RssSkipDays.DayMasks[(int)day]] = value; }
		}

		[XmlElement("day")]
		public string[] Days
		{
			get
			{
				List<string> skipped = new List<string>();

				foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
				{
					if (this[day])
						skipped.Add(day.ToString("G"));
				}

				return skipped.ToArray();
			}
			set
			{
				this.days = new BitVector32(EmptyDays);
				if (value == null)
					return;

				foreach (string day in value)
				{
					if (Enum.IsDefined(typeof(DayOfWeek), day))
						this[(DayOfWeek)Enum.Parse(typeof(DayOfWeek), day)] = true;
				}
			}
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return this.days.Data == EmptyDays;
		}

		#endregion Methods
	}

	#endregion RssSkipDays

	#region RssSkipHours

	/// <summary>
	/// RSS 2.0 SkipHours
	///		http://blogs.law.harvard.edu/tech/skipHoursDays
	/// </summary>
	[Serializable]
	public class RssSkipHours
	{
		#region Constants

		private static readonly int[] HourMasks = new int[24];
		private const int EmptyHours = 0x0;
		private const int MinHour = 0;
		private const int MaxHour = 23;

		#endregion Constants

		#region Fields

		private BitVector32 hours = new BitVector32(EmptyHours);

		#endregion Fields

		#region Init

		static RssSkipHours()
		{
			int i = MinHour;
			RssSkipHours.HourMasks[i] = BitVector32.CreateMask(0);
			for (i++; i<=MaxHour; i++)
			{
				RssSkipHours.HourMasks[i] = BitVector32.CreateMask(RssSkipHours.HourMasks[i-1]);
			}
		}

		public RssSkipHours() { }

		#endregion Init

		#region Properties

		[XmlIgnore]
		public bool this[int hour]
		{
			get
			{
				if (hour < MinHour || hour > MaxHour)
					return false;

				return this.hours[RssSkipHours.HourMasks[hour]];
			}
			set
			{
				if (hour < MinHour || hour > MaxHour)
					return;

				this.hours[RssSkipHours.HourMasks[hour]] = value;
			}
		}

		[XmlElement("hour")]
		public int[] Hours
		{
			get
			{
				List<int> skipped = new List<int>();

				for (int i=MinHour; i<=MaxHour; i++)
				{
					if (this[i])
						skipped.Add(i);
				}

				return skipped.ToArray();
			}
			set
			{
				this.hours = new BitVector32(EmptyHours);
				if (value == null)
					return;

				foreach (int i in value)
				{
					this[i] = true;
				}
			}
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return this.hours.Data == EmptyHours;
		}

		#endregion Methods
	}

	#endregion RssSkipHours

	#region RssSource

	/// <summary>
	/// RSS 2.0 Source
	///		http://blogs.law.harvard.edu/tech/rss#ltsourcegtSubelementOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssSource
	{
		#region Fields

		private Uri url = null;
		private string value = null;

		#endregion Fields

		#region Init

		public RssSource() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the url of the source.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("url")]
		public string Url
		{
			get
			{
				if (this.url == null)
					return null;

				return this.url.AbsoluteUri;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.url = null;
					return;
				}

				this.url = new Uri(value, UriKind.Absolute);
			}
		}

		/// <summary>
		/// Gets and sets the name of the RSS channel that the item came from.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties
	}

	#endregion RssSource

	#region RssTextInput

	/// <summary>
	/// RSS 2.0 TextInput
	///		http://blogs.law.harvard.edu/tech/rss#lttextinputgtSubelementOfLtchannelgt
	/// </summary>
	[Serializable]
	public class RssTextInput
	{
		#region Fields

		private string title = null;
		private string description = null;
		private string name = null;
		private Uri link = null;

		#endregion Fields

		#region Init

		public RssTextInput() { }

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the title of the submit button.
		/// </summary>
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Gets and sets the description of the text input area.
		/// </summary>
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		/// <summary>
		/// Gets and sets the name of the text input field.
		/// </summary>
		[XmlElement("name")]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>
		/// Gets and sets the text input request url.
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

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Title) &&
				String.IsNullOrEmpty(this.Description) &&
				String.IsNullOrEmpty(this.Name) &&
				String.IsNullOrEmpty(this.Link);
		}

		#endregion Methods
	}

	#endregion RssTextInput
}
