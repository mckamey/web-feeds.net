using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Notifier.Atom
{
	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-3.2
	/// </summary>
	[Serializable]
	public class AtomPerson
	{
		#region Fields

		private string name = null;
		private string uri = null;
		private string email = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomPerson() { }

		#endregion Init

		#region Properties

		[XmlElement("name")]
		public string Name
		{
			get
			{
				if (this.name == null)
				{
					return String.Empty;
				}
				return this.name;
			}
			set { this.name = value; }
		}

		[XmlElement("uri")]
		[DefaultValue(null)]
		public string Uri
		{
			get { return this.uri; }
			set { this.uri = value; }
		}

		[XmlElement("email")]
		[DefaultValue(null)]
		public string Email
		{
			get { return this.email; }
			set { this.email = value; }
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			if (!String.IsNullOrEmpty(this.Email))
			{
				return String.Format("\"{0}\" <{1}>", this.Name, this.Email);
			}
			if (!String.IsNullOrEmpty(this.Uri))
			{
				return String.Format("\"{0}\" <{1}>", this.Name, this.Uri);
			}
			return String.Format("\"{0}\"", this.Name);
		}

		#endregion Object Overrides
	}
}
