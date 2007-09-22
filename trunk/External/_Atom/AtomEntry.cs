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
	public class AtomEntry
	{
		#region Fields

		private string id = null;
		private AtomText title = null;
		private AtomText summary = null;
		private AtomLink link = null;
		private AtomDate modified = null;
		private AtomDate issued = null;
		private AtomPerson author = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomEntry() { }

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
		[XmlElement("summary")]
		public AtomText Summary
		{
			get { return this.summary; }
			set { this.summary = value; }
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

		[DefaultValue(null)]
		[XmlElement("issued")]
		public AtomDate Issued
		{
			get { return this.issued; }
			set { this.issued = value; }
		}

		[DefaultValue(null)]
		[XmlElement("id")]
		public string ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		[DefaultValue(null)]
		[XmlElement("author")]
		public AtomPerson Author
		{
			get { return this.author; }
			set { this.author = value; }
		}

		#endregion Properties
	}
}
