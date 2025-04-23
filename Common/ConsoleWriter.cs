namespace Telesyk.StockQuotes
{
	public static class ConsoleWriter
	{
		#region Private fields

		private static bool _lastIsLine = false;
		private static bool _writed = false;

		#endregion

		#region Public methods

		public static void WriteParagraph(string text)
			=> writeParagraph(text);

		public static void WriteLine(string text)
			=> writeLine(text);

		public static void Write(string text)
			=> write(text);

		#endregion

		#region Private methods

		private static void writeParagraph(string text)
		{
			if (_writed && _lastIsLine)
				Console.WriteLine();

			write(text);

			_lastIsLine = false;
		}

		private static void writeLine(string text)
		{
			if (_writed && !_lastIsLine)
				Console.WriteLine();

			write(text);

			_lastIsLine = true;
		}

		private static void write(string text)
		{
			if (_writed)
				Console.WriteLine();

			_writed = true;
			_lastIsLine = false;

			Console.Write(text);
		}

		#endregion
	}
}
