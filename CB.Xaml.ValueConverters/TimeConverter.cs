using System;
using System.Globalization;
using System.Windows;


namespace CB.Xaml.ValueConverters
{
    public static class TimeConverter
    {
        #region Methods
        public static double DateTimeToDouble(DateTime dateTime, TimePart timePart)
        {
            double ticks = dateTime.Ticks;

            switch (timePart)
            {
                case TimePart.Tick:
                    return ticks;

                case TimePart.Second:
                    return ticks / TimeSpan.TicksPerSecond;

                case TimePart.Minute:
                    return ticks / TimeSpan.TicksPerMinute;

                case TimePart.Hour:
                    return ticks / TimeSpan.TicksPerHour;

                case TimePart.Day:
                    return ticks / TimeSpan.TicksPerDay;

                default:
                    return ticks / TimeSpan.TicksPerMillisecond;
            }
        }

        public static DateTime DoubleToDateTime(double value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick:
                    return new DateTime((long)value);

                case TimePart.Second:
                    return new DateTime((long)(TimeSpan.TicksPerSecond * value));

                case TimePart.Minute:
                    return new DateTime((long)(TimeSpan.TicksPerMinute * value));

                case TimePart.Hour:
                    return new DateTime((long)(TimeSpan.TicksPerHour * value));

                case TimePart.Day:
                    return new DateTime((long)(TimeSpan.TicksPerDay * value));

                default:
                    return new DateTime((long)(TimeSpan.TicksPerMillisecond * value));
            }
        }

        public static Duration DoubleToDuration(double value, TimePart timePart)
        {
            return new Duration(DoubleToTimeSpan(value, timePart));
        }

        public static TimeSpan DoubleToTimeSpan(double value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick:
                    return TimeSpan.FromTicks((long)value);

                case TimePart.Minute:
                    return TimeSpan.FromMinutes(value);

                case TimePart.Second:
                    return TimeSpan.FromSeconds(value);

                case TimePart.Hour:
                    return TimeSpan.FromHours(value);

                case TimePart.Day:
                    return TimeSpan.FromDays(value);

                default:
                    return TimeSpan.FromMilliseconds(value);
            }
        }

        public static double DurationToDouble(Duration value, TimePart timePart)
        {
            if (value.HasTimeSpan)
            {
                return TimeSpanToDouble(value.TimeSpan, timePart);
            }
            return 0.0;
        }

        public static double TimeSpanToDouble(TimeSpan value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick:
                    return value.Ticks;

                case TimePart.Second:
                    return value.TotalSeconds;

                case TimePart.Minute:
                    return value.TotalMinutes;

                case TimePart.Hour:
                    return value.TotalHours;

                case TimePart.Day:
                    return value.TotalDays;

                default:
                    return value.TotalMilliseconds;
            }
        }
        #endregion


        #region Implementation
        internal static object ConvertDoubleToTime(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                var d = (double)value;
                var timePart = GetTimePart(parameter, culture);

                if (targetType == typeof(TimeSpan))
                {
                    return DoubleToTimeSpan(d, timePart);
                }

                if (targetType == typeof(Duration))
                {
                    return DoubleToDuration(d, timePart);
                }

                if (targetType == typeof(DateTime))
                {
                    return DoubleToDateTime(d, timePart);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        internal static object ConvertTimeToDouble(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(double))
            {
                var timePart = GetTimePart(parameter, culture);

                if (value is TimeSpan)
                {
                    return TimeSpanToDouble((TimeSpan)value, timePart);
                }

                if (value is Duration)
                {
                    return DurationToDouble((Duration)value, timePart);
                }

                if (value is DateTime)
                {
                    return DateTimeToDouble((DateTime)value, timePart);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        internal static TimePart GetTimePart(object parameter, CultureInfo culture)
        {
            if (parameter is TimePart)
            {
                return (TimePart)parameter;
            }

            if (parameter is string)
            {
                switch ((parameter as string).ToLower())
                {
                    case "d":
                    case "day":
                    case "days":
                        return TimePart.Day;

                    case "h":
                    case "hr":
                    case "hour":
                    case "hours":
                        return TimePart.Hour;

                    case "m":
                    case "min":
                    case "minute":
                    case "minutes":
                        return TimePart.Minute;

                    case "s":
                    case "sec":
                    case "second":
                    case "seconds":
                        return TimePart.Second;

                    case "ms":
                    case "millisecond":
                    case "milliseconds":
                        return TimePart.Millisecond;

                    case "t":
                    case "tk":
                    case "tick":
                    case "ticks":
                        return TimePart.Tick;
                }
            }

            return TimePart.Millisecond;
        }
        #endregion
    }
}