namespace Telesyk.StockQuotes
{
	public sealed class Basket
	{
		#region Private fields

		private static Lazy<Basket> _instance = new Lazy<Basket>(() => new Basket());
		private SortedDictionary<decimal, int> _values = new SortedDictionary<decimal, int>();
		private SortedDictionary<decimal, int> _valuesBuffer = new SortedDictionary<decimal, int>();
		private List<decimal> _modes = new List<decimal>();
		private object _locker = new object();

		private decimal _minValue;
		private decimal _maxValue;
		private ulong _count;
		private int _modeDuplicates;
		private decimal _sum;
		private ulong _lastId;
		private ulong _losted;

		#endregion

		#region Constructors

		private Basket()
		{
			
		}

		#endregion

		#region Public Properties

		public static Basket Instance => _instance.Value;

		#endregion

		#region Public methods

		public void Increment(ulong id, decimal value)
			=> increment(id, value);

		public CalculatedData Last()
			=> last();

		#endregion

		#region Private methods

		private void increment(ulong id, decimal value)
		{
			lock (_locker)
			{
				if (_count == 0 && id > 1)
					_lastId = id - 1;

				if (id < _lastId)
					_lastId = 0;

				_losted += id - 1 - _lastId;

				_lastId = id;

				if (!_values.ContainsKey(value))
					_values.Add(value, 0);

				_values[value]++;

				if (_values[value] > _modeDuplicates)
				{
					_modes.Clear();
					_modeDuplicates++;
				}

				if (_values[value] >= _modeDuplicates && _modeDuplicates != 1)
					_modes.Add(value);

				if (!_valuesBuffer.ContainsKey(value))
					_valuesBuffer.Add(value, 0);

				_valuesBuffer[value] = _values[value];

				_count++;
				_sum += value;

				if (_minValue == 0)
					_minValue = value;

				if (value < _minValue)
					_minValue = value;

				if (value > _maxValue)
					_maxValue = value;
			}
		}

		private CalculatedData last()
		{
			SortedDictionary<decimal, int>? buffer = null;

			var minValue = 0m;
			var maxValue = 0m;
			var count = 0ul;
			var uniqueCount = 0;
			var losted = 0ul;
			var sum = 0m;
			var median = 0m;
			var differenceSum = 0m;
			var modeDuplicates = 0m;
			decimal[]? modes = null;

			lock (_locker)
			{
				count = _count;
				uniqueCount = _values.Count;
				losted = _losted;
				minValue = _minValue;
				maxValue = _maxValue;
				sum = _sum;
				modeDuplicates = _modeDuplicates;

				_modes.Sort();
				modes = _modes.ToArray();

				buffer = _valuesBuffer;
				_valuesBuffer = new SortedDictionary<decimal, int>();
			}

			var average = Math.Round(sum / (count == 0 ? 1 : count), Settings.Current.Decimals);

			var medianPosition = count / 2;
			var countIsEven = count % 2 == 0;

			var position = 0ul;

			foreach (var value in buffer.Keys)
			{
				var valueCount = (ulong)buffer[value];
				var difference = value - average;

				differenceSum += difference * difference * valueCount;

				var positionPrevious = position;
				position += valueCount;

				if (medianPosition > positionPrevious && medianPosition <= position)
					median = value;

				if (countIsEven && medianPosition == positionPrevious)
					median = Math.Round((median + value) / 2, Settings.Current.Decimals); 
			}

			lock (_locker)
			{
				var buferNews = _valuesBuffer;
				_valuesBuffer = buffer;
				
				foreach (var entry in buferNews)
					_valuesBuffer[entry.Key] = entry.Value;
			}

			var devergence = Math.Round((decimal)Math.Sqrt((double)differenceSum / (count == 0 ? 1 : count)), 4);

			return new CalculatedData(count, uniqueCount, sum, average, median, devergence, losted, minValue, maxValue, modeDuplicates, modes);
		}

		#endregion
	}
}
