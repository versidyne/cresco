
Imports System.Text
Imports System.Security.Cryptography

Public Class MD5

    Public Function Generate(ByVal Text As String) As String
        ' ----- Generate a hash. Return an empty string
        '       if there are any problems.
        Dim plainBytes As Byte()
        Dim hashEngine As MD5CryptoServiceProvider
        Dim hashBytes As Byte()
        Dim hashText As String

        Try
            ' ----- Convert the plain text to a byte array.
            plainBytes = Encoding.UTF8.GetBytes(Text)

            ' ----- Select one of the hash engines.
            hashEngine = New MD5CryptoServiceProvider

            ' ----- Get the hash of the plain text bytes.
            hashBytes = hashEngine.ComputeHash(plainBytes)

            ' ----- Convert the hash bytes to a hexadecimal string.
            hashText = Replace(BitConverter.ToString(hashBytes), "-", "")
            Return hashText
        Catch
            Return ""
        End Try
    End Function

End Class
