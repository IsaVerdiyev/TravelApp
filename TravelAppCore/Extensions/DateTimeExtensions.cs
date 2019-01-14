using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TravelAppCore.Extensions
{
    public enum DateInterval { Day, Week, Month, Year, All };

    public static class DateTimeExtensions
    {


        public static DateTime SearchedDayOfWeek(this DateTime datetime, DayOfWeek startOfWeek)
        {
            //int diff = (7 + (((int)datetime.DayOfWeek + 6) % 7 - ((int)startOfWeek + 6) % 7)) % 7;
            //return datetime.AddDays(-1 * diff).Date;

            int diff = ((int)startOfWeek + 6) % 7 - ((int)datetime.DayOfWeek + 6) % 7;
            return datetime.AddDays(diff);
        }

        public static DateTime SearchFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime SearchLastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.SearchFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime SearchFirstDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public static DateTime SearchLastDayOfYear(this DateTime dateTime)
        {
            return dateTime.SearchFirstDayOfYear().AddYears(1).AddDays(-1);
        }

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static bool IsInCurrentDateInterval(this DateTime dateTime, DateTime selectedDate, DateInterval dateInterval)
        {
            if (dateInterval == DateInterval.Day)
            {
                return dateTime.Date == selectedDate.Date;
            }
            else if (dateInterval == DateInterval.Week)
            {
                return dateTime.Date >= selectedDate.SearchedDayOfWeek(DayOfWeek.Monday).Date && dateTime.Date <= selectedDate.SearchedDayOfWeek(DayOfWeek.Sunday).Date;
            }
            else if (dateInterval == DateInterval.Month)
            {
                return dateTime.Date >= selectedDate.SearchFirstDayOfMonth().Date && dateTime.Date <= selectedDate.SearchLastDayOfMonth().Date;
            }
            else if (dateInterval == DateInterval.Year)
            {
                return dateTime.Date >= selectedDate.SearchFirstDayOfYear().Date && dateTime.Date <= selectedDate.SearchLastDayOfYear().Date;
            }
            else if (dateInterval == DateInterval.All)
            {
                return true;
            }
            else
                throw new Exception("Wrong parameters were passed");
        }

        public static DateTime? GetPastDate(this DateTime dateTime, DateInterval dateInterval)
        {
            if (dateInterval == DateInterval.Day)
            {
                return dateTime.AddDays(-1);
            }
            else if (dateInterval == DateInterval.Week)
            {
                return dateTime.AddDays(-7);
            }
            else if (dateInterval == DateInterval.Month)
            {
                return dateTime.AddMonths(-1);
            }
            else if (dateInterval == DateInterval.Year)
            {
                return dateTime.AddYears(-1);
            }
            else if (dateInterval == DateInterval.All)
            {
                return null;
            }
            else
            {
                throw new Exception("Wrong parameters were passed to GetPastDate function");
            }
        }

        public static DateTime? GetNextDate(this DateTime dateTime, DateInterval dateInterval)
        {
            if (dateInterval == DateInterval.Day)
            {
                return dateTime.AddDays(1);
            }
            else if (dateInterval == DateInterval.Week)
            {
                return dateTime.AddDays(7);
            }
            else if (dateInterval == DateInterval.Month)
            {
                return dateTime.AddMonths(1);
            }
            else if (dateInterval == DateInterval.Year)
            {
                return dateTime.AddYears(1);
            }
            else if (dateInterval == DateInterval.All)
            {
                return null;
            }
            else
            {
                throw new Exception("Wrong parameters were passed to GetPastDate function");
            }
        }

        public static DateTime GetDateFromSecondsSince1970(int daysSince1970)
        {

            DateTime date1970 = new DateTime(1970, 1, 1);
            return date1970.AddSeconds(daysSince1970);
        }
    }
}
