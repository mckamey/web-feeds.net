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

namespace WebFeeds.Feeds.Atom
{
	/// <summary>
	/// Based upon Atom 1.0
	///		http://tools.ietf.org/html/rfc4287
	/// </summary>
	public class AtomHandler : FeedHandler
	{
		#region Properties

		public override string AppSettingsKey
		{
			get { return "AtomXslt"; }
		}

		protected override string MimeType
		{
			get { return "application/atom+xml"; }
		}

		protected override Type FeedType
		{
			get { return typeof(AtomFeed10); }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Implementations should override this method to handle errors during Atom generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		/// <returns>AtomFeed10</returns>
		/// <remarks>
		/// The default implementation handles any exceptions during the Atom generation by
		/// producing the exception stack trace as a valid Atom document.
		/// </remarks>
		protected override object HandleError(HttpContext context, System.Exception exception)
		{
			AtomFeed10 feed = new AtomFeed10();
			feed.Updated = new AtomDate(DateTime.UtcNow);
			feed.Title = new AtomText("Server Error");
			feed.SubTitle = new AtomText("An error occurred while generating this feed. See feed items for details.");

			//AtomCategory atomCategory = new AtomCategory("error");
			//feed.Categories.Add(atomCategory);

			while (exception != null)
			{
				AtomEntry entry = new AtomEntry();
				entry.Title = new AtomText(exception.GetType().Name);

#if DEBUG
				entry.Summary = new AtomText("<pre>"+exception+"</pre>");
				entry.Summary.Type = AtomTextType.html;
#else
				entry.Summary = new AtomText(exception.Message);
#endif
				AtomLink link = new AtomLink(exception.HelpLink);
				entry.Links.Add(link);
				entry.Published = feed.Updated;
				feed.Entries.Add(entry);

				exception = exception.InnerException;
			}

			return feed;
		}

		#endregion Methods
	}
}