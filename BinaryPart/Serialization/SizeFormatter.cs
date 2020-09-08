using Sidenote.DOM;
using System.Xml;

namespace Sidenote.Serialization
{
	internal class SizeFormatter : FormatterBase<SizeFormatter>
	{
		public SizeFormatter() : base("Size") { }

		protected override bool IsHandledObject(object obj)
		{
			return obj is IPositionedObject;
		}

		protected override bool DeserializeAttributes(XmlReader reader, object parent, PatchStore patchStore)
		{
			var deserializedObject = (IPositionedObject)parent;

			Size size = new Size();

			string widthStr = reader.GetAttribute(WidthAttributeName);
			if (!string.IsNullOrEmpty(widthStr))
			{
				size.Width = double.Parse(widthStr);
			}

			string heightStr = reader.GetAttribute(HeightAttributeName);
			if (!string.IsNullOrEmpty(heightStr))
			{
				size.Height = double.Parse(heightStr);
			}

			deserializedObject.Size = size;
			return true;
		}

		protected override void SerializeAttributes(object obj, XmlWriter writer)
		{
			var serializedObject = (IPositionedObject) obj;
			writer.WriteAttributeString(WidthAttributeName, Converter.ToString(serializedObject.Size.Width));
			writer.WriteAttributeString(HeightAttributeName, Converter.ToString(serializedObject.Size.Height));
		}

		private static string HeightAttributeName = "height";
		private static string WidthAttributeName = "width";
	}
}
