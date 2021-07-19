using System;

namespace AptumEngine.Core
{
    public struct Rang01 : IComparable, IComparable<Rang01>, IConvertible, IEquatable<Rang01>, IFormattable
    {
        #region PRIV_IMPL
        private static float Clamp(float f) => MathF.Max(MathF.Min(1, f), 0);
        
        float _value;
        public Rang01(float value)
        {
            _value = Clamp(value);
        }

        private float Value
        {
            get => _value;
            set => _value = Clamp(value);
        }
        #endregion

        #region FLOAT_WRAPPER_FUNC
        public int CompareTo(Rang01 other)
        {
            return Value.CompareTo(other.Value);
        }
        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public bool Equals(Rang01 other)
        {
            return Value.Equals(other.Value);
        }

        public TypeCode GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToBoolean(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToByte(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToChar(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDecimal(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDouble(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt64(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSByte(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSingle(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)Value).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt16(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt32(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt64(provider);
        }
        #endregion

        #region OPERATORS
        public static implicit operator float(Rang01 r) =>
            r._value;
        public static implicit operator Rang01(float f) =>
            new Rang01(f);
        public static implicit operator Rang01(double d) =>
            new Rang01((float)d);
        public static Rang01 operator +(Rang01 a, Rang01 b) =>
            new Rang01(a.Value + b.Value);
        public static Rang01 operator -(Rang01 a, Rang01 b) =>
            new Rang01(a.Value - b.Value);
        public static Rang01 operator *(Rang01 a, Rang01 b) =>
            new Rang01(a.Value * b.Value);
        public static Rang01 operator /(Rang01 a, Rang01 b) =>
            new Rang01(a.Value / b.Value);
        public static Rang01 operator %(Rang01 a, Rang01 b) =>
            new Rang01(a.Value % b.Value);
        #endregion
    }
}
