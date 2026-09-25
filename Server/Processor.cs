using System.Globalization;
using System.Text;

namespace Telesyk.StockQuotes
{
    internal static class Processor
    {
        private const int columnMargin = 3;

        private static readonly int valueLength;
        private static readonly int columnLength;
        private static readonly int columnCount;

        private static int top = 2;
        private static int column = 1;

        private static decimal lastValue = -1;

        static Processor()
        {
            valueLength = $"{(int)Settings.Current.MaxValue}".Length + Settings.Current.Decimals + (Settings.Current.Decimals > 0 ? 1 : 0);
            columnLength = valueLength + columnMargin;
            columnCount = (Console.BufferWidth - columnMargin) / columnLength;

            Console.BufferWidth = Console.WindowWidth = columnCount * columnLength + columnMargin;
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
                Console.CursorLeft = columnMargin;
                Console.CursorTop = 1;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write($"Started! Press Enter for pausing...       Press Ctrl+C or Ctrl+Break to quit.");//                                                                                  ");

                generator.Start();

                Console.ReadKey();

                generator.Cancel();

                Console.CursorTop = 1;
                Console.CursorLeft = columnMargin;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write($"Paused! Press Enter to continue...        Press Ctrl+C or Ctrl+Break to quit.");// Type \"quit\" or \"q\" and press Enter for quit or just press Enter to continue: ");

                Console.ReadKey();
            }
        }

        private static void generateNewValue(object sender, ValueEventArgs args)
        {
            if (lastValue > -1)
            {
                Console.CursorTop = top;
                Console.CursorLeft = (column - 1) * columnLength + columnMargin;

                Console.ForegroundColor = ConsoleColor.DarkGreen;

                writeValue(lastValue);
            }

            lastValue = args.Value;

            top++;

            if (top == Console.WindowHeight - 1)
            {
                top = 3;

                column++;

                if (column > columnCount)
                    column = 1;
            }

            if (column == 1 && Console.CursorTop > 2)
            {
                Console.CursorLeft = 0;
                Console.Write(symbols(' ', columnMargin));
            }

            Console.CursorTop = top;
            Console.CursorLeft = (column - 1) * columnLength + columnMargin;

            Console.ForegroundColor = ConsoleColor.Yellow;

            writeValue(args.Value);
        }

        private static void writeValue(decimal value)
        {
            var start = $"{(int)Settings.Current.MaxValue}".Length - $"{(int)value}".Length;
            var zeros = valueLength - start - $"{value}".Length;
            var end = columnLength - $"{value}".Length - start;

            Console.Write(symbols(' ', start) + $"{value}" + symbols('0', zeros) + symbols(' ', end));
        }

        private static string symbols(char symbol, int quantity)
        {
            var result = new StringBuilder();

            for (var i = 0; i < quantity; i++)
                result.Append(symbol);

            return $"{result}";
        }
    }
}
