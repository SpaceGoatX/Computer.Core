using System;

namespace Computer.Core
{
    public static class Tvm
    {
        public static readonly Func<double, int, double> FvFactor = (rate, nbPeriod) => Math.Pow(1 + rate, nbPeriod);
        
        public static readonly Func<double, int, double> PvFactor = (rate, nbPeriod) => 1 / FvFactor(rate, nbPeriod);

        public static readonly Func<double, int, double, double> PvFactorAnnuityCertain =
            (rate, nbPeriod, paymentDue) => paymentDue * ((1 - PvFactor(rate, nbPeriod)) / rate);

        public static readonly Func<double, int, double, double> FvFactorAnnuityCertain =
            (rate, nbPeriod, paymentDue) =>
                paymentDue * (PvFactorAnnuityCertain(rate, nbPeriod, paymentDue) * FvFactor(rate, nbPeriod));

        public static readonly Func<double, int, double, double, double, double> Pv =
            (rate, nbPeriod, pmt, futureValue, paymentDue) =>
                -(pmt * PvFactorAnnuityCertain(rate, nbPeriod, paymentDue) +
                futureValue * PvFactor(rate, nbPeriod));

        public static readonly Func<double, int, double, double, double, double> Fv =
            (rate, nbPeriod, pmt, presentValue, paymentDue) =>
                -(pmt * FvFactorAnnuityCertain(rate, nbPeriod, paymentDue) + presentValue *
                  FvFactor(rate, nbPeriod));

        public static readonly Func<double, int, double, double, double, double> Pmt =
            (rate, nbPeriod, presentValue, futureValue, paymentDue) =>
                -(presentValue + futureValue * PvFactor(rate, nbPeriod)) /
                PvFactorAnnuityCertain(rate, nbPeriod, paymentDue);
    }
}