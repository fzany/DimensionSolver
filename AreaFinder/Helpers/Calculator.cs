using System;
namespace AreaFinder.Helpers
{
	public class Calculator
	{
        public static (int, int, int) GetLowestDivisor(int A, int B) //500x600
        {
            int divisionFactor = 1;
            //Get the lowest number.
            var lowest = Math.Min(A, B);

            //Return if the lower part of the ratio is less than 2.
            int loopIndex = 2;
            if (loopIndex > lowest) //dont run
            {
                return (A, B, divisionFactor);
            }

            while (loopIndex < lowest)
            {
                if (Math.DivRem(A, loopIndex).Remainder == 0 && Math.DivRem(B, loopIndex).Remainder == 0)
                {
                    A = A / loopIndex;
                    B = B / loopIndex;
                    divisionFactor = divisionFactor * loopIndex;
                }
                else
                    loopIndex++;

            }

            return (A, B, divisionFactor);
        }
    }
}

