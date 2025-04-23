namespace Telesyk.StockQuotes
{
	public sealed class QuoteGenerator : ProcessorBase
	{
		#region Private Members

		private Sender _sender = new Sender();
		private ulong _id;

		#endregion

		#region Constructors

		public QuoteGenerator()
		{
			
		}

		#endregion

		#region Public methods

		public override void Dispose()
		{
			_sender.Dispose();
		}

		#endregion

		#region Protected methods

		protected override void Process()
		{
			var random = new Random();
			var deepRatio = (int)Math.Pow(10, Settings.Current.Decimals + 1);

			while (!MustBeStopped)
			{
				var value = Math.Round((decimal)random.Next((int)(Settings.Current.MinValue * deepRatio), (int)(Settings.Current.MaxValue * deepRatio)) / deepRatio, Settings.Current.Decimals);

				_id++;
				_sender.Send(_id, value);

				NotifyValue(_id, value);

				var delay = Settings.Current.GenerationDelayMin == Settings.Current.GenerationDelayMax ? Settings.Current.GenerationDelayMin : random.Next(Settings.Current.GenerationDelayMin, Settings.Current.GenerationDelayMax);

				if (delay > 0)
					Thread.Sleep(delay);
			}

			MustBeStopped = Started = false;
		}

		#endregion
	}
}
