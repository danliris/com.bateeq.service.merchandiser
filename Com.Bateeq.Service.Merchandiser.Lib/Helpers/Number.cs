using System;
using System.Globalization;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class Number
    {
        public static string ToRupiah(dynamic number)
        {
            try
            {
                return number.ToString("C2", CultureInfo.CreateSpecificCulture("id-ID"));
            }
            catch (Exception)
            {
                return number.ToString();
            }
        }

        public static string ToRupiahWithoutSymbol(dynamic number)
        {
            try
            {
                return number.ToString("N2", CultureInfo.CreateSpecificCulture("id-ID"));
            }
            catch (Exception)
            {
                return number.ToString();
            }
        }

        public static string ToDollar(dynamic number)
        {
            try
            {
                return number.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch (Exception)
            {
                return number.ToString();
            }
        }
    }
}
