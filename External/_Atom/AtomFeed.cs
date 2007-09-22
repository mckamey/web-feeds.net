using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Notifier.Atom
{
	/// <summary>
	/// http://tools.ietf.org/html/rfc4287
	/// </summary>
	[Serializable]
	[XmlInclude(typeof(AtomFeed03))]
	[XmlInclude(typeof(AtomFeed10))]
	public class AtomFeed
	{
		#region Constants

		public const string AtomSpecificationUrl = "http://tools.ietf.org/html/rfc4287";
		protected const string ElementName = "feed";

		#endregion Constants

		#region Fields

		private AtomText title = null;
		private AtomText tagline = null;
		private AtomLink link = null;
		private AtomDate modified = null;
		private List<AtomEntry> entries = new List<AtomEntry>();

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		protected AtomFeed() { }

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("title")]
		public AtomText Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		[DefaultValue(null)]
		[XmlElement("tagline")]
		public AtomText TagLine
		{
			get { return this.tagline; }
			set { this.tagline = value; }
		}

		[DefaultValue(null)]
		[XmlElement("link")]
		public AtomLink Link
		{
			get { return this.link; }
			set { this.link = value; }
		}

		[DefaultValue(null)]
		[XmlElement("modified")]
		public AtomDate Modified
		{
			get { return this.modified; }
			set { this.modified = value; }
		}

		[XmlElement("entry")]
		public List<AtomEntry> Entries
		{
			get { return this.entries; }
			set { this.entries = value; }
		}

		#endregion Properties
	}

	[XmlRoot(AtomFeed10.ElementName, Namespace=AtomFeed10.Namespace)]
	public class AtomFeed10 : AtomFeed
	{
		#region Constants

		private const string Namespace = "http://www.w3.org/2005/Atom";

		#endregion Constants
	}

	[XmlRoot(AtomFeed10.ElementName, Namespace=AtomFeed03.Namespace)]
	public class AtomFeed03 : AtomFeed
	{
		#region Constants

		private const string Namespace = "http://purl.org/atom/ns#";

		#endregion Constants

		#region Fields

		private int fullCount = -1;

		#endregion Fields

		#region Properties

		[DefaultValue(-1)]
		[XmlElement("fullcount")]
		public int FullCount
		{
			get { return this.fullCount; }
			set { this.fullCount = value; }
		}

		#endregion Properties
	}
}
