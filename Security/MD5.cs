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

using System.Text;
using System.Security.Cryptography;



namespace Versidyne.Security
{
	public class MD5
	{
		
		public string Generate(string Text)
		{
			// ----- Generate a hash. Return an empty string
			//       if there are any problems.
			byte[] plainBytes = null;
			MD5CryptoServiceProvider hashEngine = default(MD5CryptoServiceProvider);
			byte[] hashBytes = null;
			string hashText = default(string);
			
			try
			{
				// ----- Convert the plain text to a byte array.
				plainBytes = Encoding.UTF8.GetBytes(Text);
				
				// ----- Select one of the hash engines.
				hashEngine = new MD5CryptoServiceProvider();
				
				// ----- Get the hash of the plain text bytes.
				hashBytes = hashEngine.ComputeHash(plainBytes);
				
				// ----- Convert the hash bytes to a hexadecimal string.
				hashText = BitConverter.ToString(hashBytes).Replace("-", "");
				return hashText;
			}
			catch
			{
				return "";
			}
		}
		
	}
	
}
