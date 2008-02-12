using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Atom
{
	/// <summary>
	/// Common shared Atom base
	/// </summary>
	/// <remarks>
	/// atomBase
	///		atomAuthor*
	///		atomCategory*
	///		atomContributor*
	///		atomId
	///		atomLink*
	///		atomRights?
	///		atomTitle
	///		atomUpdated
	/// </remarks>
	public abstract class AtomBase
	{
		#region Fields

		private string id = null;
		private AtomText rights = null;
		private AtomText title = null;
		private AtomDate updated = null;

		private List<AtomPerson> authors = new List<AtomPerson>();
		private List<AtomText> categories = new List<AtomText>();
		private List<AtomPerson> contributors = new List<AtomPerson>();
		private List<AtomLink> links = new List<AtomLink>();

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("author")]
		public List<AtomPerson> Authors
		{
			get { return this.authors; }
			set { this.authors = value; }
		}

		[DefaultValue(null)]
		[XmlElement("category")]
		public List<AtomText> Categories
		{
			get { return this.categories; }
			set { this.categories = value; }
		}

		[DefaultValue(null)]
		[XmlElement("contributor")]
		public List<AtomPerson> Contributors
		{
			get { return this.contributors; }
			set { this.contributors = value; }
		}

		[DefaultValue(null)]
		[XmlElement("id")]
		public string ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		[DefaultValue(null)]
		[XmlElement("link")]
		public List<AtomLink> Links
		{
			get { return this.links; }
			set { this.links = value; }
		}

		[DefaultValue(null)]
		[XmlElement("rights")]
		public AtomText Rights
		{
			get { return this.rights; }
			set { this.rights = value; }
		}

		[DefaultValue(null)]
		[XmlElement("title")]
		public AtomText Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		[DefaultValue(null)]
		[XmlElement("updated")]
		public AtomDate Updated
		{
			get { return this.updated; }
			set { this.updated = value; }
		}

		#endregion Properties
	}

	/// <summary>
	/// http://tools.ietf.org/html/rfc4287#section-4.2.11
	/// </summary>
	/// <remarks>
	/// atomSource : atomBase
	///		atomGenerator?
	///		atomIcon?
	///		atomSubtitle?
	/// </remarks>
	public class AtomSource : AtomBase
	{
		#region Fields

		private string generator = null;
		private string icon = null;
		private AtomText subtitle = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("generator")]
		public string Generator
		{
			get { return this.generator; }
			set { this.generator = value; }
		}

		[DefaultValue(null)]
		[XmlElement("icon")]
		public string Icon
		{
			get { return this.icon; }
			set { this.icon = value; }
		}

		[DefaultValue(null)]
		[XmlElement("subtitle")]
		public AtomText SubTitle
		{
			get { return this.subtitle; }
			set { this.subtitle = value; }
		}

		#endregion Properties
	}
}
