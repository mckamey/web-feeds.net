#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2006-2009 Stephen M. McKamey

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
#endregion License

using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rss
{
	#region RssCategory

	/// <summary>
	/// RSS 2.0 Category
	///		http://blogs.law.harvard.edu/tech/rss#ltcategorygtSubelementOfLtitemgt
	///		http://blogs.law.harvard.edu/tech/rss#syndic8
	/// </summary>
	[Serializable]
	public class RssCategory : RssBase
	{
		#region Fields

		private string domain = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public RssCategory()
		{
		}

		public RssCategory(string value)
		{
			this.value = value;
		}

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
			set { this.domain = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets a slash-delimited string which identifies a hierarchic location in the indicated taxonomy.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return this.value; }
			set { this.value = String.IsNullOrEmpty(value) ? null : value; }
		}

		#endregion Properties

		#region Operators

		public static implicit operator RssCategory(string value)
		{
			return new RssCategory(value);
		}

		public static explicit operator string(RssCategory value)
		{
			return value.Value;
		}

		#endregion Operators
	}

	#endregion RssCategory

	#region RssCloud

	/// <summary>
	/// RSS 2.0 Cloud
	///		http://blogs.law.harvard.edu/tech/rss#ltcloudgtSubelementOfLtchannelgt
	///		http://blogs.law.harvard.edu/tech/soapMeetsRss#rsscloudInterface
	/// </summary>
	[Serializable]
	public class RssCloud : RssBase
	{
		#region Fields

		private string domain = null;
		private int port = Int32.MinValue;
		private string path = null;
		private string registerProcedure = null;
		private string protocol = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets the domain.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("domain")]
		public string Domain
		{
			get { return this.domain; }
			set { this.domain = String.IsNullOrEmpty(value) ? null : value; }
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
			set { this.path = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the registerProcedure.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("registerProcedure")]
		public string RegisterProcedure
		{
			get { return this.registerProcedure; }
			set { this.registerProcedure = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the protocol.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("protocol")]
		public string Protocol
		{
			get { return this.protocol; }
			set { this.protocol = String.IsNullOrEmpty(value) ? null : value; }
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

	#region RssDate

	[Serializable]
	public struct RssDate
	{
		#region Fields

		private DateTime? value;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="date"></param>
		public RssDate(DateTime date)
		{
			this.value = date;
		}

		#endregion Init

		#region Properties

		[XmlIgnore]
		public DateTime Value
		{
			get
			{
				if (!this.value.HasValue)
				{
					throw new InvalidOperationException("RssDate object must have a value.");
				}
				return this.value.Value;
			}
			set { this.value = value; }
		}

		[XmlIgnore]
		public bool HasValue
		{
			get { return this.value.HasValue; }
		}

		/// <summary>
		/// Gets and sets the DateTime using RFC-822 date format.
		/// For serialization purposes only, use the PubDate property instead.
		/// </summary>
		[XmlText]
		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string Value_Rfc822
		{
			get
			{
				if (!this.value.HasValue)
				{
					return null;
				}

				return this.value.Value.ToString("R");
			}
			set
			{
				DateTime dateTime;
				if (!DateTime.TryParse(value, out dateTime))
				{
					this.value = null;
					return;
				}

				this.value = dateTime.ToUniversalTime();
			}
		}

		#endregion Properties

		#region Methods

		public DateTime GetValueOrDefault(DateTime defaultValue)
		{
			if (!this.value.HasValue)
			{
				return defaultValue;
			}
			return this.value.Value;
		}

		#endregion Methods

		#region Object Overrides

		public override string ToString()
		{
			return this.Value.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is RssDate)
			{
				return this.value.Equals(((RssDate)obj).value);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public override int GetHashCode()
		{
			if (!this.value.HasValue)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		#endregion Object Overrides

		#region Operators

		public static implicit operator RssDate(DateTime value)
		{
			return new RssDate(value);
		}

		public static explicit operator DateTime(RssDate value)
		{
			return value.Value;
		}

		#endregion Operators
	}

	#endregion RssDate

	#region RssEnclosure

	/// <summary>
	/// RSS 2.0 Enclosure
	///		http://blogs.law.harvard.edu/tech/rss#ltenclosuregtSubelementOfLtitemgt
	///		http://www.thetwowayweb.com/payloadsforrss
	///		http://www.reallysimplesyndication.com/discuss/msgReader$221
	/// </summary>
	[Serializable]
	public class RssEnclosure : RssBase, IUriProvider
	{
		#region Fields

		private Uri url = null;
		private long length = 0;
		private string type = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets the URL where the enclosure is located.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("url")]
		public string Url
		{
			get { return ExtensibleBase.ConvertToString(this.url); }
			set { this.url = ExtensibleBase.ConvertToUri(value); }
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
			set { this.type = String.IsNullOrEmpty(value) ? null : value; }
		}

		[XmlIgnore]
		public bool HasValue
		{
			get
			{
				return (this.Length > 0) ||
					!String.IsNullOrEmpty(this.Url) &&
					!String.IsNullOrEmpty(this.Type);
			}
		}

		#endregion Properties

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.url; }
		}

		#endregion IUriProvider Members
	}

	#endregion RssEnclosure

	#region RssGuid

	/// <summary>
	/// RSS 2.0 Guid
	///		http://blogs.law.harvard.edu/tech/rss#ltguidgtSubelementOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssGuid : RssBase, IUriProvider
	{
		#region Fields

		private bool isPermaLink = true;
		private Uri value = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets if the identifier is a permanent URL.
		/// </summary>
		[DefaultValue(true)]
		[XmlAttribute("isPermaLink")]
		public bool IsPermaLink
		{
			get
			{
				string link = this.Value;

				return this.isPermaLink &&
					(link != null) &&
					link.StartsWith(Uri.UriSchemeHttp);
			}
			set { this.isPermaLink = value; }
		}

		/// <summary>
		/// Gets and sets the globally unique identifier, may be an url or other unique string.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return ExtensibleBase.ConvertToString(this.value); }
			set { this.value = ExtensibleBase.ConvertToUri(value); }
		}

		[XmlIgnore]
		public bool HasValue
		{
			get
			{
				return !String.IsNullOrEmpty(this.Value);
			}
		}

		#endregion Properties

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.value; }
		}

		#endregion IUriProvider Members
	}

	#endregion RssGuid

	#region RssImage

	/// <summary>
	/// RSS 2.0 Image
	///		http://blogs.law.harvard.edu/tech/rss#ltimagegtSubelementOfLtchannelgt
	/// </summary>
	[Serializable]
	public class RssImage : RssBase, IUriProvider
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

		#region Properties

		/// <summary>
		/// Gets and sets the url to which the image is linked.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("url")]
		public string Url
		{
			get { return ExtensibleBase.ConvertToString(this.url); }
			set { this.url = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets the title of the image (alternate text).
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the url to which the image is linked.
		/// </summary>
		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("link")]
		public string Link
		{
			get { return ExtensibleBase.ConvertToString(this.link); }
			set { this.link = ExtensibleBase.ConvertToUri(value); }
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

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.url; }
		}

		#endregion IUriProvider Members
	}

	#endregion RssImage

	#region RssPerson

	/// <summary>
	/// RSS 2.0 Email
	///		http://blogs.law.harvard.edu/tech/rss#ltauthorgtSubelementOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssPerson : RssBase
	{
		#region Constants

		private const string EmailFormat = "{0} ({1})";

		#endregion Constants

		#region Fields

		private string name = null;
		private string email = null;

		#endregion Fields

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
				{
					return this.email;
				}

				if (String.IsNullOrEmpty(this.email))
				{
					return this.name;
				}

				return String.Format(RssPerson.EmailFormat, this.email.Trim(), this.name.Trim());
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

	#endregion RssPerson

	#region RssSkipDays

	/// <summary>
	/// RSS 2.0 SkipDays
	///		http://blogs.law.harvard.edu/tech/skipHoursDays
	/// </summary>
	[Serializable]
	public class RssSkipDays : RssBase
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
					{
						skipped.Add(day.ToString("G"));
					}
				}

				return skipped.ToArray();
			}
			set
			{
				this.days = new BitVector32(EmptyDays);
				if (value == null)
				{
					return;
				}

				foreach (string day in value)
				{
					try
					{
						this[(DayOfWeek)Enum.Parse(typeof(DayOfWeek), day, true)] = true;
					}
					catch
					{
						continue;
					}
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
	public class RssSkipHours : RssBase
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

		#endregion Init

		#region Properties

		[XmlIgnore]
		public bool this[int hour]
		{
			get
			{
				if (hour < MinHour || hour > MaxHour)
				{
					return false;
				}

				return this.hours[RssSkipHours.HourMasks[hour]];
			}
			set
			{
				if (hour < MinHour || hour > MaxHour)
				{
					return;
				}

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
				{
					return;
				}

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
	public class RssSource : RssBase
	{
		#region Fields

		private Uri url = null;
		private string value = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets the url of the source.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("url")]
		public string Url
		{
			get { return ExtensibleBase.ConvertToString(this.url); }
			set { this.url = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets the name of the RSS channel that the item came from.
		/// </summary>
		[DefaultValue(null)]
		[XmlText]
		public string Value
		{
			get { return this.value; }
			set { this.value = String.IsNullOrEmpty(value) ? null : value; }
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
	public class RssTextInput : RssBase
	{
		#region Fields

		private string title = null;
		private string description = null;
		private string name = null;
		private Uri link = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets the title of the submit button.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the description of the text input area.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the name of the text input field.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("name")]
		public string Name
		{
			get { return this.name; }
			set { this.name = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the text input request url.
		/// </summary>
		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("link")]
		public string Link
		{
			get { return ExtensibleBase.ConvertToString(this.link); }
			set { this.link = ExtensibleBase.ConvertToUri(value); }
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
