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



namespace Versidyne.System
{
	public class Startup
	{
		
		//setvalue
		public void Add(string ApplicationName, string ApplicationPath)
		{
			Microsoft.Win32.RegistryKey CU = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
			CU.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			
			CU.SetValue(ApplicationName, ApplicationPath);
			
		}
		//remove value
		public void Remove(string ApplicationName)
		{
			Microsoft.Win32.RegistryKey CU = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
			CU.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			
			CU.DeleteValue(ApplicationName, false);
			
		}
		
	}
	
}
