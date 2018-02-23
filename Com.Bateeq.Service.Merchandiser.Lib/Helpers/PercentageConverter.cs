using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class PercentageConverter
    {
        public static double ToFraction(dynamic number)
        {
            try
            {
                return (double)number / 100; 
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static double ToPercent(dynamic number)
        {
            try
            {
                return (double)number * 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
