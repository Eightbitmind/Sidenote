using Microsoft.Office.Interop.OneNote;
using Sidenote.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Sidenote.DOM
{
	internal class Section : Node, IIdentifiableObject, INamedObject, IUserCreatedObject, ISection
	{
		#region INode members

		public override IList<INode> Children
		{
			get
			{
				if (this.children == null)
				{
					this.children = new List<INode>();
					string childrenXml;
					ApplicationManager.Application.GetHierarchy(
						this.ID,
						Microsoft.Office.Interop.OneNote.HierarchyScope.hsChildren,
						out childrenXml);
					Debug.Assert(!string.IsNullOrEmpty(childrenXml));
					var textReader = new StringReader(childrenXml);

					var xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.IgnoreComments = true;
					xmlReaderSettings.IgnoreWhitespace = true;
					xmlReaderSettings.IgnoreProcessingInstructions = true;
					XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);

					if (!SectionContentParser.Instance.Parse(xmlReader, this))
					{
						throw new Exception("could not parse section content");
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

		#region ISection members

		public string Path { get; }
		public string Color { get; }

		#endregion

		internal Section(
			uint depth,
			INode parent,
			string name,
			string id,
			DateTime lastModifiedTime,
			string path,
			string Color)
			: base(type: "Section", depth: depth, parent: parent)
		{
			this.ID = id;
			this.Name = name;
			this.LastModifiedTime = lastModifiedTime;
			this.Path = path;
			this.Color = Color;
		}
	}
}
