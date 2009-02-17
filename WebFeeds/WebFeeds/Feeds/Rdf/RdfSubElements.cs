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
using System.Collections.Generic;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rdf
{
	#region RdfImage

	/// <summary>
	/// RDF 1.0 Image
	///		http://web.resource.org/rss/1.0/spec#s5.4
	/// </summary>
	[Serializable]
	[XmlType("image", Namespace=RdfFeed.NamespaceRss10)]
	public class RdfImage : RdfBase, IUriProvider
	{
		#region Fields

		private Uri url = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("url", Namespace=RdfFeed.NamespaceRss10)]
		public string Url
		{
			get { return ExtensibleBase.ConvertToString(this.url); }
			set { this.url = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets 
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("about", Namespace=RdfFeed.NamespaceRdf)]
		public override string About
		{
			get { return this.Url; }
			set { this.Url = value; }
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Url) &&
				String.IsNullOrEmpty(this.Title);
		}

		#endregion Methods

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.url; }
		}

		#endregion IUriProvider Members
	}

	#endregion RdfImage

	#region RdfTextInput

	/// <summary>
	/// RDF 1.0 TextInput
	///		http://web.resource.org/rss/1.0/spec#s5.6
	/// </summary>
	[Serializable]
	[XmlType("input", Namespace=RdfFeed.NamespaceRss10)]
	public class RdfTextInput : RdfItem
	{
		#region Fields

		private string name = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("name", Namespace=RdfFeed.NamespaceRss10)]
		public string Name
		{
			get { return this.name; }
			set { this.name = String.IsNullOrEmpty(value) ? null : value; }
		}

		#endregion Properties

		#region Methods

		public bool IsEmpty()
		{
			return String.IsNullOrEmpty(this.Title) &&
				String.IsNullOrEmpty(this.Description) &&
				String.IsNullOrEmpty(this.Link) &&
				String.IsNullOrEmpty(this.Name);
		}

		#endregion Methods
	}

	#endregion RdfTextInput

	#region RdfResource

	[XmlType("li", Namespace=RdfFeed.NamespaceRss10)]
	public class RdfResource
	{
		#region Fields

		private RdfBase target = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfResource()
		{
			this.target = null;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfResource(RdfBase target)
		{
			this.target = target;
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets the RDF association for the target
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute("resource", Namespace=RdfFeed.NamespaceRdf)]
		public string Resource
		{
			get { return (this.target != null) ? this.target.About : null; }
			set { }
		}

		#endregion Properties

		#region Operators

		public static implicit operator RdfResource(RdfBase value)
		{
			return new RdfResource(value);
		}

		public static explicit operator RdfBase(RdfResource value)
		{
			return value.target;
		}

		#endregion Operators
	}

	#endregion RdfResource

	#region RdfSequence

	public class RdfSequence
	{
		#region Fields

		private RdfFeed target = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfSequence()
		{
			this.target = null;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfSequence(RdfFeed target)
		{
			this.target = target;
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlArray("Seq", Namespace=RdfFeed.NamespaceRdf)]
		public List<RdfResource> Items
		{
			get
			{
				if (this.target == null ||
		            this.target.Items == null ||
		            this.target.Items.Count == 0)
				{
					return null;
				}

				List<RdfResource> items = new List<RdfResource>(this.target.Items.Count);
				foreach (RdfBase item in this.target.Items)
				{
					items.Add(new RdfResource(item));
				}
				return items;
			}
			set { }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ItemsSpecified
		{
			get
			{
				List<RdfResource> items = this.Items;

				return (items != null) && (items.Count > 0);
			}
			set { }
		}

		#endregion Properties
	}

	#endregion RdfSequence
}
