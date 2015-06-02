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



/// <summary>
/// Interface host.
/// </summary>
/// <remarks></remarks>
namespace Versidyne.Plugins
{
	public interface Host
	{
		
		/// <summary>
		///
		/// </summary>
		/// <param name="strFeedback"></param>
		/// <remarks></remarks>
		void ShowFeedback(string strFeedback);
		
	}
	
	/// <summary>
	/// Interface plugin.
	/// </summary>
	/// <remarks></remarks>
	public interface Plugin
	{
		
		/// <summary>
		///
		/// </summary>
		/// <param name="Host"></param>
		/// <remarks></remarks>
		void Initialize(Host Host);
		
		/// <summary>
		///
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		string Title {get;}
		
		/// <summary>
		///
		/// </summary>
		/// <param name="Args">Arguments that will be passed to the plugin.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		int Main(Array Args);
		
	}
}
