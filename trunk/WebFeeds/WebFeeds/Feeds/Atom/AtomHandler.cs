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
	public class AtomHandler : System.Web.IHttpHandler
	{
		#region Constants

		public const string AppSettingsKey_AtomXslt = "AtomXslt";

		#endregion Constants

		#region IHttpHandler Members

		bool IHttpHandler.IsReusable
		{
			get { return true; }
		}

		void IHttpHandler.ProcessRequest(System.Web.HttpContext context)
		{
			AtomFeed10 feed = null;
			try
			{
				feed = this.GenerateAtomFeed(context);
			}
			catch (Exception ex)
			{
				try { feed = this.HandleError(context, ex); }
				catch { }
			}
			AtomHandler.WriteAtomXml(context, feed);
		}

		#endregion IHttpHandler Members

		#region Atom Handler Methods

		/// <summary>
		/// Implementations should override this method to produce a custom Atom feed based upon the request URL.
		/// </summary>
		/// <param name="context">HttpContext provides access to request</param>
		/// <returns>AtomFeed10</returns>
		/// <remarks>
		/// The default implementation is a unit test which deserializes an Atom 1.0 feed
		/// located at the URL provided in the query string param "url".
		/// 
		/// This tests the round-trip serialization of the Atom object model.
		/// </remarks>
		protected virtual AtomFeed10 GenerateAtomFeed(System.Web.HttpContext context)
		{
			// this test code deserializes the Atom 1.0 feed and then serializes it
			string url = context.Request["url"];
			if (String.IsNullOrEmpty(url) || !url.StartsWith(Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
				return null;

			using (System.Net.WebClient client = new System.Net.WebClient())
			{
				using (System.IO.Stream stream = client.OpenRead(url))
				{
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(AtomFeed10));
					return serializer.Deserialize(stream) as AtomFeed10;
				}
			}
		}

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
		protected virtual AtomFeed10 HandleError(System.Web.HttpContext context, System.Exception exception)
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

		#endregion Atom Handler Methods

		#region Xslt Methods

		/// <summary>
		/// Creates the absolute url for the Atom XSLT.
		/// </summary>
		/// <param name="baseUri"></param>
		/// <returns></returns>
		private static string GetAtomXslt(Uri baseUri)
		{
			string atomXslt = System.Configuration.ConfigurationManager.AppSettings[AtomHandler.AppSettingsKey_AtomXslt];
			if (baseUri != null && !String.IsNullOrEmpty(atomXslt))
			{
				return new Uri(baseUri, atomXslt).AbsoluteUri;
			}

			return atomXslt;
		}

		/// <summary>
		/// Renders the XSLT processor instruction.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="baseUri"></param>
		private static void AddXsltInstruction(XmlWriter writer, Uri baseUri)
		{
			string atomXslt = AtomHandler.GetAtomXslt(baseUri);
			if (!String.IsNullOrEmpty(atomXslt))
			{
				// add a stylesheet for browser viewing
				writer.WriteProcessingInstruction("xml-stylesheet",
					String.Format("type=\"text/xsl\" href=\"{0}\" version=\"1.0\"", atomXslt));
			}
		}

		#endregion Xslt Methods

		#region Xml Methods

		/// <summary>
		/// Controls the XML serialization and response header generation.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="atom"></param>
		/// <remarks>
		/// This has been tweaked to specifically output XML according to Atom 1.0.
		/// </remarks>
		private static void WriteAtomXml(System.Web.HttpContext context, object atom)
		{
			context.Response.Clear();
			context.Response.ClearContent();
			context.Response.ClearHeaders();
			context.Response.ContentType = "application/xml";
			context.Response.ContentEncoding = System.Text.Encoding.UTF8;
			context.Response.AddHeader("Content-Disposition", "inline;filename=atom.xml");

			if (atom == null)
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

				AtomHandler.AddXsltInstruction(writer, context.Request.Url);

				// write out atom
				XmlSerializer serializer = new XmlSerializer(atom.GetType());
				serializer.Serialize(writer, atom);
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