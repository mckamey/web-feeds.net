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
using System.Xml.Serialization;
using System.Globalization;

using WebFeeds.Feeds.Extensions;

namespace WebFeeds.Feeds.Rss
{
	/// <summary>
	/// RSS 2.0 Channel
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	[Serializable]
	public abstract class RssChannelBase : RssBase, IUriProvider
	{
		#region Fields

		// required
		private string title = String.Empty;
		private Uri link = null;
		private string description = String.Empty;

		// optional
		private CultureInfo language = CultureInfo.InvariantCulture;
		private string copyright = null;
		private RssPerson managingEditor = null;
		private RssPerson webMaster = null;
		private RssDate pubDate;
		private RssDate lastBuildDate;
		private string generator = null;
		private string docs = null;
		private RssCloud cloud = null;
		private int ttl = Int32.MinValue;
		private RssImage image = null;
		private string rating = null;
		private RssTextInput textInput = null;
		private RssSkipHours skipHours = null;
		private RssSkipDays skipDays = null;

		#endregion Fields

		#region Properties

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("title")]
		public string Title
		{
			get { return this.title; }
			set { this.title = String.IsNullOrEmpty(value) ? String.Empty : value; }
		}

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
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

		/// <remarks>
		/// Required even if empty.
		/// </remarks>
		[XmlElement("description")]
		public string Description
		{
			get { return this.description; }
			set { this.description = String.IsNullOrEmpty(value) ? String.Empty : value; }
		}

		[DefaultValue("")]
		[XmlElement("language")]
		public string Language
		{
			get { return this.language.Name; }
			set
			{
				if (value == null)
				{
					value = String.Empty;
				}
				this.language = CultureInfo.GetCultureInfo(value.Trim());
			}
		}

		[DefaultValue(null)]
		[XmlElement("copyright")]
		public string Copyright
		{
			get { return this.copyright; }
			set { this.copyright = String.IsNullOrEmpty(value) ? null : value; }
		}

		/// <summary>
		/// Gets and sets the managing editor of the channel.
		/// </summary>
		[XmlElement("managingEditor")]
		public RssPerson ManagingEditor
		{
			get
			{
				if (this.managingEditor == null)
				{
					this.managingEditor = new RssPerson();
				}

				return this.managingEditor;
			}
			set { this.managingEditor = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ManagingEditorSpecified
		{
			get { return (this.managingEditor != null && !this.managingEditor.IsEmpty()); }
			set { }
		}

		/// <summary>
		/// Gets and sets the webMaster of the channel.
		/// </summary>
		[XmlElement("webMaster")]
		public RssPerson WebMaster
		{
			get
			{
				if (this.webMaster == null)
				{
					this.webMaster = new RssPerson();
				}

				return this.webMaster;
			}
			set { this.webMaster = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool WebMasterSpecified
		{
			get { return (this.webMaster != null && !this.webMaster.IsEmpty()); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("pubDate")]
		public RssDate PubDate
		{
			get { return this.pubDate; }
			set { this.pubDate = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool PubDateSpecified
		{
			get { return this.pubDate.HasValue; }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("lastBuildDate")]
		public RssDate LastBuildDate
		{
			get { return this.lastBuildDate; }
			set { this.lastBuildDate = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool LastBuildDateSpecified
		{
			get { return this.lastBuildDate.HasValue; }
			set { }
		}

		[XmlElement("category")]
		public readonly List<RssCategory> Categories = new List<RssCategory>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool CategoriesSpecified
		{
			get { return (this.Categories.Count > 0); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("generator")]
		public string Generator
		{
			get { return this.generator; }
			set { this.generator = String.IsNullOrEmpty(value) ? null : value; }
		}

		[DefaultValue(null)]
		[XmlElement("docs")]
		public string Docs
		{
			get { return this.docs; }
			set { this.docs = String.IsNullOrEmpty(value) ? null : value; }
		}

		[DefaultValue(null)]
		[XmlElement("cloud")]
		public RssCloud Cloud
		{
			get
			{
				if (this.cloud == null)
				{
					this.cloud = new RssCloud();
				}

				return this.cloud;
			}
			set { this.cloud = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool CloudSpecified
		{
			get { return (this.cloud != null && !this.cloud.IsEmpty()); }
			set { }
		}

		[DefaultValue(Int32.MinValue)]
		[XmlElement("ttl")]
		public int Ttl
		{
			get { return this.ttl; }
			set
			{
				if (value < 0)
					this.ttl = Int32.MinValue;
				else
					this.ttl = value;
			}
		}

		[DefaultValue(null)]
		[XmlElement("image")]
		public RssImage Image
		{
			get
			{
				if (this.image == null)
				{
					this.image = new RssImage();
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

		[DefaultValue(null)]
		[XmlElement("rating")]
		public string Rating
		{
			get { return this.rating; }
			set { this.rating = String.IsNullOrEmpty(value) ? null : value; }
		}

		[XmlElement("textInput")]
		public RssTextInput TextInput
		{
			get
			{
				if (this.textInput == null)
				{
					this.textInput = new RssTextInput();
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

		[DefaultValue(null)]
		[XmlElement("skipHours")]
		public RssSkipHours SkipHours
		{
			get
			{
				if (this.skipHours == null)
				{
					this.skipHours = new RssSkipHours();
				}

				return this.skipHours;
			}
			set { this.skipHours = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool SkipHoursSpecified
		{
			get { return (this.skipHours != null && !this.skipHours.IsEmpty()); }
			set { }
		}

		[DefaultValue(null)]
		[XmlElement("skipDays")]
		public RssSkipDays SkipDays
		{
			get
			{
				if (this.skipDays == null)
				{
					this.skipDays = new RssSkipDays();
				}

				return this.skipDays;
			}
			set { this.skipDays = value; }
		}

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool SkipDaysSpecified
		{
			get { return (this.skipDays != null && !this.skipDays.IsEmpty()); }
			set { }
		}

		#endregion Properties

		#region IUriProvider Members

		Uri IUriProvider.Uri
		{
			get { return this.link; }
		}

		#endregion IUriProvider Members
	}

	/// <summary>
	/// RSS 2.0 Channel
	///		http://blogs.law.harvard.edu/tech/rss
	/// </summary>
	/// <remarks>
	/// XmlSerializer serializes public fields before public properties
	/// and serializes base class members before derriving class members.
	/// Since RssChannel uses a readonly field for Items it must be placed
	/// in a derriving class in order to make sure items serialize last.
	/// </remarks>
	public class RssChannel : RssChannelBase
	{
		#region Properties

		[XmlElement("item")]
		public readonly List<RssItem> Items = new List<RssItem>();

		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ItemsSpecified
		{
			get { return (this.Items.Count > 0); }
			set { }
		}

		#endregion Properties

		#region INamespaceProvider Members

		public override void AddNamespaces(XmlSerializerNamespaces namespaces)
		{
			foreach (RssItem item in this.Items)
			{
				item.AddNamespaces(namespaces);
			}

			base.AddNamespaces(namespaces);
		}

		#endregion INamespaceProvider Members
	}
}