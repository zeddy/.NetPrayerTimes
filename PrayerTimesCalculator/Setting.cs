namespace PrayerTimesCalculator
{
    public class Setting
    {
        public double Imsak { get; set; }
        public bool IsImsakInMinutes { get; set; }

        public double DhuhurInMinutes { get; set; }

        public AsrJuristics AsrJuristic { get; set; }
        public HighLatitudeMethods HighLatitudeMethod { get; set; }

        // from calc param
        public double Fajr { get; set; }

        public double Magrib { get; set; }
        public bool IsMagribInMinutes { get; set; }

        public double Isha { get; set; }
        public bool IsIshaInMinutes { get; set; }

        public MidnightMethods Midnight { get; set; }
    }
}