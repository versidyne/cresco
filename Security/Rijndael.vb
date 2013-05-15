Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class Rijndael

    Private HashGenerator As New MD5

    Public Function StringEncrypt(ByVal plainText As String, ByVal keyText As String) As String
        ' ----- Encrypt some text. Return an empty string
        '       if there are any problems.
        Try

            ' ----- Remove any possible null characters.
            Dim workText As String = plainText.Replace(vbNullChar, "")

            ' ----- Convert plain text to byte array.
            Dim workBytes() As Byte = Encoding.UTF8.GetBytes(plainText)

            ' ----- Convert key string to 32-byte key array.
            Dim keyBytes() As Byte = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText))

            ' ----- Create initialization vector.
            Dim IV() As Byte = {50, 199, 10, 159, 132, 55, 236, 189, 51, 243, 244, 91, 17, 136, 39, 230}

            ' ----- Create the Rijndael engine.
            Dim rijndael As New RijndaelManaged

            ' ----- Bytes will flow through a memory stream.
            Dim memoryStream As New MemoryStream()

            ' ----- Create the cryptography transform.
            Dim cryptoTransform As ICryptoTransform
            cryptoTransform = rijndael.CreateEncryptor(keyBytes, IV)

            ' ----- Bytes will be processed by CryptoStream.
            Dim cryptoStream As New CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write)

            ' ----- Move the bytes through the processing stream.
            cryptoStream.Write(workBytes, 0, workBytes.Length)
            cryptoStream.FlushFinalBlock()

            ' ----- Convert binary data to a viewable string.
            Dim encrypted As String = Convert.ToBase64String(memoryStream.ToArray)

            ' ----- Close the streams.
            memoryStream.Close()
            cryptoStream.Close()

            ' ----- Return the encrypted string result.
            Return encrypted


        Catch ex As Exception

            MsgBox(ex.Message)

            Return ""

        End Try

    End Function

    Public Function StringDecrypt(ByVal encrypted As String, ByVal keyText As String) As String
        ' ----- Decrypt a previously encrypted string. The key
        '       must match the one used to encrypt the string.
        '       Return an empty string on error.
        Try
            ' ----- Convert encrypted string to a byte array.
            Dim workBytes() As Byte = Convert.FromBase64String(encrypted)

            ' ----- Convert key string to 32-byte key array.
            Dim keyBytes() As Byte = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText))

            ' ----- Create initialization vector.
            Dim IV() As Byte = { _
               50, 199, 10, 159, 132, 55, 236, 189, _
               51, 243, 244, 91, 17, 136, 39, 230}

            ' ----- Decrypted bytes will be stored in
            '       a temporary array.
            Dim tempBytes(workBytes.Length - 1) As Byte

            ' ----- Create the Rijndael engine.
            Dim rijndael As New RijndaelManaged

            ' ----- Bytes will flow through a memory stream.
            Dim memoryStream As New MemoryStream(workBytes)

            ' ----- Create the cryptography transform.
            Dim cryptoTransform As ICryptoTransform
            cryptoTransform = _
               rijndael.CreateDecryptor(keyBytes, IV)

            ' ----- Bytes will be processed by CryptoStream.
            Dim cryptoStream As New CryptoStream( _
               memoryStream, cryptoTransform, _
               CryptoStreamMode.Read)

            ' ----- Move the bytes through the processing stream.
            cryptoStream.Read(tempBytes, 0, tempBytes.Length)

            ' ----- Close the streams.
            memoryStream.Close()
            cryptoStream.Close()

            ' ----- Convert the decrypted bytes to a string.
            Dim plainText As String = _
               Encoding.UTF8.GetString(tempBytes)

            ' ----- Return the decrypted string result.
            Return plainText.Replace(vbNullChar, "")
        Catch
            Return ""
        End Try
    End Function

    Public Sub FileEncrypt(ByVal sourceFile As String, _
          ByVal destinationFile As String, _
          ByVal keyText As String)
        ' ----- Create file streams.
        Dim sourceStream As New FileStream( _
           sourceFile, FileMode.Open, FileAccess.Read)
        Dim destinationStream As New FileStream( _
           destinationFile, FileMode.Create, FileAccess.Write)

        ' ----- Convert key string to 32-byte key array.
        Dim keyBytes() As Byte = _
           Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText))

        ' ----- Create initialization vector.
        Dim IV() As Byte = { _
           50, 199, 10, 159, 132, 55, 236, 189, _
           51, 243, 244, 91, 17, 136, 39, 230}

        ' ----- Create a Rijndael engine.
        Dim rijndael As New RijndaelManaged

        ' ----- Create the cryptography transform.
        Dim cryptoTransform As ICryptoTransform
        cryptoTransform = _
           rijndael.CreateEncryptor(keyBytes, IV)

        ' ----- Bytes will be processed by CryptoStream.
        Dim cryptoStream As New CryptoStream( _
           destinationStream, cryptoTransform, _
           CryptoStreamMode.Write)

        ' ----- Process bytes from one file into the other.
        Const BlockSize As Integer = 4096
        Dim buffer(BlockSize) As Byte
        Dim bytesRead As Integer
        Do
            bytesRead = sourceStream.Read(buffer, 0, BlockSize)
            If (bytesRead = 0) Then Exit Do
            cryptoStream.Write(buffer, 0, bytesRead)
        Loop

        ' ----- Close the streams.
        cryptoStream.Close()
        sourceStream.Close()
        destinationStream.Close()

    End Sub

    Public Sub FileDecrypt(ByVal sourceFile As String, ByVal destinationFile As String, ByVal keyText As String)

        ' ----- Create file streams.
        Dim sourceStream As New FileStream(sourceFile, FileMode.Open, FileAccess.Read)
        Dim destinationStream As New FileStream(destinationFile, FileMode.Create, FileAccess.Write)

        ' ----- Convert key string to 32-byte key array.
        Dim keyBytes() As Byte = Encoding.UTF8.GetBytes(HashGenerator.Generate(keyText))

        ' ----- Create initialization vector.
        Dim IV() As Byte = { _
           50, 199, 10, 159, 132, 55, 236, 189, _
           51, 243, 244, 91, 17, 136, 39, 230}

        ' ----- Create a Rijndael engine.
        Dim rijndael As New RijndaelManaged

        ' ----- Create the cryptography transform.
        Dim cryptoTransform As ICryptoTransform
        cryptoTransform = _
           rijndael.CreateDecryptor(keyBytes, IV)

        ' ----- Bytes will be processed by CryptoStream.
        Dim cryptoStream As New CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write)

        ' ----- Process bytes from one file into the other.
        Const BlockSize As Integer = 4096
        Dim buffer(BlockSize) As Byte
        Dim bytesRead As Integer
        Do
            bytesRead = sourceStream.Read(buffer, 0, BlockSize)
            If (bytesRead = 0) Then Exit Do
            cryptoStream.Write(buffer, 0, bytesRead)
        Loop

        ' ----- Close the streams.
        cryptoStream.Close()
        sourceStream.Close()
        destinationStream.Close()
    End Sub

End Class