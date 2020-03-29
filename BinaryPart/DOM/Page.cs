using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
				//if (this.children == null)
				//{
				//	this.children = new List<INode>();
				//	IFormatter formatter = FormatterManager.PageContentFormatter;
				//	bool success = formatter.Deserialize(this);
				//	Debug.Assert(success);
				//}

				//return this.children;

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
		public DateTime CreationTime { get; }
		public DateTime LastModifiedTime { get; }

		#endregion

		#region IPage members

		public uint PageLevel { get; }

		#endregion

		internal Page(
			uint depth,
			INode parent,
			string name,
			string id,
			DateTime creationTime,
			DateTime lastModifiedTime,
			uint pageLevel)
			: base(type: "Page", depth: depth, parent: parent)
		{
			this.ID = id;
			this.Name = name;
			this.CreationTime = creationTime;
			this.LastModifiedTime = lastModifiedTime;
			this.PageLevel = pageLevel;
		}
	}
}
