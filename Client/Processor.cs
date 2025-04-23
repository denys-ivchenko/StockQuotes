using System.Globalization;
using System.Text;

using Strings = Telesyk.StockQuotes.Properties.Strings;

namespace Telesyk.StockQuotes
{
	internal static class Processor
	{
		public static void Start()
			=> start();

		private static void start()
		{
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("uk-UA");
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("uk-UA");

			Console.OutputEncoding = Encoding.UTF8;

			Console.Title = "Quotes";
			Console.CursorVisible = false;

			using (var receiver = new Receiver())
			{
				Task.Run(() => startDelayInterval(receiver));

				receiver.Start();

				while (true)
				{
					var key = new ConsoleKeyInfo();

					while (key.Key != ConsoleKey.Enter)
						key = Console.ReadKey();

					var data = Basket.Instance.Last();

					Console.Clear();

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.MaxValue}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.MaxValue)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.MinValue}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.MinValue)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.ValueCount}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.Count}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.UniqueCount}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.UniqueCount}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.Sum}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Sum)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.Median}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Median)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.Average}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Average)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.Divergence}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Devergence)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{Strings.Losted}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.Losted}");

					if (data.Modes.Length > 0)
					{
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"{Strings.ModeDuplicates}: ");
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine($"{data.ModeDuplicates}");

						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"{Strings.Modes} (");
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write($"{data.Modes.Length}");
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"): ");

						for (int i = 0; i < data.Modes.Length; i++)
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.Write($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Modes[i])}");

							if (i < data.Modes.Length - 1)
							{
								Console.ForegroundColor = ConsoleColor.White;
								Console.Write("; ");
							}
						}

						Console.WriteLine();
					}
				}
			}
		}

		private static void startDelayInterval(Receiver receiver)
		{
			if (Settings.Current.CrashInterval > 0 && Settings.Current.CrashDuration > 0)
				while (true)
				{
					Thread.Sleep(Settings.Current.CrashInterval);
					receiver.Cancel();
					Thread.Sleep(Settings.Current.CrashDuration);
					receiver.Start();
				}
		}
	}
}
