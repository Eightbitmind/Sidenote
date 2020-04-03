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

					if (!PageContentParser.Instance.Parse(xmlReader, this))
					{
						throw new Exception("could not parse page content");
					}
				}
				return this.children;
			}
		}

		#endregion

		#region IIdentifiableObject members

		public string ID { get; }

		#endregion

		#region INamedObject members

		public string Name { get; }

		#endregion

		#region IUserCreatedObject members

		public string Author { get; }
		public string AuthorInitials { get; }
		public DateTime CreationTime { get; set; }
		public DateTime LastModifiedTime { get; set; }

		#endregion

		#region IPage members

		public uint PageLevel { get; }

		public void Save()
		{
			StringBuilder pageContent = new StringBuilder();

			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;

			XmlWriter xmlWriter = XmlWriter.Create(
				pageContent,
				// @"C:\Users\aeulitz\AppData\Local\Temp\Sidenote.SerializationTest.xml",
				xmlWriterSettings);

			PageContentParser.Instance.Serialize(this, xmlWriter);

			xmlWriter.Flush();
			xmlWriter.Close();

			ApplicationManager.Application.UpdatePageContent(pageContent.ToString(), this.LastModifiedTime.ToUniversalTime(), XMLSchema.xs2013, false);
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
			this.ID = id;
			this.Name = name;
			this.PageLevel = pageLevel;
		}
	}
}
