using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class PercentageConverter
    {
        public static double ToFraction(double number)
        {
            return number / 100;
        }

        public static double ToPercent(double number)
        {
            return number * 100;
        }
    }
}
