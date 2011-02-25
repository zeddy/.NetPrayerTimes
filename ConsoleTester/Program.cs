using System;
using PrayerTimesCalculator;

namespace ConsoleTester
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Using Muslim World League Method");
            Console.WriteLine("--- Location: Jakarta ----");
            var calc1 = new Calculator();
            var res1 = calc1.GetPrayerTimes(DateTime.Today, -6.17, 106.83, 0, +7);  // Jakarta
            Console.WriteLine("Fajr: {0}", res1.Fajr);
            Console.WriteLine("Dhuhur: {0}", res1.Dhuhur);
            Console.WriteLine("Asr: {0}", res1.Asr);
            Console.WriteLine("Magrib: {0}", res1.Magrib);
            Console.WriteLine("Isha: {0}", res1.Isha);
            Console.WriteLine();

            Console.WriteLine("Using Makkah Method");
            Console.WriteLine("--- Location: Dubai ----");
            var calc2 = new Calculator(CalculationMethods.Makkah, AsrJuristics.Standard, HighLatitudeMethods.NightMiddle);
            var res2 = calc2.GetPrayerTimes(DateTime.Today, 25.2522, 55.2800, 0, +4);   // Dubai
            Console.WriteLine("Fajr: {0}", res2.Fajr);
            Console.WriteLine("Dhuhur: {0}", res2.Dhuhur);
            Console.WriteLine("Asr: {0}", res2.Asr);
            Console.WriteLine("Magrib: {0}", res2.Magrib);
            Console.WriteLine("Isha: {0}", res2.Isha);
            Console.WriteLine();

            Console.WriteLine("Using ISNA Method");
            Console.WriteLine("--- Location: Redmond ----");
            var calc3 = new Calculator(CalculationMethods.ISNA, AsrJuristics.Standard, HighLatitudeMethods.AngleBased);
            var res3 = calc3.GetPrayerTimes(DateTime.Today, 47.6833, -122.1231, 0, -8);
            Console.WriteLine("Fajr: {0}", res3.Fajr);
            Console.WriteLine("Dhuhur: {0}", res3.Dhuhur);
            Console.WriteLine("Asr: {0}", res3.Asr);
            Console.WriteLine("Magrib: {0}", res3.Magrib);
            Console.WriteLine("Isha: {0}", res3.Isha);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
