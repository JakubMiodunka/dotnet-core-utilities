namespace Utilities;

/// <summary>
/// Set of static methods helpful during interactions with C# data types.
/// </summary>
public static class DataTypesUtilities
{
    /// <summary>
    /// Determines maximal value, that can be stored by specified numeric data type.
    /// </summary>
    /// <param name="type">
    /// Numeric data type, which maximal value shall be determined.
    /// </param>
    /// <returns>
    /// Maximal value, which provided numeric data type can store.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when specified data type is a null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified data type is not sported.
    /// </exception>
    public static double GetTypeMaxValue(Type type)
    {
        if (type is null)
        {
            const string ErrorMessage = "Specified data type is a null reference:";
            throw new ArgumentNullException(ErrorMessage);
        }

        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Byte:
                return Convert.ToDouble(Byte.MaxValue);
            case TypeCode.SByte:
                return Convert.ToDouble(SByte.MaxValue);
            case TypeCode.UInt16:
                return Convert.ToDouble(UInt16.MaxValue);
            case TypeCode.UInt32:
                return Convert.ToDouble(UInt32.MaxValue);
            case TypeCode.UInt64:
                return Convert.ToDouble(UInt64.MaxValue);
            case TypeCode.Int16:
                return Convert.ToDouble(Int16.MaxValue);
            case TypeCode.Int32:
                return Convert.ToDouble(Int32.MaxValue);
            case TypeCode.Int64:
                return Convert.ToDouble(Int64.MaxValue);
            case TypeCode.Decimal:
                return Convert.ToDouble(Decimal.MaxValue);
            case TypeCode.Double:
                return Convert.ToDouble(Double.MaxValue);
            case TypeCode.Single:
                return Convert.ToDouble(Single.MaxValue);
            default:
                string errorMessage = $"Specified data type is not supported: {type.Name}";
                throw new ArgumentOutOfRangeException(errorMessage);
        }
    }

    /// <summary>
    /// Determines minimal value, that can be stored by specified numeric data type.
    /// </summary>
    /// <param name="type">
    /// Numeric data type, which minimal value shall be determined.
    /// </param>
    /// <returns>
    /// Minimal value, which provided numeric data type can store.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when specified data type is a null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified data type is not sported.
    /// </exception>
    public static double GetTypeMinValue(Type type)
    {
        if (type is null)
        {
            const string ErrorMessage = "Specified data type is a null reference:";
            throw new ArgumentNullException(ErrorMessage);
        }

        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Byte:
                return Convert.ToDouble(Byte.MinValue);
            case TypeCode.SByte:
                return Convert.ToDouble(SByte.MinValue);
            case TypeCode.UInt16:
                return Convert.ToDouble(UInt16.MinValue);
            case TypeCode.UInt32:
                return Convert.ToDouble(UInt32.MinValue);
            case TypeCode.UInt64:
                return Convert.ToDouble(UInt64.MinValue);
            case TypeCode.Int16:
                return Convert.ToDouble(Int16.MinValue);
            case TypeCode.Int32:
                return Convert.ToDouble(Int32.MinValue);
            case TypeCode.Int64:
                return Convert.ToDouble(Int64.MinValue);
            case TypeCode.Decimal:
                return Convert.ToDouble(Decimal.MinValue);
            case TypeCode.Double:
                return Convert.ToDouble(Double.MinValue);
            case TypeCode.Single:
                return Convert.ToDouble(Single.MinValue);
            default:
                string errorMessage = $"Specified data type is not supported: {type.Name}";
                throw new ArgumentOutOfRangeException(errorMessage);
        }
    }

}
