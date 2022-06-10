using System;
namespace AreaFinder.Models
{
	public class Dual
	{
        public Dimension Type { get; set; }
        public int OriginalValue { get; set; }
        public double Value { get; set; }
        public double FinalValue { get; set; }
        public Position Position { get; set; }
    }

    public enum Dimension
    {
        Lower,
        Higher
    }
   public enum Position
    {
        X,
        Y
    }
}

