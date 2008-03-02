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
using System.Xml.Serialization;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Feed interface
	/// </summary>
	public interface IWebFeed : IWebFeedItem, INamespaceProvider
	{
		/// <summary>
		/// Gets the MIME Type designation for the feed
		/// </summary>
		string MimeType { get; }

		/// <summary>
		/// Gets the link to an image
		/// </summary>
		Uri ImageLink { get; }

		/// <summary>
		/// Gets the copyright
		/// </summary>
		string Copyright { get; }

		/// <summary>
		/// Gets a list of feed items
		/// </summary>
		IList<IWebFeedItem> Items { get; }
	}

	/// <summary>
	/// Item interface
	/// </summary>
	public interface IWebFeedItem
	{
		/// <summary>
		/// Gets a unique identifier
		/// </summary>
		Uri ID { get; }

		/// <summary>
		/// Gets the title
		/// </summary>
		string Title { get; }

		/// <summary>
		/// Gets the description
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets the author
		/// </summary>
		string Author { get; }

		/// <summary>
		/// Gets the initial date published
		/// </summary>
		DateTime? Published { get; }

		/// <summary>
		/// Gets the date last updated
		/// </summary>
		DateTime? Updated { get; }

		/// <summary>
		/// Gets the link to the full version
		/// </summary>
		Uri Link { get; }

		/// <summary>
		/// Gets the link to comments on this item
		/// </summary>
		Uri ThreadLink { get; }

		/// <summary>
		/// Gets the number of comments on this item
		/// </summary>
		int? ThreadCount { get; }

		/// <summary>
		/// Gets the number of comments on this item
		/// </summary>
		DateTime? ThreadUpdated { get; }
	}

	public interface INamespaceProvider
	{
		/// <summary>
		/// Adds additional namespace URIs for the feed
		/// </summary>
		void AddNamespaces(XmlSerializerNamespaces namespaces);
	}

	public interface IUriProvider
	{
		Uri Uri { get; }
	}
}
