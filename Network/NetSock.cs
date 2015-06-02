using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net.Security;

// Remove eventually
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Versidyne.Network {
	public class NetSock {
		
#region Callouts
		
		//Client
		private TcpClient Client = new TcpClient();
		private NetworkStream ClNetStream;
		private SslStream ClSslStream;
		private bool ClSsl = false;
		//Server
		private TcpListener Listener;
		private ArrayList Connections = new ArrayList();
		//Universal
		private Thread ConnectionThread;
		
#endregion
		
#region Events
		
		/// <summary>
		/// Data received on the connection.
		/// </summary>
		/// <param name="Data"></param>
		/// <remarks></remarks>
		public delegate void DataArrivalEventHandler(byte[] Data);
		private DataArrivalEventHandler DataArrivalEvent;
		
		public event DataArrivalEventHandler DataArrival {
			add {
				DataArrivalEvent = (DataArrivalEventHandler) System.Delegate.Combine(DataArrivalEvent, value);
			}
			remove {
				DataArrivalEvent = (DataArrivalEventHandler) System.Delegate.Remove(DataArrivalEvent, value);
			}
		}
		
		/// <summary>
		/// Error occurs. Sends a message.
		/// </summary>
		/// <param name="Data"></param>
		/// <remarks></remarks>
		public delegate void SocketErrorEventHandler(string Data);
		private SocketErrorEventHandler SocketErrorEvent;
		
		public event SocketErrorEventHandler SocketError
		{
			add {
				SocketErrorEvent = (SocketErrorEventHandler) System.Delegate.Combine(SocketErrorEvent, value);
			}
			remove {
				SocketErrorEvent = (SocketErrorEventHandler) System.Delegate.Remove(SocketErrorEvent, value);
			}
		}
		
		/// <summary>
		/// Client connects to the remote server.
		/// </summary>
		/// <param name="Connection"></param>
		/// <remarks></remarks>
		public delegate void ConnectionEventHandler(bool Connection);
		private ConnectionEventHandler ConnectionEvent;
		
		public event ConnectionEventHandler Connection {
			add {
				ConnectionEvent = (ConnectionEventHandler) System.Delegate.Combine(ConnectionEvent, value);
			}
			remove {
				ConnectionEvent = (ConnectionEventHandler) System.Delegate.Remove(ConnectionEvent, value);
			}
		}
		
		/// <summary>
		/// Server receives a connection.
		/// </summary>
		/// <remarks></remarks>
		public delegate void ConnectionRequestEventHandler(int Connection, bool Connected);
		private ConnectionRequestEventHandler ConnectionRequestEvent;
		
		public event ConnectionRequestEventHandler ConnectionRequest {
			add {
				ConnectionRequestEvent = (ConnectionRequestEventHandler) System.Delegate.Combine(ConnectionRequestEvent, value);
			}
			remove {
				ConnectionRequestEvent = (ConnectionRequestEventHandler) System.Delegate.Remove(ConnectionRequestEvent, value);
			}
		}
		
		
#endregion
		
#region Client Functions
		
		public void Connect(string IP, int Port) {
			try {
				Client = new TcpClient();
				Client.Connect(IP, Port);
				if (ConnectionEvent != null)
					ConnectionEvent(Client.Connected);
				ConnectionThread = new Thread(new System.Threading.ThreadStart(this.KeepAlive));
				ConnectionThread.Start();
			} catch (Exception ex) {
				if (SocketErrorEvent != null)
					SocketErrorEvent(ex.Message);
			}
		} public void Disconnect() {
			//Close connection
			Client.Close();
			//Close Stream
			ClNetStream.Close();
			//Send connection event
			if (ConnectionEvent != null)
				ConnectionEvent(Client.Connected);
			//Destroy thread
			ConnectionThread.Abort();
		} public void SendData(string Data) {
			byte[] SendBytes = Encoding.UTF8.GetBytes(Data);
			SendData(SendBytes);
		} public void SendData(byte[] Data) {
			if (ClSsl == true) {
				SendData(Data, ClSslStream);
			} else {
				SendData(Data, Client.GetStream());
			}
		} public bool Connected {
			get {
				return Client.Connected;
			}
		} public bool SSL {
			get {
				return ClSsl;
			} set {
				ClSsl = value;
			}
		} private void KeepAlive() {
			try {
				bool ClConnected = false;
				bool ClCanRead = false;
				if (ClSsl == true)
				{
					//Retreive Connection
					ClSslStream = new SslStream(Client.GetStream());
					ClConnected = true;
					ClCanRead = ClSslStream.CanRead;
				}
				else
				{
					//Retreive Connection
					ClNetStream = Client.GetStream();
					ClConnected = Client.Connected;
					ClCanRead = ClNetStream.CanRead;
				}
				//Create Loop
				while (ClConnected)
				{
					//Check if stream is readable
					if (ClCanRead == true)
					{
						// Read the NetworkStream into a byte buffer.
						byte[] Bytes = new byte[Client.ReceiveBufferSize + 1];
						//Do
						int Connection = 0;
						if (ClSsl == true) {
							Connection = ClSslStream.Read(Bytes, 0, Client.ReceiveBufferSize);
						}
						else {
							Connection = ClNetStream.Read(Bytes, 0, Client.ReceiveBufferSize);
						}
						//Loop While NetStream.DataAvailable
						if (Connection == 0) {
							//Close socket on thread
							Disconnect();
						}
						else {
							//Output the data received from the server
							if (DataArrivalEvent != null)
								DataArrivalEvent(Bytes);
						}
					} else {
						if (SocketErrorEvent != null)
							SocketErrorEvent("Connection is unreadable.");
					}
				}
			}
			catch (Exception ex) {
				if (SocketErrorEvent != null)
					SocketErrorEvent(ex.Message);
			}
		}
		
#endregion
		
#region Server Functions
		
		public void Listen(int Port)
		{
			
			Listener = new TcpListener(Port);
			
			Listener.Start();
			
			ConnectionThread = new Thread(new System.Threading.ThreadStart(this.ConnectionListener));
			ConnectionThread.Start();
			
		}
		
		public void StopListening()
		{
			
			Listener.Stop();
			
		}
		
		private void ConnectionListener()
		{
			
			bool Listening = true;
			
			try
			{
				
				//Create a continous loop
				while (Listening)
				{
					
					//Accept the pending client connection and return a TcpClient initialized for communication.
					TcpClient ConClient = Listener.AcceptTcpClient();
					
					if (ConClient.Connected)
					{
						
						//Send connection to handler
                        Thread HandlerThread = new Thread(obj => ConnectionHandler(ConClient));
                        HandlerThread.Start(ConClient);
                        
					}
					
				}
				
			}
			catch (Exception ex)
			{
				
				if (SocketErrorEvent != null)
					SocketErrorEvent(ex.Message);
				
			}
			
		}
		
		private void ConnectionHandler(TcpClient ConClient)
		{
			
			//Get the stream
			NetworkStream NetStream = ConClient.GetStream();
			
			//Add the stream to the list
			int ID = Connections.Add(NetStream);
			
			//Raise the connection event
			if (ConnectionRequestEvent != null)
				ConnectionRequestEvent(ID, ConClient.Connected);
			
			bool Connected = true;
			
			while (Connected)
			{
				
				if (NetStream.CanRead)
				{
					
					//Read the stream into a byte array
					byte[] Buffer = new byte[ConClient.ReceiveBufferSize + 1];
					int Bytes = NetStream.Read(Buffer, 0, ConClient.ReceiveBufferSize);
					byte[] Data = Buffer;
					
					if (Bytes == 0)
					{
						
						//Stop the loop
						Connected = false;
						//Remove the connection
						Connections.Remove(NetStream);
						//Close the connection
						NetStream.Close();
						//Raise the connection event
						if (ConnectionRequestEvent != null)
							ConnectionRequestEvent(ID, ConClient.Connected);
						//Close the socket
						ConClient.Close();
						
					}
					else
					{
						
						//Return the data received from the client to the console.
						if (DataArrivalEvent != null)
							DataArrivalEvent(Data);
						
						//Send message back as mirrored.
						//Dim responseString As String = "Welcome to the Vexis Pre-Alpha Server."
						//SendData(ClientData, NetStream)
						SendAll(Data);
						
						//RaiseEvent ReceivedData("Message Sent /> : " + responseString)
						//Any communication with the remote client using the TcpClient can go here.
						//Close TcpListener and TcpClient.
						//ConClient.Close()
						
					}
					
				}
				
			}
			
		}
		
		private void SendAll(byte[] Data)
		{
			
			NetworkStream NetStream = default(NetworkStream);
			
			foreach (NetworkStream tempLoopVar_NetStream in Connections)
			{
				NetStream = tempLoopVar_NetStream;
				
				SendData(Data, NetStream);
				
			}
			
		}
		
#endregion
		
#region Universal Functions
		
		private void SendData(byte[] Data, NetworkStream NetStream)
		{
			
			if (NetStream.CanWrite)
			{
				
				NetStream.Write(Data, 0, Data.Length);
				
			}
			else
			{
				
				if (SocketErrorEvent != null)
					SocketErrorEvent("Connection is unwritable.");
				
			}
			
		}
		
		private void SendData(byte[] Data, SslStream SslStream)
		{
			
			if (SslStream.CanWrite)
			{
				
				SslStream.Write(Data, 0, Data.Length);
				
			}
			else
			{
				
				if (SocketErrorEvent != null)
					SocketErrorEvent("Connection is unwritable.");
				
			}
			
		}
		
		public byte[] StringToBytes(string inString)
		{
			
			byte[] TempBytes = null;
			
			for (var i = 0; i <= inString.Length - 1; i++)
			{
				Array.Resize(ref TempBytes, (int) i + 1);
				TempBytes[(int) i] = (byte) (Strings.Asc(inString.Substring(i + 1 - 1, 1)));
			}
			
			return TempBytes;
			
		}
		
#endregion
		
	}
	
}
