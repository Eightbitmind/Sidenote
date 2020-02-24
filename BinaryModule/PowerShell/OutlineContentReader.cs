using Sidenote.DOM;
using Sidenote.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Provider;

// Findings:
// - The Select-String cmdlet does not appear to engange DriveProvider.GetContentReader(). I'm
//   guessing that Select-String is performing its own file read operations. It is for that reason
//   that I stop developing the code in this class until I have compelling usage scenario.
// - The Select-String cmdlet passes on objects of type Microsoft.PowerShell.Commands.MatchInfo
//   with the following members:
//       Name         MemberType Definition
//       ----         ---------- ----------
//       Equals       Method     bool Equals(System.Object obj)
//       GetHashCode  Method     int GetHashCode()
//       GetType      Method     type GetType()
//       RelativePath Method     string RelativePath(string directory)
//       ToString     Method     string ToString(), string ToString(string directory)
//       Context      Property   Microsoft.PowerShell.Commands.MatchInfoContext Context { get; set; }
//       Filename     Property   string Filename { get; }
//       IgnoreCase   Property   bool IgnoreCase { get; set; }
//       Line         Property   string Line { get; set; }
//       LineNumber   Property   int LineNumber { get; set; }
//       Matches      Property   System.Text.RegularExpressions.Match[] Matches { get; set; }
//       Path         Property   string Path { get; set; }
//       Pattern      Property   string Pattern { get; set; }
// - For searches in files, the 'Path' member is the path name of the file in which the search
//   pattern was found; the line 'LineNumber' property is the number of the line in which the
//   search pattern was found; the 'Context' property is null; the 'Line' property is the text of
//   the line in which the pattern was found.

namespace Sidenote.PowerShell
{
	internal class OutlineContentReader : IContentReader
	{
		internal OutlineContentReader(Outline outline)
		{
			Validator.ValidateArgNotNull(outline, "outline");
			this.outline = outline;

			// depth-first traversal of outline elements

			var stack = new Stack<IEnumerator<INode>>();
			stack.Push(this.outline.Children.GetEnumerator());

			while (stack.Count > 0)
			{
				var enumerator = stack.Pop();
				if (enumerator.MoveNext())
				{
					// TODO: skip over non-OutlineElements?

					this.descendants.Add(enumerator.Current);
					stack.Push(enumerator);
					stack.Push(enumerator.Current.Children.GetEnumerator());
				}
			}

		}

		~OutlineContentReader()
		{
			this.Dispose(false);
		}

		#region IContentReader members

		public void Close()
		{
			// called at the end of a Get-Content operations
		}

		public System.Collections.IList Read(long readCount)
		{
			System.Diagnostics.Debug.Assert(readCount >= 0);

			if (this.currentDescendantIndex >= this.descendants.Count)
			{
				// throw new InvalidOperationException("attempting to read beyond end of content");
				// it appears this method needs to return null to end a Get-Content execution
				return null;
			}

			int maxReadCount = Math.Min((int)readCount, this.descendants.Count - this.currentDescendantIndex);
			var result = this.descendants.GetRange(this.currentDescendantIndex, maxReadCount);
			this.currentDescendantIndex += maxReadCount;
			return result;
		}

		public void Seek(long offset, SeekOrigin origin)
		{
			System.Diagnostics.Debug.Assert(offset >= 0);

			switch (origin)
			{
				case SeekOrigin.Begin:
					this.currentDescendantIndex = Math.Min((int)offset, this.descendants.Count);
					break;

				case SeekOrigin.Current:
					{
						int maxOffset = Math.Min((int)offset, this.descendants.Count - this.currentDescendantIndex);
						this.currentDescendantIndex += maxOffset;
						break;
					}

				case SeekOrigin.End:
					{
						int maxOffset = Math.Min((int)offset, this.descendants.Count);
						this.currentDescendantIndex = this.descendants.Count - maxOffset;
						break;
					}
			}
		}

		#endregion

		#region IDisposable members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		protected void Dispose(bool disposing)
		{
			if (this.disposed) return;

			try
			{
				if (disposing)
				{

				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		private Outline outline;

		private List<INode> descendants = new List<INode>();
		private int currentDescendantIndex = 0;

		private bool disposed = false;
	}
}
