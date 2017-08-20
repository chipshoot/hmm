﻿/*
 * This Money class gives you the ability to work with money of multiple currencies
 * as if it were numbers.
 * It looks and behaves like a decimal.
 * Super light: Only a 64bit double and 16bit int are used to persist an instance.
 * Super fast: Access to the internal double value for fast calculations.
 * Currency codes are used to get everything from the MS localization classes.
 * All lookups happen from a singleton dictionary.
 * Formatting and significant digits are automatically handled.
 * An allocation function also allows even distribution of Money.
 *
 * References:
 * Martin Fowler patterns
 * Making Money with C# : http://www.lindelauf.com/?p=17
 * http://www.codeproject.com/Articles/28244/A-Money-type-for-the-CLR?msg=3679755
 * A few other articles on the web around representing money types
 * http://en.wikipedia.org/wiki/ISO_4217
 * http://www.currency-iso.org/iso_index/iso_tables/iso_tables_a1.htm
 *
 * Important!
 * Although the .Amount property wraps the class as Decimal, this Money class uses double to store the Money value internally.
 * Only 15 decimal digits of accuracy are guaranteed! (16 if the first digit is smaller than 9)
 * It should be fairly simple to replace the internal double with a decimal if this is not sufficient and performance is not an issue.
 * Decimal operations are MUCH slower than double (average of 15x)
 * http://stackoverflow.com/questions/366852/c-sharp-decimal-datatype-performance
 * Use the .InternalAmount property to get to the double member.
 * All the Money comparison operators use the Decimal wrapper with significant digits for the currency.
 * All the Money arithmetic (+-/*) operators use the internal double value.
 */

using System;
using System.Globalization;

namespace Hmm.Utility.Currency
{
    public class Money : IComparable<Money>, IEquatable<Money>, IComparable
    {
        #region private fields

        private const int DecimalDigits = 2;

        private double _amount;

        private readonly CurrencyCodeType _currencyCode;

        #endregion private fields

        #region Constructors

        public Money() : this(0d)
        {
        }

        public Money(decimal amount) : this((double)amount)
        {
        }

        public Money(decimal amount, CurrencyCodeType currencyCode = CurrencyCodeType.Cad) : this((double)amount, currencyCode)
        {
        }

        public Money(double amount, CurrencyCodeType currencyCode = CurrencyCodeType.Cad)
        {
            _amount = amount;
            _currencyCode = currencyCode;
        }

        #endregion Constructors

        #region Public Properties

        public static bool AllowImplicitConversion { get; set; }

        /// <summary>
        /// Gets the CurrentCulture from the CultureInfo object and creates a CurrencyCodes enum object.
        /// </summary>
        /// <returns>The CurrencyCodes enum of the current locale.</returns>
        public static CurrencyCodeType LocalCurrencyCode
        {
            get { return (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol); }
        }

        /// <summary>
        /// Rounds the _amount to the number of significant decimal digits
        /// of the associated currency using MidpointRounding.AwayFromZero.
        /// </summary>
        /// <returns>A decimal with the _amount rounded to the significant number of decimal digits.</returns>
        public decimal Amount
        {
            get
            {
                return Decimal.Round((Decimal)_amount, DecimalDigits, MidpointRounding.AwayFromZero);
            }
        }

        public string CurrencyCode => "CAD";

        public string CurrencyName => "CAD";

        public string CurrencySymbol => "$";

        /// <summary>
        /// Accesses the internal representation of the value of the Money
        /// </summary>
        /// <returns>A decimal with the internal _amount stored for this Money.</returns>
        public double InternalAmount
        {
            get { return _amount; }

            set { _amount = value; }
        }

        /// <summary>
        /// Represents the ISO code for the currency
        /// </summary>
        /// <returns>An Int16 with the ISO code for the current currency</returns>
        public Int16 ISOCode
        {
            get { return (Int16)_currencyCode; }
        }

        /// <summary>
        /// Truncates the _amount to the number of significant decimal digits
        /// of the associated currency.
        /// </summary>
        /// <returns>A decimal with the _amount truncated to the significant number of decimal digits.</returns>
        public decimal TruncatedAmount
        {
            get
            {
                return (decimal)((long)Math.Truncate(_amount * DecimalDigits)) / DecimalDigits;
            }
        }

        #endregion Public Properties

        #region Money Operators

        public static Money operator -(Money first, Money second)
        {
            CheckCurrencyType(first, second, "-");

            return new Money(first._amount - second._amount, first._currencyCode);
        }

        public static bool operator !=(Money first, Money second)
        {
            return first != null && !first.Equals(second);
        }

        public static Money operator *(Money first, Money second)
        {
            CheckCurrencyType(first, second, "*");

            return new Money(first._amount * second._amount, first._currencyCode);
        }

        public static Money operator /(Money first, Money second)
        {
            CheckCurrencyType(first, second, "/");

            return new Money(first._amount / second._amount, first._currencyCode);
        }

        public static Money operator +(Money first, Money second)
        {
            CheckCurrencyType(first, second, "+");

            return new Money(first._amount + second._amount, first._currencyCode);
        }

        public static bool operator <(Money first, Money second)
        {
            CheckCurrencyType(first, second, "<");

            return first.Amount < second.Amount;
        }

        public static bool operator <=(Money first, Money second)
        {
            CheckCurrencyType(first, second, "<=");

            return first.Amount <= second.Amount;
        }

        public static bool operator ==(Money first, Money second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Amount == second.Amount;
        }

        public static bool operator >(Money first, Money second)
        {
            CheckCurrencyType(first, second, ">");

            return first.Amount > second.Amount;
        }

        public static bool operator >=(Money first, Money second)
        {
            CheckCurrencyType(first, second, ">=");

            return first.Amount >= second.Amount;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Money))
            {
                throw new ArgumentException("Argument must be Money");
            }

            return CompareTo((Money)obj);
        }

        public int CompareTo(Money other)
        {
            if (this < other)
            {
                return -1;
            }

            return this > other ? 1 : 0;
        }

        public override bool Equals(object obj)
        {
            return (obj is Money) && Equals((Money)obj);
        }

        public bool Equals(Money other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return ((CurrencySymbol == other.CurrencySymbol) && (Amount == other.Amount));
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() ^ _currencyCode.GetHashCode();
        }

        #endregion Money Operators

        #region Cast Operators

        public static implicit operator Money(decimal amount)
        {
            return new Money(amount, LocalCurrencyCode);
        }

        public static implicit operator Money(double amount)
        {
            return new Money(amount, LocalCurrencyCode);
        }

        public static Money operator -(Money money, decimal value)
        {
            return money - (double)value;
        }

        public static Money operator -(Money money, double value)
        {
            if (money == null)
            {
                throw new ArgumentNullException("money");
            }

            return new Money(money._amount - value, money._currencyCode);
        }

        public static bool operator !=(Money money, decimal value)
        {
            return !(money == value);
        }

        public static bool operator !=(Money money, double value)
        {
            return !(money == value);
        }

        public static Money operator *(Money money, decimal value)
        {
            return money * (double)value;
        }

        public static Money operator *(Money money, double value)
        {
            if (money == null)
            {
                throw new ArgumentNullException("money");
            }

            return new Money(money._amount * value, money._currencyCode);
        }

        public static Money operator /(Money money, decimal value)
        {
            return money / (double)value;
        }

        public static Money operator /(Money money, double value)
        {
            if (money == null)
            {
                throw new ArgumentNullException("money");
            }

            return new Money(money._amount / value, money._currencyCode);
        }

        public static Money operator +(Money money, decimal value)
        {
            return money + (double)value;
        }

        public static Money operator +(Money money, double value)
        {
            if (money == null)
            {
                throw new ArgumentNullException("money");
            }

            return new Money(money._amount + value, money._currencyCode);
        }

        public static bool operator ==(Money money, decimal value)
        {
            if (ReferenceEquals(money, null))
            {
                return false;
            }

            return (money.Amount == value);
        }

        public static bool operator ==(Money money, double value)
        {
            if (ReferenceEquals(money, null))
            {
                return false;
            }

            return (money.Amount == (decimal)value);
        }

        #endregion Cast Operators

        #region Functions

        /// <summary>
        /// Evenly distributes the _amount over n parts, resolving remainders that occur due to rounding
        /// errors, thereby guaranteeing the post-condition: result->sum(r|r._amount) = _amount and
        /// x elements in result are greater than at least one of the other elements, where x = _amount mod n.
        /// </summary>
        /// <param name="n">Number of parts over which the _amount is to be distributed.</param>
        /// <returns>Array with distributed Money amounts.</returns>
        public Money[] Allocate(int n)
        {
            double cents = Math.Pow(10, DecimalDigits);
            double lowResult = ((long)Math.Truncate(_amount / n * cents)) / cents;
            double highResult = lowResult + 1.0d / cents;
            var results = new Money[n];

            var remainder = (int)((_amount * cents) % n);
            for (var i = 0; i < remainder; i++)
            {
                results[i] = new Money((Decimal)highResult, _currencyCode);
            }

            for (var i = remainder; i < n; i++)
            {
                results[i] = new Money((Decimal)lowResult, _currencyCode);
            }

            return results;
        }

        public Money Clone()
        {
            return new Money(_amount, _currencyCode);
        }

        public Money Convert(CurrencyCodeType toCurrency, double rate)
        {
            return new Money(_amount * rate, toCurrency);
        }

        public Money Copy()
        {
            return new Money(Amount, _currencyCode);
        }

        #endregion Functions

        #region private methods

        private static void CheckCurrencyType(Money first, Money second, string operation)
        {
            if (first.CurrencyCode != second.CurrencyCode)
            {
                throw new ArgumentException($"Different money found {first.CurrencyName} {operation} {second.CurrencyName}");
            }
        }

        #endregion private methods
    }
}