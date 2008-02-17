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

namespace WebFeeds.Feeds.Modules
{
	public class FeedExtension
	{
		#region Fields

		private List<XmlNode> elements = new List<XmlNode>();
		private List<XmlNode> attributes = new List<XmlNode>();

		#endregion Fields

		#region Properties

		[XmlAnyAttribute]
		public List<XmlNode> ExtendedAttributes
		{
			get { return this.attributes; }
			set { this.attributes = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool ExtendedAttributesSpecified
		{
			get { return (this.attributes != null && this.attributes.Count > 0); }
		}

		[XmlAnyElement]
		public List<XmlNode> ExtendedElements
		{
			get { return this.elements; }
			set { this.elements = value; }
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool ExtendedElementsSpecified
		{
			get { return (this.elements != null && this.elements.Count > 0); }
		}

		public virtual void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			foreach (XmlNode node in this.ExtendedAttributes)
			{
				namespaces.Add(node.Prefix, node.NamespaceURI);
			}
			foreach (XmlNode node in this.ExtendedElements)
			{
				namespaces.Add(node.Prefix, node.NamespaceURI);
			}
		}

		#endregion Properties
	}
}
