using System;

namespace Telesyk.StockQuotes
{
	public delegate void ValueEventHandler(object sender, ValueEventArgs args);
	
	public class ValueEventArgs : EventArgs
	{
		internal ValueEventArgs(ulong id, decimal value)
		{
			Id = id;
			Value = value;
		}
		
		public ulong Id { get; }
		
		public decimal Value { get; }
	}
}
