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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Extensions
{
	/// <summary>
	/// Dublin Core Metadata Element Set, Version 1.1
	///		http://dublincore.org/documents/dces/
	///		http://web.resource.org/rss/1.0/modules/dc/
	/// </summary>
	public class DublinCore : IExtensionAdapter
	{
		#region Constants

		public const string Prefix = "dc";
		public const string Namespace = "http://purl.org/dc/elements/1.1/";

		private static readonly XmlDocument NodeCreator = new XmlDocument();

		#endregion Constants

		#region Fields

		private readonly Dictionary<TermName, XmlElement> DcTerms = new Dictionary<TermName, XmlElement>();

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets the values for DublinCore extensions
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public string this[DublinCore.TermName term]
		{
			get
			{
				if (!this.DcTerms.ContainsKey(term))
				{
					return null;
				}

				return this.DcTerms[term].InnerText;
			}
			set { this.Add(term, value); }
		}

		public ICollection<TermName> Terms
		{
			get { return this.DcTerms.Keys; }
		}

		#endregion Properties

		#region Methods

		public void Add(DublinCore.TermName term, string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				this.Remove(term);
				return;
			}

			XmlElement node = DublinCore.NodeCreator.CreateElement(
				DublinCore.Prefix,
				term.ToString().ToLowerInvariant(), //TODO: use the XmlEnumAttribute to convert the term name
				DublinCore.Namespace);

			node.InnerText = value;

			this.DcTerms[term] = node;
		}

		public bool Remove(DublinCore.TermName term)
		{
			return this.DcTerms.Remove(term);
		}

		#endregion Methods

		#region IExtensionProvider Members

		IEnumerable<XmlAttribute> IExtensionAdapter.GetAttributeEntensions()
		{
			// Dublin Core does not specify attributes
			return null;
		}

		IEnumerable<XmlElement> IExtensionAdapter.GetElementExtensions()
		{
			if (this.DcTerms.Count < 1)
			{
				return null;
			}

			List<XmlElement> extensions = new List<XmlElement>(this.DcTerms.Count);

			foreach (DublinCore.TermName term in this.DcTerms.Keys)
			{
				extensions.Add(this.DcTerms[term]);
			}

			return extensions;
		}

		void IExtensionAdapter.SetAttributeEntensions(IEnumerable<XmlAttribute> attributes)
		{
			// Dublin Core does not specify attributes
		}

		void IExtensionAdapter.SetElementExtensions(IEnumerable<XmlElement> elements)
		{
			foreach (XmlElement element in elements)
			{
				if (!DublinCore.Namespace.Equals(element.NamespaceURI, StringComparison.InvariantCulture))
				{
					continue;
				}

				try
				{
					TermName term = (TermName)Enum.Parse(typeof(TermName), element.LocalName, true);
					this.DcTerms[term] = element;
				}
				catch
				{
					continue;
				}
			}
		}

		#endregion IExtensionProvider Members

		#region TermName enum

		public enum TermName
		{
			[XmlEnum("contributor")]
			Contributor,

			[XmlEnum("coverage")]
			Coverage,

			[XmlEnum("creator")]
			Creator,

			[XmlEnum("date")]
			Date,

			[XmlEnum("description")]
			Description,

			[XmlEnum("format")]
			Format,

			[XmlEnum("identifier")]
			Identifier,

			[XmlEnum("language")]
			Language,

			[XmlEnum("publisher")]
			Publisher,

			[XmlEnum("relation")]
			Relation,

			[XmlEnum("rights")]
			Rights,

			[XmlEnum("source")]
			Source,

			[XmlEnum("subject")]
			Subject,

			[XmlEnum("title")]
			Title,

			[XmlEnum("type")]
			Type
		}

		#endregion TermName enum
	}
}