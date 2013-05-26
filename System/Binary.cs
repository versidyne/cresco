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
using System.Windows.Forms;


namespace Versidyne.System
{
	public class Binary
	{
		
		public byte[] Read(string Path)
		{
			
			FileStream Stream = default(FileStream);
			byte[] Bytes = null;
			
			try
			{
				
				Stream = new FileStream(Path, FileMode.Open);
				
				BinaryReader BinaryStream = new BinaryReader(Stream);
				
				try
				{
					
					Bytes = BinaryStream.ReadBytes((int) Stream.Length);
					BinaryStream.Close();
					
				}
				catch (Exception ex)
				{
					
					MessageBox.Show("1" + ex.Message);
					
				}
				
			}
			catch (Exception ex)
			{
				
				MessageBox.Show("2" + ex.Message);
				
			}
			
			return Bytes;
			
		}
		
		public bool Write(string Path, byte[] Data)
		{
			
			bool ReturnValue = false;
			
			FileStream Stream = default(FileStream);
			
			try
			{
				
				Stream = new FileStream(Path, FileMode.Create);
				
				BinaryWriter BinaryStream = new BinaryWriter(Stream);
				
				try
				{
					
					BinaryStream.Write(Data);
					BinaryStream.Close();
					
				}
				catch (Exception ex)
				{
					
					MessageBox.Show(ex.Message);
					
				}
				
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message);
				
			}
			
			return ReturnValue;
			
		}
		
	}
	
}
