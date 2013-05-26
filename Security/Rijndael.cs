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
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;


namespace Versidyne.Security
{
	public class Rijndael
	{
		
		private MD5 HashGenerator = new MD5();
		
		public string StringEncrypt(string plainText, string keyText)
		{
			// ----- Encrypt some text. Return an empty string
			//       if there are any problems.
			try
			{
				
				// ----- Remove any possible null characters.
				string workText = plainText.Replace(Constants.vbNullChar, "");
				
				// ----- Convert plain text to byte array.
				byte[] workBytes = Encoding.UTF8.GetBytes(plainText);
				
				// ----- Convert key string to 32-byte key array.
				byte[] keyBytes = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText));
				
				// ----- Create initialization vector.
				byte[] IV = new byte[] {(byte) 50, (byte) 199, (byte) 10, (byte) 159, (byte) 132, (byte) 55, (byte) 236, (byte) 189, (byte) 51, (byte) 243, (byte) 244, (byte) 91, (byte) 17, (byte) 136, (byte) 39, (byte) 230};
				
				// ----- Create the Rijndael engine.
				RijndaelManaged rijndael = new RijndaelManaged();
				
				// ----- Bytes will flow through a memory stream.
				MemoryStream memoryStream = new MemoryStream();
				
				// ----- Create the cryptography transform.
				ICryptoTransform cryptoTransform = default(ICryptoTransform);
				cryptoTransform = rijndael.CreateEncryptor(keyBytes, IV);
				
				// ----- Bytes will be processed by CryptoStream.
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
				
				// ----- Move the bytes through the processing stream.
				cryptoStream.Write(workBytes, 0, workBytes.Length);
				cryptoStream.FlushFinalBlock();
				
				// ----- Convert binary data to a viewable string.
				string encrypted = Convert.ToBase64String(memoryStream.ToArray());
				
				// ----- Close the streams.
				memoryStream.Close();
				cryptoStream.Close();
				
				// ----- Return the encrypted string result.
				return encrypted;
				
				
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message);
				
				return "";
				
			}
			
		}
		
		public string StringDecrypt(string encrypted, string keyText)
		{
			// ----- Decrypt a previously encrypted string. The key
			//       must match the one used to encrypt the string.
			//       Return an empty string on error.
			try
			{
				// ----- Convert encrypted string to a byte array.
				byte[] workBytes = Convert.FromBase64String(encrypted);
				
				// ----- Convert key string to 32-byte key array.
				byte[] keyBytes = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText));
				
				// ----- Create initialization vector.
				byte[] IV = new byte[] {(byte) 50, (byte) 199, (byte) 10, (byte) 159, (byte) 132, (byte) 55, (byte) 236, (byte) 189, (byte) 51, (byte) 243, (byte) 244, (byte) 91, (byte) 17, (byte) 136, (byte) 39, (byte) 230};
				
				// ----- Decrypted bytes will be stored in
				//       a temporary array.
				byte[] tempBytes = new byte[workBytes.Length - 1 + 1];
				
				// ----- Create the Rijndael engine.
				RijndaelManaged rijndael = new RijndaelManaged();
				
				// ----- Bytes will flow through a memory stream.
				MemoryStream memoryStream = new MemoryStream(workBytes);
				
				// ----- Create the cryptography transform.
				ICryptoTransform cryptoTransform = default(ICryptoTransform);
				cryptoTransform = rijndael.CreateDecryptor(keyBytes, IV);
				
				// ----- Bytes will be processed by CryptoStream.
				CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
				
				// ----- Move the bytes through the processing stream.
				cryptoStream.Read(tempBytes, 0, tempBytes.Length);
				
				// ----- Close the streams.
				memoryStream.Close();
				cryptoStream.Close();
				
				// ----- Convert the decrypted bytes to a string.
				string plainText = Encoding.UTF8.GetString(tempBytes);
				
				// ----- Return the decrypted string result.
				return plainText.Replace(Constants.vbNullChar, "");
			}
			catch
			{
				return "";
			}
		}
		
		public void FileEncrypt(string sourceFile, string destinationFile, string keyText)
		{
			// ----- Create file streams.
			FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			
			// ----- Convert key string to 32-byte key array.
			byte[] keyBytes = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText));
			
			// ----- Create initialization vector.
			byte[] IV = new byte[] {(byte) 50, (byte) 199, (byte) 10, (byte) 159, (byte) 132, (byte) 55, (byte) 236, (byte) 189, (byte) 51, (byte) 243, (byte) 244, (byte) 91, (byte) 17, (byte) 136, (byte) 39, (byte) 230};
			
			// ----- Create a Rijndael engine.
			RijndaelManaged rijndael = new RijndaelManaged();
			
			// ----- Create the cryptography transform.
			ICryptoTransform cryptoTransform = default(ICryptoTransform);
			cryptoTransform = rijndael.CreateEncryptor(keyBytes, IV);
			
			// ----- Bytes will be processed by CryptoStream.
			CryptoStream cryptoStream = new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write);
			
			// ----- Process bytes from one file into the other.
			const int BlockSize = 4096;
			byte[] buffer = new byte[BlockSize + 1];
			int bytesRead = default(int);
			do
			{
				bytesRead = sourceStream.Read(buffer, 0, BlockSize);
				if (bytesRead == 0)
				{
					break;
				}
				cryptoStream.Write(buffer, 0, bytesRead);
			} while (true);
			
			// ----- Close the streams.
			cryptoStream.Close();
			sourceStream.Close();
			destinationStream.Close();
			
		}
		
		public void FileDecrypt(string sourceFile, string destinationFile, string keyText)
		{
			
			// ----- Create file streams.
			FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			
			// ----- Convert key string to 32-byte key array.
			byte[] keyBytes = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText));
			
			// ----- Create initialization vector.
			byte[] IV = new byte[] {(byte) 50, (byte) 199, (byte) 10, (byte) 159, (byte) 132, (byte) 55, (byte) 236, (byte) 189, (byte) 51, (byte) 243, (byte) 244, (byte) 91, (byte) 17, (byte) 136, (byte) 39, (byte) 230};
			
			// ----- Create a Rijndael engine.
			RijndaelManaged rijndael = new RijndaelManaged();
			
			// ----- Create the cryptography transform.
			ICryptoTransform cryptoTransform = default(ICryptoTransform);
			cryptoTransform = rijndael.CreateDecryptor(keyBytes, IV);
			
			// ----- Bytes will be processed by CryptoStream.
			CryptoStream cryptoStream = new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write);
			
			// ----- Process bytes from one file into the other.
			const int BlockSize = 4096;
			byte[] buffer = new byte[BlockSize + 1];
			int bytesRead = default(int);
			do
			{
				bytesRead = sourceStream.Read(buffer, 0, BlockSize);
				if (bytesRead == 0)
				{
					break;
				}
				cryptoStream.Write(buffer, 0, bytesRead);
			} while (true);
			
			// ----- Close the streams.
			cryptoStream.Close();
			sourceStream.Close();
			destinationStream.Close();
		}
		
	}
}
