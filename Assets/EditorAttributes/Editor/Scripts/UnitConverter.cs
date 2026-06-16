using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    internal enum UnitCategory
    {
        Custom,
        Time,
        Length,
        Mass,
        Volume,
        Area,
        Temperature,
        Angle,
        Speed,
        Force,
        Energy,
        Power,
        Pressure,
        ElectricCurrent,
        Voltage,
        Resistance,
        Capacitance,
        Inductance,
        Frequency,
        Data,
        Density,
        FuelEconomy,
        Percentage
    }

    [Serializable]
    internal class UnitDefinition
    {
        public string unitName;

        internal Unit unit;
        public UnitCategory category;

        [EnableField(nameof(category), UnitCategory.Custom)]
        public string categoryName;

        public string unitLabel;

        [Tooltip("How many base units equal one of this unit. Must be 1 for the base unit")]
        public double baseFactor;

        internal UnitDefinition(Unit unit, string unitLabel, UnitCategory category, double baseFactor)
        {
            this.unit = unit;
            this.unitLabel = unitLabel;
            this.category = category;
            this.baseFactor = baseFactor;

            categoryName = category.ToString();
            unitName = unit.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is UnitDefinition other && unitName == other.unitName;
        }

        public override int GetHashCode()
        {
            return unitName.GetHashCode();
        }
    }

    internal class UnitConverter
    {
        internal string from;
        internal string to;

        internal string unitLabel;
        internal double conversion;

        internal static readonly HashSet<UnitDefinition> UNIT_DEFINITIONS = new()
        {
            // Time (base = seconds)
            new UnitDefinition(Unit.Microsecond, "μs", UnitCategory.Time, 1e-6d),
            new UnitDefinition(Unit.Millisecond, "ms", UnitCategory.Time, 1e-3d),
            new UnitDefinition(Unit.Second, "s", UnitCategory.Time, 1d),
            new UnitDefinition(Unit.Minute, "m", UnitCategory.Time, 60d),
            new UnitDefinition(Unit.Hour, "h", UnitCategory.Time, 3600d),
            new UnitDefinition(Unit.Day, "d", UnitCategory.Time, 86_400d),
            new UnitDefinition(Unit.Week, "wk", UnitCategory.Time, 7d * 86_400d),
            new UnitDefinition(Unit.Month, "mo", UnitCategory.Time, 30.44d * 86_400d),
            new UnitDefinition(Unit.Year, "yr", UnitCategory.Time, 365.2425d * 86_400d),
            new UnitDefinition(Unit.Decade, "decade", UnitCategory.Time, 10d * 31_556_952d),
            new UnitDefinition(Unit.Century, "century", UnitCategory.Time, 100d * 31_556_952d),
            new UnitDefinition(Unit.Millennium, "millennium", UnitCategory.Time, 1000d * 31_556_952d),

            // Length / Distance (base = meter)
            new UnitDefinition(Unit.Angstrom, "Å", UnitCategory.Length, 1e-10d),
            new UnitDefinition(Unit.Nanometer, "nm", UnitCategory.Length, 1e-9d),
            new UnitDefinition(Unit.Micrometer, "μm", UnitCategory.Length, 1e-6d),
            new UnitDefinition(Unit.Millimeter, "mm", UnitCategory.Length, 1e-3d),
            new UnitDefinition(Unit.Centimeter, "cm", UnitCategory.Length, 1e-2d),
            new UnitDefinition(Unit.Decimeter, "dm", UnitCategory.Length, 1e-1d),
            new UnitDefinition(Unit.Meter, "m", UnitCategory.Length, 1d),
            new UnitDefinition(Unit.Decameter, "dam", UnitCategory.Length, 10d),
            new UnitDefinition(Unit.Hectometer, "hm", UnitCategory.Length, 100d),
            new UnitDefinition(Unit.Kilometer, "km", UnitCategory.Length, 1000d),

            new UnitDefinition(Unit.Inch, "in", UnitCategory.Length, 0.0254d),
            new UnitDefinition(Unit.Foot, "ft", UnitCategory.Length, 0.3048d),
            new UnitDefinition(Unit.Yard, "yd", UnitCategory.Length, 0.9144d),
            new UnitDefinition(Unit.Chain, "chain", UnitCategory.Length, 20.1168d),
            new UnitDefinition(Unit.Furlong, "fur", UnitCategory.Length, 201.168d),
            new UnitDefinition(Unit.Mile, "mi", UnitCategory.Length, 1609.344d),
            new UnitDefinition(Unit.NauticalMile, "nmi", UnitCategory.Length, 1852d),

            // Mass / Weight (base = gram)
            new UnitDefinition(Unit.Carat, "ct", UnitCategory.Mass, 0.2d),
            new UnitDefinition(Unit.Milligram, "mg", UnitCategory.Mass, 0.001d),
            new UnitDefinition(Unit.Centigram, "cg", UnitCategory.Mass, 0.01d),
            new UnitDefinition(Unit.Decigram, "dg", UnitCategory.Mass, 0.1d),
            new UnitDefinition(Unit.Gram, "g", UnitCategory.Mass, 1d),
            new UnitDefinition(Unit.Decagram, "dag", UnitCategory.Mass, 10d),
            new UnitDefinition(Unit.Hectogram, "hg", UnitCategory.Mass, 100d),
            new UnitDefinition(Unit.Kilogram, "kg", UnitCategory.Mass, 1000d),
            new UnitDefinition(Unit.MetricTonne, "t", UnitCategory.Mass, 1_000_000d),

            new UnitDefinition(Unit.Ounce, "oz", UnitCategory.Mass, 28.3495d),
            new UnitDefinition(Unit.Pound, "lb", UnitCategory.Mass, 453.592d),
            new UnitDefinition(Unit.Stone, "st", UnitCategory.Mass, 6350.29d),
            new UnitDefinition(Unit.ShortTon, "short ton", UnitCategory.Mass, 907184.74d),
            new UnitDefinition(Unit.LongTon, "long ton", UnitCategory.Mass, 1_016_046.91d),

            // Volume (base = liter)
            new UnitDefinition(Unit.Milliliter, "ml", UnitCategory.Volume, 0.001d),
            new UnitDefinition(Unit.Centiliter, "cl", UnitCategory.Volume, 0.01d),
            new UnitDefinition(Unit.Deciliter, "dl", UnitCategory.Volume, 0.1d),
            new UnitDefinition(Unit.Liter, "l", UnitCategory.Volume, 1d),
            new UnitDefinition(Unit.Decaliter, "dal", UnitCategory.Volume, 10d),
            new UnitDefinition(Unit.Hectoliter, "hl", UnitCategory.Volume, 100d),
            new UnitDefinition(Unit.Kiloliter, "kl", UnitCategory.Volume, 1000d),

            new UnitDefinition(Unit.CubicMillimeter, "mm³", UnitCategory.Volume, 1e-6d),
            new UnitDefinition(Unit.CubicCentimeter, "cm³", UnitCategory.Volume, 0.001d),
            new UnitDefinition(Unit.CubicDecimeter, "dm³", UnitCategory.Volume, 1d),
            new UnitDefinition(Unit.CubicMeter, "m³", UnitCategory.Volume, 1000d),
            new UnitDefinition(Unit.CubicDecameter, "dam³", UnitCategory.Volume, 1e6d),
            new UnitDefinition(Unit.CubicHectometer, "hm³", UnitCategory.Volume, 1e8d),
            new UnitDefinition(Unit.CubicKilometer, "km³", UnitCategory.Volume, 1e12d),

            new UnitDefinition(Unit.CubicInch, "in³", UnitCategory.Volume, 0.0163871d),
            new UnitDefinition(Unit.CubicFoot, "ft³", UnitCategory.Volume, 28.3168d),
            new UnitDefinition(Unit.CubicYard, "yd³", UnitCategory.Volume, 764.555d),
            new UnitDefinition(Unit.CubicFurlong, "fur³", UnitCategory.Volume, 8.809e7d),
            new UnitDefinition(Unit.CubicMile, "mi³", UnitCategory.Volume, 4.168e10d),

            new UnitDefinition(Unit.Teaspoon_US, "tsp", UnitCategory.Volume, 0.00492892d),
            new UnitDefinition(Unit.Tablespoon_US, "tbsp", UnitCategory.Volume, 0.0147868d),
            new UnitDefinition(Unit.FluidOunce_US, "fl oz", UnitCategory.Volume, 0.0295735d),
            new UnitDefinition(Unit.Cup_US, "cup", UnitCategory.Volume, 0.236588d),
            new UnitDefinition(Unit.Pint_US, "pt", UnitCategory.Volume, 0.473176d),
            new UnitDefinition(Unit.Quart_US, "qt", UnitCategory.Volume, 0.946353d),
            new UnitDefinition(Unit.Gallon_US, "gal", UnitCategory.Volume, 3.78541d),

            new UnitDefinition(Unit.Teaspoon_UK, "tsp", UnitCategory.Volume, 0.00591939d),
            new UnitDefinition(Unit.Tablespoon_UK, "tbsp", UnitCategory.Volume, 0.0177582d),
            new UnitDefinition(Unit.FluidOunce_UK, "fl oz", UnitCategory.Volume, 0.0284131d),
            new UnitDefinition(Unit.Cup_UK, "cup", UnitCategory.Volume, 0.284131d),
            new UnitDefinition(Unit.Pint_UK, "pt", UnitCategory.Volume, 0.568261d),
            new UnitDefinition(Unit.Quart_UK, "qt", UnitCategory.Volume, 1.13652d),
            new UnitDefinition(Unit.Gallon_UK, "gal", UnitCategory.Volume, 4.54609d),

            // Area (base = square meter)
            new UnitDefinition(Unit.SquareAngstrom, "Å²", UnitCategory.Area, 1e-20d),
            new UnitDefinition(Unit.SquareNanometer, "nm²", UnitCategory.Area, 1e-18d),
            new UnitDefinition(Unit.SquareMicrometer, "μm²", UnitCategory.Area, 1e-12d),
            new UnitDefinition(Unit.SquareMillimeter, "mm²", UnitCategory.Area, 1e-6d),
            new UnitDefinition(Unit.SquareCentimeter, "cm²", UnitCategory.Area, 1e-4d),
            new UnitDefinition(Unit.SquareDecimeter, "dm²", UnitCategory.Area, 0.01d),
            new UnitDefinition(Unit.SquareMeter, "m²", UnitCategory.Area, 1d),
            new UnitDefinition(Unit.SquareDecameter, "dam²", UnitCategory.Area, 100d),
            new UnitDefinition(Unit.SquareHectometer, "hm²", UnitCategory.Area, 10000d),
            new UnitDefinition(Unit.SquareKilometer, "km²", UnitCategory.Area, 1_000_000d),

            new UnitDefinition(Unit.SquareInch, "in²", UnitCategory.Area, 0.00064516d),
            new UnitDefinition(Unit.SquareFoot, "ft²", UnitCategory.Area, 0.092903d),
            new UnitDefinition(Unit.SquareYard, "yd²", UnitCategory.Area, 0.836127d),
            new UnitDefinition(Unit.SquareChain, "chain²", UnitCategory.Area, 404.686d),
            new UnitDefinition(Unit.SquareFurlong, "fur²", UnitCategory.Area, 25_292.9d),
            new UnitDefinition(Unit.SquareMile, "mi²", UnitCategory.Area, 2.59e6d),
            new UnitDefinition(Unit.SquareNauticalMile, "nmi²", UnitCategory.Area, 3.43e6d),

            new UnitDefinition(Unit.Hectare, "ha", UnitCategory.Area, 10_000d),
            new UnitDefinition(Unit.Acre, "acre", UnitCategory.Area, 4046.86d),

            // Angle (base = radian)
            new UnitDefinition(Unit.Degree, "°", UnitCategory.Angle, Mathf.PI / 180d),
            new UnitDefinition(Unit.Radian, "rad", UnitCategory.Angle, 1d),
            new UnitDefinition(Unit.Gradian, "gon", UnitCategory.Angle, Mathf.PI / 200d),
            new UnitDefinition(Unit.MinuteOfArc, "'", UnitCategory.Angle, Mathf.PI / (180d * 60d)),
            new UnitDefinition(Unit.SecondOfArc, "\"", UnitCategory.Angle, Mathf.PI / (180d * 3600d)),

            // Speed (base = meters per second)
            new UnitDefinition(Unit.CentimetersPerSecond, "cm/s", UnitCategory.Speed, 0.01d),
            new UnitDefinition(Unit.MetersPerSecond, "m/s", UnitCategory.Speed, 1d),
            new UnitDefinition(Unit.KilometersPerHour, "km/h", UnitCategory.Speed, 0.277778d),
            new UnitDefinition(Unit.FeetPerSecond, "ft/s", UnitCategory.Speed, 0.3048d),
            new UnitDefinition(Unit.MilesPerHour, "mph", UnitCategory.Speed, 0.44704d),
            new UnitDefinition(Unit.Knots, "kn", UnitCategory.Speed, 0.514444d),
            new UnitDefinition(Unit.Mach, "mach", UnitCategory.Speed, 343d),

            // Energy (base: Joule)
            new UnitDefinition(Unit.ElectronVolt, "eV", UnitCategory.Energy, 1.60218e-19d),
            new UnitDefinition(Unit.Joule, "J", UnitCategory.Energy, 1d),
            new UnitDefinition(Unit.Kilojoule, "kJ", UnitCategory.Energy, 1_000d),
            new UnitDefinition(Unit.Calorie, "cal", UnitCategory.Energy, 4.184d),
            new UnitDefinition(Unit.FoodCalorie, "kcal", UnitCategory.Energy, 4_184d),
            new UnitDefinition(Unit.FootPound, "ft⋅lb", UnitCategory.Energy, 1.35582d),
            new UnitDefinition(Unit.BritishThermalUnit, "BTU", UnitCategory.Energy, 1_055.06d),
            new UnitDefinition(Unit.KilowattHour, "kWh", UnitCategory.Energy, 3_600_000d),

            // Power (base: Watt)
            new UnitDefinition(Unit.Watt, "W", UnitCategory.Power, 1d),
            new UnitDefinition(Unit.Kilowatt, "kW", UnitCategory.Power, 1_000d),
            new UnitDefinition(Unit.Megawatt, "MW", UnitCategory.Power, 1_000_000d),
            new UnitDefinition(Unit.Gigawatt, "GW", UnitCategory.Power, 1_000_000_000d),
            new UnitDefinition(Unit.Horsepower, "hp", UnitCategory.Power, 745.7d),

            // Pressure (base: Pascal)
            new UnitDefinition(Unit.Atmosphere, "atm", UnitCategory.Pressure, 101_325d),
            new UnitDefinition(Unit.TechnicalAtmosphere, "at", UnitCategory.Pressure, 98_066.5d),
            new UnitDefinition(Unit.Bar, "bar", UnitCategory.Pressure, 100_000d),
            new UnitDefinition(Unit.Millibar, "mbar", UnitCategory.Pressure, 100d),
            new UnitDefinition(Unit.Torr, "Torr", UnitCategory.Pressure, 133.322d),
            new UnitDefinition(Unit.Pascal, "Pa", UnitCategory.Pressure, 1d),
            new UnitDefinition(Unit.Kilopascal, "kPa", UnitCategory.Pressure, 1_000d),
            new UnitDefinition(Unit.Megapascal, "MPa", UnitCategory.Pressure, 1_000_000d),
            new UnitDefinition(Unit.Gigapascal, "GPa", UnitCategory.Pressure, 1_000_000_000d),
            new UnitDefinition(Unit.PoundsPerSquareInch, "psi", UnitCategory.Pressure, 6_894.76d),
            new UnitDefinition(Unit.PoundsPerSquareFoot, "psf", UnitCategory.Pressure, 47.8803d),
            new UnitDefinition(Unit.KiloPSI, "ksi", UnitCategory.Pressure, 6_894_760d),
            new UnitDefinition(Unit.InchOfWater, "inH₂O", UnitCategory.Pressure, 249.089d),
            new UnitDefinition(Unit.FootOfWater, "ftH₂O", UnitCategory.Pressure, 2_988.98d),
            new UnitDefinition(Unit.MillimetersOfWater, "mmH₂O", UnitCategory.Pressure, 9.80665d),
            new UnitDefinition(Unit.CentimetersOfWater, "cmH₂O", UnitCategory.Pressure, 98.0665d),
            new UnitDefinition(Unit.MillimetersOfMercury, "mmHg", UnitCategory.Pressure, 133.322d),
            new UnitDefinition(Unit.InchesOfMercury, "inHg", UnitCategory.Pressure, 3_386.39d),
            new UnitDefinition(Unit.KilogramForcePerSquareCentimeter, "kgf/cm²", UnitCategory.Pressure, 98_066.5d),

            // Electric Current (base: Ampere)
            new UnitDefinition(Unit.Ampere, "A", UnitCategory.ElectricCurrent, 1d),
            new UnitDefinition(Unit.Milliampere, "mA", UnitCategory.ElectricCurrent, 0.001d),
            new UnitDefinition(Unit.Microampere, "µA", UnitCategory.ElectricCurrent, 0.000001d),

            // Voltage (base: Volt)
            new UnitDefinition(Unit.Volt, "V", UnitCategory.Voltage, 1d),
            new UnitDefinition(Unit.Millivolt, "mV", UnitCategory.Voltage, 0.001d),
            new UnitDefinition(Unit.Kilovolt, "kV", UnitCategory.Voltage, 1_000d),

            // Resistance (base: Ohm)
            new UnitDefinition(Unit.Ohm, "Ω", UnitCategory.Resistance, 1d),
            new UnitDefinition(Unit.Milliohm, "mΩ", UnitCategory.Resistance, 0.001d),
            new UnitDefinition(Unit.Kiloohm, "kΩ", UnitCategory.Resistance, 1_000d),
            new UnitDefinition(Unit.Megaohm, "MΩ", UnitCategory.Resistance, 1_000_000d),

            // Capacitance (base: Farad)
            new UnitDefinition(Unit.Farad, "F", UnitCategory.Capacitance, 1d),
            new UnitDefinition(Unit.Millifarad, "mF", UnitCategory.Capacitance, 0.001d),
            new UnitDefinition(Unit.Microfarad, "µF", UnitCategory.Capacitance, 1e-6d),
            new UnitDefinition(Unit.Nanofarad, "nF", UnitCategory.Capacitance, 1e-9d),
            new UnitDefinition(Unit.Picofarad, "pF", UnitCategory.Capacitance, 1e-12d),

            // Inductance (base: Henry)
            new UnitDefinition(Unit.Henry, "H", UnitCategory.Inductance, 1d),
            new UnitDefinition(Unit.Millihenry, "mH", UnitCategory.Inductance, 0.001d),
            new UnitDefinition(Unit.Microhenry, "µH", UnitCategory.Inductance, 0.000001d),

            // Frequency (base: Hertz)
            new UnitDefinition(Unit.Hertz, "Hz", UnitCategory.Frequency, 1d),
            new UnitDefinition(Unit.Kilohertz, "kHz", UnitCategory.Frequency, 1_000d),
            new UnitDefinition(Unit.Megahertz, "MHz", UnitCategory.Frequency, 1_000_000d),
            new UnitDefinition(Unit.Gigahertz, "GHz", UnitCategory.Frequency, 1_000_000_000d),
            new UnitDefinition(Unit.Terahertz, "THz", UnitCategory.Frequency, 1_000_000_000_000d),

            // Data Storage (base: Byte)
            new UnitDefinition(Unit.Bit, "b", UnitCategory.Data, 0.125d),
            new UnitDefinition(Unit.Nibble, "nib", UnitCategory.Data, 0.5d),
            new UnitDefinition(Unit.Byte, "B", UnitCategory.Data, 1d),
            new UnitDefinition(Unit.Kilobit, "kb", UnitCategory.Data, 125d),
            new UnitDefinition(Unit.Megabit, "Mb", UnitCategory.Data, 125_000d),
            new UnitDefinition(Unit.Gigabit, "Gb", UnitCategory.Data, 125_000_000d),
            new UnitDefinition(Unit.Terabit, "Tb", UnitCategory.Data, 125_000_000_000d),
            new UnitDefinition(Unit.Petabit, "Pb", UnitCategory.Data, 125_000_000_000_000d),
            new UnitDefinition(Unit.Exabit, "Eb", UnitCategory.Data, 125_000_000_000_000_000d),
            new UnitDefinition(Unit.Zettabit, "Zb", UnitCategory.Data, 1.5625e+19d),
            new UnitDefinition(Unit.Yottabit, "Yb", UnitCategory.Data, 2e+22d),

            new UnitDefinition(Unit.Kilobyte, "KB", UnitCategory.Data, 1_000d),
            new UnitDefinition(Unit.Megabyte, "MB", UnitCategory.Data, 1_000_000d),
            new UnitDefinition(Unit.Gigabyte, "GB", UnitCategory.Data, 1_000_000_000d),
            new UnitDefinition(Unit.Terabyte, "TB", UnitCategory.Data, 1_000_000_000_000d),
            new UnitDefinition(Unit.Petabyte, "PB", UnitCategory.Data, 1_000_000_000_000_000d),
            new UnitDefinition(Unit.Exabyte, "EB", UnitCategory.Data, 1_000_000_000_000_000_000d),
            new UnitDefinition(Unit.Zettabyte, "ZB", UnitCategory.Data, 1e+21d),
            new UnitDefinition(Unit.Yottabyte, "YB", UnitCategory.Data, 1e+24d),

            new UnitDefinition(Unit.Kibibyte, "KiB", UnitCategory.Data, 1024d),
            new UnitDefinition(Unit.Mebibyte, "MiB", UnitCategory.Data, 1_048_576d),
            new UnitDefinition(Unit.Gibibyte, "GiB", UnitCategory.Data, 1_073_741_824d),
            new UnitDefinition(Unit.Tebibyte, "TiB", UnitCategory.Data, 1_099_511_627_776d),
            new UnitDefinition(Unit.Pebibyte, "PiB", UnitCategory.Data, 1.1259e+15d),
            new UnitDefinition(Unit.Exbibyte, "EiB", UnitCategory.Data, 1.1529e+18d),
            new UnitDefinition(Unit.Zebibyte, "ZiB", UnitCategory.Data, 1.1806e+21d),
            new UnitDefinition(Unit.Yobibyte, "YiB", UnitCategory.Data, 1.2089e+24d),

            // Density (base: kg/m³)
            new UnitDefinition(Unit.KilogramPerCubicMeter, "kg/m³", UnitCategory.Density, 1d),
            new UnitDefinition(Unit.GramPerCubicCentimeter, "g/cm³", UnitCategory.Density, 1000d),
            new UnitDefinition(Unit.PoundPerCubicFoot, "lb/ft³", UnitCategory.Density, 16.0185d),
            new UnitDefinition(Unit.PoundPerGallon, "lb/gal", UnitCategory.Density, 119.826d),

            // Fuel Economy (inverse-based for comparison)
            new UnitDefinition(Unit.MilesPerGallon_US, "mpg", UnitCategory.FuelEconomy, 1d),
            new UnitDefinition(Unit.MilesPerGallon_UK, "mpg", UnitCategory.FuelEconomy, 1.20095d),
            new UnitDefinition(Unit.LitersPer100Kilometers, "L/100km", UnitCategory.FuelEconomy, 235.215d),

            // Percent (base: %m)
            new UnitDefinition(Unit.PercentMultiplier, "%m", UnitCategory.Percentage, 1d),
            new UnitDefinition(Unit.Percent, "%", UnitCategory.Percentage, 0.01d),
            new UnitDefinition(Unit.Permille, "‰", UnitCategory.Percentage, 0.001d),
            new UnitDefinition(Unit.Permyriad, "‱", UnitCategory.Percentage, 0.0001d)
        };

        internal static Dictionary<(string from, string to), UnitConverter> UNIT_CONVERSION_MAP = new();

        internal static UnitConverter GetConversion(string from, string to)
        {
            if (UNIT_CONVERSION_MAP.Count == 0)
                UNIT_CONVERSION_MAP = GenerateConversionMap();

            if (UNIT_CONVERSION_MAP.TryGetValue((from, to), out var converter))
                return converter;

            return null;
        }

        internal static bool IsUnitInCategory(Unit unit, UnitCategory category)
        {
            return UNIT_DEFINITIONS.Any((unitDefinition) => unitDefinition.unitName == unit.ToString() &&
                                                            unitDefinition.categoryName == category.ToString());
        }

        internal static Dictionary<(string from, string to), UnitConverter> GenerateConversionMap()
        {
            Dictionary<(string from, string to), UnitConverter> map = new();

            EditorAttributesSettings.instance.AddCustomDefinitions();

            var groups = UNIT_DEFINITIONS.GroupBy((unitDefinition) => unitDefinition.categoryName);

            foreach (var group in groups)
            {
                var unitDefinitions = group.ToList();

                foreach (var fromDefinition in unitDefinitions)
                foreach (var toDefinition in unitDefinitions)
                {
                    var conversion = fromDefinition.baseFactor != 0d
                        ? fromDefinition.baseFactor / toDefinition.baseFactor
                        : 0d;

                    map[(fromDefinition.unitName, toDefinition.unitName)] = new UnitConverter
                    {
                        from = fromDefinition.unitName,
                        to = toDefinition.unitName,
                        unitLabel = fromDefinition.unitLabel,
                        conversion = conversion
                    };
                }
            }

            return map;
        }
    }
}