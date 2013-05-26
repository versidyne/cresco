using System;
using System.Windows.Forms;

namespace Versidyne.Network
{
	public class Http
	{
		
		public string Retreive(string Url)
		{
			
			System.Net.WebClient _WebClient = new System.Net.WebClient();
			string _Result = _WebClient.DownloadString(Url);
			
			return _Result;
			
		}
		
		public string Retreive_Alt(string Url)
		{
			
			string _Result = null;
			
			System.Net.WebRequest _WebRequest = null;
			
			_WebRequest = System.Net.WebRequest.Create(Url);
			_WebRequest.Method = "GET";
			
			System.Net.WebResponse Response = default(System.Net.WebResponse);
			
			try
			{
				
				Response = _WebRequest.GetResponse();
				
			}
			catch (System.Net.WebException exc)
			{
				
				Response = exc.Response;
				
			}
			
			if (Response == null)
			{
				
				throw (new Exception(System.Net.HttpStatusCode.NotFound.ToString()));
				
			}
			
			using (System.IO.StreamReader Reader = new System.IO.StreamReader(Response.GetResponseStream()))
			{
				
				_Result = Reader.ReadToEnd();
				
			}
			
			
			return _Result;
			
		}
		
		public System.Drawing.Image Retreive_Image(string Url)
		{
			
			System.Net.WebRequest _WebRequest = null;
			System.Drawing.Image _NormalImage = null;
			
			try
			{
				
				_WebRequest = System.Net.WebRequest.Create(Url);
				
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message);
				
				return _NormalImage;
				
//				return default(System.Drawing.Image);
				
			}
			
			try
			{
				
				_NormalImage = System.Drawing.Image.FromStream(_WebRequest.GetResponse().GetResponseStream());
				
				return _NormalImage;
				
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message);
				
				return _NormalImage;
				
//				return default(System.Drawing.Image);
				
			}
			
		}
		
	}
	
}
