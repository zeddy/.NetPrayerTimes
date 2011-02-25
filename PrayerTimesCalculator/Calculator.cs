/*
 * A translation of the original JavaScript code written by Hamid Zarrabi-Zadeh
 * Reference: http://praytimes.org/code/
 */

using System;
using System.Collections.Generic;

namespace PrayerTimesCalculator
{

    public class Calculator
    {
        private readonly Method _calcMethod;
        private Setting _settings;
        private readonly Dictionary<TimeNames, double> _offsets = new Dictionary<TimeNames, double>();

        private double _julianDate;
        private double _latitude;
        private double _longitude;
        private double _elevation;
        private double _timezone;
        private TimeFormats _timeFormat;

        public Calculator()
            : this(Defaults.CALCULATION_METHOD, 
            Defaults.ASR_JURISTIC,
            Defaults.HIGH_LATITUDE_METHOD)
        {
        }

        public Calculator(CalculationMethods m, AsrJuristics a, HighLatitudeMethods h)
        {
            _calcMethod = MethodFactory.Build(m);

            InitSettings(a, h);

            InitOffsets();
        }

        private void InitOffsets()
        {
            _offsets[TimeNames.Imsak] = 0;
            _offsets[TimeNames.Fajr] = 0;
            _offsets[TimeNames.Sunrise] = 0;
            _offsets[TimeNames.Dhuhur] = 0;
            _offsets[TimeNames.Asr] = 0;
            _offsets[TimeNames.Sunset] = 0;
            _offsets[TimeNames.Magrib] = 0;
            _offsets[TimeNames.Isha] = 0;
            _offsets[TimeNames.Midnight] = 0;
        }

        private void InitSettings(AsrJuristics a, HighLatitudeMethods h)
        {
            _settings = new Setting
                            {
                                Imsak = Defaults.IMSAK_MINUTE,
                                IsImsakInMinutes = true,
                                DhuhurInMinutes = Defaults.DHUHUR_MINUTE,
                                AsrJuristic = a,
                                HighLatitudeMethod = h
                            };

            // from calc param
            _settings.Fajr = _calcMethod.Params.Fajr;

            _settings.Magrib = _calcMethod.Params.Magrib;
            _settings.IsMagribInMinutes = _calcMethod.Params.IsMagribInMinutes;

            _settings.Isha = _calcMethod.Params.Isha;
            _settings.IsIshaInMinutes = _calcMethod.Params.IsIshaInMinutes;

            _settings.Midnight = _calcMethod.Params.Midnight;

            _timeFormat = Defaults.TIME_FORMAT;
        }

        public PrayerTimeDto GetPrayerTimes(DateTime date, double lat, double lng, double elev, double timezone, 
            bool isDst = false, TimeFormats format = TimeFormats.Hour12)
        {
            _latitude = lat;
            _longitude = lng;
            _elevation = elev;
            _timezone = timezone + (isDst ? 1 : 0);
            _timeFormat = format;
            _julianDate = ConvertToJulian(date) - _longitude / (15 * 24);

            return ComputeTimes();
        }

        private PrayerTimeDto ComputeTimes()
        {
            var times = new Times(5, 5, 6, 12, 13, 18, 18, 18);

            times = ComputePrayerTimes(times);

            times = AdjustTimes(times);

            // Add midnight time
            times.Midnight = _settings.Midnight == MidnightMethods.Jafari
                                 ? times.Magrib +
                                   TimeDiff(times.Magrib, times.Fajr)/2.0
                                 : times.Sunset +
                                   TimeDiff(times.Sunset, times.Sunrise)/2.0;

            times = TuneTimes(times);

            return ModifyFormats(times);
        }

        private Times TuneTimes(Times times)
        {
            // Apply offsets to the times
            times = times.ApplyOffset(_offsets);

            return times;
        }

        private Times AdjustTimes(Times times)
        {
            times = times.ToLongitudePortions(_timezone, _longitude);

            if (_settings.HighLatitudeMethod != HighLatitudeMethods.None)
                times = AdjustHighLatitudes(times);

            if (_settings.IsImsakInMinutes)
                times.Imsak = times.Fajr - _settings.Imsak/60.0;

            if (_settings.IsMagribInMinutes)
                times.Magrib = times.Sunset + _settings.Magrib/60.0;

            if (_settings.IsIshaInMinutes)
                times.Isha = times.Magrib + _settings.Isha/60.0;

            times.Dhuhur += _settings.DhuhurInMinutes/60.0;

            return times;

        }

        private Times AdjustHighLatitudes(Times times)
        {
            var nightTime = TimeDiff(times.Sunset, times.Sunrise);

            times.Imsak = AdjustHLTime(times.Imsak, times.Sunrise, _settings.Imsak, nightTime, Directions.CCW);
            times.Fajr = AdjustHLTime(times.Fajr, times.Sunrise, _settings.Fajr, nightTime, Directions.CCW);
            times.Isha = AdjustHLTime(times.Isha, times.Sunset, _settings.Isha, nightTime);
            times.Magrib = AdjustHLTime(times.Magrib, times.Sunset, _settings.Magrib, nightTime);

            return times;
        }

        private double AdjustHLTime(double time, double basedOn, double angle, double night, Directions dir = Directions.CW)
        {
            var portion = NightPortion(angle, night);
            var timeDiff = dir == Directions.CCW
                               ? TimeDiff(time, basedOn)
                               : TimeDiff(basedOn, time);
            if (double.IsNaN(time) || timeDiff > portion)
                time = basedOn + (dir == Directions.CCW ? -portion : portion);

            return time;
        }

        private double NightPortion(double angle, double night)
        {
            var method = _settings.HighLatitudeMethod;
            double portion;
            switch (method)
            {
                case HighLatitudeMethods.AngleBased:
                    portion = 1.0/60.0*angle;
                    break;

                case HighLatitudeMethods.OneSeventh:
                    portion = 1.0/7.0;
                    break;

                default:
                    portion = 1.0/2.0; // MidNight
                    break;
            }
            return portion*night;
        }

        private Times ComputePrayerTimes(Times times)
        {
            times = times.ToDayPortions();

            var imsak = SunAngleTime(_settings.Imsak, times.Imsak, Directions.CCW);
            var fajr = SunAngleTime(_settings.Fajr, times.Fajr, Directions.CCW);
            var sunrise = SunAngleTime(RiseSetAngle(), times.Sunrise, Directions.CCW);
            var dhuhur = MidDay(times.Dhuhur);
            var asr = AsrTime(AsrFactor(_settings.AsrJuristic), times.Asr);
            var sunset = SunAngleTime(RiseSetAngle(), times.Sunset);
            var magrib = SunAngleTime(_settings.Magrib, times.Magrib);
            var isha = SunAngleTime(_settings.Isha, times.Isha);

            var t = new Times(imsak, fajr, sunrise, dhuhur, asr, sunset, magrib, isha);
            return t;
        }

        private double AsrTime(int asrFactor, double time)
        {
            var declination = SunPosition(_julianDate + time).Declination;
            var angle = -MathUtils.ArcCot(asrFactor + MathUtils.Tan(Math.Abs(_latitude - declination)));
            return SunAngleTime(angle, time);
        }

        private static int AsrFactor(AsrJuristics asrJuristic)
        {
            return asrJuristic == AsrJuristics.Hanafi ? 2 : 1;
        }

        private double RiseSetAngle()
        {
            //var earthRad = 6371009; // in metres
            //var angle = MathUtils.ArcCos(earthRad/(earthRad + _elevation));

            var angle = 0.0347*Math.Sqrt(_elevation); // an approximation
            return 0.833 + angle;
        }

        private double SunAngleTime(double angle, double time, Directions dir = Directions.CW)
        {
            var declination = SunPosition(_julianDate + time).Declination;
            var noon = MidDay(time);
            var t = 1.0 / 15.0 *
                    MathUtils.ArcCos(
                        (-MathUtils.Sin(angle) - MathUtils.Sin(declination)*MathUtils.Sin(_latitude))/
                        (MathUtils.Cos(declination)*MathUtils.Cos(_latitude))
                        );
            return noon + (dir == Directions.CCW ? -t : t);
        }

        private double MidDay(double time)
        {
            var eqt = SunPosition(_julianDate + time).EquationOfTime;
            var noon = MathUtils.FixHour(12 - eqt);
            return noon;
        }

        // compute declination angle of sun and equation of time
        // Ref: http://aa.usno.navy.mil/faq/docs/SunApprox.php
        private static SunPositionDto SunPosition(double jd)
        {
            var D = jd - 2451545.0;
            var g = MathUtils.FixAngle(357.529 + 0.98560028*D);
            var q = MathUtils.FixAngle(280.459 + 0.98564736*D);
            var L = MathUtils.FixAngle(q + 1.915*MathUtils.Sin(g) + 0.020*MathUtils.Sin(2*g));

            //var R = 1.00014 - 0.01671*MathUtils.Cos(g) - 0.00014*MathUtils.Cos(2*g);
            var e = 23.439 - 0.00000036*D;

            var RA = MathUtils.ArcTan2(MathUtils.Cos(e)*MathUtils.Sin(L), MathUtils.Cos(L))/15.0;
            var eqt = q/15.0 - MathUtils.FixHour(RA);
            var decl = MathUtils.ArcSin(MathUtils.Sin(e)*MathUtils.Sin(L));

            return new SunPositionDto {Declination = decl, EquationOfTime = eqt};
        }

        private PrayerTimeDto ModifyFormats(Times times)
        {
            return times.GetFormattedTimes(_timeFormat);
        }

        private static double TimeDiff(double time1, double time2)
        {
            return MathUtils.FixHour(time2 - time1);
        }

        // convert Gregorian date to Julian day
        // Ref: Astronomical Algorithms by Jean Meeus
        private static double ConvertToJulian(DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            double A = Math.Floor(year/100.0);
            double B = 2 - A + Math.Floor(A/4.0);

            var JD = Math.Floor(365.25*(year + 4716)) +
                     Math.Floor(30.6001*(month + 1)) + day + B - 1524.5;
            return JD;
        }
    }
}
