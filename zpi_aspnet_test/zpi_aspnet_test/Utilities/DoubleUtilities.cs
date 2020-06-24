using System;
using System.Globalization;

namespace zpi_aspnet_test.Utilities
{
   public static class DoubleUtilities
   {
	   public static double ParsePrice(string price)
	   {
		   var format = new NumberFormatInfo();

		   if (price.Contains(".") && price.Contains(","))
		   {
			   format.NumberGroupSeparator = ",";
			   format.NumberDecimalSeparator = ".";
		   }
		   else if (price.Contains("."))
		   {
			   format.NumberDecimalSeparator = ".";
		   }
		   else if (price.Contains(","))
		   {
			   format.NumberDecimalSeparator = ",";
		   }

		   return Convert.ToDouble(price, format);
	   }
   }
}