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

using System.IO;
using System.Reflection;


/// <summary>
/// Plugin services.
/// </summary>
/// <remarks></remarks>
namespace Versidyne.Plugins
{
	public class Services
	{
		
		public struct AvailablePlugin
		{
			public string AssemblyPath;
			public string ClassName;
		}
		
		public static AvailablePlugin[] FindPlugins(string strPath, string strInterface)
		{
			ArrayList Plugins = new ArrayList();
			string[] strDLLs = null;
			int intIndex = default(int);
			Assembly objDLL = default(Assembly);
			
			//Go through all DLLs in the directory, attempting to load them
			strDLLs = Directory.GetFileSystemEntries(strPath, "*.dll");
			for (intIndex = 0; intIndex <= strDLLs.Length - 1; intIndex++)
			{
				try
				{
					objDLL = Assembly.LoadFrom(strDLLs[intIndex]);
					ExamineAssembly(objDLL, strInterface, Plugins);
				}
				catch (Exception)
				{
					//Error loading DLL, we don't need to do anything special
				}
			}
			
			//Return all plugins found
			AvailablePlugin[] Results = new AvailablePlugin[Plugins.Count - 1 + 1];
			
			if (Plugins.Count != 0)
			{
				Plugins.CopyTo(Results);
				return Results;
			}
			else
			{
				return null;
			}
		}
		
		private static void ExamineAssembly(Assembly objDLL, string strInterface, ArrayList Plugins)
		{
			Type objType = default(Type);
			Type objInterface = default(Type);
			AvailablePlugin Plugin = new AvailablePlugin();
			
			//Loop through each type in the DLL
			foreach (Type tempLoopVar_objType in objDLL.GetTypes())
			{
				objType = tempLoopVar_objType;
				//Only look at public types
				if (objType.IsPublic == true)
				{
					//Ignore abstract classes
					if (!(System.Convert.ToInt32((objType.Attributes & TypeAttributes.Abstract)) == (int) TypeAttributes.Abstract ))
					{
						
						//See if this type implements our interface
						objInterface = objType.GetInterface(strInterface, true);
						
						if (!(objInterface == null))
						{
							//It does
							Plugin = new AvailablePlugin();
							Plugin.AssemblyPath = objDLL.Location;
							Plugin.ClassName = objType.FullName;
							Plugins.Add(Plugin);
						}
						
					}
				}
			}
		}
		
		public static object CreateInstance(AvailablePlugin Plugin)
		{
			Assembly objDLL = default(Assembly);
			object objPlugin = default(object);
			
			try
			{
				//Load dll
				objDLL = Assembly.LoadFrom(Plugin.AssemblyPath);
				
				//Create and return class instance
				objPlugin = objDLL.CreateInstance(Plugin.ClassName);
			}
			catch (Exception)
			{
				return null;
			}
			
			return objPlugin;
		}
		
	}
	
}
