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


namespace Versidyne.Protocols
{
	public class JSON
	{
		
		private string Key = "<|(cell)|>";
		private string SepKey = "<|(row)|>";
		private string Version = "0.1.0.0";
		
		public string Encode(string ToString, string FromString, string Type, string Data)
		{
			
			string EncodedData = Key + Version + Key + ToString + Key + FromString + Key + Type + Key + Data + Key;
			
			return EncodedData;
			
		}
		
		public string[] Decode(string Code)
		{
			
			string[] DecodedData = null;
			string[] Keys = new string[] {Key};
			
			DecodedData = Code.Split(Keys.ToString().ToCharArray(), StringSplitOptions.None);
			
			return DecodedData;
			
		}
		
		public Array MDConvert(string Data)
		{
			
			//basic variables
			Array Basic = default(Array);
			string Row = default(string);
			
			//complex variables
			ArrayList Dimensions = (ArrayList) null;
			Array Dimension = (Array) null;
			
			//intial split to single dimensional array
			Basic = Data.Split(SepKey.ToCharArray()[0]);
			
			//access each row
			foreach (string tempLoopVar_Row in Basic)
			{
				Row = tempLoopVar_Row;
				
				//create array out of row
				Dimension = Row.Split(Key.ToCharArray()[0]);
				
				//add to main array
				Dimensions.Add(Dimension);
				
			}
			
			return Dimensions.ToArray();
			
		}
		
		public string MDConvert(Array Dimensions)
		{
			
			//basic variables
			string Basic = null;
			
			//complex variables
			Array Dimension = default(Array);
			string Cell = default(string);
			
			//access each dimension
			foreach (Array tempLoopVar_Dimension in Dimensions)
			{
				Dimension = tempLoopVar_Dimension;
				
				string Row = null;
				
				//access each cell
				foreach (string tempLoopVar_Cell in Dimension)
				{
					Cell = tempLoopVar_Cell;
					
					//create string out of dimension
					Row += Key + Cell;
					
				}
				
				//add to main string
				Basic += Row;
				
			}
			
			return Basic;
			
		}
		
	}
	
}
