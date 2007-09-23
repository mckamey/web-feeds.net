using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Feeds.Atom
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
		private AtomDate published = null;
		private AtomSource source = null;
		private AtomText summary = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomEntry() { }

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
		public AtomDate Published
		{
			get { return this.published; }
			set { this.published = value; }
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
	/// Adaptor for Atom 0.3 compatibility
	/// </summary>
	[Serializable]
	public class AtomEntry03 : AtomEntry
	{
		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomEntry03() { }

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("modified")]
		public AtomDate Modified
		{
			get { return base.Updated; }
			set { base.Updated = value; }
		}

		[DefaultValue(null)]
		[XmlElement("issued")]
		public AtomDate Issued
		{
			get { return base.Published; }
			set { base.Published = value; }
		}

		#endregion Properties
	}
}
