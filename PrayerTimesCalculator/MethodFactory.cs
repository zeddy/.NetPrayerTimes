namespace PrayerTimesCalculator
{
    public static class MethodFactory
    {
        public static Method Build(CalculationMethods m)
        {
            Method toReturn;
            switch(m)
            {
                case CalculationMethods.MWL:
                    toReturn = BuildForMwl();
                    break;

                case CalculationMethods.ISNA:
                    toReturn = BuildForIsna();
                    break;

                case CalculationMethods.Egypt:
                    toReturn = BuildForEgypt();
                    break;

                case CalculationMethods.Makkah:
                    toReturn = BuildForMakkah();
                    break;
                    
                case CalculationMethods.Karachi:
                    toReturn = BuildForKarachi();
                    break;

                case CalculationMethods.Tehran:
                    toReturn = BuildForTehran();
                    break;

                case CalculationMethods.Jafari:
                    toReturn = BuildForJafari();
                    break;

                default:
                    toReturn = BuildForMakkah();
                    break;
            }

            return toReturn;
        }

        private static Method BuildForJafari()
        {
            var param = new Param { Fajr = 16, Isha = 14, Magrib = 4, Midnight = MidnightMethods.Jafari };
            var m = new Method { Name = "Shia Ithna-Ashari, Leva Institute, Qum", Params = param };
            return m;
        }

        private static Method BuildForTehran()
        {
            var param = new Param {Fajr = 17.7, Isha = 14, Magrib = 4.5, Midnight = MidnightMethods.Jafari};
            var m = new Method { Name = "Institute of Geophysics, University of Tehran", Params = param };
            return m;
        }

        private static Method BuildForKarachi()
        {
            var param = new Param
                            {
                                Fajr = 18,
                                Isha = 18,
                                Magrib = 0,
                                IsMagribInMinutes = true,
                                Midnight = MidnightMethods.Standard
                            };
            var m = new Method { Name = "University of Islamic Sciences, Karachi", Params = param };
            return m;
        }

        private static Method BuildForMakkah()
        {
            var param = new Param
                            {
                                Fajr = 18.5,
                                Isha = 90,
                                IsIshaInMinutes = true,
                                Magrib = 0,
                                IsMagribInMinutes = true,
                                Midnight = MidnightMethods.Standard
                            };
            var m = new Method { Name = "Umm Al-Qura University, Makkah", Params = param };
            return m;
        }

        private static Method BuildForEgypt()
        {
            var param = new Param
                            {
                                Fajr = 19.5,
                                Isha = 17.5,
                                Magrib = 0,
                                IsMagribInMinutes = true,
                                Midnight = MidnightMethods.Standard
                            };
            var m = new Method { Name = "Egyptian General Authority of Survey", Params = param };
            return m;
        }

        private static Method BuildForIsna()
        {
            var param = new Param
                            {
                                Fajr = 15,
                                Isha = 15,
                                Magrib = 0,
                                IsMagribInMinutes = true,
                                Midnight = MidnightMethods.Standard
                            };
            var m = new Method { Name = "Islamic Society of North America (ISNA)", Params = param };
            return m;
        }

        private static Method BuildForMwl()
        {
            var param = new Param
                            {
                                Fajr = 18,
                                Isha = 17,
                                Magrib = 0,
                                IsMagribInMinutes = true,
                                Midnight = MidnightMethods.Standard
                            };
            var m = new Method {Name = "Muslim World League", Params = param};
            return m;
        }
    }
}