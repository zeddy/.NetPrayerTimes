namespace PrayerTimesCalculator
{
    public enum TimeNames
    {
        Imsak,
        Fajr,
        Sunrise,
        Dhuhur,
        Asr,
        Sunset,
        Magrib,
        Isha,
        Midnight
    }

    public enum AsrJuristics
    {
        Standard,
        Hanafi
    }

    public enum MidnightMethods
    {
        Standard,
        Jafari
    }

    public enum HighLatitudeMethods
    {
        NightMiddle,    // middle of night
        AngleBased,     // angle/60th of night
        OneSeventh,     // 1/7 of night
        None            // no adjustment
    }

    public enum TimeFormats
    {
        Hour24,
        Hour12,
        Float
    }

    public enum CalculationMethods
    {
        MWL,        // Muslim World League
        ISNA,       // Islamic Society of North America
        Egypt,      // Egyptian General Authority of Survey
        Makkah,     // Umm Al Qura University, Makkah
        Karachi,    // University of Islamic Sciences, Karachi
        Tehran,     // Institute of Geophysics, University of Tehran
        Jafari,     // Shia Ithna-Ashari, Leva Institute, Qum
    }

    public enum Directions
    {
        CW,     // clockwise
        CCW     // counter-clockwise
    }

}