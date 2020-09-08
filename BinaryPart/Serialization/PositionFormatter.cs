using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class PositionFormatter : FormatterBase<PositionFormatter>
	{
		public PositionFormatter() : base("Position") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is IPositionedObject;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var deserializedObject = (IPositionedObject)parent;

			Position position = new Position();

			string xStr = reader.GetAttribute(XAttributeName);
			if (!string.IsNullOrEmpty(xStr))
			{
				position.X = double.Parse(xStr);
			}

			string yStr = reader.GetAttribute(YAttributeName);
			if (!string.IsNullOrEmpty(xStr))
			{
				position.Y = double.Parse(yStr);
			}

			string zStr = reader.GetAttribute(ZAttributeName);
			if (!string.IsNullOrEmpty(zStr))
			{
				position.Z = uint.Parse(zStr);
			}

			deserializedObject.Position = position;
			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (IPositionedObject)obj;
			writer.WriteAttributeString(XAttributeName, Converter.ToString(serializedObject.Position.X));
			writer.WriteAttributeString(YAttributeName, Converter.ToString(serializedObject.Position.Y));
			writer.WriteAttributeString(ZAttributeName, Converter.ToString(serializedObject.Position.Z));
		}

		private static string XAttributeName = "x";
		private static string YAttributeName = "y";
		private static string ZAttributeName = "z";
	}
}
