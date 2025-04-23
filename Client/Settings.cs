using System.Xml;

using Resources = Telesyk.StockQuotes.Properties.Resources;

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

		public int RouterCount { get; private set; }

		public int DelayInterval { get; private set; }

		public int DelayDuration { get; private set; }

		#endregion

		#region Overridies

		protected override void Init(XmlDocument config)
		{
			var nodeRouterCount = config.SelectSingleNode("//settings/multicast/router-count");

			if (!int.TryParse(nodeRouterCount?.InnerText, out int routerCount))
				ThrowConfigurationException(Resources.Configuration_Name_RouterCount);

			RouterCount = routerCount;

			var nodeDelayInterval = config.SelectSingleNode("//settings/delay-interval");
			var nodeDelayDuration = config.SelectSingleNode("//settings/delay-duration");

			int.TryParse(nodeDelayInterval?.InnerText, out int delayInterval);
			int.TryParse(nodeDelayDuration?.InnerText, out int delayDuration);

			DelayInterval = delayInterval;
			DelayDuration = delayDuration;
		}

		#endregion
	}
}
