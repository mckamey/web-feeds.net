using System;

namespace WebFeeds.Feeds
{
	/// <summary>
	/// Feed interface
	/// </summary>
	public interface IWebFeed
	{
		/// <summary>
		/// Gets the MIME Type designation for the feed
		/// </summary>
		string MimeType { get; }
	}
}
