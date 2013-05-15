Imports System
Imports System.Text
Imports System.IO
Imports System.IO.Compression

Public Class Compress

    Public Function StringCompress( _
          ByVal originalText As String) As Byte()
        ' ----- Generate a compressed version of a string.
        '       First, convert the string to a byte array.
        Dim workBytes() As Byte = _
           Encoding.UTF8.GetBytes(originalText)

        ' ----- Bytes will flow through a memory stream.
        Dim memoryStream As New MemoryStream()

        ' ----- Use the newly created memory stream for the
        '       compressed data.
        Dim zipStream As New GZipStream(memoryStream, _
           CompressionMode.Compress, True)
        zipStream.Write(workBytes, 0, workBytes.Length)
        zipStream.Flush()

        ' ----- Close the compression stream.
        zipStream.Close()

        ' ----- Return the compressed bytes.
        Return memoryStream.ToArray
    End Function

    Public Function BytesDecompress( _
          ByVal compressed() As Byte) As String
        ' ----- Uncompress a previously compressed string.
        '       Extract the length for the decompressed string.
        Dim lastFour(3) As Byte
        Array.Copy(compressed, compressed.Length - 4, _
           lastFour, 0, 4)
        Dim bufferLength As Integer = _
           BitConverter.ToInt32(lastFour, 0)

        ' ----- Create an uncompressed bytes buffer.
        Dim buffer(bufferLength - 1) As Byte

        ' ----- Bytes will flow through a memory stream.
        Dim memoryStream As New MemoryStream(compressed)

        ' ----- Create the decompression stream.
        Dim decompressedStream As New GZipStream( _
           memoryStream, CompressionMode.Decompress, True)

        ' ----- Read and decompress the data into the buffer.
        decompressedStream.Read(buffer, 0, bufferLength)

        ' ----- Convert the bytes to a string.
        Return Encoding.UTF8.GetString(buffer)
    End Function

    Public Sub FileCompress(ByVal sourceFile As String, _
          ByVal destinationFile As String)
        ' ----- Decompress a previously compressed string.
        '       First, create the input file stream.
        Dim sourceStream As New FileStream( _
           sourceFile, FileMode.Open, FileAccess.Read)

        ' ----- Create the output file stream.
        Dim destinationStream As New FileStream( _
        destinationFile, FileMode.Create, FileAccess.Write)

        ' ----- Bytes will be processed by a compression
        '       stream.
        Dim compressedStream As New GZipStream( _
           destinationStream, CompressionMode.Compress, True)

        ' ----- Process bytes from one file into the other.
        Const BlockSize As Integer = 4096
        Dim buffer(BlockSize) As Byte
        Dim bytesRead As Integer
        Do
            bytesRead = sourceStream.Read(buffer, 0, BlockSize)
            If (bytesRead = 0) Then Exit Do
            compressedStream.Write(buffer, 0, bytesRead)
        Loop

        ' ----- Close all the streams.
        sourceStream.Close()
        compressedStream.Close()
        destinationStream.Close()
    End Sub

    Public Sub FileDecompress(ByVal sourceFile As String, _
          ByVal destinationFile As String)
        ' ----- Compress the entire contents of a file, and
        '       store the result in a new file. First, get
        '       the files as streams.
        Dim sourceStream As New FileStream( _
           sourceFile, FileMode.Open, FileAccess.Read)
        Dim destinationStream As New FileStream( _
           destinationFile, FileMode.Create, FileAccess.Write)

        ' ----- Bytes will be processed through a
        '       decompression stream.
        Dim decompressedStream As New GZipStream( _
           sourceStream, CompressionMode.Decompress, True)

        ' ----- Process bytes from one file into the other.
        Const BlockSize As Integer = 4096
        Dim buffer(BlockSize) As Byte
        Dim bytesRead As Integer
        Do
            bytesRead = decompressedStream.Read(buffer, _
               0, BlockSize)
            If (bytesRead = 0) Then Exit Do
            destinationStream.Write(buffer, 0, bytesRead)
        Loop

        ' ----- Close all the streams.
        sourceStream.Close()
        decompressedStream.Close()
        destinationStream.Close()
    End Sub

End Class