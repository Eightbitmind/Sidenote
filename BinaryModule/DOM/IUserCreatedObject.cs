using System;

namespace Sidenote.DOM
{
	public interface IUserCreatedObject
	{
		string Author { get; }
		string AuthorInitials { get; }
		DateTime CreationTime { get; }
		DateTime LastModifiedTime { get; }
	}
}
