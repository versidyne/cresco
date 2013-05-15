Imports System.Text
Imports System.Threading
Imports System.Net.Sockets
Imports System.Net.Security

Public Class NetSock

#Region "Callouts"

    'Client Based
    Private Client As New TcpClient
    Private ClNetStream As NetworkStream
    Private ClSslStream As SslStream
    Private ClSsl As Boolean = False
    'Server Based
    Private Listener As TcpListener
    Private Connections As New ArrayList
    'Universal Based
    Private ConnectionThread As Thread

#End Region

#Region "Events"

    ''' <summary>
    ''' Data received on the connection.
    ''' </summary>
    ''' <param name="Data"></param>
    ''' <remarks></remarks>
    Event DataArrival(ByVal Data As Byte())
    ''' <summary>
    ''' Error occurs. Sends a message.
    ''' </summary>
    ''' <param name="Data"></param>
    ''' <remarks></remarks>
    Event SocketError(ByVal Data As String)
    ''' <summary>
    ''' Client connects to the remote server.
    ''' </summary>
    ''' <param name="Connection"></param>
    ''' <remarks></remarks>
    Event Connection(ByVal Connection As Boolean)
    ''' <summary>
    ''' Server receives a connection.
    ''' </summary>
    ''' <remarks></remarks>
    Event ConnectionRequest(ByVal Connection As Integer, ByVal Connected As Boolean)

#End Region

#Region "Client Functions"

    Public Sub Connect(ByVal IP As String, ByVal Port As Integer)

        Try

            Client = New TcpClient

            Client.Connect(IP, Port)

            RaiseEvent Connection(Client.Connected)

            ConnectionThread = New Thread(AddressOf Me.KeepAlive)
            ConnectionThread.Start()

        Catch ex As Exception

            RaiseEvent SocketError(ex.Message)

        End Try

    End Sub

    Public Sub Disconnect()

        'Close connection
        Client.Close()
        'Close Stream
        ClNetStream.Close()
        'Send connection event
        RaiseEvent Connection(Client.Connected)
        'Destroy thread
        ConnectionThread.Abort()

    End Sub

    Public Sub SendData(ByVal Data As String)

        Dim SendBytes As [Byte]() = Encoding.UTF8.GetBytes(Data)
        SendData(SendBytes)

    End Sub

    Public Sub SendData(ByVal Data As Byte())

        If ClSsl = True Then

            SendData(Data, ClSslStream)

        Else

            SendData(Data, Client.GetStream())

        End If

    End Sub

    Public ReadOnly Property Connected() As Boolean

        Get

            Return Client.Connected()

        End Get

    End Property

    Public Property SSL() As Boolean

        Get

            Return ClSsl

        End Get

        Set(ByVal value As Boolean)

            ClSsl = value

        End Set

    End Property

    'Private Functions

    Private Sub KeepAlive()

        Try

            Dim ClConnected As Boolean = False
            Dim ClCanRead As Boolean = False

            If ClSsl = True Then

                'Retreive Connection
                ClSslStream = New SslStream(Client.GetStream)
                ClConnected = True
                ClCanRead = ClSslStream.CanRead

            Else

                'Retreive Connection
                ClNetStream = Client.GetStream()
                ClConnected = Client.Connected
                ClCanRead = ClNetStream.CanRead

            End If

            'Create Loop
            Do While ClConnected

                'Check if stream is readable
                If ClCanRead = True Then

                    ' Read the NetworkStream into a byte buffer.
                    Dim Bytes(Client.ReceiveBufferSize) As Byte

                    'Do

                    Dim Connection As Integer = 0

                    If ClSsl = True Then

                        Connection = ClSslStream.Read(Bytes, 0, CInt(Client.ReceiveBufferSize))

                    Else

                        Connection = ClNetStream.Read(Bytes, 0, CInt(Client.ReceiveBufferSize))

                    End If

                    'Loop While NetStream.DataAvailable

                    If Connection = 0 Then

                        'Close socket on thread
                        Disconnect()

                    Else

                        'Output the data received from the server
                        RaiseEvent DataArrival(Bytes)

                    End If

                Else

                    RaiseEvent SocketError("Connection is unreadable.")

                End If

            Loop

        Catch ex As Exception

            RaiseEvent SocketError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Server Functions"

    Public Sub Listen(ByVal Port As Integer)

        Listener = New TcpListener(Port)

        Listener.Start()

        ConnectionThread = New Thread(AddressOf Me.ConnectionListener)
        ConnectionThread.Start()

    End Sub

    Public Sub StopListening()

        Listener.Stop()

    End Sub

    Private Sub ConnectionListener()

        Dim Listening As Boolean = True

        Try

            'Create a continous loop
            Do While Listening

                'Accept the pending client connection and return a TcpClient initialized for communication. 
                Dim ConClient As TcpClient = Listener.AcceptTcpClient()

                If ConClient.Connected Then

                    'Send connection to handler
                    Dim HandlerThread As New Thread(AddressOf Me.ConnectionHandler)
                    HandlerThread.Start(ConClient)

                End If

            Loop

        Catch ex As Exception

            RaiseEvent SocketError(ex.Message)

        End Try

    End Sub

    Private Sub ConnectionHandler(ByVal ConClient As TcpClient)

        'Get the stream
        Dim NetStream As NetworkStream = ConClient.GetStream()

        'Add the stream to the list
        Dim ID As Integer = Connections.Add(NetStream)

        'Raise the connection event
        RaiseEvent ConnectionRequest(ID, ConClient.Connected)

        Dim Connected As Boolean = True

        Do While Connected

            If NetStream.CanRead Then

                'Read the stream into a byte array
                Dim Buffer(ConClient.ReceiveBufferSize) As Byte
                Dim Bytes As Integer = NetStream.Read(Buffer, 0, CInt(ConClient.ReceiveBufferSize))
                Dim Data As Byte() = Buffer

                If Bytes = 0 Then

                    'Stop the loop
                    Connected = False
                    'Remove the connection
                    Connections.Remove(NetStream)
                    'Close the connection
                    NetStream.Close()
                    'Raise the connection event
                    RaiseEvent ConnectionRequest(ID, ConClient.Connected)
                    'Close the socket
                    ConClient.Close()

                Else

                    'Return the data received from the client to the console.
                    RaiseEvent DataArrival(Data)

                    'Send message back as mirrored.
                    'Dim responseString As String = "Welcome to the Vexis Pre-Alpha Server."
                    'SendData(ClientData, NetStream)
                    SendAll(Data)

                    'RaiseEvent ReceivedData("Message Sent /> : " + responseString)
                    'Any communication with the remote client using the TcpClient can go here.
                    'Close TcpListener and TcpClient.
                    'ConClient.Close()

                End If

            End If

        Loop

    End Sub

    Private Sub SendAll(ByVal Data As Byte())

        Dim NetStream As NetworkStream

        For Each NetStream In Connections

            SendData(Data, NetStream)

        Next

    End Sub

#End Region

#Region "Universal Functions"

    Private Sub SendData(ByVal Data As Byte(), ByVal NetStream As NetworkStream)

        If NetStream.CanWrite Then

            NetStream.Write(Data, 0, Data.Length)

        Else

            RaiseEvent SocketError("Connection is unwritable.")

        End If

    End Sub

    Private Sub SendData(ByVal Data As Byte(), ByVal SslStream As SslStream)

        If SslStream.CanWrite Then

            SslStream.Write(Data, 0, Data.Length)

        Else

            RaiseEvent SocketError("Connection is unwritable.")

        End If

    End Sub

    Function StringToBytes(ByVal inString As String) As Byte()

        Dim TempBytes() As Byte = Nothing

        For i = 0 To Len(inString) - 1
            ReDim Preserve TempBytes(i)
            TempBytes(i) = Asc(Mid(inString, i + 1, 1))
        Next

        Return TempBytes

    End Function

#End Region

End Class
