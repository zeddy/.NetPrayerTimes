using System;
using System.Collections.Generic;

namespace PrayerTimesCalculator
{
    public class Times
    {
        public double Imsak { get; set; }
        public double Fajr { get; set; }
        public double Sunrise { get; set; }
        public double Dhuhur { get; set; }
        public double Asr { get; set; }
        public double Sunset { get; set; }
        public double Magrib { get; set; }
        public double Isha { get; set; }
        public double Midnight { get; set; }

        public Times(double imsak,
            double fajr,
            double sunrise,
            double dhuhur,
            double asr,
            double sunset,
            double magrib,
            double isha)
        {
            Imsak = imsak;
            Fajr = fajr;
            Sunrise = sunrise;
            Dhuhur = dhuhur;
            Asr = asr;
            Sunset = sunset;
            Magrib = magrib;
            Isha = isha;
        }

        public Times ToDayPortions()
        {
            var t = new Times(
                Imsak/24,
                Fajr/24,
                Sunrise/24,
                Dhuhur/24,
                Asr/24,
                Sunset/24,
                Magrib/24,
                Isha/24);

            return t;
        }

        public Times ToLongitudePortions(double timezone, double longitude)
        {
            var t = new Times(
                Imsak + timezone - longitude / 15,
                Fajr + timezone - longitude / 15,
                Sunrise + timezone - longitude / 15,
                Dhuhur + timezone - longitude / 15,
                Asr + timezone - longitude / 15,
                Sunset + timezone - longitude / 15,
                Magrib + timezone - longitude / 15,
                Isha + timezone - longitude / 15);

            return t;
        }

        public Times ApplyOffset(Dictionary<TimeNames, double> offsets)
        {
            var t = new Times(
                Imsak + offsets[TimeNames.Imsak] / 60,
                Fajr + offsets[TimeNames.Fajr] / 60,
                Sunrise + offsets[TimeNames.Sunrise] / 60,
                Dhuhur + offsets[TimeNames.Dhuhur] / 60,
                Asr + offsets[TimeNames.Asr] / 60,
                Sunset + offsets[TimeNames.Sunset] / 60,
                Magrib + offsets[TimeNames.Magrib] / 60,
                Isha + offsets[TimeNames.Isha] / 60);

            return t;
        }

        public PrayerTimeDto GetFormattedTimes(TimeFormats format)
        {
            var ptime = new PrayerTimeDto();

            ptime.Fajr = FormatTime(Fajr, format);
            ptime.Dhuhur = FormatTime(Dhuhur, format);
            ptime.Asr = FormatTime(Asr, format);
            ptime.Magrib = FormatTime(Magrib, format);
            ptime.Isha = FormatTime(Isha, format);

            return ptime;
        }

        private static string FormatTime(double time, TimeFormats format)
        {
            if (double.IsNaN(time)) return Defaults.INVALID_TIME;

            if (format == TimeFormats.Float) return time.ToString();

            time = MathUtils.FixHour(time + 0.5/60); // add 0.5 minutes to round
            var hours = Math.Floor(time);
            var minutes = Math.Floor((time - hours)*60);

            bool showAmPm = format == TimeFormats.Hour12;
            string amPm = string.Empty;
            if (showAmPm)
                amPm = hours < 12 ? "am" : "pm";

            if (format == TimeFormats.Hour12)
                hours = ((hours + 12 - 1)%12 + 1);

            var intHours = Convert.ToInt32(hours);
            var intMinutes = Convert.ToInt32(minutes);

            var timeStr = string.Format("{0:D2}:{1:D2} {2}",
                                        intHours,
                                        intMinutes,
                                        amPm);
            return timeStr;
        }
    }
}
