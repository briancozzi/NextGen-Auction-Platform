using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.ExtensionMethod
{
    public static class DateTimeZoneExtension
    {
        public static DateTime ConvertTimeFromUtcToUserTimeZone(this DateTime dateTime, string timeZoneInfoId)
        {
            TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, info);
        }

        public static DateTime ConverUserTimeZoneToUtcTime(this DateTime dateTime, string timeZOneId)
        {
            TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById(timeZOneId);
            var currentDateTime = new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(currentDateTime, info);
        }
    }
}
