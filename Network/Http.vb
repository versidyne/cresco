Public Class Http

    Public Function Retreive(ByVal Url As String) As String

        Dim _WebClient As System.Net.WebClient = New System.Net.WebClient()
        Dim _Result As String = _WebClient.DownloadString(Url)

        Return _Result

    End Function

    Public Function Retreive_Alt(ByVal Url As String) As String

        Dim _Result As String = Nothing

        Dim _WebRequest As Net.WebRequest = Nothing

        _WebRequest = Net.WebRequest.Create(Url)
        _WebRequest.Method = "GET"

        Dim Response As Net.WebResponse

        Try

            Response = _WebRequest.GetResponse()

        Catch exc As Net.WebException

            Response = exc.Response

        End Try

        If Response Is Nothing Then

            Throw New Exception(Net.HttpStatusCode.NotFound)

        End If

        Using Reader As New IO.StreamReader(Response.GetResponseStream())

            _Result = Reader.ReadToEnd()

        End Using

        Return _Result

    End Function

    Public Function Retreive_Image(ByVal Url As String) As Drawing.Image

        Dim _WebRequest As Net.WebRequest = Nothing
        Dim _NormalImage As Drawing.Image = Nothing

        Try

            _WebRequest = Net.WebRequest.Create(Url)

        Catch ex As Exception

            MsgBox(ex.Message)

            Return _NormalImage

            Exit Function

        End Try

        Try

            _NormalImage = Drawing.Image.FromStream(_WebRequest.GetResponse().GetResponseStream())

            Return _NormalImage

        Catch ex As Exception

            MsgBox(ex.Message)

            Return _NormalImage

            Exit Function

        End Try

    End Function

End Class
