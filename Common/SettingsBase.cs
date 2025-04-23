using System.Net;
using System.Xml;

using Strings = Telesyk.StockQuotes.Properties.Strings;

namespace Telesyk.StockQuotes
{
	public abstract class SettingsBase
	{
		#region Constructors

		protected SettingsBase()
		{
			init();
		}

		#endregion

		#region Public members

		public int Decimals { get; private set; }

		public IPAddress MulticastIP { get; private set; } = null!;

		public int MulticastPort { get; private set; }

		#endregion

		#region Protected methods

		abstract protected void Init(XmlDocument config);

		protected void ThrowConfigurationException(string configurationName)
		{
			throw new ApplicationException(string.Format(Strings.Configuration_Exception_Message, configurationName));
		}

		#endregion

		#region Private methods

		private void init()
		{
			var config = new XmlDocument();
			config.Load($"{AppDomain.CurrentDomain.BaseDirectory}settings.config");

			var nodeMulticastIP = config.SelectSingleNode("//settings/multicast/ip")!;
			var nodeMulticastPort = config.SelectSingleNode("//settings/multicast/port");
			var nodeDecimals = config.SelectSingleNode("//settings/decimals");

			Decimals = int.TryParse(nodeDecimals?.InnerText, out int decimals) ? decimals : 4;

			if (!int.TryParse(nodeMulticastPort?.InnerText, out int multicastPort))
				ThrowConfigurationException(Strings.Configuration_Name_Port);

			MulticastPort = multicastPort;

			try { MulticastIP = IPAddress.Parse(nodeMulticastIP.InnerText); }
			catch { ThrowConfigurationException(Strings.Configuration_Name_IP); }

			Init(config);
		}

		#endregion
	}
}
