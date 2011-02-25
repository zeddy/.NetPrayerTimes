namespace PrayerTimesCalculator
{
    public class Param
    {
        public double Fajr { get; set; }
        
        public double Magrib { get; set; }
        public bool IsMagribInMinutes { get; set; }

        public double Isha { get; set; }
        public bool IsIshaInMinutes { get; set; }

        public MidnightMethods Midnight { get; set; }
    }
}
