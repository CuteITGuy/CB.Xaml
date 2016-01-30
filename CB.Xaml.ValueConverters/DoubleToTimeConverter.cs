using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CB.Xaml.ValueConverters
{
    [ValueConversion(typeof(double), typeof(TimeSpan)),
     ValueConversion(typeof(double), typeof(DateTime)),
     ValueConversion(typeof(double), typeof(Duration))]
    public class DoubleToTimeConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeConverter.ConvertDoubleToTime(value, targetType, parameter, culture);
            /*if (value is Double)
            {
                Double d = (Double)value;
                TimePart timePart = TimeConverter.GetTimePart(parameter, culture);

                if (targetType == typeof(TimeSpan))
                {
                    return TimeConverter.DoubleToTimeSpan(d, timePart);
                }

                if (targetType == typeof(Duration))
                {
                    return TimeConverter.DoubleToDuration(d, timePart);
                }

                if (targetType == typeof(DateTime))
                {
                    return TimeConverter.DoubleToDateTime(d, timePart);
                }
            }

            return DependencyProperty.UnsetValue;*/
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeConverter.ConvertTimeToDouble(value, targetType, parameter, culture);
            /*if (targetType == typeof(Double))
            {
                TimePart timePart = TimeConverter.GetTimePart(parameter, culture);

                if (value is TimeSpan)
                {
                    return TimeConverter.TimeSpanToDouble((TimeSpan)value, timePart);
                }

                if (value is Duration)
                {
                    return TimeConverter.DurationToDouble((Duration)value, timePart);
                }

                if (value is DateTime)
                {
                    return TimeConverter.DateTimeToDouble((DateTime)value, timePart);
                }
            }

            return DependencyProperty.UnsetValue;*/
        }
        #endregion


        /*#region Implementation
        private double ConvertFromDateTime(DateTime dateTime, TimePart timePart)
        {
            double ticks = dateTime.Ticks;

            switch (timePart)
            {
                case TimePart.Tick: return ticks;

                case TimePart.Second: return ticks / TimeSpan.TicksPerSecond;

                case TimePart.Minute: return ticks / TimeSpan.TicksPerMinute;

                case TimePart.Hour: return ticks / TimeSpan.TicksPerHour;

                case TimePart.Day: return ticks / TimeSpan.TicksPerDay;

                default: return ticks / TimeSpan.TicksPerMillisecond;
            }
        }

        private double ConvertFromDuration(Duration value, TimePart timePart)
        {
            if (value.HasTimeSpan)
            {
                return ConvertFromTimeSpan(value.TimeSpan, timePart);
            }
            return 0.0;
        }

        private double ConvertFromTimeSpan(TimeSpan value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick: return value.Ticks;

                case TimePart.Second: return value.TotalSeconds;

                case TimePart.Minute: return value.TotalMinutes;

                case TimePart.Hour: return value.TotalHours;

                case TimePart.Day: return value.TotalDays;

                default: return value.TotalMilliseconds;
            }
        }

        private DateTime ConvertToDateTime(double value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick: return new DateTime((long)value);

                case TimePart.Second: return new DateTime((long)(TimeSpan.TicksPerSecond * value));

                case TimePart.Minute: return new DateTime((long)(TimeSpan.TicksPerMinute * value));

                case TimePart.Hour: return new DateTime((long)(TimeSpan.TicksPerHour * value));

                case TimePart.Day: return new DateTime((long)(TimeSpan.TicksPerDay * value));

                default: return new DateTime((long)(TimeSpan.TicksPerMillisecond * value));
            }
        }

        private Duration ConvertToDuration(double value, TimePart timePart)
        {
            return new Duration(ConvertToTimeSpan(value, timePart));
        }

        private TimeSpan ConvertToTimeSpan(double value, TimePart timePart)
        {
            switch (timePart)
            {
                case TimePart.Tick: return TimeSpan.FromTicks((long)value);

                case TimePart.Minute: return TimeSpan.FromMinutes(value);

                case TimePart.Second: return TimeSpan.FromSeconds(value);

                case TimePart.Hour: return TimeSpan.FromHours(value);

                case TimePart.Day: return TimeSpan.FromDays(value);

                default: return TimeSpan.FromMilliseconds(value);
            }
        }

        private TimePart GetTimePart(object parameter, CultureInfo culture)
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
                    case "days": return TimePart.Day;

                    case "h":
                    case "hr":
                    case "hour":
                    case "hours": return TimePart.Hour;

                    case "m":
                    case "min":
                    case "minute":
                    case "minutes": return TimePart.Minute;

                    case "s":
                    case "sec":
                    case "second":
                    case "seconds": return TimePart.Second;

                    case "ms":
                    case "millisecond":
                    case "milliseconds": return TimePart.Millisecond;

                    case "t":
                    case "tk":
                    case "tick":
                    case "ticks": return TimePart.Tick;
                }
            }

            return TimePart.Millisecond;
        }
        #endregion*/
    }
}