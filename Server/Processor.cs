using System.Globalization;

namespace Telesyk.StockQuotes
{
	internal static class Processor
	{
		private static int _heightLength = Console.WindowHeight - 1;
		private static int _columnLength;
		private static int _columnCount;
		private static int _top = 2;
		private static int _left = 1;
		private static decimal _lastValue = -1;
		private static bool _quit;

		static Processor()
		{
			_columnLength = $"{((int)Settings.Current.MaxValue)}".Length + Settings.Current.Decimals + 3;
			_columnCount = (Console.BufferWidth < 96 ? 96 : Console.BufferWidth - 1) / _columnLength;

			Console.BufferWidth = Console.WindowWidth = _columnCount * _columnLength + 1;
		}

		public static void Start()
			=> start();

		private static void start()
		{
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

			Console.Title = "Quotes Generator";
			Console.CursorVisible = false;

			using var generator = new QuoteGenerator();

			generator.NewValue += generateNewValue;

			while (true)
			{
				Console.CursorLeft = Console.CursorTop = 0;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write($" Started! Press Enter for pausing... Press Ctrl+C or Ctrl+Break to quit.");//                                                                                  ");

				generator.Start();

				Console.ReadKey();

				generator.Cancel();

				Console.CursorLeft = Console.CursorTop = 0;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write($" Paused!  Press Enter to continue... Press Ctrl+C or Ctrl+Break to quit.");// Type \"quit\" or \"q\" and press Enter for quit or just press Enter to continue: ");

				Console.ReadKey();

				//var command = Console.ReadLine()?.Trim().ToLower();

				//if (command == "quit" || command == "q")
				//	break;
			}
		}

		private static void generateNewValue(object sender, ValueEventArgs args)
		{
			if (_lastValue > -1)
			{
				Console.CursorTop = _top - 1;
				Console.CursorLeft = _left * _columnLength - _columnLength + 1;

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.Write(_lastValue);
			}

			_lastValue = args.Value;

			if (_top == _heightLength)
			{
				_top = 2;

				_left++;

				if (_left > _columnCount)
					_left = 1;
			}

			Console.CursorTop = _top;
			Console.CursorLeft = _left * _columnLength - _columnLength + 1;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(args.Value);

			_top++;
		}
	}
}
