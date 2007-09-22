using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Notifier.Atom
{
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

	public enum AtomTextType
	{
		text,
		html,
		xhtml
	}
}
