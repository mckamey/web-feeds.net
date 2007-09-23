using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Feeds.Atom
{
	#region AtomContent

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.1.3
	/// </summary>
	[Serializable]
	public class AtomContent
	{
		#region Fields

		private string type = null;
		private string src = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomContent() { }

		#endregion Init

		#region Properties

		[DefaultValue(AtomTextType.text)]
		[XmlIgnore]
		public AtomTextType TextType
		{
			get
			{
				if (String.IsNullOrEmpty(this.type))
				{
					return AtomTextType.text;
				}
				try
				{
					return (AtomTextType)Enum.Parse(typeof(AtomTextType), this.type, false);
				}
				catch
				{
					return AtomTextType.text;
				}
			}
			set { this.type = value.ToString(); }
		}

		[DefaultValue(null)]
		[XmlAttribute("type")]
		public string Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		[DefaultValue(null)]
		[XmlAttribute("src")]
		public string Src
		{
			get { return this.src; }
			set { this.src = value; }
		}

		[XmlText]
		[DefaultValue(null)]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			return this.Value;
		}

		#endregion Object Overrides
	}

	#endregion AtomContent

	#region AtomDate

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-3.3
	/// </summary>
	[Serializable]
	public class AtomDate
	{
		#region Fields

		private DateTime value = DateTime.MinValue;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomDate() { }

		#endregion Init

		#region Properties

		[XmlIgnore]
		public DateTime Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		[XmlText]
		[DefaultValue(null)]
		public string Value_Iso8601
		{
			get
			{
				if (this.value == DateTime.MinValue)
				{
					return null;
				}
				return this.value.ToString("s");
			}
			set
			{
				if (String.IsNullOrEmpty(value) ||
					!DateTime.TryParse(value, out this.value))
				{
					this.value = DateTime.MinValue;
				}
			}
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			return this.Value.ToString();
		}

		#endregion Object Overrides
	}

	#endregion AtomDate

	#region AtomLink

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.7
	/// </summary>
	[Serializable]
	public class AtomLink
	{
		#region Fields

		private string rel = null;
		private string href = null;
		private string type = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomLink() { }

		#endregion Init

		#region Properties

		[XmlAttribute("rel")]
		public string Rel
		{
			get { return this.rel; }
			set { this.rel = value; }
		}

		[XmlAttribute("href")]
		[DefaultValue(null)]
		public string Href
		{
			get { return this.href; }
			set { this.href = value; }
		}

		[XmlAttribute("type")]
		[DefaultValue(null)]
		public string Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			return this.Href;
		}

		#endregion Object Overrides
	}

	#endregion AtomLink

	#region AtomPerson

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

	#endregion AtomPerson

	#region AtomText

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-3.1
	/// </summary>
	[Serializable]
	public class AtomText
	{
		#region Fields

		private AtomTextType type = AtomTextType.text;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public AtomText() { }

		#endregion Init

		#region Properties

		[DefaultValue(AtomTextType.text)]
		[XmlAttribute("type")]
		public AtomTextType Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		[XmlText]
		[DefaultValue(null)]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			return this.Value;
		}

		#endregion Object Overrides
	}

	#endregion AtomText

	#region AtomTextType

	public enum AtomTextType
	{
		text,
		html,
		xhtml
	}

	#endregion AtomTextType
}
