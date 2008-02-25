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
using System.Web;
using System.ComponentModel;
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

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ElementExtensionsSpecified
		{
			get { return this.ElementExtensions.Count > 0; }
			set { }
		}

		[XmlAnyAttribute]
		public readonly List<XmlAttribute> AttributeExtensions = new List<XmlAttribute>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool AttributeExtensionsSpecified
		{
			get { return this.AttributeExtensions.Count > 0; }
			set { }
		}

		#endregion Fields

		#region Methods

		/// <summary>
		/// Applies the extensions in adapter to ExtensibleBase
		/// </summary>
		/// <param name="adapter"></param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		#region Utility Methods

		public static string ConvertToString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.Utc);
		}

		public static DateTime? ConvertToDateTime(string value)
		{
			DateTime dateTime;
			if (!DateTime.TryParse(value, out dateTime))
			{
				return null;
			}
			return dateTime;
		}

		public static string ConvertToString(Uri uri)
		{
			if (uri == null)
			{
				return null;
			}

			return HttpUtility.UrlPathEncode(uri.ToString());
		}

		public static Uri ConvertToUri(string value)
		{
			Uri uri;
			if (String.IsNullOrEmpty(value) ||
				!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out uri))
			{
				return null;
			}

			return uri;
		}

		#endregion Utility Methods
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