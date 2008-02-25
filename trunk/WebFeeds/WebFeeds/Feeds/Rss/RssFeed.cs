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
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rss
{
	/// <summary>
	/// Really Simple Syndication (RSS 2.0)
	///		http://www.rssboard.org/rss-specification
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	[XmlRoot(RssFeed.RootElement, Namespace=RssFeed.Namespace)]
	public class RssFeed : RssBase, IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://blogs.law.harvard.edu/tech/rss";
		protected internal const string RootElement = "rss";
		protected internal const string Namespace = "";
		protected internal const string MimeType = "application/rss+xml";

		#endregion Constants

		#region Fields

		private RssChannel channel = null;
		private Version version = new Version(2,0);

		#endregion Fields

		#region Properties

		[XmlElement("channel")]
		public RssChannel Channel
		{
			get
			{
				if (this.channel == null)
				{
					this.channel = new RssChannel();
				}

				return this.channel;
			}
			set { this.channel = value; }
		}

		[XmlAttribute("version")]
		public string Version
		{
			get { return (this.version == null) ? null : this.version.ToString(); }
			set { this.version = String.IsNullOrEmpty(value) ? null : new Version(value); }
		}

		#endregion Properties

		#region IWebFeed Members

		string IWebFeed.MimeType
		{
			get { return RssFeed.MimeType; }
		}

		string IWebFeed.Copyright
		{
			get { return this.Channel.Copyright; }
		}

		Uri IWebFeed.Image
		{
			get
			{
				if (!this.Channel.ImageSpecified)
				{
					return null;
				}
				return ((IUriProvider)this.Channel.Image).Uri;
			}
		}

		IList<IWebFeedItem> IWebFeed.Items
		{
			get { return this.Channel.Items.ToArray(); }
		}

		#endregion IWebFeedItem Members

		#region INamespaceProvider Members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			namespaces.Add("", RssFeed.Namespace);

			this.Channel.AddNamespaces(namespaces);

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members

		#region IWebFeedItem Members

		Uri IWebFeedItem.ID
		{
			get { return ((IUriProvider)this.Channel).Uri; }
		}

		string IWebFeedItem.Title
		{
			get { return this.Channel.Title; }
		}

		string IWebFeedItem.Description
		{
			get { return this.Channel.Description; }
		}

		string IWebFeedItem.Author
		{
			get
			{
				if (!this.Channel.ManagingEditorSpecified)
				{
					if (!this.Channel.WebMasterSpecified)
					{
						return null;
					}
					if (String.IsNullOrEmpty(this.Channel.WebMaster.Name))
					{
						return this.Channel.WebMaster.Email;
					}
					return this.Channel.WebMaster.Name;
				}
				if (String.IsNullOrEmpty(this.Channel.ManagingEditor.Name))
				{
					return this.Channel.ManagingEditor.Email;
				}
				return this.Channel.ManagingEditor.Name;
			}
		}

		DateTime? IWebFeedItem.Published
		{
			get
			{
				if (!this.Channel.PubDate.HasValue)
				{
					return null;
				}

				return this.Channel.PubDate.Value;
			}
		}

		DateTime? IWebFeedItem.Updated
		{
			get
			{
				if (!this.Channel.LastBuildDate.HasValue)
				{
					return null;
				}

				return this.Channel.LastBuildDate.Value;
			}
		}

		Uri IWebFeedItem.Link
		{
			get { return ((IUriProvider)this.Channel).Uri; }
		}

		#endregion IWebFeedItem Members
	}
}
