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
	public class RssItem : RssBase, IWebFeedItem
	{
		#region Fields

		// required
		private string title = null;
		private Uri link = null;
		private string description = null;

		//optional
		private RssPerson author = null;
		private Uri comments = null;
		private RssEnclosure enclosure = null;
		private RssGuid guid = null;
		private RssDate pubDate;
		private RssSource source = null;

		// extensions
		private DublinCore dublinCore = null;
		private string contentEncoded = null;
		private Uri wfwComment = null;
		private Uri wfwCommentRss = null;
		private int? slashComments = null;

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
			set { this.title = String.IsNullOrEmpty(value) ? null : value; }
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
			set { this.description = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the author of the item.
		/// </summary>
		[XmlElement("author")]
		public RssPerson Author
		{
			get
			{
				if (this.author == null)
				{
					this.author = new RssPerson();
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

		#region Content Extensions

		/// <summary>
		/// Gets and sets the encoded content for this item
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RssItem.ContentEncodedElement, Namespace=RssItem.ContentNamespace)]
		public string ContentEncoded
		{
			get { return this.contentEncoded; }
			set { this.contentEncoded = value; }
		}

		#endregion Content Extensions

		#region WellFormedWeb Extensions

		/// <summary>
		/// Gets and sets the Uri to which comments can be POSTed
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RssItem.WfwCommentElement, Namespace=RssItem.WfwNamespace)]
		public string WfwComment
		{
			get { return ExtensibleBase.ConvertToString(this.wfwComment); }
			set { this.wfwComment = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets the Uri at which a feed of comments can be found
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RssItem.WfwCommentRssElement, Namespace=RssItem.WfwNamespace)]
		public string WfwCommentRss
		{
			get { return ExtensibleBase.ConvertToString(this.wfwCommentRss); }
			set { this.wfwCommentRss = ExtensibleBase.ConvertToUri(value); }
		}

		#endregion WellFormedWeb Extensions

		#region Slash Extensions

		/// <summary>
		/// Gets and sets the number of comments for this item
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RssItem.SlashCommentsElement, Namespace=RssItem.SlashNamespace)]
		public int SlashComments
		{
			get
			{
				if (!this.slashComments.HasValue)
				{
					return 0;
				}
				return this.slashComments.Value;
			}
			set { this.slashComments = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool SlashCommentsSpecified
		{
			get { return this.slashComments.HasValue; }
			set { }
		}

		#endregion Slash Extensions

		#region IWebFeedItem Members

		/// <summary>
		/// Allows IWebFeedItem to access DublinCore
		/// </summary>
		/// <remarks>
		/// Note this only gets filled on first access
		/// </remarks>
		private DublinCore DublinCore
		{
			get
			{
				if (this.dublinCore == null)
				{
					this.dublinCore = new DublinCore();
					this.FillExtensions(dublinCore);
				}
				return this.dublinCore;
			}
		}

		Uri IWebFeedBase.ID
		{
			get
			{
				if (this.guid == null)
				{
					return null;
				}

				return ((IUriProvider)this.guid).Uri;
			}
		}

		string IWebFeedBase.Title
		{
			get
			{
				string title = this.Title;
				if (String.IsNullOrEmpty(title))
				{
					title = this.DublinCore[DublinCore.TermName.Title];
				}
				return title;
			}
		}

		string IWebFeedBase.Description
		{
			get
			{
				string description = this.description;
				if (String.IsNullOrEmpty(description))
				{
					description = this.ContentEncoded;
					if (String.IsNullOrEmpty(description))
					{
						description = this.DublinCore[DublinCore.TermName.Description];
						if (String.IsNullOrEmpty(description))
						{
							description = this.DublinCore[DublinCore.TermName.Subject];
						}
					}
				}
				return description;
			}
		}

		string IWebFeedBase.Author
		{
			get
			{
				if (!this.AuthorSpecified)
				{
					string author = this.DublinCore[DublinCore.TermName.Creator];
					if (String.IsNullOrEmpty(author))
					{
						author = this.DublinCore[DublinCore.TermName.Contributor];

						if (String.IsNullOrEmpty(author))
						{
							author = this.DublinCore[DublinCore.TermName.Publisher];
						}
					}
					return author;
				}
				if (String.IsNullOrEmpty(this.author.Name))
				{
					return this.author.Email;
				}
				return this.author.Name;
			}
		}

		DateTime? IWebFeedBase.Published
		{
			get
			{
				if (!this.pubDate.HasValue)
				{
					string date = this.DublinCore[DublinCore.TermName.Date];
					return ExtensibleBase.ConvertToDateTime(date);
				}

				return this.pubDate.Value;
			}
		}

		DateTime? IWebFeedBase.Updated
		{
			get { return ((IWebFeedBase)this).Published; }
		}

		Uri IWebFeedBase.Link
		{
			get { return this.link; }
		}

		Uri IWebFeedBase.ImageLink
		{
			get
			{
				if (!this.EnclosureSpecified)
				{
					return null;
				}

				string type = this.enclosure.Type;
				if (String.IsNullOrEmpty(type) ||
					!type.StartsWith("image", StringComparison.InvariantCultureIgnoreCase))
				{
					return null;
				}

				return ((IUriProvider)this.enclosure).Uri;
			}
		}

		Uri IWebFeedItem.ThreadLink
		{
			get
			{
				if (this.comments == null)
				{
					return this.wfwCommentRss;
				}
				return this.comments;
			}
		}

		int? IWebFeedItem.ThreadCount
		{
			get
			{
				if (!this.SlashCommentsSpecified)
				{
					return null;
				}
				return this.SlashComments;
			}
		}

		DateTime? IWebFeedItem.ThreadUpdated
		{
			get { return null; }
		}

		#endregion IWebFeedItem Members

		#region INamespaceProvider Members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			if (this.contentEncoded != null)
			{
				namespaces.Add(RssItem.ContentPrefix, RssItem.ContentNamespace);
			}

			if (this.wfwComment != null || this.wfwCommentRss != null)
			{
				namespaces.Add(RssItem.WfwPrefix, RssItem.WfwNamespace);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}
}
