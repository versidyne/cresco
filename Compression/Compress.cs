// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using System.Collections;
using System.Linq;
// End of VB project level imports

using System.Text;
using System.IO;
using System.IO.Compression;


namespace Versidyne.Compression
{
	public class Compress
	{
		
		public byte[] StringCompress(string originalText)
		{
			// ----- Generate a compressed version of a string.
			//       First, convert the string to a byte array.
			byte[] workBytes = Encoding.UTF8.GetBytes(originalText);
			
			// ----- Bytes will flow through a memory stream.
			MemoryStream memoryStream = new MemoryStream();
			
			// ----- Use the newly created memory stream for the
			//       compressed data.
			GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
			zipStream.Write(workBytes, 0, workBytes.Length);
			zipStream.Flush();
			
			// ----- Close the compression stream.
			zipStream.Close();
			
			// ----- Return the compressed bytes.
			return memoryStream.ToArray();
		}
		
		public string BytesDecompress(byte[] compressed)
		{
			// ----- Uncompress a previously compressed string.
			//       Extract the length for the decompressed string.
			byte[] lastFour = new byte[4];
			Array.Copy(compressed, compressed.Length - 4, lastFour, 0, 4);
			int bufferLength = BitConverter.ToInt32(lastFour, 0);
			
			// ----- Create an uncompressed bytes buffer.
			byte[] buffer = new byte[bufferLength - 1 + 1];
			
			// ----- Bytes will flow through a memory stream.
			MemoryStream memoryStream = new MemoryStream(compressed);
			
			// ----- Create the decompression stream.
			GZipStream decompressedStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
			
			// ----- Read and decompress the data into the buffer.
			decompressedStream.Read(buffer, 0, bufferLength);
			
			// ----- Convert the bytes to a string.
			return Encoding.UTF8.GetString(buffer);
		}
		
		public void FileCompress(string sourceFile, string destinationFile)
		{
			// ----- Decompress a previously compressed string.
			//       First, create the input file stream.
			FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			
			// ----- Create the output file stream.
			FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			
			// ----- Bytes will be processed by a compression
			//       stream.
			GZipStream compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);
			
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
				compressedStream.Write(buffer, 0, bytesRead);
			} while (true);
			
			// ----- Close all the streams.
			sourceStream.Close();
			compressedStream.Close();
			destinationStream.Close();
		}
		
		public void FileDecompress(string sourceFile, string destinationFile)
		{
			// ----- Compress the entire contents of a file, and
			//       store the result in a new file. First, get
			//       the files as streams.
			FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			
			// ----- Bytes will be processed through a
			//       decompression stream.
			GZipStream decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);
			
			// ----- Process bytes from one file into the other.
			const int BlockSize = 4096;
			byte[] buffer = new byte[BlockSize + 1];
			int bytesRead = default(int);
			do
			{
				bytesRead = decompressedStream.Read(buffer, 0, BlockSize);
				if (bytesRead == 0)
				{
					break;
				}
				destinationStream.Write(buffer, 0, bytesRead);
			} while (true);
			
			// ----- Close all the streams.
			sourceStream.Close();
			decompressedStream.Close();
			destinationStream.Close();
		}
		
	}
}
