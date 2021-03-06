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
using System.Xml;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rdf
{
	/// <summary>
	/// RDF 1.0 Base
	///		http://web.resource.org/rss/1.0/spec#s5.3
	/// </summary>
	[Serializable]
	public abstract class RdfBase : ExtensibleBase, IUriProvider
	{
		#region Fields

		private string title = String.Empty;
		private Uri link = null;
		private string about = null;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets and sets a descriptive title for the channel.
		/// </summary>
		/// <remarks>
		/// Suggested maximum length is 40 characters.
		/// Required even if empty.
		/// </remarks>
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = String.IsNullOrEmpty(value) ? String.Empty : value; }
		}

		/// <summary>
		/// Gets and sets the URL to which an HTML rendering of the channel title will link,
		/// commonly the parent site's home or news page.
		/// </summary>
		/// <remarks>
		/// Suggested maximum length is 500 characters.
		/// </remarks>
		[DefaultValue(null)]
		[XmlElement("link")]
		public string Link
		{
			get
			{
				string value = ExtensibleBase.ConvertToString(this.link);
				return String.IsNullOrEmpty(value) ? String.Empty : value;
			}
			set { this.link = ExtensibleBase.ConvertToUri(value); }
		}

		/// <summary>
		/// Gets and sets a URL link to the described resource
		/// </summary>
		[XmlAttribute("about", Namespace=RdfFeed.NamespaceRdf)]
		public virtual string About
		{
			get
			{
				if (String.IsNullOrEmpty(this.about))
				{
					return this.Link;
				}
				return this.about;
			}
			set { this.about = String.IsNullOrEmpty(value) ? String.Empty : value; }
		}

		#endregion Properties

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.link; }
		}

		#endregion IUriProvider Members
	}
}