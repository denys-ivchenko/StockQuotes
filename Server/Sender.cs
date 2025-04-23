using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Telesyk.StockQuotes
{
	public sealed class Sender : IDisposable
	{
		#region Private fields

		private UdpClient _client;
		private IPEndPoint _endPoint;

		#endregion

		#region Constructors

		internal Sender()
		{
			_client = new UdpClient();
			_endPoint = new IPEndPoint(Settings.Current.MulticastIP, Settings.Current.MulticastPort);
		}

		#endregion

		#region Public methods

		public bool Send(ulong id, decimal value)
			=> send(id, value);

		#region Interfaces implementations

		public void Dispose()
		{
			_client.Dispose();
		}

		#endregion

		#endregion

		#region Private methods

		private bool send(ulong id, decimal value)
		{
			var bytes = new List<byte>(BitConverter.GetBytes(Convert.ToDouble(value)));
			bytes.AddRange(BitConverter.GetBytes(id));

			var count = 0;

			try { count = _client.Send(bytes.ToArray(), bytes.Count, _endPoint); }
			catch (Exception error)
			{
				Console.WriteLine(error.Message);
			}

			return count > 0;
		}

		#endregion
	}
}
