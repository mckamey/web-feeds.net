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

namespace WebFeeds.Feeds.Atom
{
	/// <summary>
	/// The Atom Syndication Format
	///		http://tools.ietf.org/html/rfc4287#section-4.1.1
	/// </summary>
	/// <remarks>
	/// atomFeed : atomSource
	///		atomLogo?
	///		atomEntry*
	/// </remarks>
	[Serializable]
	[XmlRoot(AtomFeed.RootElement, Namespace=AtomFeed.Namespace)]
	public class AtomFeed : AtomSource, IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://tools.ietf.org/html/rfc4287";
		protected internal const string Prefix = "";
		protected internal const string Namespace = "http://www.w3.org/2005/Atom";
		protected internal const string RootElement = "feed";
		public const string MimeType = "application/atom+xml";

		#endregion Constants

		#region Properties

		[XmlElement("entry")]
		public readonly List<AtomEntry> Entries = new List<AtomEntry>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool EntriesSpecified
		{
			get { return (this.Entries.Count > 0); }
			set { }
		}

		#endregion Properties

		#region IWebFeed Members

		string IWebFeed.MimeType
		{
			get { return AtomFeed.MimeType; }
		}

		string IWebFeed.Copyright
		{
			get
			{
				if (!this.RightsSpecified)
				{
					return null;
				}
				return this.Rights.StringValue;
			}
		}

		Uri IWebFeed.ImageLink
		{
			get
			{
				if (this.LogoUri == null)
				{
					return this.IconUri;
				}
				return this.LogoUri;
			}
		}

		IList<IWebFeedItem> IWebFeed.Items
		{
			get { return this.Entries.ToArray(); }
		}

		#endregion IWebFeed Members

		#region IWebFeedItem Members

		Uri IWebFeedItem.ID
		{
			get { return ((IUriProvider)this).Uri; }
		}

		string IWebFeedItem.Title
		{
			get
			{
				if (this.Title == null)
				{
					return null;
				}
				return this.Title.StringValue;
			}
		}

		string IWebFeedItem.Description
		{
			get
			{
				if (this.SubTitle == null)
				{
					return null;
				}
				return this.SubTitle.StringValue;
			}
		}

		string IWebFeedItem.Author
		{
			get
			{
				if (!this.AuthorsSpecified)
				{
					if (!this.ContributorsSpecified)
					{
						return null;
					}
					foreach (AtomPerson person in this.Contributors)
					{
						if (!String.IsNullOrEmpty(person.Name))
						{
							return person.Name;
						}
						if (!String.IsNullOrEmpty(person.Email))
						{
							return person.Name;
						}
					}
				}

				foreach (AtomPerson person in this.Authors)
				{
					if (!String.IsNullOrEmpty(person.Name))
					{
						return person.Name;
					}
					if (!String.IsNullOrEmpty(person.Email))
					{
						return person.Name;
					}
				}

				return null;
			}
		}

		DateTime? IWebFeedItem.Published
		{
			get { return ((IWebFeedItem)this).Updated; }
		}

		DateTime? IWebFeedItem.Updated
		{
			get
			{
				if (!this.Updated.HasValue)
				{
					return null;
				}

				return this.Updated.Value;
			}
		}

		Uri IWebFeedItem.Link
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return null;
				}

				Uri alternate = null;
				foreach (AtomLink link in this.Links)
				{
					if ("alternate".Equals(link.Rel))
					{
						return ((IUriProvider)link).Uri;
					}
					else if (alternate == null && !"self".Equals(link.Rel))
					{
						return ((IUriProvider)link).Uri;
					}
				}

				return alternate;
			}
		}

		Uri IWebFeedItem.ThreadLink
		{
			get { return null; }
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
			namespaces.Add(AtomFeed.Prefix, AtomFeed.Namespace);
			namespaces.Add(AtomFeed.XmlPrefix, AtomFeed.XmlNamespace);

			foreach (AtomEntry entry in this.Entries)
			{
				entry.AddNamespaces(namespaces);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}

	/// <summary>
	/// Adapter for Atom 0.3 compatibility
	/// </summary>
	[Serializable]
	[XmlRoot(AtomFeedOld.RootElement, Namespace=AtomFeedOld.Namespace)]
	public class AtomFeedOld : AtomSource, IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://www.mnot.net/drafts/draft-nottingham-atom-format-02.html";
		protected internal const string Prefix = "";
		protected internal const string Namespace = "http://purl.org/atom/ns#";
		protected internal const string RootElement = "feed";
		protected internal const string MimeType = "application/atom+xml";

		#endregion Constants

		#region Fields

		private Version version = new Version(0, 3);

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		[Obsolete("Atom 0.3 is for backwards compatibility and should only be used for deserialization", true)]
		public AtomFeedOld()
		{
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlAttribute("version")]
		public string Version
		{
			get { return (this.version == null) ? null : this.version.ToString(); }
			set { this.version = String.IsNullOrEmpty(value) ? null : new Version(value); }
		}

		[DefaultValue(null)]
		[XmlElement("tagline")]
		public AtomText TagLine
		{
			get { return base.SubTitle; }
			set { base.SubTitle = value; }
		}

		[DefaultValue(null)]
		[XmlElement("copyright")]
		public AtomText Copyright
		{
			get { return base.Rights; }
			set { base.Rights = value; }
		}

		[DefaultValue(null)]
		[XmlElement("modified")]
		public AtomDate Modified
		{
			get { return base.Updated; }
			set { base.Updated = value; }
		}

		[DefaultValue(0)]
		[XmlElement("fullcount")]
		public int FullCount
		{
			get
			{
				if (this.Entries == null)
				{
					return 0;
				}
				return this.Entries.Count;
			}
			set { }
		}

		[XmlElement("entry")]
		public readonly List<AtomEntryOld> Entries = new List<AtomEntryOld>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool EntriesSpecified
		{
			get { return (this.Entries.Count > 0); }
			set { }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool SubTitleSpecified
		{
			get { return false; }
			set { }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool RightsSpecified
		{
			get { return false; }
			set { }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool UpdatedSpecified
		{
			get { return false; }
			set { }
		}

		#endregion Properties

		#region IWebFeed Members

		string IWebFeed.MimeType
		{
			get { return AtomFeedOld.MimeType; }
		}

		string IWebFeed.Copyright
		{
			get
			{
				if (!this.RightsSpecified)
				{
					return null;
				}
				return this.Rights.StringValue;
			}
		}

		Uri IWebFeed.ImageLink
		{
			get
			{
				if (this.LogoUri == null)
				{
					return this.IconUri;
				}
				return this.LogoUri;
			}
		}

		IList<IWebFeedItem> IWebFeed.Items
		{
			get { return this.Entries.ToArray(); }
		}

		#endregion IWebFeed Members

		#region IWebFeedItem Members

		Uri IWebFeedItem.ID
		{
			get { return ((IUriProvider)this).Uri; }
		}

		string IWebFeedItem.Title
		{
			get
			{
				if (this.Title == null)
				{
					return null;
				}
				return this.Title.StringValue;
			}
		}

		string IWebFeedItem.Description
		{
			get
			{
				if (this.SubTitle == null)
				{
					return null;
				}
				return this.SubTitle.StringValue;
			}
		}

		string IWebFeedItem.Author
		{
			get
			{
				if (!this.AuthorsSpecified)
				{
					if (!this.ContributorsSpecified)
					{
						return null;
					}
					foreach (AtomPerson person in this.Contributors)
					{
						if (!String.IsNullOrEmpty(person.Name))
						{
							return person.Name;
						}
						if (!String.IsNullOrEmpty(person.Email))
						{
							return person.Name;
						}
					}
				}

				foreach (AtomPerson person in this.Authors)
				{
					if (!String.IsNullOrEmpty(person.Name))
					{
						return person.Name;
					}
					if (!String.IsNullOrEmpty(person.Email))
					{
						return person.Name;
					}
				}

				return null;
			}
		}

		DateTime? IWebFeedItem.Published
		{
			get { return ((IWebFeedItem)this).Updated; }
		}

		DateTime? IWebFeedItem.Updated
		{
			get
			{
				if (!this.Updated.HasValue)
				{
					return null;
				}

				return this.Updated.Value;
			}
		}

		Uri IWebFeedItem.Link
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return null;
				}

				Uri alternate = null;
				foreach (AtomLink link in this.Links)
				{
					switch (link.Relation)
					{
						case AtomLinkRelation.Alternate:
						{
							return ((IUriProvider)link).Uri;
						}
						case AtomLinkRelation.Related:
						case AtomLinkRelation.Enclosure:
						{
							if (alternate == null)
							{
								alternate = ((IUriProvider)link).Uri;
							}
							break;
						}
						default:
						{
							continue;
						}
					}
				}

				return alternate;
			}
		}

		Uri IWebFeedItem.ThreadLink
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return null;
				}

				foreach (AtomLink link in this.Links)
				{
					if (link.Relation == AtomLinkRelation.Replies)
					{
						return ((IUriProvider)link).Uri;
					}
				}

				return null;
			}
		}

		int? IWebFeedItem.ThreadCount
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return null;
				}

				foreach (AtomLink link in this.Links)
				{
					if (link.Relation == AtomLinkRelation.Replies &&
						link.ThreadCountSpecified)
					{
						return link.ThreadCount;
					}
				}

				return null;
			}
		}

		DateTime? IWebFeedItem.ThreadUpdated
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return null;
				}

				foreach (AtomLink link in this.Links)
				{
					if (link.Relation == AtomLinkRelation.Replies &&
						link.ThreadUpdatedSpecified)
					{
						return link.ThreadUpdated.Value;
					}
				}

				return null;
			}
		}

		#endregion IWebFeedItem Members

		#region INamespaceProvider Members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			namespaces.Add(AtomFeedOld.Prefix, AtomFeedOld.Namespace);
			namespaces.Add(AtomFeedOld.XmlPrefix, AtomFeedOld.XmlNamespace);

			foreach (AtomEntry entry in this.Entries)
			{
				entry.AddNamespaces(namespaces);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.11
	/// </summary>
	/// <remarks>
	/// atomSource : atomBase
	///		atomGenerator?
	///		atomIcon?
	///		atomLogo?
	///		atomSubtitle?
	/// </remarks>
	public class AtomSource : AtomBase
	{
		#region Fields

		private AtomGenerator generator = null;
		private Uri icon = null;
		private Uri logo = null;
		private AtomText subtitle = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("generator")]
		public AtomGenerator Generator
		{
			get { return this.generator; }
			set { this.generator = value; }
		}

		[DefaultValue(null)]
		[XmlElement("icon")]
		public string Icon
		{
			get { return ExtensibleBase.ConvertToString(this.icon); }
			set { this.icon = ExtensibleBase.ConvertToUri(value); }
		}

		[XmlIgnore]
		protected Uri IconUri
		{
			get { return this.icon; }
		}

		[DefaultValue(null)]
		[XmlElement("logo")]
		public string Logo
		{
			get { return ExtensibleBase.ConvertToString(this.logo); }
			set { this.logo = ExtensibleBase.ConvertToUri(value); }
		}

		[XmlIgnore]
		protected Uri LogoUri
		{
			get { return this.logo; }
		}

		[DefaultValue(null)]
		[XmlElement("subtitle")]
		public AtomText SubTitle
		{
			get { return this.subtitle; }
			set { this.subtitle = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool SubTitleSpecified
		{
			get { return true; }
			set { }
		}

		#endregion Properties
	}
}
