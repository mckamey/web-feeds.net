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

namespace WebFeeds.Feeds.Rdf
{
	/// <summary>
	/// RDF 1.0 Item
	///		http://web.resource.org/rss/1.0/spec#s5.5
	/// </summary>
	public class RdfItem : RdfBase, IWebFeedItem
	{
		#region Fields

		private string description = String.Empty;

		// extensions
		private DublinCore dublinCore = null;
		private string contentEncoded = null;
		private Uri wfwComment = null;
		private Uri wfwCommentRss = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets a brief description of the channel's content, function, source, etc.
		/// </summary>
		/// <remarks>
		/// Suggested maximum length is 500 characters.
		/// Required even if empty.
		/// </remarks>
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = String.IsNullOrEmpty(value) ? String.Empty : value; }
		}

		protected internal string Copyright
		{
			get { return this.DublinCore[DublinCore.TermName.Rights]; }
		}

		/// <summary>
		/// Gets and sets the encoded content for this item
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RdfItem.ContentEncodedElement, Namespace=RdfItem.ContentNamespace)]
		public string ContentEncoded
		{
			get { return this.contentEncoded; }
			set { this.contentEncoded = value; }
		}

		/// <summary>
		/// Gets and sets the Uri to which comments can be POSTed
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RdfItem.WfwCommentElement, Namespace=RdfItem.WfwNamespace)]
		public string WfwComment
		{
			get { return ExtensibleBase.ConvertToString(this.wfwComment); }
			set { this.wfwComment = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets the Uri at which a feed of comments can be found
		/// </summary>
		[DefaultValue(null)]
		[XmlElement(RdfItem.WfwCommentRssElement, Namespace=RdfItem.WfwNamespace)]
		public string WfwCommentRss
		{
			get { return ExtensibleBase.ConvertToString(this.wfwCommentRss); }
			set { this.wfwCommentRss = ExtensibleBase.ConvertToUri(value); }
		}

		#endregion Properties

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

		Uri IWebFeedItem.ID
		{
			get
			{
				Uri id = ((IUriProvider)this).Uri;
				if (id == null)
				{
					Uri.TryCreate(this.About, UriKind.RelativeOrAbsolute, out id);
				}
				return id;
			}
		}

		string IWebFeedItem.Title
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

		string IWebFeedItem.Description
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

		string IWebFeedItem.Author
		{
			get
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
		}

		DateTime? IWebFeedItem.Published
		{
			get
			{
				string date = this.DublinCore[DublinCore.TermName.Date];
				return ExtensibleBase.ConvertToDateTime(date);
			}
		}

		DateTime? IWebFeedItem.Updated
		{
			get
			{
				string date = this.DublinCore[DublinCore.TermName.Date];
				return ExtensibleBase.ConvertToDateTime(date);
			}
		}

		Uri IWebFeedItem.Link
		{
			get { return ((IUriProvider)this).Uri; }
		}

		Uri IWebFeedItem.ThreadLink
		{
			get { return this.wfwCommentRss; }
		}

		int? IWebFeedItem.ThreadCount
		{
			get { return null; }
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
				namespaces.Add(RdfItem.ContentPrefix, RdfItem.ContentNamespace);
			}

			if (this.wfwComment != null || this.wfwCommentRss != null)
			{
				namespaces.Add(RdfItem.WfwPrefix, RdfItem.WfwNamespace);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}
}
