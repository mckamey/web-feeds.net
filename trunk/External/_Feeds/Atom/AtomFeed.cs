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
	/// http://tools.ietf.org/html/rfc4287#section-4.1.1
	/// </summary>
	/// <remarks>
	/// atomFeed : atomSource
	///		atomLogo?
	///		atomEntry*
	/// </remarks>
	[Serializable]
	[XmlInclude(typeof(AtomFeed03))]
	[XmlInclude(typeof(AtomFeed10))]
	public abstract class AtomFeed : AtomSource
	{
		#region Constants

		public const string AtomSpecificationUrl = "http://tools.ietf.org/html/rfc4287";
		protected const string ElementName = "feed";

		#endregion Constants

		#region Fields

		private string logo = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		protected AtomFeed() { }

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("logo")]
		public string Logo
		{
			get { return this.logo; }
			set { this.logo = value; }
		}

		#endregion Properties
	}

	[XmlRoot(AtomFeed10.ElementName, Namespace=AtomFeed10.Namespace)]
	public class AtomFeed10 : AtomFeed
	{
		#region Constants

		private const string Namespace = "http://www.w3.org/2005/Atom";

		#endregion Constants

		#region Fields

		private List<AtomEntry> entries = new List<AtomEntry>();

		#endregion Fields

		#region Properties

		[XmlElement("entry")]
		public List<AtomEntry> Entries
		{
			get { return this.entries; }
			set { this.entries = value; }
		}

		#endregion Properties
	}

	/// <summary>
	/// http://www.mnot.net/drafts/draft-nottingham-atom-format-02.html
	/// </summary>
	[XmlRoot(AtomFeed03.ElementName, Namespace=AtomFeed03.Namespace)]
	public class AtomFeed03 : AtomFeed
	{
		#region Constants

		private const string Namespace = "http://purl.org/atom/ns#";

		#endregion Constants

		#region Fields

		private List<AtomEntry03> entries = new List<AtomEntry03>();

		#endregion Fields

		#region Properties

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
		}

		[XmlElement("entry")]
		public List<AtomEntry03> Entries
		{
			get { return this.entries; }
			set { this.entries = value; }
		}

		#endregion Properties
	}
}
