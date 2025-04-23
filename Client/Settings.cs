using System.Xml;

using Strings = Telesyk.StockQuotes.Properties.Strings;

namespace Telesyk.StockQuotes
{
	public sealed class Settings : SettingsBase
	{
		#region Private fields

		private static readonly Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings());

		#endregion

		#region Constructors

		private Settings()
			: base()
		{
			
		}

		#endregion

		#region Public members

		public static Settings Current => _instance.Value;

		public int RouterQuantity { get; private set; }

		public int CrashInterval { get; private set; }

		public int CrashDuration { get; private set; }

		#endregion

		#region Overridies

		protected override void Init(XmlDocument config)
		{
			initCrashSettings(config);
			initRouteSettings(config);
		}

		#endregion

		#region Private methods

		private void initCrashSettings(XmlDocument config)
		{
			var nodeCrashDelay = config.SelectSingleNode("//settings/crash-delay");

			int.TryParse(nodeCrashDelay?.Attributes?["interval"]?.InnerText, out int crashInterval);
			int.TryParse(nodeCrashDelay?.Attributes?["duration"]?.InnerText, out int crashDuration);

			CrashInterval = crashInterval;
			CrashDuration = crashDuration;
		}

		private void initRouteSettings(XmlDocument config)
		{
			var nodeRouterCount = config.SelectSingleNode("//settings/multicast/router-quantity");

			if (!int.TryParse(nodeRouterCount?.InnerText, out int routerQuantity))
				ThrowConfigurationException(Strings.Configuration_Name_RouterQuantity);

			RouterQuantity = routerQuantity;
		}

		#endregion
	}
}
