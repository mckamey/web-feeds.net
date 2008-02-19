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
	/// The Atom Syndication Format
	///		http://tools.ietf.org/html/rfc4287#section-4.1.1
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

		protected internal const string RootElement = "feed";
		protected internal const string MimeType = "application/atom+xml";

		#endregion Constants

		#region Fields

		private string logo = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
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

	[XmlRoot(AtomFeed10.RootElement, Namespace=AtomFeed10.Namespace)]
	public class AtomFeed10 : AtomFeed, IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://tools.ietf.org/html/rfc4287";
		protected internal const string Namespace = "http://www.w3.org/2005/Atom";

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

		#region IWebFeed Members

		[XmlIgnore]
		string IWebFeed.MimeType
		{
			get { return AtomFeed10.MimeType; }
		}

		void INamespaceProvider.AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			namespaces.Add("", AtomFeed10.Namespace);
			namespaces.Add("xml", AtomFeed10.XmlNamespace);
		}

		#endregion IWebFeed Members
	}

	/// <summary>
	/// Adapter for Atom 0.3 compatibility
	/// </summary>
	[XmlRoot(AtomFeed03.RootElement, Namespace=AtomFeed03.Namespace)]
	public class AtomFeed03 : AtomFeed, IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://www.mnot.net/drafts/draft-nottingham-atom-format-02.html";
		protected internal const string Namespace = "http://purl.org/atom/ns#";

		#endregion Constants

		#region Fields

		private List<AtomEntry03> entries = new List<AtomEntry03>();
		private Version version = new Version(0, 3);

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		[Obsolete("Atom 0.3 is for backwards compatibility and should only be used for deserialization", true)]
		public AtomFeed03()
		{
		}

		#endregion Init

		#region Properties

		[XmlAttribute("version")]
		public string Version
		{
			get { return this.version.ToString(); }
			set { this.version = new Version(value); }
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
		}

		[XmlElement("entry")]
		public List<AtomEntry03> Entries
		{
			get { return this.entries; }
			set { this.entries = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public override bool SubTitleSpecified
		{
			get { return false; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public override bool RightsSpecified
		{
			get { return false; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public override bool UpdatedSpecified
		{
			get { return false; }
		}

		#endregion Properties

		#region IWebFeed Members

		[XmlIgnore]
		string IWebFeed.MimeType
		{
			get { return AtomFeed03.MimeType; }
		}

		void INamespaceProvider.AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			namespaces.Add("", AtomFeed03.Namespace);
			namespaces.Add("xml", AtomFeed10.XmlNamespace);
		}

		#endregion IWebFeed Members
	}
}
