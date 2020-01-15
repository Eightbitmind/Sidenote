using Sidenote.DOM;
using Sidenote.Serialization;
using Sidenote.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;
using System.Text.RegularExpressions;

namespace Sidenote.PowerShell
{
	// Get-Content : Cannot use interface. The IContentCmdletProvider interface is not implemented by this provider.

	// Commands and the provider calls they generate
	//
	// ls on:
	// 		ItemExists(path @"ON:\")
	// 		IsItemContainer(path: @"ON:\")
	// 		GetChildItems(path: @"ON:\", recurse: false)
	// 			GetChildName(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
	// 		
	// ls "on:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}"
	// 		GetChildName(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
	// 		GetChildName(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
	// 		ItemExists(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
	// 		IsItemContainer(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
	// 		GetChildItems(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}", recurse: false)

	[CmdletProvider(
		"OneNote", // e.g. Provider name in the Get-PSDrive output
		ProviderCapabilities.None
	)]
	public class DriveProvider : NavigationCmdletProvider
	{
		// Class Hierarchy:
		//
		// CmdletProvider
		//     DriveCmdletProvider
		//         ItemCmdletProvider
		//             ContainerCmdletProvider
		//                 NavigationCmdletProvider
		//
		// CmdletProvider:
		// - ???
		//
		// DriveCmdletProvider:
		// - create and remove drives, list of initial drives
		// - members
		//       protected virtual Collection<PSDriveInfo> InitializeDefaultDrives();
		//       protected virtual PSDriveInfo NewDrive(PSDriveInfo drive);
		//       protected virtual object NewDriveDynamicParameters();
		//       protected virtual PSDriveInfo RemoveDrive(PSDriveInfo drive);
		//
		// ItemCmdletProvider:
		// - set of commands for getting and setting data on one or more items
		// - members
		//       protected virtual void ClearItem(string path);
		//       protected virtual object ClearItemDynamicParameters(string path);
		//       protected virtual string[] ExpandPath(string path);
		//       protected virtual void GetItem(string path);
		//       protected virtual object GetItemDynamicParameters(string path);
		//       protected virtual void InvokeDefaultAction(string path);
		//       protected virtual object InvokeDefaultActionDynamicParameters(string path);
		//       protected abstract bool IsValidPath(string path);
		//       protected virtual bool ItemExists(string path);
		//       protected virtual object ItemExistsDynamicParameters(string path);
		//       protected virtual void SetItem(string path, object value);
		//       protected virtual object SetItemDynamicParameters(string path, object value);
		//
		// ContainerCmdletProvider:
		// - base class for providers that expose a single level of items.
		// - members
		//       protected virtual bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter);
		//       protected virtual void CopyItem(string path, string copyPath, bool recurse);
		//       protected virtual object CopyItemDynamicParameters(string path, string destination, bool recurse);
		//       protected virtual void GetChildItems(string path, bool recurse);
		//       protected virtual void GetChildItems(string path, bool recurse, uint depth);
		//       protected virtual object GetChildItemsDynamicParameters(string path, bool recurse);
		//       protected virtual void GetChildNames(string path, ReturnContainers returnContainers);
		//       protected virtual object GetChildNamesDynamicParameters(string path);
		//       protected virtual bool HasChildItems(string path);
		//       protected virtual void NewItem(string path, string itemTypeName, object newItemValue);
		//       protected virtual object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue);
		//       protected virtual void RemoveItem(string path, bool recurse);
		//       protected virtual object RemoveItemDynamicParameters(string path, bool recurse);
		//       protected virtual void RenameItem(string path, string newName);
		//       protected virtual object RenameItemDynamicParameters(string path, string newName);
		//
		// NavigationCmdletProvider:
		// - base class for providers that expose a hierarch of item and containers.
		// - members
		//       protected virtual string GetChildName(string path);
		//       protected virtual string GetParentPath(string path, string root);
		//       protected virtual bool IsItemContainer(string path);
		//       protected virtual string MakePath(string parent, string child);
		//       protected string MakePath(string parent, string child, bool childIsLeaf);
		//       protected virtual void MoveItem(string path, string destination);
		//       protected virtual object MoveItemDynamicParameters(string path, string destination);
		//       protected virtual string NormalizeRelativePath(string path, string basePath);

		public DriveProvider()
		{
		}

		protected override ProviderInfo Start(ProviderInfo providerInfo)
		{
			return base.Start(providerInfo);
		}

		#region DriveCmdletProvider members

		protected override Collection<PSDriveInfo> InitializeDefaultDrives()
		{
			PSDriveInfo oneNoteDriveInfo = new PSDriveInfo(
				name: "ON",
				provider: this.ProviderInfo,
				root: driveName + pathSeparator,
				description: "driveInfo_description_where_does_it_surface",
				credential: this.Credential);

			return new Collection<PSDriveInfo> { oneNoteDriveInfo };
		}

		/// <summary>
		/// Creates a new drive.
		/// </summary>
		/// <param name="driveInfo">Object containing information about the drive to be created.</param>
		/// <returns>
		/// An object representing the newly created drive.
		/// </returns>
		/// <remarks>
		/// I guess this method is what ends up getting called in response to invoking the PS command
		///     New-PSDrive -Name <string> -PSProvider <string> -Root <string> -Description <string> ...
		/// The <c>New-PSDrive</c> arguments reach this method via the <paramref name="driveInfo"/>
		/// argument.
		/// I guess the object returned by this method is inserted into the list of drives that can be
		/// seen via the <c>Get-PSDrive</c> PS command.
		/// It seems that PS itself prevents the creation of multiple drives with the same name, there's
		/// no need for us to do bookkeeping with regard to this.
		/// </remarks>
		protected override PSDriveInfo NewDrive(PSDriveInfo driveInfo)
		{
			Validator.ValidateArgNotNull(driveInfo, "driveInfo");
			// return new PSDriveInfo(driveInfo.Name, driveInfo.Provider, driveInfo.Root, driveInfo.Description, driveInfo.Credential);
			return driveInfo;
		}

		/// <summary>
		/// Deletes the specified drive.
		/// </summary>
		/// <param name="driveInfo">
		/// Object containing information about the drive to delete. This is the same object
		/// we return from the <see cref="NewDrive"/> method.
		/// </param>
		/// <returns>No idea.</returns>
		/// <remarks>
		/// I guess this method ends up getting called in response to the
		/// <code>
		///     Remove-PSDrive [-Name] <string[]> [-PSProvider <string[]>] ...
		/// </code>
		/// PS command.
		/// </remarks>
		protected override PSDriveInfo RemoveDrive(PSDriveInfo driveInfo)
		{
			return driveInfo;
		}

		#endregion

		#region ItemCmdletProvider members

		/// <summary>
		/// Gets the item at the specified path.
		/// </summary>
		/// <param name="path">The path to the item to retrieve.</param>
		/// <remarks>
		/// Must be implemented (base method throws PSNotSupportedException). Needed to access to the provider objects using the Get-Item and Get-Childitem cmdlets.
		/// </remarks>
		protected override void GetItem(string path)
		{
			throw new Exception("When is this method used?");
#if YELLOWBOX_BONEYARD
			IList<string> pathItems = SplitPath(path);

			IUIAutomationElement element = GetElement(UIAManager.RootElement, pathItems);
			if (element == null) return;

			this.WriteItemObject(
				item: UIElement.Wrap(element, path),
				path: path,
				isContainer: true);
#endif
		}

		private INode GetNode(string path)
		{
			string[] pathParts = path.Split(new[] { DriveProvider.pathSeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			IFormatter notebooksFormatter = FormatterManager.RootContentFormatter;

			INode currentNode = new Node(null);
			bool success = notebooksFormatter.Deserialize(currentNode);
			Debug.Assert(success);

			for (int i = 1; i < pathParts.Length; ++i)
			{
				bool foundChild = false;
				foreach (INode child in currentNode.Children)
				{
					var identifiableChild = child as IIdentifiableObject;

					if (identifiableChild != null)
					{
						if (string.Compare(pathParts[i], identifiableChild.ID, StringComparison.OrdinalIgnoreCase) == 0)
						{
							currentNode = child;
							foundChild = true;
							break;
						}
					}
				}
				if (!foundChild) return null;
			}

			return currentNode;


		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/// <example>
		/// ls on:
		/// 	ItemExists(path: @"ON:\")
		/// 	
		/// 	The drive name has been capitalized and a path separator has been
		/// 	appended.
		/// 	
		/// ls "on:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}"
		/// 	ItemExists(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
		/// 	
		/// 	The drive name has been capitalized.
		/// 	
		/// </example>
		protected override bool ItemExists(string path)
		{
			//string[] pathParts = path.Split(new[] { DriveProvider.pathSeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			//if (pathParts.Length == 1)
			//{
			//	return true;
			//}

			//IFormatter notebooksFormatter = FormatterManager.RootContentFormatter;

			//INode currentNode = new Node(null);
			//bool success = notebooksFormatter.Deserialize(currentNode);
			//Debug.Assert(success);

			//for (int i = 1; i < pathParts.Length; ++i)
			//{
			//	bool foundChild = false;
			//	foreach (INode child in currentNode.Children)
			//	{
			//		var identifiableChild = child as IIdentifiableObject;

			//		if (identifiableChild != null)
			//		{
			//			if (string.Compare(pathParts[i], identifiableChild.ID, StringComparison.OrdinalIgnoreCase) == 0)
			//			{
			//				currentNode = child;
			//				foundChild = true;
			//				break;
			//			}
			//		}
			//	}
			//	if (!foundChild) return false;
			//}

			//return true;

			return this.GetNode(path) != null;

#if YELLOWBOX_BONEYARD
			bool returnValue = false;
			IList<string> pathItems;

			if (!string.IsNullOrEmpty(currentPath) && string.CompareOrdinal(currentPath, path) == 0)
			{
				returnValue = true;
			}
			else if (TrySplitPath(path, out pathItems))
			{
				if (pathItems.Count == 0)  // just the drive, e.g. path == "UI:\\"
				{
					returnValue = true;
				}
				else
				{
					returnValue = GetElement(UIAManager.RootElement, pathItems) != null;
				}
			}

			return returnValue;
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/// <remarks>
		/// I have not yet encountered a scenario in which this method is being called.
		/// </remarks>
		protected override bool IsValidPath(string path)
		{
			throw new Exception("When is this method used?");
		}

		#endregion

		#region ContainerCmdletProvider members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recurse"></param>
		/// <remarks>
		/// Instead of returning objects, this method needs to pass the respective
		/// objects to the 'WriteItemObject' method. 
		/// It could be that the <paramref name="path"/> argument is prefixed with the
		/// current drive's 'root' path.
		/// This method is called with commands like "ls abc". However, a command like
		/// "ls *" will invoke the 'GetChildNames' instead.
		/// </remarks>
		/// <example>
		/// ls on:
		/// 	GetChildItems(path: @"ON:\", recurse: false)
		/// 	
		/// 	The drive name has been capitalized and a trailing path separator has been appended
		/// 	even though it was not specified in the 'ls' command.
		/// 	
		/// ls "ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}"
		/// 	GetChildItems(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}", recurse: false)
		/// 	
		/// 	Note the absence of a trailing path separator.
		/// 	
		/// </example>
		protected override void GetChildItems(string path, bool recurse)
		{
			//string[] pathParts = path.Split(new[] { DriveProvider.pathSeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			//INode currentNode = new Node(null);
			//IFormatter notebooksFormatter = FormatterManager.RootContentFormatter;
			//bool success = notebooksFormatter.Deserialize(currentNode);
			//Debug.Assert(success);

			//StringBuilder childPath = new StringBuilder(DriveProvider.driveName);

			//for (int i = 1; i < pathParts.Length; ++i)
			//{
			//	bool foundChild = false;
			//	foreach (INode child in currentNode.Children)
			//	{
			//		var identifiableChild = child as IIdentifiableObject;
			//		if (identifiableChild != null)
			//		{
			//			string id = identifiableChild.ID;
			//			if (string.Compare(pathParts[i], id, StringComparison.OrdinalIgnoreCase) == 0)
			//			{
			//				childPath.Append(DriveProvider.pathSeparatorChar).Append(id);
			//				currentNode = child;
			//				foundChild = true;
			//				break;
			//			}
			//		}
			//	}
			//	Debug.Assert(foundChild);
			//}

			INode node = this.GetNode(path);

			if (node == null) return;

			string childPathPrefix = path.EndsWith(DriveProvider.pathSeparator) ? path : path + DriveProvider.pathSeparator;

			foreach (INode child in node.Children)
			{
				var identifiableChild = child as IIdentifiableObject;
				if (identifiableChild != null)
				{
					string childPath = childPathPrefix + identifiableChild.ID;
					WriteItemObject(
						item: child,
						path: childPath,
						isContainer: true);
				}
			}

#if YELLOWBOX_BONEYARD
			IList<string> pathItems = SplitPath(path);

			IUIAutomationElement parent = GetElement(UIAManager.RootElement, pathItems);
			if (parent == null) return;

			if (recurse)
			{
				GetDescendantItems(parent, path);
			}
			else
			{
				for (
					IUIAutomationElement child = UIAManager.CurrentTreeWalker.GetFirstChildElement(parent);
					child != null;
					child = UIAManager.CurrentTreeWalker.GetNextSiblingElement(child))
				{
					string childPath = JoinPath(path, Converter.RuntimeIdToString(child.GetRuntimeId()));

					using (var restoreCurrentPath = new AutoRestorer<string>(oldPath => { currentPath = oldPath; }, currentPath, childPath))
					{
						WriteItemObject(
							item: UIElement.Wrap(child, childPath),
							path: childPath,
							isContainer: true);
					}
				}
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="returnContainers"></param>
		/// <remarks>
		/// Must be implemented (base method throws PSNotSupportedException).
		/// </remarks>
		protected override void GetChildNames(string path, ReturnContainers returnContainers)
		{
			throw new Exception("When is this method used?");
#if YELLOWBOX_BONEYARD
			IList<string> pathItems = SplitPath(path);

			IUIAutomationElement parent = GetElement(UIAManager.RootElement, pathItems);
			if (parent == null) return;

			for (
				IUIAutomationElement child = UIAManager.CurrentTreeWalker.GetFirstChildElement(parent);
				child != null;
				child = UIAManager.CurrentTreeWalker.GetNextSiblingElement(child))
			{
				string item = Converter.RuntimeIdToString(child.GetRuntimeId());

				WriteItemObject(
					item: item,
					path: path, //  + pathSeparator + item,
					isContainer: true);
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns>
		/// </returns>
		/// <remarks>
		/// Must be implemented (base method throws PSNotSupportedException).
		/// </remarks>
		protected override bool HasChildItems(string path)
		{
			throw new Exception("When is this method used?");
#if YELLOWBOX_BONEYARD
			IList<string> pathItems = SplitPath(path);

			IUIAutomationElement parent = GetElement(UIAManager.RootElement, pathItems);
			if (parent == null)
			{
				return false;
			}

			bool returnValue = UIAManager.CurrentTreeWalker.GetFirstChildElement(parent) != null;
			return returnValue;
#endif
		}

		#endregion

		#region NavigationCmdletProvider members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/// <example>
		/// ls on:
		/// 	GetChildName(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
		/// 	
		///	ls "on:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}"
		/// 	GetChildName(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
		/// 	
		/// </example>
		protected override string GetChildName(string path)
		{
			int separatorPos = path.LastIndexOf(pathSeparator);

			Debug.Assert(separatorPos > 0);
			Debug.Assert(separatorPos < path.Length - 1);

			string childName = path.Substring(separatorPos + 1);
			return childName;
		}

		/// <summary>
		/// Checks whether the specified item is a container.
		/// </summary>
		/// <param name="path"></param>
		/// <returns>True if the item is a container.</returns>
		/// <example>
		/// ls on:
		/// 	IsItemContainer(path: @"ON:\")
		/// 	
		/// ls "on:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}"
		/// 	IsItemContainer(path: @"ON:\{3496EC60-2025-4F0E-93EB-3F832AB9702C}{1}{B0}")
		/// </example>
		protected override bool IsItemContainer(string path)
		{
			INode node = this.GetNode(path);
			if (node == null) return false;
			return node.Children.Count > 0;

			// throw new Exception("When is this method used?");
			// 'base.IsItemContainer(path);' throws NotSupported exception
			// return HasChildItems(path); // wouldn't allow us to cd into a leave UI element
			// return true;
		}

		#endregion

#if YELLOWBOX_BONEYARD
		private void GetDescendantItems(IUIAutomationElement element, string path)
		{
			IUIAutomationTreeWalker w = UIAManager.CurrentTreeWalker;

			Queue<Tuple<IUIAutomationElement, string>> q = new Queue<Tuple<IUIAutomationElement, string>>();
			q.Enqueue(Tuple.Create(element, path));

			while (q.Count > 0)
			{
				Tuple<IUIAutomationElement, string> t = q.Dequeue();

				for (IUIAutomationElement e = w.GetFirstChildElement(t.Item1); e != null; e = w.GetNextSiblingElement(e))
				{
					string p = JoinPath(t.Item2, Converter.RuntimeIdToString(e.GetRuntimeId()));

					WriteItemObject(
						item: UIElement.Wrap(e, p),
						path: p,
						isContainer: true);

					q.Enqueue(Tuple.Create(e, p));
				}
			}
		}

		public static string GetPath(IUIAutomationElement element)
		{
			Stack<string> ids = new Stack<string>();
			for (; element != null; element = UIAManager.CurrentTreeWalker.GetParentElement(element))
			{
				ids.Push(Converter.RuntimeIdToString(element.GetRuntimeId()));
			}

			// remove runtime id of root element (represented by drive letter)
			ids.Pop();

			StringBuilder path = new StringBuilder(UIDriveProvider.driveName);

			while (ids.Count > 0)
			{
				path.Append(UIDriveProvider.pathSeparator).Append(ids.Pop());
			}

			return path.ToString();
		}

		private IUIAutomationElement GetElement(IUIAutomationElement current, IEnumerable<string> pathItems)
		{
			foreach (string pathItem in pathItems)
			{
				bool foundChild = false;
				for (
					IUIAutomationElement child = UIAManager.CurrentTreeWalker.GetFirstChildElement(current);
					child != null;
					child = UIAManager.CurrentTreeWalker.GetNextSiblingElement(child))
				{
					if (string.CompareOrdinal(
						Converter.RuntimeIdToString(child.GetRuntimeId()),
						pathItem) == 0)
					{
						foundChild = true;
						current = child;
						break;
					}
				}

				if (!foundChild) return null;
			}

			return current;
		}
#endif

		private static string JoinPath(string path1, string path2)
		{
			Debug.Assert(!string.IsNullOrEmpty(path1));
			Debug.Assert(!string.IsNullOrEmpty(path2));
			Debug.Assert(!path2.StartsWith(pathSeparator));

			StringBuilder jointPath = new StringBuilder(path1);

			if (!path1.EndsWith(pathSeparator))
			{
				jointPath.Append(pathSeparator);
			}

			return jointPath.Append(path2).ToString();
		}

		private static string JoinPath(string path, IEnumerable<string> pathItems)
		{
			Debug.Assert(!string.IsNullOrEmpty(path));
			StringBuilder sb = new StringBuilder(path);
			foreach (string pathItem in pathItems)
			{
				Debug.Assert(!string.IsNullOrEmpty(pathItem));
				Debug.Assert(!pathItem.StartsWith(pathSeparator));
				sb.Append(pathSeparator).Append(pathItem);
			}
			return sb.ToString();
		}

		private static IList<string> SplitPath(string path)
		{
			IList<string> pathItems;
			if (!TrySplitPath(path, out pathItems))
			{
				throw new ArgumentException(string.Format("Path \"{0}\" is syntactically incorrect", path));
			}
			return pathItems;
		}

		private static bool TrySplitPath(
			string path,
			out IList<string> pathItems)
		{
			pathItems = new List<string>();

			Match match = pathPattern.Match(path);

			if (!match.Success)
			{
				return false;
			}

			Group matchPathItems = match.Groups["pathItems"];

			if (matchPathItems.Success)
			{
				foreach (Capture pathItem in matchPathItems.Captures)
				{
					if (string.CompareOrdinal(pathItem.Value, "..") == 0)
					{
						if (pathItems.Count > 0)
						{
							pathItems.RemoveAt(pathItems.Count - 1);
						}
					}
					else
					{
						pathItems.Add(pathItem.Value);
					}
				}
			}

			return true;
		}

		private string currentPath;

		private static Regex pathPattern = new Regex(
			@"^UI:(?:\\(?<pathItems>[\w.]+))*\\?$",
			RegexOptions.Compiled | RegexOptions.CultureInvariant);

		private const string driveName = @"ON:";


		private const char pathSeparatorChar = '\\';
		private const string pathSeparator = @"\";

	}

}
