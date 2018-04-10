using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class NumberConverter
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
