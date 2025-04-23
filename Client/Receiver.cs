using System.Net;
using System.Net.Sockets;

namespace Telesyk.StockQuotes
{
	public sealed class Receiver : ProcessorBase
	{
		#region Private fields

		private UdpClient _client = new UdpClient(Settings.Current.MulticastPort);

		#endregion

		#region Constructors

		internal Receiver()
		{
			init();
		}

		#endregion

		#region Public methods

		public override void Dispose()
		{
			_client.Dispose();
		}

		#endregion

		#region Protected methods

		protected override void Process()
		{
			IPEndPoint? point = null;

			_client.AllowNatTraversal(true);

			while (!MustBeStopped)
			{
				byte[]? bytes = null;

				try { bytes = new List<byte>(_client.Receive(ref point)).ToArray(); }
				catch (Exception error)
				{
					Console.WriteLine(error.Message);
					continue;
				}

				if (bytes is not null && bytes.Length is not 0)
				{
					var value = Convert.ToDecimal(BitConverter.ToDouble(bytes, 0));
					var id = BitConverter.ToUInt64(bytes, 8);

					Basket.Instance.Increment(id, value);
				}
			}

			MustBeStopped = Started = false;
		}

		#endregion

		#region Private methods

		private void init()
		{
			try { _client.JoinMulticastGroup(Settings.Current.MulticastIP, Settings.Current.RouterQuantity); }
			catch (Exception error)
			{
				Console.WriteLine(error.Message);
				Console.ReadLine();
			}
		}

		#endregion
	}
}
