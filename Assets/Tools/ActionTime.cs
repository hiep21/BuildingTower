using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tool
{
    public class ActionTime
    {
        public static long ConvertTimeFromSecondToDay(int timeSecond)
        {
            int day;
            int hour;
            int minute;
            int time = 0;
            if (timeSecond >= 86400)
            {
                day = timeSecond / 86400;
                timeSecond = timeSecond - day * 86400;
                time += day * 1000000;
            }
            if (timeSecond >= 3600)
            {
                hour = timeSecond / 3600;
                timeSecond = timeSecond - hour * 3600;
                time += hour * 10000;
            }
            if (timeSecond >= 60)
            {
                minute = timeSecond / 60;
                timeSecond = timeSecond - minute * 60;
                time += minute * 100;
            }
            if (timeSecond < 60)
            {
                time += timeSecond;
            }
            return time;
        }
        public static long ConvertTimeFromDayToSecond(long time)
        {
            int day, hour, minute;
            int totalSeconds = 0;


            if (time >= 1000000)
            {
                day = (int)(time / 1000000);
                time = time - day * 1000000;
                totalSeconds += day * 86400;
            }


            if (time >= 10000)
            {
                hour = (int)(time / 10000);
                time = time - hour * 10000;
                totalSeconds += hour * 3600;
            }

            if (time >= 100)
            {
                minute = (int)(time / 100);
                time = time - minute * 100;
                totalSeconds += minute * 60;
            }

            if (time > 0)
            {
                totalSeconds += (int)time;
            }

            return totalSeconds;
        }
        public static int DistanceTimeSecond(string subtrahend, string minuend, bool isAbs = true)
        {
            // Chuyển string thành DateTime
            DateTime dateTime1 = DateTime.ParseExact(subtrahend, "yyyyMMddHHmmss", null);
            DateTime dateTime2 = DateTime.ParseExact(minuend, "yyyyMMddHHmmss", null);

            // Tính khoảng cách thời gian và chuyển về giây
            TimeSpan difference = dateTime2 - dateTime1;
            if (isAbs)
            {
                return (int)Math.Abs(difference.TotalSeconds);
            }
            else
            {
                return (int)difference.TotalSeconds;
            }
        }
        public static long AddTimeSecond(string timeCurrent, int valueSecond)
        {
            // Chuyển đổi string time thành DateTime
            DateTime dateTime = DateTime.ParseExact(timeCurrent, "yyyyMMddHHmmss", null);

            // Cộng thêm số giây được chỉ định
            DateTime futureTime = dateTime.AddSeconds(valueSecond);

            // Chuyển đổi trở lại định dạng string "yyyyMMddHHmmss"
            return long.Parse(futureTime.ToString("yyyyMMddHHmmss"));
        }
        public static long AddTimeDay(string timeCurrent, int valueDay, bool isStartDay)
        {
            // Chuyển đổi string time thành DateTime
            DateTime dateTime = DateTime.ParseExact(timeCurrent, "yyyyMMddHHmmss", null);

            // Cộng thêm số ngày được chỉ định
            DateTime futureTime = dateTime.AddDays(valueDay);

            if (isStartDay)
            {
                return long.Parse(futureTime.ToString("yyyyMMdd000000"));
            }
            else
            {
                // Chuyển đổi trở lại định dạng string "yyyyMMddHHmmss"
                return long.Parse(futureTime.ToString("yyyyMMddHHmmss"));
            }
        }
        public static long AddTime(string timeCurrent, string timeAdd)
        {
            DateTime dateTime = DateTime.ParseExact(timeCurrent, "yyyyMMddHHmmss", null);
            DateTime timeToAdd = DateTime.ParseExact(timeAdd, "yyyyMMddHHmmss", null);

            // Tính khoảng cách thời gian (có thể âm hoặc dương)
            TimeSpan difference = timeToAdd - DateTime.MinValue;
            DateTime futureTime = dateTime.Add(difference);

            return long.Parse(futureTime.ToString("yyyyMMddHHmmss"));
        }

    }

}