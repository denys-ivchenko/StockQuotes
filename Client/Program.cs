using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using strings = Telesyk.StockQuotes.Properties.Resources;

namespace Telesyk.StockQuotes
{
	class Program
	{
		public static void Main(string[] args)
		{
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

			Console.Title = "Quotes";
			
			using (var receiver = new Receiver())
			{
				Task.Run(() => startDelayInterval(receiver));
				
				receiver.Start();
				
				while(true)
				{
					var input = Console.ReadLine()?.Trim().ToLower();

					if (input == "q" || input == "quit")
						break;

					var data = Basket.Instance.Last();

					Console.Clear();

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.MaxValue}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.MaxValue)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.MinValue}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.MinValue)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Count}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.Count}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.UniqueCount}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.UniqueCount}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Sum}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Sum)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Median}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Median)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Average}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Average)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Divergence}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{string.Format($"{{0:f{Settings.Current.Decimals}}}", data.Devergence)}");

					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{strings.Failed}: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"{data.Failed}");

					if (data.Modes.Length > 0)
					{
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"{strings.ModeCount}: ");
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine($"{data.ModeCount}");

						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"{strings.Modes} (");
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
			if (Settings.Current.DelayInterval > 0 && Settings.Current.DelayDuration > 0)
				while (true)
				{
					Thread.Sleep(Settings.Current.DelayInterval);
					receiver.Cancel();
					Thread.Sleep(Settings.Current.DelayDuration);
					receiver.Start();
				}
		}
	}
}
