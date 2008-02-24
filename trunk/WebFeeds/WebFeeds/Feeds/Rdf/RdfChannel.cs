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

namespace WebFeeds.Feeds.Rdf
{
	/// <summary>
	/// RDF 1.0 Channel
	///		http://web.resource.org/rss/1.0/spec#s5.3
	/// </summary>
	[Serializable]
	[XmlType("channel", Namespace=RdfFeed.NamespaceRss10)]
	public class RdfChannel : RdfItem
	{
		#region Fields

		private RdfFeed parent = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets an RDF association between the optional image element
		/// and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("image", Namespace=RdfFeed.NamespaceRss10)]
		public RdfResource Image
		{
			get
			{
				if (this.parent == null ||
					!this.parent.ImageSpecified)
				{
					return null;
				}

				return new RdfResource(this.parent.Image);
			}
			set {  }
		}

		/// <summary>
		/// Gets and sets an RDF table of contents, associating the document's items
		/// with this particular RSS channel.
		/// </summary>
		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("items", Namespace=RdfFeed.NamespaceRss10)]
		public RdfSequence Items
		{
			get { return new RdfSequence(this.parent); }
			set {  }
		}

		/// <summary>
		/// Gets and sets an RDF association between the optional image element and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("textinput", Namespace=RdfFeed.NamespaceRss10)]
		public RdfResource TextInput
		{
			get
			{
				if (this.parent == null ||
					!this.parent.TextInputSpecified)
				{
					return null;
				}

				return new RdfResource(this.parent.TextInput);
			}
			set { }
		}

		#endregion Properties

		#region Methods

		internal void SetParent(RdfFeed feed)
		{
			this.parent = feed;
		}

		#endregion Methods
	}
}