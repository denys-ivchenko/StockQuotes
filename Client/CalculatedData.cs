namespace Telesyk.StockQuotes
{
	public record struct CalculatedData(ulong Count, int UniqueCount, decimal Sum, decimal Average, decimal Median, decimal Devergence, ulong Failed, decimal MinValue, decimal MaxValue, decimal ModeCount, decimal[] Modes);
}
