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
using System.Xml;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Rss
{
	/// <summary>
	/// Based upon RSS 2.0
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	public class RssHandler : FeedHandler
	{
		#region Properties

		public override string AppSettingsKey
		{
			get { return "WebFeeds.RssXslt"; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Implementations should override this method to handle errors during RSS generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		/// <returns>RssFeed</returns>
		/// <remarks>
		/// The default implementation handles any exceptions during the RSS generation by
		/// producing the exception stack trace as a valid RSS document.
		/// </remarks>
		protected override IWebFeed HandleError(HttpContext context, Exception exception)
		{
			RssFeed feed = new RssFeed();
			feed.Channel.LastBuildDate = DateTime.UtcNow;
			feed.Channel.Title = "Server Error";
			feed.Channel.Description = "An error occurred while generating this feed. See feed items for details.";

			RssCategory rssCategory = new RssCategory();
			rssCategory.Value = "error";
			feed.Channel.Categories.Add(rssCategory);

			while (exception != null)
			{
				RssItem item = new RssItem();
				item.Title = exception.GetType().Name;
#if DEBUG
				item.Description = "<pre>"+exception+"</pre>";
#else
				item.Description = exception.Message;
#endif
				item.Link = exception.HelpLink;
				item.PubDate = feed.Channel.LastBuildDate;
				feed.Channel.Items.Add(item);

				exception = exception.InnerException;
			}

			return feed;
		}

		#endregion Methods
	}
}