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
using System.Xml;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Atom
{
	#region AtomCategory

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.2
	/// </summary>
	[Serializable]
	public class AtomCategory : AtomCommonAttributes
	{
		#region Fields

		private string scheme = null;
		private string term = null;
		private string label = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomCategory()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="text"></param>
		public AtomCategory(string term)
		{
			this.term = term;
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlAttribute("scheme")]
		public string Scheme
		{
			get { return this.scheme; }
			set { this.scheme = value; }
		}

		[DefaultValue(null)]
		[XmlAttribute("term")]
		public string Term
		{
			get { return this.term; }
			set { this.term = value; }
		}

		[DefaultValue(null)]
		[XmlAttribute("label")]
		public string Label
		{
			get { return this.label; }
			set { this.label = value; }
		}

		[XmlText]
		[DefaultValue(null)]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties

		#region Operators

		public static implicit operator AtomCategory(string value)
		{
			return new AtomCategory(value);
		}

		public static explicit operator string(AtomCategory value)
		{
			return value.Value;
		}

		#endregion Operators
	}

	#endregion AtomCategory

	#region AtomContent

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.1.3
	/// </summary>
	[Serializable]
	public class AtomContent : AtomText
	{
		#region Fields

		private string src = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomContent() { }

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="text"></param>
		public AtomContent(string text)
			: base(text)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="xhtml"></param>
		public AtomContent(XmlNode xhtml)
			: base(xhtml)
		{
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlAttribute("src")]
		public string Src
		{
			get { return this.src; }
			set { this.src = value; }
		}

		#endregion Properties

		#region Operators

		public static implicit operator AtomContent(string value)
		{
			return new AtomContent(value);
		}

		public static implicit operator AtomContent(XmlNode value)
		{
			return new AtomContent(value);
		}

		public static explicit operator string(AtomContent value)
		{
			return value.Value;
		}

		public static explicit operator XmlNode(AtomContent value)
		{
			return value.XhtmlValue;
		}

		#endregion Operators
	}

	#endregion AtomContent

	#region AtomDate

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-3.3
	/// </summary>
	[Serializable]
	public struct AtomDate
	{
		#region Fields

		private DateTime? value;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="date"></param>
		public AtomDate(DateTime date)
		{
			this.value = date;
		}

		#endregion Init

		#region Properties

		[XmlIgnore]
		public DateTime Value
		{
			get
			{
				if (!this.value.HasValue)
				{
					throw new InvalidOperationException("AtomDate object must have a value.");
				}
				return this.value.Value;
			}
			set { this.value = value; }
		}

		[XmlIgnore]
		public bool HasValue
		{
			get { return this.value.HasValue; }
		}

		/// <summary>
		/// Gets and sets the DateTime using ISO-8601 date format.
		/// For serialization purposes only, use the PubDate property instead.
		/// </summary>
		[XmlText]
		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string Value_Iso8601
		{
			get
			{
				if (!this.value.HasValue)
				{
					return null;
				}
				return this.value.Value.ToString("s")+'Z';
			}
			set
			{
				DateTime dateTime;
				if (!DateTime.TryParse(value, out dateTime))
				{
					this.value = null;
					return;
				}

				this.value = dateTime.ToUniversalTime();
			}
		}

		#endregion Properties

		#region Methods

		public DateTime GetValueOrDefault(DateTime defaultValue)
		{
			if (!this.value.HasValue)
			{
				return defaultValue;
			}
			return this.value.Value;
		}

		#endregion Methods

		#region Object Overrides

		public override string ToString()
		{
			return this.Value.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is AtomDate)
			{
				return this.value.Equals(((AtomDate)obj).value);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public override int GetHashCode()
		{
			if (!this.value.HasValue)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		#endregion Object Overrides

		#region Operators

		public static implicit operator AtomDate(DateTime value)
		{
			return new AtomDate(value);
		}

		public static explicit operator DateTime(AtomDate value)
		{
			return value.Value;
		}

		#endregion Operators
	}

	#endregion AtomDate

	#region AtomGenerator

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.4
	/// </summary>
	[Serializable]
	public class AtomGenerator : AtomCommonAttributes
	{
		#region Fields

		private string uri = null;
		private string version = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomGenerator()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="text"></param>
		public AtomGenerator(string text)
		{
			this.value = text;
		}

		#endregion Init

		#region Properties

		[XmlAttribute("uri")]
		[DefaultValue(null)]
		public string Uri
		{
			get { return this.uri; }
			set { this.uri = value; }
		}

		[XmlAttribute("version")]
		[DefaultValue(null)]
		public string Version
		{
			get { return this.version; }
			set { this.version = value; }
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

		#region Operators

		public static implicit operator AtomGenerator(string value)
		{
			return new AtomGenerator(value);
		}

		public static explicit operator string(AtomGenerator value)
		{
			return value.Value;
		}

		#endregion Operators
	}

	#endregion AtomGenerator

	#region AtomLink

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.7
	/// </summary>
	[Serializable]
	public class AtomLink : AtomCommonAttributes
	{
		#region Fields

		private string rel = null;
		private string type = null;
		private string href = null;
		private string hreflang = null;
		private string title = null;
		private string length = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomLink()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="link"></param>
		public AtomLink(string link)
		{
			this.href = link;
		}

		#endregion Init

		#region Properties

		[XmlAttribute("rel")]
		public string Rel
		{
			get { return this.rel; }
			set { this.rel = value; }
		}

		[XmlAttribute("type")]
		[DefaultValue(null)]
		public string Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		[XmlAttribute("href")]
		[DefaultValue(null)]
		public string Href
		{
			get { return this.href; }
			set { this.href = value; }
		}

		[XmlAttribute("hreflang")]
		[DefaultValue(null)]
		public string HrefLang
		{
			get { return this.hreflang; }
			set { this.hreflang = value; }
		}

		[XmlAttribute("length")]
		[DefaultValue(null)]
		public string Length
		{
			get { return this.length; }
			set { this.length = value; }
		}

		[XmlAttribute("title")]
		[DefaultValue(null)]
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
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
	public class AtomPerson : AtomCommonAttributes
	{
		#region Fields

		private string name = null;
		private string uri = null;
		private string email = null;

		#endregion Fields

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
	public class AtomText : AtomCommonAttributes
	{
		#region Fields

		private AtomTextType type = AtomTextType.Text;
		private string mediaType = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomText()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="text"></param>
		public AtomText(string text)
		{
			this.value = text;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="xhtml"></param>
		public AtomText(XmlNode xhtml)
		{
			this.XhtmlValue = xhtml;
		}

		#endregion Init

		#region Properties

		[DefaultValue(AtomTextType.Text)]
		[XmlIgnore]
		public AtomTextType Type
		{
			get { return this.type; }
			set
			{
				this.type = value;
				this.mediaType = null;
			}
		}

		[DefaultValue(null)]
		[XmlAttribute("type")]
		public string MediaType
		{
			get
			{
				if (this.type == AtomTextType.Text)
				{
					return this.mediaType;
				}

				return this.type.ToString().ToLowerInvariant();
			}
			set
			{
				try
				{
					// Enum.IsDefined doesn't allow case-insensitivity
					this.type = (AtomTextType)Enum.Parse(typeof(AtomTextType), value, true);
					this.mediaType = null;
				}
				catch
				{
					this.type = AtomTextType.Text;
					this.mediaType = value;
				}
			}
		}

		[XmlText]
		[DefaultValue(null)]
		public string Value
		{
			get
			{
				if (this.type == AtomTextType.Xhtml)
				{
					return null;
				}
				return this.value;
			}
			set { this.value = value; }
		}

		/// <summary>
		/// Gets and sets the Value using XmlNodes.
		/// For serialization purposes only, use the Value property instead.
		/// </summary>
		[DefaultValue(null)]
		[XmlAnyElement(Namespace="http://www.w3.org/1999/xhtml")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public XmlNode XhtmlValue
		{
			get
			{
				if (this.type != AtomTextType.Xhtml)
				{
					return null;
				}
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(this.value);
				return doc;
			}
			set
			{
				this.type = AtomTextType.Xhtml;
				this.value = value.OuterXml;
			}
		}

		#endregion Properties

		#region Object Overrides

		public override string ToString()
		{
			return this.Value;
		}

		#endregion Object Overrides

		#region Operators

		public static implicit operator AtomText(string value)
		{
			return new AtomText(value);
		}

		public static implicit operator AtomText(XmlNode value)
		{
			return new AtomText(value);
		}

		public static explicit operator string(AtomText value)
		{
			return value.Value;
		}

		public static explicit operator XmlNode(AtomText value)
		{
			return value.XhtmlValue;
		}

		#endregion Operators
	}

	#endregion AtomText

	#region AtomTextType

	public enum AtomTextType
	{
		[XmlEnum("text")]
		Text,

		[XmlEnum("html")]
		Html,

		[XmlEnum("xhtml")]
		Xhtml
	}

	#endregion AtomTextType
}
