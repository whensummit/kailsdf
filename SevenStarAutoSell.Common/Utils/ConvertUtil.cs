using System;

namespace SevenStarAutoSell.Common.Utils
{
    public static class ConvertUtil
    {
        public static string Escape(string value)
        {
            value = value.Replace("\r", "\\r");
            value = value.Replace("\n", "\\n");
            value = value.Replace("\t", "    ");
            value = value.Replace("\\", "\\\\");
            value = value.Replace("\"", "\\\"");
            return value;
        }

        public static bool ToBool(object o)
        {
            return Convert.ToBoolean(o);
        }

        public static bool ToBool(object o, bool defaultValue)
        {
            bool flag;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                flag = Convert.ToBoolean(o);
            }
            catch
            {
                flag = defaultValue;
            }
            return flag;
        }

        public static object ToBool(object o, object defaultValue)
        {
            object flag;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                flag = Convert.ToBoolean(o);
            }
            catch
            {
                flag = defaultValue;
            }
            return flag;
        }

        public static string ToChineseNumber(decimal number)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string ch1 = "";
            string ch2 = "";
            int nzero = 0;
            number = Math.Abs(number);
            long num = (long)(number * new decimal(100));
            str4 = num.ToString();
            int j = str4.Length;
            if (j > 15)
            {
                return "溢出";
            }
            str2 = str2.Substring(15 - j);
            for (int i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);
                int temp = Convert.ToInt32(str3);
                if (i != j - 3 && i != j - 7 && i != j - 11 && i != j - 15)
                {
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero++;
                    }
                    else if (!(str3 != "0") || nzero == 0)
                    {
                        ch1 = str1.Substring(temp, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        ch1 = string.Concat("零", str1.Substring(temp, 1));
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                }
                else if (str3 != "0" && nzero != 0)
                {
                    ch1 = string.Concat("零", str1.Substring(temp, 1));
                    ch2 = str2.Substring(i, 1);
                    nzero = 0;
                }
                else if (str3 != "0" && nzero == 0)
                {
                    ch1 = str1.Substring(temp, 1);
                    ch2 = str2.Substring(i, 1);
                    nzero = 0;
                }
                else if (str3 == "0" && nzero >= 3)
                {
                    ch1 = "";
                    ch2 = "";
                    nzero++;
                }
                else if (j < 11)
                {
                    ch1 = "";
                    ch2 = str2.Substring(i, 1);
                    nzero++;
                }
                else
                {
                    ch1 = "";
                    nzero++;
                }
                if (i == j - 11 || i == j - 3)
                {
                    ch2 = str2.Substring(i, 1);
                }
                str5 = string.Concat(str5, ch1, ch2);
                if (i == j - 1 && str3 == "0")
                {
                    str5 = string.Concat(str5, "整");
                }
            }
            if (number == decimal.Zero)
            {
                str5 = "零元整";
            }
            return str5;
        }

        public static string ToDateString(DateTime o)
        {
            string[] str = new string[5];
            int year = o.Year;
            str[0] = year.ToString("0000");
            str[1] = "/";
            year = o.Month;
            str[2] = year.ToString("00");
            str[3] = "/";
            year = o.Day;
            str[4] = year.ToString("00");
            return string.Concat(str);
        }

        public static string ToDateString(object o)
        {
            return ConvertUtil.ToDateString(ConvertUtil.ToDateTime(o));
        }

        public static string ToDateString(object o, string defaultValue)
        {
            string dateString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateString = ConvertUtil.ToDateString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateString = defaultValue;
            }
            return dateString;
        }

        public static object ToDateString(object o, object defaultValue)
        {
            object dateString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateString = ConvertUtil.ToDateString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateString = defaultValue;
            }
            return dateString;
        }

        public static DateTime ToDateTime(object o)
        {
            return Convert.ToDateTime(o);
        }

        public static DateTime ToDateTime(object o, DateTime defaultValue)
        {
            DateTime dateTime;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTime = Convert.ToDateTime(o);
            }
            catch
            {
                dateTime = defaultValue;
            }
            return dateTime;
        }

        public static object ToDateTime(object o, object defaultValue)
        {
            object dateTime;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTime = Convert.ToDateTime(o);
            }
            catch
            {
                dateTime = defaultValue;
            }
            return dateTime;
        }

        public static string ToDateTimeSecondString(DateTime o)
        {
            string[] str = new string[11];
            int year = o.Year;
            str[0] = year.ToString("0000");
            str[1] = "/";
            year = o.Month;
            str[2] = year.ToString("00");
            str[3] = "/";
            year = o.Day;
            str[4] = year.ToString("00");
            str[5] = " ";
            year = o.Hour;
            str[6] = year.ToString("00");
            str[7] = ":";
            year = o.Minute;
            str[8] = year.ToString("00");
            str[9] = ":";
            year = o.Second;
            str[10] = year.ToString("00");
            return string.Concat(str);
        }

        public static string ToDateTimeSecondString(object o)
        {
            return ConvertUtil.ToDateTimeSecondString(ConvertUtil.ToDateTime(o));
        }

        public static string ToDateTimeSecondString(object o, string defaultValue)
        {
            string dateTimeSecondString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTimeSecondString = ConvertUtil.ToDateTimeSecondString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateTimeSecondString = defaultValue;
            }
            return dateTimeSecondString;
        }

        public static object ToDateTimeSecondString(object o, object defaultValue)
        {
            object dateTimeSecondString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTimeSecondString = ConvertUtil.ToDateTimeSecondString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateTimeSecondString = defaultValue;
            }
            return dateTimeSecondString;
        }

        public static string ToDateTimeString(DateTime o)
        {
            string[] str = new string[9];
            int year = o.Year;
            str[0] = year.ToString("0000");
            str[1] = "/";
            year = o.Month;
            str[2] = year.ToString("00");
            str[3] = "/";
            year = o.Day;
            str[4] = year.ToString("00");
            str[5] = " ";
            year = o.Hour;
            str[6] = year.ToString("00");
            str[7] = ":";
            year = o.Minute;
            str[8] = year.ToString("00");
            return string.Concat(str);
        }

        public static string ToDateTimeString(object o)
        {
            return ConvertUtil.ToDateTimeString(ConvertUtil.ToDateTime(o));
        }

        public static string ToDateTimeString(object o, string defaultValue)
        {
            string dateTimeString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTimeString = ConvertUtil.ToDateTimeString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateTimeString = defaultValue;
            }
            return dateTimeString;
        }

        public static object ToDateTimeString(object o, object defaultValue)
        {
            object dateTimeString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                dateTimeString = ConvertUtil.ToDateTimeString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                dateTimeString = defaultValue;
            }
            return dateTimeString;
        }

        public static decimal ToDecimal(object o)
        {
            return Convert.ToDecimal(o);
        }

        public static decimal ToDecimal(object o, decimal defaultValue)
        {
            decimal num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToDecimal(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static object ToDecimal(object o, object defaultValue)
        {
            object num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToDecimal(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static double ToDouble(object o)
        {
            return Convert.ToDouble(o);
        }

        public static double ToDouble(object o, double defaultValue)
        {
            double num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToDouble(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static object ToDouble(object o, object defaultValue)
        {
            object num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToDouble(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static float ToFloat(object o)
        {
            return Convert.ToSingle(o);
        }

        public static float ToFloat(object o, float defaultValue)
        {
            float single;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                single = Convert.ToSingle(o);
            }
            catch
            {
                single = defaultValue;
            }
            return single;
        }

        public static object ToFloat(object o, object defaultValue)
        {
            object single;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                single = Convert.ToSingle(o);
            }
            catch
            {
                single = defaultValue;
            }
            return single;
        }

        public static int ToInt(object o)
        {
            return Convert.ToInt32(o);
        }

        public static int ToInt(object o, int defaultValue)
        {
            int num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToInt32(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static object ToInt(object o, object defaultValue)
        {
            object num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToInt32(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static long ToLong(object o)
        {
            return Convert.ToInt64(o);
        }

        public static long ToLong(object o, long defaultValue)
        {
            long num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToInt64(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static object ToLong(object o, object defaultValue)
        {
            object num;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                num = Convert.ToInt64(o);
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }

        public static string ToMoneyString(object o)
        {
            return ConvertUtil.ToMoneyString(o, "0.00");
        }

        public static string ToMoneyString(object o, string format)
        {
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return string.Empty;
            }
            if (o is decimal)
            {
                return ((decimal)o).ToString(format);
            }
            if (o is float)
            {
                return ((float)o).ToString(format);
            }
            if (o is double)
            {
                return ((double)o).ToString(format);
            }
            if (o is int)
            {
                return ((int)o).ToString(format);
            }
            if (!(o is long))
            {
                return o.ToString();
            }
            return ((long)o).ToString(format);
        }

        public static string ToString(object o)
        {
            return o.ToString();
        }

        public static string ToString(object o, string defaultValue)
        {
            string str;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                str = o.ToString();
            }
            catch
            {
                str = defaultValue;
            }
            return str;
        }

        public static object ToString(object o, object defaultValue)
        {
            object str;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                str = o.ToString();
            }
            catch
            {
                str = defaultValue;
            }
            return str;
        }

        public static string ToTimeString(DateTime o)
        {
            int hour = o.Hour;
            string str = hour.ToString("00");
            hour = o.Minute;
            return string.Concat(str, ":", hour.ToString("00"));
        }

        public static string ToTimeString(object o)
        {
            return ConvertUtil.ToTimeString(ConvertUtil.ToDateTime(o));
        }

        public static string ToTimeString(object o, string defaultValue)
        {
            string timeString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                timeString = ConvertUtil.ToTimeString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                timeString = defaultValue;
            }
            return timeString;
        }

        public static object ToTimeString(object o, object defaultValue)
        {
            object timeString;
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
            {
                return defaultValue;
            }
            try
            {
                timeString = ConvertUtil.ToTimeString(ConvertUtil.ToDateTime(o));
            }
            catch
            {
                timeString = defaultValue;
            }
            return timeString;
        }
    }
}