using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaLib.Web.Feeds.Atom
{
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
}
