using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Feeds.Atom
{
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
}
