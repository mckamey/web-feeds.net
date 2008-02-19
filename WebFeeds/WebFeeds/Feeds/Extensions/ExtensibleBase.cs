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
	/// Allows any generic extensions expressed as XmlElements and XmlAttributes
	/// </summary>
	public abstract class ExtensibleBase : INamespaceProvider
	{
		#region Fields

		[XmlAnyElement]
		public readonly List<XmlElement> ElementExtensions = new List<XmlElement>();

		[XmlAnyAttribute]
		public readonly List<XmlAttribute> AttributeExtensions = new List<XmlAttribute>();

		#endregion Fields

		#region Methods

		/// <summary>
		/// Applies the extensions in adapter to ExtensibleBase
		/// </summary>
		/// <param name="adapter"></param>
		public void AddExtensions(IExtensionAdapter adapter)
		{
			if (adapter == null)
			{
				return;
			}

			IEnumerable<XmlAttribute> attributes = adapter.GetAttributeEntensions();
			if (attributes != null)
			{
				this.AttributeExtensions.AddRange(attributes);
			}

			IEnumerable<XmlElement> elements = adapter.GetElementExtensions();
			if (elements != null)
			{
				this.ElementExtensions.AddRange(elements);
			}
		}

		/// <summary>
		/// Extracts the extensions in this ExtensibleBase into adapter
		/// </summary>
		/// <param name="adapter"></param>
		public void FillExtensions(IExtensionAdapter adapter)
		{
			if (adapter == null)
			{
				return;
			}

			adapter.SetAttributeEntensions(this.AttributeExtensions);
			adapter.SetElementExtensions(this.ElementExtensions);
		}

		#endregion Methods

		#region INamespaceProvider Members

		public virtual void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			foreach (XmlNode node in this.AttributeExtensions)
			{
				namespaces.Add(node.Prefix, node.NamespaceURI);
			}
			foreach (XmlNode node in this.ElementExtensions)
			{
				namespaces.Add(node.Prefix, node.NamespaceURI);
			}
		}

		#endregion INamespaceProvider Members
	}

	/// <summary>
	/// Interface that adapters implement to apply / extract additional elements and attributes
	/// </summary>
	public interface IExtensionAdapter
	{
		IEnumerable<XmlAttribute> GetAttributeEntensions();
		IEnumerable<XmlElement> GetElementExtensions();

		void SetAttributeEntensions(IEnumerable<XmlAttribute> attributes);
		void SetElementExtensions(IEnumerable<XmlElement> elements);
	}
}