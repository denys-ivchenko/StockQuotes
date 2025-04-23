namespace Telesyk.StockQuotes
{
	class Program
	{
		public static void Main(string[] args)
		{
			try { Processor.Start(); }
			catch (Exception error)
			{
				using var text = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}log.txt", false);

				text.WriteLine(error.InnerException?.Message ?? error.Message);
			}
		}
	}
}
