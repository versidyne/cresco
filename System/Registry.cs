// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Linq;
// End of VB project level imports


//Imports Microsoft.Win32

namespace Versidyne.System
{
	public class Registry
	{
		
		/// <summary>
		/// Edits the value of a Registry Key. The key will be created if it does not already exist.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Value"></param>
		/// <param name="Path"></param>
		/// <remarks></remarks>
		public void Add(string Name, string Value, string Path)
		{
			
			Microsoft.Win32.RegistryKey CU = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Path);
			
			
			CU.OpenSubKey(Path, true);
			
			CU.SetValue(Name, Value);
			
			
		}
		
		/// <summary>
		/// Removes the key
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Path"></param>
		/// <remarks></remarks>
		public void Remove(string Name, string Path)
		{
			
			Microsoft.Win32.RegistryKey CU = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Path);
			
			
			CU.OpenSubKey(Path, true);
			
			CU.DeleteValue(Name, false);
			
			CU.DeleteSubKey(Name);
			
			
		}
		
	}
	
}
