namespace Sidenote.DOM
{
	internal class QuickStyle : Node, INamedObject, IQuickStyle
	{
		#region INamedObject members

		public string Name { get; }

		#endregion

		#region IQuickStyle members

		public uint Index { get; }

		public string FontColor
		{
			get
			{
				return this.fontColor;
			}
			internal set
			{
				this.fontColor = value;
			}
		}

		public string HighlightColor
		{
			get
			{
				return this.highlightColor;
			}
			internal set
			{
				this.highlightColor = value;
			}
		}

		public string FontName { get; }
		public double FontSize { get; internal set; }

		public bool Bold
		{
			get
			{
				return this.bold;
			}

			internal set
			{
				this.bold = value;
			}
		}

		public bool Italic
		{
			get
			{
				return this.italic;
			}

			internal set
			{
				this.italic = value;

			}
		}

		public bool Underline
		{
			get
			{
				return this.underline;
			}

			internal set
			{
				this.underline = value;

			}
		}

		public bool Strikethrough
		{
			get
			{
				return this.strikethrough;
			}

			internal set
			{
				this.strikethrough = value;

			}
		}

		public bool Superscript
		{
			get
			{
				return this.superscript;
			}

			internal set
			{
				this.superscript = value;

			}
		}

		public bool Subscript
		{
			get
			{
				return this.subscript;
			}

			internal set
			{
				this.subscript = value;
			}
		}

		public float SpaceBefore
		{
			get
			{
				return this.spaceBefore;
			}

			internal set
			{
				this.spaceBefore = value;
			}
		}

		public float SpaceAfter
		{
			get
			{
				return this.spaceAfter;
			}

			internal set
			{
				this.spaceAfter = value;
			}
		}

		#endregion

		internal QuickStyle(
			uint index,
			string name,
			string fontName,
			double fontSize,
			uint depth,
			INode parent)
			: base(type: "QuickStyle", depth: depth, parent: parent)
		{
			this.Name = name;
			this.Index = index;
			this.FontName = fontName;
			this.FontSize = fontSize;
		}

		internal static string FontColorDefaultValue = "automatic";
		private string fontColor = QuickStyle.FontColorDefaultValue;

		internal static string HighlightColorDefaultValue = "automatic";
		private string highlightColor = QuickStyle.HighlightColorDefaultValue;

		internal static bool BoldDefaultValue = false;
		private bool bold = QuickStyle.BoldDefaultValue;

		internal static bool ItalicDefaultValue = false;
		private bool italic = QuickStyle.ItalicDefaultValue;

		internal static bool UnderlineDefaultValue = false;
		private bool underline = QuickStyle.UnderlineDefaultValue;

		internal static bool StrikethroughDefaultValue = false;
		private bool strikethrough = QuickStyle.StrikethroughDefaultValue;

		internal static bool SuperscriptDefaultValue = false;
		private bool superscript = QuickStyle.SuperscriptDefaultValue;

		internal static bool SubscriptDefaultValue = false;
		private bool subscript = QuickStyle.SubscriptDefaultValue;

		internal static float SpaceBeforeDefaultValue = 0;
		private float spaceBefore = QuickStyle.SpaceBeforeDefaultValue;

		internal static float SpaceAfterDefaultValue = 0;
		private float spaceAfter = QuickStyle.SpaceAfterDefaultValue;
	}
}
