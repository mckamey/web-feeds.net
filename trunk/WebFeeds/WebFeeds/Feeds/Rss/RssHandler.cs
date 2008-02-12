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
	public class RssHandler : System.Web.IHttpHandler
	{
		#region Constants

		public const string AppSettingsKey_RssXslt = "RssXslt";

		#endregion Constants

		#region IHttpHandler Members

		bool IHttpHandler.IsReusable
		{
			get { return true; }
		}

		void IHttpHandler.ProcessRequest(System.Web.HttpContext context)
		{
			RssFeed feed = null;
			try
			{
				feed = this.GenerateRssFeed(context);
			}
			catch (Exception ex)
			{
				try { feed = this.HandleError(context, ex); }
				catch { }
			}
			RssHandler.WriteRssXml(context, feed);
		}

		#endregion IHttpHandler Members

		#region RSS Handler Methods

		/// <summary>
		/// Implementations should override this method to produce a custom RSS feed based upon the request URL.
		/// </summary>
		/// <param name="context">HttpContext provides access to request</param>
		/// <returns>RssFeed</returns>
		/// <remarks>
		/// The default implementation is a unit test which deserializes an RSS 2.0 feed
		/// located at the URL provided in the query string param "url".
		/// 
		/// This tests the round-trip serialization of the RSS object model.
		/// </remarks>
		protected virtual RssFeed GenerateRssFeed(System.Web.HttpContext context)
		{
			// this test code deserializes the RSS 2.0 feed and then serializes it
			string url = context.Request["url"];
			if (String.IsNullOrEmpty(url) || !url.StartsWith(Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
				return null;

			using (System.Net.WebClient client = new System.Net.WebClient())
			{
				using (System.IO.Stream stream = client.OpenRead(url))
				{
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(RssFeed));
					return serializer.Deserialize(stream) as RssFeed;
				}
			}
		}

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
		protected virtual RssFeed HandleError(System.Web.HttpContext context, System.Exception exception)
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

		#endregion RSS Handler Methods

		#region Xslt Methods

		/// <summary>
		/// Creates the absolute url for the RSS XSLT.
		/// </summary>
		/// <param name="baseUri"></param>
		/// <returns></returns>
		private static string GetRssXslt(Uri baseUri)
		{
			string rssXslt = System.Configuration.ConfigurationManager.AppSettings[RssHandler.AppSettingsKey_RssXslt];
			if (baseUri != null && !String.IsNullOrEmpty(rssXslt))
			{
				return new Uri(baseUri, rssXslt).AbsoluteUri;
			}

			return rssXslt;
		}

		/// <summary>
		/// Renders the XSLT processor instruction.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="baseUri"></param>
		private static void AddXsltInstruction(XmlWriter writer, Uri baseUri)
		{
			string rssXslt = RssHandler.GetRssXslt(baseUri);
			if (!String.IsNullOrEmpty(rssXslt))
			{
				// add a stylesheet for browser viewing
				writer.WriteProcessingInstruction("xml-stylesheet",
					String.Format("type=\"text/xsl\" href=\"{0}\" version=\"1.0\"", rssXslt));
			}
		}

		#endregion Xslt Methods

		#region Xml Methods

		/// <summary>
		/// Controls the XML serialization and response header generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="rss"></param>
		/// <remarks>
		/// This has been tweaked to specifically output XML according to RSS 2.0.
		/// </remarks>
		private static void WriteRssXml(System.Web.HttpContext context, object rss)
		{
			context.Response.Clear();
			context.Response.ClearContent();
			context.Response.ClearHeaders();
			context.Response.ContentType = "application/xml";
			context.Response.ContentEncoding = System.Text.Encoding.UTF8;
			context.Response.AddHeader("Content-Disposition", "inline;filename=rss.xml");

			if (rss == null)
				return;

			XmlWriter writer = null;
			try
			{
				// setup document formatting, make human readable
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.CheckCharacters = true;
				settings.CloseOutput = true;
				settings.ConformanceLevel = ConformanceLevel.Document;
				settings.Encoding = System.Text.Encoding.UTF8;
				settings.Indent = true;
				settings.IndentChars = "\t";
				writer = XmlWriter.Create(context.Response.OutputStream, settings);

				RssHandler.AddXsltInstruction(writer, context.Request.Url);

				// write out rss
				XmlSerializer serializer = new XmlSerializer(rss.GetType());
				serializer.Serialize(writer, rss);
			}
			catch (Exception ex)
			{
				context.Response.Write(ex);
			}
			finally
			{
				if (writer != null)
				{
					writer.Flush();
					writer.Close();
				}
			}
		}

		#endregion Xml Methods
	}
}