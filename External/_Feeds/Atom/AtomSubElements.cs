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
	#region AtomContent

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.2
	/// </summary>
	[Serializable]
	public class AtomCategory
	{
		#region Fields

		private string label = null;
		private string scheme = null;
		private string term = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomCategory() { }

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
		[XmlAttribute("label")]
		public string Label
		{
			get { return this.label; }
			set { this.label = value; }
		}

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

		[XmlText]
		[DefaultValue(null)]
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties
	}

	#endregion AtomContent

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
		public AtomContent(string text) : base(text)
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

		private DateTime? value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomDate() { }

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
			get { return this.value.Value; }
			set { this.value = value; }
		}

		[XmlText]
		[DefaultValue(null)]
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

		#region Object Overrides

		public override string ToString()
		{
			return this.Value.ToString();
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
		private string hreflang = null;
		private string type = null;
		private string title = null;
		private string length = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomLink() { }

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
		/// Ctor
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

		private AtomTextType textType = AtomTextType.text;
		private string type = null;
		private string value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public AtomText() { }

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="text"></param>
		public AtomText(string text)
		{
			this.value = text;
		}

		#endregion Init

		#region Properties

		[XmlIgnore]
		[DefaultValue(AtomTextType.text)]
		public AtomTextType TextType
		{
			get { return this.textType; }
			set
			{
				this.textType = value;
				this.type = value.ToString();
			}
		}

		[DefaultValue(null)]
		[XmlAttribute("type")]
		public string Type
		{
			get { return this.type; }
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.textType = AtomTextType.text;
					this.type = null;
					return;
				}

				this.type = value;
				try
				{
					this.textType = (AtomTextType)Enum.Parse(
						typeof(AtomTextType),
						value.Substring(value.IndexOf("/")+1), // crude MIME parse
						false);
				}
				catch
				{
					this.textType = AtomTextType.text;
				}
			}
		}

		System.Xml.XmlNode xhtmlValue = null;
		[DefaultValue(null)]
		[XmlElement("div", Namespace="http://www.w3.org/1999/xhtml")]
		public System.Xml.XmlNode XhtmlValue
		{
			get { return this.xhtmlValue; }
			set { this.xhtmlValue = value; }
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
