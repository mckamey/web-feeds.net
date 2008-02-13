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

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Base class for feed generator handlers.
	/// </summary>
	public abstract class FeedHandler : IHttpHandler
	{
		#region Properties

		/// <summary>
		/// Gets the AppSettings Key which designates where to point the XSLT.
		/// </summary>
		public abstract string AppSettingsKey
		{
			get;
		}

		/// <summary>
		/// Gets the MIME Type designation for this feed type
		/// </summary>
		protected abstract string MimeType
		{
			get;
		}

		/// <summary>
		/// Gets the Type of the root Feed object
		/// </summary>
		protected abstract Type FeedType
		{
			get;
		}

		#endregion Properties

		#region IHttpHandler Members

		bool IHttpHandler.IsReusable
		{
			get { return true; }
		}

		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			object feed = null;
			try
			{
				feed = this.GenerateFeed(context);
			}
			catch (Exception ex)
			{
				try { feed = this.HandleError(context, ex); }
				catch { }
			}
			this.WriteFeedXml(context, feed);
		}

		#endregion IHttpHandler Members

		#region Feed Handler Methods

		/// <summary>
		/// Implementations should override this method to produce a custom feed based upon the request URL.
		/// </summary>
		/// <param name="context">HttpContext provides access to request</param>
		/// <returns>Feed</returns>
		/// <remarks>
		/// The default implementation is a unit test which deserializes a feed
		/// located at the URL provided in the query string param "url".
		/// 
		/// This tests the round-trip serialization of the Feed object model.
		/// </remarks>
		protected virtual object GenerateFeed(HttpContext context)
		{
			// this test code deserializes the feed and then serializes it
			string url = context.Request["url"];
			if (String.IsNullOrEmpty(url) || !url.StartsWith(Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			using (System.Net.WebClient client = new System.Net.WebClient())
			{
				using (System.IO.Stream stream = client.OpenRead(url))
				{
					XmlSerializer serializer = new XmlSerializer(this.FeedType);
					return serializer.Deserialize(stream);
				}
			}
		}

		/// <summary>
		/// Implementations should override this method to handle errors during Feed generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		/// <returns>Feed</returns>
		/// <remarks>
		/// The default implementation handles any exceptions during the Feed generation by
		/// producing the exception stack trace as a valid Feed document.
		/// </remarks>
		protected abstract object HandleError(HttpContext context, System.Exception exception);

		#endregion Feed Handler Methods

		#region Xslt Methods

		/// <summary>
		/// Creates the absolute url for the Feed XSLT.
		/// </summary>
		/// <param name="baseUri"></param>
		/// <returns></returns>
		private string GetFeedXslt(Uri baseUri)
		{
			string feedXslt = System.Configuration.ConfigurationManager.AppSettings[this.AppSettingsKey];
			if (baseUri != null && !String.IsNullOrEmpty(feedXslt))
			{
				return new Uri(baseUri, feedXslt).AbsoluteUri;
			}

			return feedXslt;
		}

		/// <summary>
		/// Renders the XSLT processor instruction.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="baseUri"></param>
		private void AddXsltInstruction(XmlWriter writer, Uri baseUri)
		{
			string feedXslt = this.GetFeedXslt(baseUri);
			if (!String.IsNullOrEmpty(feedXslt))
			{
				// add a stylesheet for browser viewing
				writer.WriteProcessingInstruction("xml-stylesheet",
					String.Format("type=\"text/xsl\" href=\"{0}\" version=\"1.0\"", feedXslt));
			}
		}

		#endregion Xslt Methods

		#region Xml Methods

		/// <summary>
		/// Controls the XML serialization and response header generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="feed"></param>
		/// <remarks>
		/// This has been tweaked to specifically output XML according to Feed.
		/// </remarks>
		private void WriteFeedXml(HttpContext context, object feed)
		{
			context.Response.Clear();
			context.Response.ClearContent();
			context.Response.ClearHeaders();
			context.Response.ContentType = this.MimeType;
			context.Response.ContentEncoding = System.Text.Encoding.UTF8;
			context.Response.AddHeader("Content-Disposition", "inline;filename=feed.xml");

			XmlWriter writer = null;
			try
			{
				if (feed == null)
				{
					return;
				}

				// setup document formatting, make human readable
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.CheckCharacters = true;
				settings.CloseOutput = true;
				settings.ConformanceLevel = ConformanceLevel.Document;
				settings.Encoding = System.Text.Encoding.UTF8;
				settings.Indent = true;
				settings.IndentChars = "\t";
				writer = XmlWriter.Create(context.Response.OutputStream, settings);

				this.AddXsltInstruction(writer, context.Request.Url);

				// write out feed
				XmlSerializer serializer = new XmlSerializer(feed.GetType());
				serializer.Serialize(writer, feed);
			}
			catch (Exception ex)
			{
				context.Response.Write(ex);
			}
			finally
			{
				if (context.ApplicationInstance != null)
				{
					// prevents "Transfer-Encoding: Chunked" header which chokes IE6 (unlike Response.Flush/Close)
					// and prevents ending response too early (unlike Response.End)
					context.ApplicationInstance.CompleteRequest();
				}
			}
		}

		#endregion Xml Methods
	}
}