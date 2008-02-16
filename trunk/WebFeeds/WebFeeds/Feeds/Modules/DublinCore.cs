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
using System.ComponentModel;
using System.Xml.Serialization;

namespace WebFeeds.Feeds.Modules
{
	#region DublinCore

	/// <summary>
	/// Dublin Core
	///		http://dublincore.org/documents/dces/
	///		http://web.resource.org/rss/1.0/modules/dc/
	/// </summary>
	[Serializable]
	public class DublinCore
	{
		#region Constants

		public const string Namespace = "http://purl.org/dc/elements/1.1/";

		#endregion Constants

		#region Fields

		private string title = null;
		private string creator = null;
		private string subject = null;
		private string description = null;
		private string publisher = null;
		private string contributor = null;
		private string date = null;
		private string type = null;
		private string format = null;
		private string identifier = null;
		private string source = null;
		private string language = null;
		private string relation = null;
		private string coverage = null;
		private string rights = null;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[XmlElement("title", Namespace=DublinCore.Namespace)]
		public string DcTitle
		{
			get { return this.title; }
			set { this.title = value; }
		}

		[DefaultValue(null)]
		[XmlElement("creator", Namespace=DublinCore.Namespace)]
		public string DcCreator
		{
			get { return this.creator; }
			set { this.creator = value; }
		}

		[DefaultValue(null)]
		[XmlElement("subject", Namespace=DublinCore.Namespace)]
		public string DcSubject
		{
			get { return this.subject; }
			set { this.subject = value; }
		}

		[DefaultValue(null)]
		[XmlElement("description", Namespace=DublinCore.Namespace)]
		public string DcDescription
		{
			get { return this.description; }
			set { this.description = value; }
		}

		[DefaultValue(null)]
		[XmlElement("publisher", Namespace=DublinCore.Namespace)]
		public string DcPublisher
		{
			get { return this.publisher; }
			set { this.publisher = value; }
		}

		[DefaultValue(null)]
		[XmlElement("contributor", Namespace=DublinCore.Namespace)]
		public string DcContributor
		{
			get { return this.contributor; }
			set { this.contributor = value; }
		}

		[DefaultValue(null)]
		[XmlElement("date", Namespace=DublinCore.Namespace)]
		public string DcDate
		{
			get { return this.date; }
			set { this.date = value; }
		}

		[DefaultValue(null)]
		[XmlElement("type", Namespace=DublinCore.Namespace)]
		public string DcType
		{
			get { return this.type; }
			set { this.type = value; }
		}

		[DefaultValue(null)]
		[XmlElement("format", Namespace=DublinCore.Namespace)]
		public string DcFormat
		{
			get { return this.format; }
			set { this.format = value; }
		}

		[DefaultValue(null)]
		[XmlElement("identifier", Namespace=DublinCore.Namespace)]
		public string DcIdentifier
		{
			get { return this.identifier; }
			set { this.identifier = value; }
		}

		[DefaultValue(null)]
		[XmlElement("source", Namespace=DublinCore.Namespace)]
		public string DcSource
		{
			get { return this.source; }
			set { this.source = value; }
		}

		[DefaultValue(null)]
		[XmlElement("language", Namespace=DublinCore.Namespace)]
		public string DcLanguage
		{
			get { return this.language; }
			set { this.language = value; }
		}

		[DefaultValue(null)]
		[XmlElement("relation", Namespace=DublinCore.Namespace)]
		public string DcRelation
		{
			get { return this.relation; }
			set { this.relation = value; }
		}

		[DefaultValue(null)]
		[XmlElement("coverage", Namespace=DublinCore.Namespace)]
		public string DcCoverage
		{
			get { return this.coverage; }
			set { this.coverage = value; }
		}

		[DefaultValue(null)]
		[XmlElement("rights", Namespace=DublinCore.Namespace)]
		public string DcRights
		{
			get { return this.rights; }
			set { this.rights = value; }
		}

		#endregion Properties
	}

	#endregion DublinCore
}