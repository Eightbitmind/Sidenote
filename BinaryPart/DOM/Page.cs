using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Sidenote.DOM
{
	internal class Page : Node, IIdentifiableObject, INamedObject, IUserCreatedObject, IPage
	{
		#region INode members

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
					this.DeserializeContent();
				}
				return this.children;
			}
		}

		#endregion

		#region IIdentifiableObject members

		public string ID
		{
			get
			{
				// this.DeserializeContent();
				return this.id;
			}
		}

		#endregion

		#region INamedObject members

		public string Name
		{
			get
			{
				// this.DeserializeContent();
				return this.name;
			}
		}

		#endregion

		#region IUserCreatedObject members

		public string Author
		{
			get
			{
				this.DeserializeContent();
				return this.author;

			}
		}

		public string AuthorInitials
		{
			get
			{
				this.DeserializeContent();
				return this.authorInitials;
			}
		}

		public DateTime CreationTime
		{
			get
			{
				this.DeserializeContent();
				return this.creationTime;
			}

			set
			{
				this.creationTime = value;
			}
		}

		public DateTime LastModifiedTime
		{
			get
			{
				this.DeserializeContent();
				return this.lastModifiedTime;
			}

			set
			{
				this.lastModifiedTime = value;
			}
		}

		#endregion

		#region IPage members

		public uint PageLevel {
			get
			{
				// this.DeserializeContent();
				return this.pageLevel;
			}
		}

		public void Save()
		{
			this.DeserializeContent();

			StringBuilder pageContent = new StringBuilder();

			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;

			string fileName = Path.GetTempPath() + "\\Sidenote.xml";
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			XmlWriter xmlWriter = XmlWriter.Create(
				// pageContent,
				fileName,
				xmlWriterSettings);

			PageContentFormatter.Instance.Serialize(this, xmlWriter);

			xmlWriter.Flush();
			xmlWriter.Close();

			// ApplicationManager.Application.UpdatePageContent(pageContent.ToString(), this.LastModifiedTime.ToUniversalTime(), XMLSchema.xs2013, false);
		}

		#endregion

		public DateTime EntryCreationTime { get; set; }
		public DateTime EntryLastModifiedTime { get; set; }

		internal Page(
			uint depth,
			INode parent,
			string name,
			string id,
			uint pageLevel)
			: base(type: "Page", depth: depth, parent: parent)
		{
			this.id = id;
			this.name = name;
			this.pageLevel = pageLevel;
		}

		private string id;
		private string name;
		private string author;
		private string authorInitials;
		private DateTime creationTime;
		private DateTime lastModifiedTime;
		private uint pageLevel;

		private bool contentDeserialized = false;

		private void DeserializeContent()
		{
			if (this.contentDeserialized) return;
			this.contentDeserialized = true;

			string pageXml;
			ApplicationManager.Application.GetPageContent(
				this.ID,
				out pageXml,
				PageInfo.piBasic, // 'piBasic' is the default
				XMLSchema.xs2013);
			Debug.Assert(!string.IsNullOrEmpty(pageXml));
			var textReader = new StringReader(pageXml);

			var xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

			if (!PageContentFormatter.Instance.Deserialize(xmlReader, this))
			{
				throw new Exception("could not parse page content");
			}
		}
	}
}
