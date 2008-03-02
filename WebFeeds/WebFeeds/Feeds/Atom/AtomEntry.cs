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

namespace WebFeeds.Feeds.Atom
{
	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.1.2
	/// </summary>
	/// <remarks>
	/// atomEntry : atomBase
	///		atomContent?
	///		atomPublished?
	///		atomSource?
	///		atomSummary?
	/// </remarks>
	[Serializable]
	public class AtomEntry : AtomBase, IWebFeedItem
	{
		#region Fields

		private AtomContent content = null;
		private AtomDate published;
		private AtomSource source = null;
		private AtomText summary = null;
		private int threadTotal = 0;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("content")]
		public AtomContent Content
		{
			get { return this.content; }
			set { this.content = value; }
		}

		[DefaultValue(null)]
		[XmlElement("published")]
		public virtual AtomDate Published
		{
			get { return this.published; }
			set { this.published = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool PublishedSpecified
		{
			get { return this.published.HasValue; }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("source")]
		public AtomSource Source
		{
			get { return this.source; }
			set { this.source = value; }
		}

		[DefaultValue(null)]
		[XmlElement("summary")]
		public AtomText Summary
		{
			get { return this.summary; }
			set { this.summary = value; }
		}

		[XmlElement("in-reply-to", Namespace=AtomInReplyTo.ThreadingNamespace)]
		public readonly List<AtomInReplyTo> InReplyToReferences = new List<AtomInReplyTo>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool InReplyToReferencesSpecified
		{
			get { return (this.InReplyToReferences.Count > 0); }
			set { }
		}

		/// <summary>
		/// http://tools.ietf.org/html/rfc4685#section-5
		/// </summary>
		[DefaultValue(0)]
		[XmlElement(ElementName="total", Namespace=AtomEntry.ThreadingNamespace)]
		public int ThreadTotal
		{
			get { return this.threadTotal; }
			set { this.threadTotal = (value < 0) ? 0 : value; }
		}

		#endregion Properties

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
				if (this.summary == null)
				{
					if (this.content == null)
					{
						return null;
					}
					return this.content.StringValue;
				}
				return this.summary.StringValue;
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
			get
			{
				if (!this.Published.HasValue)
				{
					return ((IWebFeedItem)this).Updated;
				}

				return this.Published.Value;
			}
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

				if (alternate == null && this.content != null)
				{
					alternate = ((IUriProvider)this.content).Uri;
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

		int IWebFeedItem.ThreadCount
		{
			get
			{
				if (!this.LinksSpecified)
				{
					return this.ThreadTotal;
				}

				foreach (AtomLink link in this.Links)
				{
					if (link.Relation == AtomLinkRelation.Replies &&
						link.ThreadCountSpecified)
					{
						return link.ThreadCount;
					}
				}

				return this.ThreadTotal;
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
			if (this.ThreadTotal > 0)
			{
				namespaces.Add(AtomEntry.ThreadingPrefix, AtomEntry.ThreadingNamespace);
			}

			if (this.InReplyToReferencesSpecified)
			{
				foreach (AtomInReplyTo inReplyTo in this.InReplyToReferences)
				{
					inReplyTo.AddNamespaces(namespaces);
				}
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}

	/// <summary>
	/// Adapter for Atom 0.3 compatibility
	/// </summary>
	[Serializable]
	public class AtomEntryOld : AtomEntry
	{
		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		[Obsolete("Atom 0.3 is for backwards compatibility and should only be used for deserialization", true)]
		public AtomEntryOld()
		{
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("modified")]
		public AtomDate Modified
		{
			get { return base.Updated; }
			set { base.Updated = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ModifiedSpecified
		{
			get { return base.UpdatedSpecified; }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("issued")]
		public AtomDate Issued
		{
			get { return base.Published; }
			set { base.Published = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool IssuedSpecified
		{
			get { return base.PublishedSpecified; }
			set { }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AtomDate Published
		{
			get { return base.Published; }
			set { base.Published = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AtomDate Updated
		{
			get { return base.Updated; }
			set { base.Updated = value; }
		}

		#endregion Properties
	}
}
