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
using System.ComponentModel;
using System.Xml.Serialization;

using WebFeeds.Feeds.Modules;

namespace WebFeeds.Feeds.Rdf
{
	/// <summary>
	/// RDF 1.0 Root
	///		http://web.resource.org/rss/1.0/spec#s5.2
	/// </summary>
	[XmlRoot(RdfFeed.RootElement, Namespace=RdfFeed.NamespaceRdf)]
	public class RdfFeed : IWebFeed
	{
		#region Constants

		public const string SpecificationUrl = "http://web.resource.org/rss/1.0/spec";
		protected internal const string RootElement = "RDF";
		protected internal const string NamespaceRdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
		protected internal const string NamespaceRss10 = "http://purl.org/rss/1.0/";
		protected internal const string NamespaceDefault = "http://purl.org/rss/1.0/";
		protected internal const string MimeType = "application/rss+xml";

		#endregion Constants

		#region Fields

		private RdfChannel channel = null;
		private List<RdfItem> items = new List<RdfItem>();
		private RdfImage image = null;
		private RdfTextInput textInput = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfFeed()
		{
		}

		#endregion Init

		#region Properties

		[DefaultValue(null)]
		[XmlElement("channel", Namespace=RdfFeed.NamespaceRss10)]
		public RdfChannel Channel
		{
			get
			{
				if (this.channel == null)
				{
					this.channel = new RdfChannel();
					this.channel.SetParent(this);
				}

				return this.channel;
			}
			set
			{
				this.channel = value;
				this.channel.SetParent(this);
			}
		}

		/// <summary>
		/// Gets and sets an RDF association between the optional image element and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("image", Namespace=RdfFeed.NamespaceRss10)]
		public RdfImage Image
		{
			get
			{
				if (this.image == null)
				{
					this.image = new RdfImage();
				}

				return this.image;
			}
			set { this.image = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool ImageSpecified
		{
			get { return (this.image != null && !this.image.IsEmpty()); }
			set { }
		}

		[XmlElement("item", Namespace=RdfFeed.NamespaceRss10)]
		public List<RdfItem> Items
		{
			get { return this.items; }
			set { this.items = value; }
		}

		[XmlIgnore]
		public RdfItem this[int index]
		{
			get { return this.items[index]; }
			set { this.items[index] = value; }
		}

		/// <summary>
		/// Gets and sets an RDF association between the optional textinput element and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("textinput", Namespace=RdfFeed.NamespaceRss10)]
		public RdfTextInput TextInput
		{
			get
			{
				if (this.textInput == null)
				{
					this.textInput = new RdfTextInput();
				}

				return this.textInput;
			}
			set { this.textInput = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool TextInputSpecified
		{
			get { return (this.textInput != null && !this.textInput.IsEmpty()); }
			set { }
		}

		#endregion Properties

		#region IWebFeed Members

		[XmlIgnore]
		string IWebFeed.MimeType
		{
			get { return RdfFeed.MimeType; }
		}

		[XmlIgnore]
		XmlSerializerNamespaces IWebFeed.Namespaces
		{
			get
			{
				XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
				namespaces.Add("", RdfFeed.NamespaceRss10);
				namespaces.Add("rdf", RdfFeed.NamespaceRdf);
				namespaces.Add("dc", DublinCore.Namespace);
				return namespaces;
			}
		}

		#endregion IWebFeed Members
	}
}
