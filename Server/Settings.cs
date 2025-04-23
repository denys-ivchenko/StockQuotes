using System.Xml;

using Strings = Telesyk.StockQuotes.Properties.Strings;

namespace Telesyk.StockQuotes
{
	public sealed class Settings : SettingsBase
	{
		#region Private fields

		private static readonly Lazy<Settings> _current = new Lazy<Settings>(() => new Settings());

		#endregion

		#region Constructors

		private Settings()
			: base()
		{
			
		}

		#endregion

		#region Public members

		public static Settings Current => _current.Value;

		public decimal MinValue { get; private set; }

		public decimal MaxValue { get; private set; }

		public int GenerationDelayMin { get; private set; }

		public int GenerationDelayMax { get; private set; }

		#endregion

		#region Overridies

		protected override void Init(XmlDocument config)
		{
			initValuesRangeSettings(config);
			initGenerationDelaySettings(config);
		}

		#endregion

		#region Private methods

		private void initValuesRangeSettings(XmlDocument config)
		{
			var nodeRange = config.SelectSingleNode("//settings/value-range")!;

			decimal.TryParse(nodeRange.Attributes?["min"]?.InnerText, out decimal minValue);
			decimal.TryParse(nodeRange.Attributes?["max"]?.InnerText, out decimal maxValue);

			if (minValue >= maxValue)
				ThrowConfigurationException(Strings.Configuration_Name_ValueRange);

			MinValue = minValue;
			MaxValue = maxValue + Math.Round((decimal)Math.Pow(10, -(Decimals + 1)), Decimals + 1);
		}

		private void initGenerationDelaySettings(XmlDocument config)
		{
			var nodeGenerationDelay = config.SelectSingleNode("//settings/generation-delay")!;

			uint.TryParse(nodeGenerationDelay.Attributes?["min"]?.InnerText, out uint generationDelayMin);
			uint.TryParse(nodeGenerationDelay.Attributes?["max"]?.InnerText, out uint generationDelayMax);

			GenerationDelayMin = (int)generationDelayMin;
			GenerationDelayMax = (int)generationDelayMax;
		}

		#endregion
	}
}
