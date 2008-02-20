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
	public class AtomEntry : AtomSource
	{
		#region Fields

		private AtomContent content = null;
		private AtomDate published;
		private AtomSource source = null;
		private AtomText summary = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomEntry()
		{
		}

		#endregion Init

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

		#endregion Properties
	}

	/// <summary>
	/// Adapter for Atom 0.3 compatibility
	/// </summary>
	[Serializable]
	public class AtomEntry03 : AtomEntry
	{
		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		[Obsolete("Atom 0.3 is for backwards compatibility and should only be used for deserialization", true)]
		public AtomEntry03()
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
