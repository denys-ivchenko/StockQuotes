namespace Telesyk.StockQuotes
{
	public abstract class ProcessorBase : IDisposable
	{
		#region Private Members

		private static object _monitor = new object();

		#endregion

		#region Constructors

		protected ProcessorBase()
		{
			
		}

		#endregion

		#region Protected Properties

		public bool Started { get; protected set; }

		protected bool MustBeStopped { get; set; }

		#endregion

		#region Public Methods

		public void Start()
		{
			lock (_monitor)
			{
				while (MustBeStopped)
					Thread.Sleep(30);

				if (!Started)
				{
					Started = true;
					Task.Run(Process);
				}
			}
		}

		public void Cancel()
		{
			if (Started && !MustBeStopped)
			{
				MustBeStopped = true;

				while (Started)
					Thread.Sleep(20);
			}
		}

		#region Abstract

		abstract public void Dispose();

		#endregion

		#endregion

		#region Protected methods

		protected void NotifyValue(ulong id, decimal value)
		{
			NewValue?.Invoke(this, new ValueEventArgs(id, value));
		}

		#region Abstract

		abstract protected void Process();

		#endregion

		#endregion

		#region Events

		public event ValueEventHandler? NewValue;

		#endregion
	}
}
