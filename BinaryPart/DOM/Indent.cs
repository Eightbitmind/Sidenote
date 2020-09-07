using System;

namespace Sidenote.DOM
{
	public class Indent
	{
		// <xsd:attribute name="indent" type="PositiveDecimal" use="required"/>
		// PositiveDecimal:
		//   <xsd:restriction base="xsd:double">
		//     <xsd:minInclusive value = "0.00" />
		//     <xsd:maxInclusive value = "1000000.00" />
 		//   </ xsd:restriction>
		public double Indentation
		{
			get
			{
				return this.indentation;
			}
			set
			{
				if (value < 0.0 || value > 1000000.0)
				{
					throw new ArgumentOutOfRangeException("Indentation");
				}

				this.indentation = value;
			}
		}

		// <xsd:attribute name="level" type="xsd:nonNegativeInteger" use="required"/>
		public uint Level { get; set; }

		private double indentation;
	}
}
