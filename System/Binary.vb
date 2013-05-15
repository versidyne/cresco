Imports System.IO

Public Class Binary

    Public Function Read(ByVal Path As String) As Byte()

        Dim Stream As FileStream
        Dim Bytes As Byte() = Nothing

        Try

            Stream = New FileStream(Path, FileMode.Open)

            Dim BinaryStream As New BinaryReader(Stream)

            Try

                Bytes = BinaryStream.ReadBytes(Stream.Length)
                BinaryStream.Close()

            Catch ex As Exception

                MsgBox("1" & ex.Message)

            End Try

        Catch ex As Exception

            MsgBox("2" & ex.Message)

        End Try

        Return Bytes

    End Function

    Public Function Write(ByVal Path As String, ByVal Data As Byte()) As Boolean

        Dim ReturnValue As Boolean = False

        Dim Stream As FileStream

        Try

            Stream = New FileStream(Path, FileMode.Create)

            Dim BinaryStream As New BinaryWriter(Stream)

            Try

                BinaryStream.Write(Data)
                BinaryStream.Close()

            Catch ex As Exception

                MsgBox(ex.Message)

            End Try

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

        Return ReturnValue

    End Function

End Class
