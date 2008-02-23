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
using System.Collections.Generic;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rss
{
	/// <summary>
	/// RSS 2.0 Item
	///		http://blogs.law.harvard.edu/tech/rss#hrelementsOfLtitemgt
	/// </summary>
	[Serializable]
	public class RssItem : RssBase
	{
		#region Fields

		// required
		private string title = null;
		private Uri link = null;
		private string description = null;

		//optional
		private RssEmail author = null;
		private Uri comments = null;
		private RssEnclosure enclosure = null;
		private RssGuid guid = null;
		private RssDate pubDate;
		private RssSource source = null;

		#endregion Fields

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
			get { return ExtensibleBase.ConvertToString(this.link); }
			set { this.link = ExtensibleBase.ConvertToUri(value); }
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
				{
					this.author = new RssEmail();
				}

				return this.author;
			}
			set { this.author = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool AuthorSpecified
		{
			get { return (this.author != null && !this.author.IsEmpty()); }
			set { }
		}

		[XmlElement("category")]
		public readonly List<RssCategory> Categories = new List<RssCategory>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool CategoriesSpecified
		{
			get { return (this.Categories.Count > 0); }
			set { }
		}

		/// <summary>
		/// Gets and sets a URL to the comments about the item.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("comments")]
		public string Comments
		{
			get { return ExtensibleBase.ConvertToString(this.comments); }
			set { this.comments = ExtensibleBase.ConvertToUri(value); }
		}

		[XmlElement("enclosure")]
		public RssEnclosure Enclosure
		{
			get
			{
				if (this.enclosure == null)
				{
					this.enclosure = new RssEnclosure();
				}
				return this.enclosure;
			}
			set { this.enclosure = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool EnclosureSpecified
		{
			get { return (this.enclosure != null) && this.enclosure.HasValue; }
			set { }
		}

		[XmlElement("guid")]
		public RssGuid Guid
		{
			get
			{
				if (this.guid == null)
				{
					this.guid = new RssGuid();
				}
				return this.guid;
			}
			set { this.guid = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool GuidSpecified
		{
			get { return (this.guid != null) && this.guid.HasValue; }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("pubDate")]
		public RssDate PubDate
		{
			get { return this.pubDate; }
			set { this.pubDate = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool PubDateSpecified
		{
			get { return this.pubDate.HasValue; }
			set { }
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
