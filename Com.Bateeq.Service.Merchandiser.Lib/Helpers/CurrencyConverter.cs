using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class CurrencyConverter
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
    }
}
