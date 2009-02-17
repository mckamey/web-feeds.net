#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2006-2009 Stephen M. McKamey

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
#endregion License

using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Atom
{
	/// <summary>
	/// Common shared Atom attributes
	///		http://tools.ietf.org/html/rfc4287#section-2
	/// </summary>
	/// <remarks>
	/// atomCommonAttributes
	///		attribute xml:base?
	///		attribute xml:lang?
	/// </remarks>
	public abstract class AtomCommonAttributes : ExtensibleBase
	{
		#region Constants

		public const string XmlPrefix = "xml";
		public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
		public const string ThreadingPrefix = "thr";
		public const string ThreadingNamespace = "http://purl.org/syndication/thread/1.0";

		#endregion Constants

		#region Fields

		private Uri xmlBase = null;
		private CultureInfo xmlLanguage = CultureInfo.InvariantCulture;

		#endregion Fields

		#region Properties

		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[XmlAttribute("lang", Namespace=AtomCommonAttributes.XmlNamespace)]
		public string XmlLanguage
		{
			get { return this.xmlLanguage.Name; }
			set { this.xmlLanguage = CultureInfo.GetCultureInfo(value); }
		}

		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[XmlAttribute("base", Namespace=AtomCommonAttributes.XmlNamespace)]
		public string XmlBase
		{
			get { return ExtensibleBase.ConvertToString(this.xmlBase); }
			set { this.xmlBase = ExtensibleBase.ConvertToUri(value); }
		}

		#endregion Properties
	}

	/// <summary>
	/// Commonly shared Atom base
	/// </summary>
	/// <remarks>
	/// atomBase
	///		atomAuthor*
	///		atomCategory*
	///		atomContributor*
	///		atomId
	///		atomLink*
	///		atomRights?
	///		atomTitle
	///		atomUpdated
	/// </remarks>
	public abstract class AtomBase : AtomCommonAttributes, IUriProvider
	{
		#region Fields

		private Uri id = null;
		private AtomText rights = null;
		private AtomText title = new AtomText();
		private AtomDate updated = new AtomDate();

		#endregion Fields

		#region Properties

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("id")]
		public string ID
		{
			get
			{
				string value = ExtensibleBase.ConvertToString(this.id);
				return String.IsNullOrEmpty(value) ? String.Empty : value;
			}
			set { this.id = ExtensibleBase.ConvertToUri(value); }
		}

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("title")]
		public AtomText Title
		{
			get { return this.title; }
			set { this.title = (value == null) ? new AtomText() : value; }
		}

		[XmlElement("author")]
		public readonly List<AtomPerson> Authors = new List<AtomPerson>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool AuthorsSpecified
		{
			get { return (this.Authors.Count > 0); }
			set { }
		}

		[XmlElement("category")]
		public readonly List<AtomCategory> Categories = new List<AtomCategory>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool CategoriesSpecified
		{
			get { return (this.Categories.Count > 0); }
			set { }
		}

		[XmlElement("contributor")]
		public readonly List<AtomPerson> Contributors = new List<AtomPerson>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ContributorsSpecified
		{
			get { return (this.Contributors.Count > 0); }
			set { }
		}

		[XmlElement("link")]
		public readonly List<AtomLink> Links = new List<AtomLink>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool LinksSpecified
		{
			get { return (this.Links.Count > 0); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("rights")]
		public AtomText Rights
		{
			get { return this.rights; }
			set { this.rights = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool RightsSpecified
		{
			get { return (this.rights != null); }
			set { }
		}

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("updated")]
		public virtual AtomDate Updated
		{
			get { return this.updated; }
			set { this.updated = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool UpdatedSpecified
		{
			get { return true; }
			set { }
		}

		#endregion Properties

		#region INamespaceProvider members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			foreach (AtomLink link in this.Links)
			{
				link.AddNamespaces(namespaces);
			}

			foreach (AtomCategory category in this.Categories)
			{
				category.AddNamespaces(namespaces);
			}

			foreach (AtomPerson person in this.Authors)
			{
				person.AddNamespaces(namespaces);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider members

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.id; }
		}

		#endregion IUriProvider Members
	}
}
