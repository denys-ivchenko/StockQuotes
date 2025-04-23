namespace Telesyk.StockQuotes
{
	public record struct CalculatedData(ulong Count, int UniqueCount, decimal Sum, decimal Average, decimal Median, decimal Devergence, ulong Losted, decimal MinValue, decimal MaxValue, decimal ModeDuplicates, decimal[] Modes);
}
