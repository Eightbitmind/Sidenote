using Sidenote.DOM;
using System;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class OEChildrenParser : ParserBase<OEChildrenParser>
	{
		public OEChildrenParser() : base("OEChildren") { }

		protected override bool ParseChildren(XmlReader reader, INode parent)
		{
			while (reader.IsStartElement())
			{
				if (!(
					OEParser.Instance.Parse(reader, parent)
				))
				{
					throw new Exception("unexpected OEChildren child element " + reader.LocalName);
				}
			}

			return true;
		}
	}
}
