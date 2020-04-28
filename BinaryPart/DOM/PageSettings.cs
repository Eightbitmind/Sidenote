namespace Sidenote.DOM
{
	internal class PageSettings : Node, IPageSettings
	{
		#region IPageSettings members

		public bool Rtl {
			get
			{
				return this.rtl;
			}

			internal set
			{
				this.rtl = value;
			}
		}

		public string Color
		{
			get
			{
				return this.color;
			}

			internal set
			{
				this.color = value;
			}
		}

		#endregion

		internal PageSettings(
			uint depth,
			INode parent)
			: base(type: "PageSettings", depth: depth, parent: parent)
		{
		}

		internal static bool RtlDefaultValue = false;
		private bool rtl = PageSettings.RtlDefaultValue;

		internal static string ColorDefaultValue = "automatic";
		private string color = PageSettings.ColorDefaultValue;
	}
}
