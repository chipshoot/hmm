//using System;
//using System.Globalization;
//using System.Linq;

//namespace Hmm.Utility.MeasureUnit
//{
//    /// <summary>
//    /// Help class to help convert between in and mm
//    /// <remarks>
//    /// The value of dimension internally saved as mm/1000 and can be convert to cm, m, inch and feet
//    /// The default dimension unit is <see cref="DimensionUnit.Millimetre" /> so when we new a
//    /// dimension object then we are setting the internal unit to millimetre and the value will adjusted
//    /// by unit parameter of constructor.
//    /// you can also get a <see cref="Dimension" /> object from five static method, e.g.
//    /// <code>
//    /// var width1 = Dimension.FromMetre(35.0);
//    /// var width2 = Dimension.FromCentimetre(0.035);
//    /// var width3 = Dimension.FromInch(20);
//    /// var widht4 = Dimension.FromFeet(20);
//    /// </code>
//    /// this can setup a dimension without manually indicate dimension's unit
//    /// you can also get the converted dimension value from five static method, e.g.
//    /// <code>
//    /// var width1 = Dimension.TotalMillimetre;
//    /// var width2 = Dimension.TotalCentimetre;
//    /// var width3 = Dimension.TotalMetre;
//    /// var width4 = Dimension.TotalInch;
//    /// var width5 = Dimension.TotalFeet;
//    /// </code>
//    /// this can right value without adjust dimension's unit
//    /// </remarks>
//    /// </summary>
//    [ImmutableObject(true)]
//    public struct Dimension : IComparable<Dimension>
//    {
//        #region private fields

//        private const string ErrorMsg = "No dimension object found";
//        private const double InternalUnitPerInch = 25400;
//        private const double DefaultDeltaIn = 0.125;
//        private readonly long _value;
//        private int _fractional;
//        private DimensionUnit _unit;

//        #endregion private fields

//        #region constructor

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Dimension"/> structure.
//        /// </summary>
//        /// <param name="value">The value of dimension, this will be adjusted to convert internal value based on unit.</param>
//        /// <param name="unit">The unit of dimension.</param>
//        /// <param name="fractional">The fractional.</param>
//        public Dimension(double value, DimensionUnit unit = DimensionUnit.Millimetre, int fractional = 3)
//        {
//            _value = InternalValue(value, unit);
//            _fractional = fractional;
//            _unit = unit;
//        }

//        /// <summary>
//        /// Prevents a default instance of the <see cref="Dimension"/> structure from being created.
//        /// <remarks>
//        /// the constructor is only used by override operators
//        /// </remarks>
//        /// </summary>
//        /// <param name="value">The value.</param>
//        /// <param name="unit">The unit.</param>
//        /// <param name="fractional">The fractional.</param>
//        private Dimension(long value, DimensionUnit unit, int fractional)
//        {
//            _value = value;
//            _unit = unit;
//            _fractional = fractional;
//        }

//        #endregion constructor

//        #region public properties

//        /// <summary>
//        /// Gets the actual amount of the dimension.
//        /// </summary>
//        /// <value>
//        /// The amount.
//        /// </value>
//        public double Value
//        {
//            get
//            {
//                switch (_unit)
//                {
//                    case DimensionUnit.Millimetre:
//                        return TotalMillimetre;

//                    case DimensionUnit.Centimetre:
//                        return TotalCentimetre;

//                    case DimensionUnit.Metre:
//                        return TotalMetre;

//                    case DimensionUnit.Inch:
//                        return TotalInch;

//                    case DimensionUnit.Feet:
//                        return TotalFeet;

//                    default:
//                        return TotalMillimetre;
//                }
//            }
//        }

//        public DimensionUnit Unit
//        {
//            get
//            {
//                return _unit;
//            }
//            set
//            {
//                _unit = value;
//            }
//        }

//        public int Fractional
//        {
//            get { return _fractional; }

//            set
//            {
//                if (value >= 0)
//                {
//                    _fractional = value;
//                }
//            }
//        }

//        public double TotalMillimetre
//        {
//            get { return Math.Round(_value / 1000.0, Fractional); }
//        }

//        public double TotalCentimetre
//        {
//            get { return Math.Round(_value / 10000.0, Fractional); }
//        }

//        public double TotalMetre
//        {
//            get { return Math.Round(_value / 100000.0, Fractional); }
//        }

//        public double TotalInch
//        {
//            get { return Math.Round(_value / InternalUnitPerInch, Fractional); }
//        }

//        public double TotalFeet
//        {
//            get { return Math.Round((_value / InternalUnitPerInch) / 12.0, Fractional); }
//        }

//        #endregion public properties

//        #region public methods

//        public static Dimension FromMillimetre(double value)
//        {
//            return new Dimension(value);
//        }

//        public static Dimension FromCentimetre(double value)
//        {
//            return new Dimension(value, DimensionUnit.Centimetre);
//        }

//        public static Dimension FromMetre(double value)
//        {
//            return new Dimension(value, DimensionUnit.Metre);
//        }

//        public static Dimension FromInch(double value)
//        {
//            return new Dimension(value, DimensionUnit.Inch);
//        }

//        public static Dimension FromFeet(double value)
//        {
//            return new Dimension(value, DimensionUnit.Feet);
//        }

//        /// <summary>
//        /// All Hadrian product's dimension, e.g. panel, pilaster, door and hardware's gap
//        /// need to be round to 0.125 when it convert to inch, so we need do some adjust
//        /// to internal value to meet this condition
//        /// </summary>
//        /// <param name="value">The dimension.</param>
//        /// <returns>The dimension which round to eight</returns>
//        public static Dimension HadrianDimensionAlign(Dimension value)
//        {
//            // get inch convert internal value
//            var valueInch = value.TotalInch;
//            double q = (int)valueInch;
//            var p = (valueInch - q) * 8;

//            Dimension result;
//            if (p >= 7.5)
//            {
//                result = FromInch(q + 1);
//                result.Unit = value.Unit;
//                return result;
//            }

//            result = FromInch(q + Math.Round(p) * DefaultDeltaIn);
//            result.Unit = value.Unit;

//            return result;
//        }

//        public static Dimension Max(params Dimension[] items)
//        {
//            if (items.Count() == 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(items), ErrorMsg);
//            }

//            var max = items.Aggregate((i1, i2) => i1 > i2 ? i1 : i2);

//            return max;
//        }

//        public static Dimension Min(params Dimension[] items)
//        {
//            if (items.Count() == 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(items), ErrorMsg);
//            }

//            var min = items.Aggregate((i1, i2) => i1 < i2 ? i1 : i2);

//            return min;
//        }

//        public static Dimension Abs(Dimension x)
//        {
//            return new Dimension(Math.Abs(x.Value), x.Unit, x.Fractional);
//        }

//        #endregion public methods

//        #region override operators

//        public static Dimension operator +(Dimension x, Dimension y)
//        {
//            var newValue = x._value + y._value;
//            return new Dimension(newValue, x.Unit, x.Fractional);
//        }

//        public static Dimension operator -(Dimension x, Dimension y)
//        {
//            var newValue = x._value - y._value;
//            return new Dimension(newValue, x.Unit);
//        }

//        public static Dimension operator *(Dimension x, int y)
//        {
//            var newValue = x._value * y;
//            return new Dimension(newValue, x.Unit, x.Fractional);
//        }

//        public static Dimension operator *(Dimension x, double y)
//        {
//            var newValue = x._value * y;
//            var newValueAsLong = (long)Math.Round(newValue, 0);

//            return new Dimension(newValueAsLong, x.Unit, x.Fractional);
//        }

//        public static Dimension operator /(Dimension x, int y)
//        {
//            var newValue = (double)x._value / y;
//            var newValueAsLong = (long)Math.Round(newValue, 0);

//            return new Dimension(newValueAsLong, x.Unit, x.Fractional);
//        }

//        public static Dimension operator /(Dimension x, double y)
//        {
//            var newValue = x._value / y;
//            var newValueAsLong = (long)Math.Round(newValue, 0);

//            return new Dimension(newValueAsLong, x.Unit, x.Fractional);
//        }

//        public static Dimension operator %(Dimension x, Dimension y)
//        {
//            var newValue = (double)x._value % y._value;
//            var newValueAsLong = (long)Math.Round(newValue, 0);

//            return new Dimension(newValueAsLong, x.Unit, x.Fractional);
//        }

//        public static bool operator !=(Dimension x, Dimension y)
//        {
//            return !x.Equals(y);
//        }

//        public static bool operator !=(Dimension x, int y)
//        {
//            return !x.Equals(new Dimension(y, x.Unit));
//        }

//        public static bool operator ==(Dimension x, Dimension y)
//        {
//            return x.Equals(y);
//        }

//        public static bool operator ==(Dimension x, int y)
//        {
//            return x == new Dimension(y, x.Unit);
//        }

//        public static bool operator >(Dimension x, Dimension y)
//        {
//            return x._value > y._value;
//        }

//        public static bool operator >(Dimension x, int y)
//        {
//            return x > new Dimension(y, x.Unit);
//        }

//        public static bool operator <(Dimension x, Dimension y)
//        {
//            return x._value < y._value;
//        }

//        public static bool operator <(Dimension x, int y)
//        {
//            return x < new Dimension(y, x.Unit);
//        }

//        public static bool operator >=(Dimension x, Dimension y)
//        {
//            return x == y || x > y;
//        }

//        public static bool operator >=(Dimension x, int y)
//        {
//            return x == y || x > y;
//        }

//        public static bool operator <=(Dimension x, Dimension y)
//        {
//            return x == y || x < y;
//        }

//        public static bool operator <=(Dimension x, int y)
//        {
//            return x == y || x < y;
//        }

//        public static Dimension operator ++(Dimension x)
//        {
//            var newValue = x._value + (long) (InternalUnitPerInch * DefaultDeltaIn);
//            return new Dimension(newValue, x.Unit);
//        }

//        public string ToString(DimensionUnitFormatType format = DimensionUnitFormatType.Millimetre)
//        {
//            string result;
//            switch (format)
//            {
//                case DimensionUnitFormatType.Millimetre:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} {1}", TotalMillimetre, StringEnum.GetStringValue(format));
//                    break;

//                case DimensionUnitFormatType.Centimetre:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} {1}", TotalCentimetre, StringEnum.GetStringValue(format));
//                    break;

//                case DimensionUnitFormatType.Metre:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} {1}", TotalMetre, StringEnum.GetStringValue(format));
//                    break;

//                case DimensionUnitFormatType.Inch:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} {1}", TotalInch, StringEnum.GetStringValue(format));
//                    break;

//                case DimensionUnitFormatType.Feet:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} {1}", TotalFeet, StringEnum.GetStringValue(format));
//                    break;

//                case DimensionUnitFormatType.MillimetreAndInch:
//                    result = string.Format(CultureInfo.InvariantCulture, "{0} mm / {1} in", TotalMillimetre, TotalInch);
//                    break;

//                default:
//                    var pSpecifier = string.Format(CultureInfo.InvariantCulture, "F{0}", Fractional);
//                    result = Value.ToString(pSpecifier, CultureInfo.InvariantCulture);
//                    break;
//            }

//            return result;
//        }

//        #endregion override operators

//        #region implementation of interface IComparable

//        public int CompareTo(Dimension other)
//        {
//            return _value.CompareTo(other._value);
//        }

//        #endregion implementation of interface IComparable

//        #region implementation of interface IEquatable

//        public bool Equals(Dimension other)
//        {
//            return _value == other._value;
//        }

//        #endregion implementation of interface IEquatable

//        #region override public methods of System.ValueType

//        public override bool Equals(object obj)
//        {
//            return obj is Dimension && Equals((Dimension)obj);
//        }

//        public override int GetHashCode()
//        {
//            return _value.GetHashCode();
//        }

//        #endregion override public methods of System.ValueType

//        #region private methods

//        private static long InternalValue(double value, DimensionUnit unit)
//        {
//            switch (unit)
//            {
//                case DimensionUnit.Millimetre:
//                    return (long)Math.Round(value * 1000.0, 0);

//                case DimensionUnit.Centimetre:
//                    return (long)Math.Round(value * 10000.0, 0);

//                case DimensionUnit.Metre:
//                    return (long)Math.Round(value * 100000.0, 0);

//                case DimensionUnit.Inch:
//                    return (long)Math.Round(value * InternalUnitPerInch, 0);

//                case DimensionUnit.Feet:
//                    return (long)Math.Round(value * InternalUnitPerInch * 12, 0);

//                default:
//                    return (long)Math.Round(value * 10.0, 0);
//            }
//        }

//        #endregion private methods
//    }
//}