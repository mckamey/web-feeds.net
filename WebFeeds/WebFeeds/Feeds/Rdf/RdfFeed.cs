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
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rdf
{
	/// <summary>
	/// RDF 1.0 Root
	///		http://web.resource.org/rss/1.0/spec#s5.2
	/// </summary>
	public abstract class RdfFeedBase : ExtensibleBase
	{
		#region Constants

		public const string SpecificationUrl = "http://web.resource.org/rss/1.0/spec";
		protected internal const string RootElement = "RDF";
		protected internal const string NamespaceRdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
		protected internal const string NamespaceRss10 = "http://purl.org/rss/1.0/";
		protected internal const string NamespaceDefault = "http://purl.org/rss/1.0/";
		public const string MimeType = "application/rss+xml";

		#endregion Constants

		#region Fields

		private RdfChannel channel = null;
		private RdfImage image = null;
		private RdfTextInput textInput = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("channel", Namespace=RdfFeed.NamespaceRss10)]
		public RdfChannel Channel
		{
			get
			{
				if (this.channel == null)
				{
					this.channel = new RdfChannel();
					this.channel.SetParent((RdfFeed)this);
				}

				return this.channel;
			}
			set
			{
				this.channel = value;
				this.channel.SetParent((RdfFeed)this);
			}
		}

		/// <summary>
		/// Gets and sets an RDF association between the optional image element and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("image", Namespace=RdfFeed.NamespaceRss10)]
		public RdfImage Image
		{
			get
			{
				if (this.image == null)
				{
					this.image = new RdfImage();
				}

				return this.image;
			}
			set { this.image = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ImageSpecified
		{
			get { return (this.image != null && !this.image.IsEmpty()); }
			set { }
		}

		/// <summary>
		/// Gets and sets an RDF association between the optional textinput element and this particular RSS channel.
		/// </summary>
		[DefaultValue(null)]
		[XmlElement("textinput", Namespace=RdfFeed.NamespaceRss10)]
		public RdfTextInput TextInput
		{
			get
			{
				if (this.textInput == null)
				{
					this.textInput = new RdfTextInput();
				}

				return this.textInput;
			}
			set { this.textInput = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool TextInputSpecified
		{
			get { return (this.textInput != null && !this.textInput.IsEmpty()); }
			set { }
		}

		#endregion Properties
	}

	/// <summary>
	/// RDF 1.0 Root
	///		http://web.resource.org/rss/1.0/spec#s5.2
	/// </summary>
	/// <remarks>
	/// XmlSerializer serializes public fields before public properties
	/// and serializes base class members before derriving class members.
	/// Since RssChannel uses a readonly field for Items it must be placed
	/// in a derriving class in order to make sure items serialize last.
	/// </remarks>
	[XmlRoot(RdfFeed.RootElement, Namespace=RdfFeed.NamespaceRdf)]
	public class RdfFeed : RdfFeedBase, IWebFeed
	{
		#region Fields

		[XmlElement("item", Namespace=RdfFeed.NamespaceRss10)]
		public readonly List<RdfItem> Items = new List<RdfItem>();

		#endregion Fields

		#region Properties

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ItemsSpecified
		{
			get { return (this.Items.Count > 0); }
			set { }
		}

		[XmlIgnore]
		public RdfItem this[int index]
		{
			get { return this.Items[index]; }
			set { this.Items[index] = value; }
		}

		#endregion Properties

		#region IWebFeed Members

		string IWebFeed.MimeType
		{
			get { return RdfFeed.MimeType; }
		}

		string IWebFeed.Copyright
		{
			get { return this.Channel.Copyright; }
		}

		IList<IWebFeedItem> IWebFeed.Items
		{
			get { return this.Items.ToArray(); }
		}

		#endregion IWebFeed Members

		#region IWebFeedBase Members

		Uri IWebFeedBase.ID
		{
			get { return ((IWebFeedBase)this.Channel).ID; }
		}

		string IWebFeedBase.Title
		{
			get { return ((IWebFeedBase)this.Channel).Title; }
		}

		string IWebFeedBase.Description
		{
			get { return ((IWebFeedBase)this.Channel).Description; }
		}

		string IWebFeedBase.Author
		{
			get { return ((IWebFeedBase)this.Channel).Author; }
		}

		DateTime? IWebFeedBase.Published
		{
			get { return ((IWebFeedBase)this.Channel).Published; }
		}

		DateTime? IWebFeedBase.Updated
		{
			get { return ((IWebFeedBase)this.Channel).Updated; }
		}

		Uri IWebFeedBase.Link
		{
			get { return ((IWebFeedBase)this.Channel).Link; }
		}

		Uri IWebFeedBase.ImageLink
		{
			get { return ((IUriProvider)this.Image).Uri; }
		}

		#endregion IWebFeedBase Members

		#region INamespaceProvider Members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			namespaces.Add("", RdfFeed.NamespaceRss10);
			namespaces.Add("rdf", RdfFeed.NamespaceRdf);

			this.Channel.AddNamespaces(namespaces);
			if (this.ImageSpecified)
			{
				this.Image.AddNamespaces(namespaces);
			}
			if (this.TextInputSpecified)
			{
				this.TextInput.AddNamespaces(namespaces);
			}

			foreach (RdfItem item in this.Items)
			{
				item.AddNamespaces(namespaces);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}
}
