using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Helper
{
    public class Helper
    {
        public static double GetNextBidAmount(double lastBidAmount)
        {
            var nextBidValue = 0.05;
            if (lastBidAmount == 0.00)
                nextBidValue = 0.05;

            if (lastBidAmount >= 0.01 && lastBidAmount <= 0.99)
                nextBidValue = 0.05;

            if (lastBidAmount >= 1 && lastBidAmount <= 4.99)
                nextBidValue = 0.25;

            if (lastBidAmount >= 5 && lastBidAmount <= 24.99)
                nextBidValue = 0.50;

            if (lastBidAmount >= 25 && lastBidAmount <= 99.99)
                nextBidValue = 1.00;

            if (lastBidAmount >= 100 && lastBidAmount <= 249.99)
                nextBidValue = 2.50;

            if (lastBidAmount >= 250 && lastBidAmount <= 499.99)
                nextBidValue = 5.00;

            if (lastBidAmount >= 500 && lastBidAmount <= 999.99)
                nextBidValue = 10.00;

            if (lastBidAmount >= 1000 && lastBidAmount <= 2499.99)
                nextBidValue = 25.00;

            if (lastBidAmount >= 2500 && lastBidAmount <= 4999.99)
                nextBidValue = 50.00;

            if (lastBidAmount >= 5000)
                nextBidValue = 100.00;

            return nextBidValue + lastBidAmount;
        }
    }
}
