namespace Utilities;

/// <summary>
/// Set of static methods helpful during interactions with C# data types.
/// </summary>
public static class DataTypesUtilities
{
    #region Checking data types ranges.
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
    /// <exception cref="NotSupportedException">
    /// Thrown, when specified data type is not sported.
    /// </exception>
    public static double GetTypeMaxValue(Type type)
    {
        if (type is null)
        {
            string argumentName = nameof(type);
            const string ErrorMessage = "Specified data type is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
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
                throw new NotSupportedException(errorMessage);
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
    /// <exception cref="NotSupportedException">
    /// Thrown, when specified data type is not sported.
    /// </exception>
    public static double GetTypeMinValue(Type type)
    {
        if (type is null)
        {
            string argumentName = nameof(type);
            const string ErrorMessage = "Specified data type is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
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
                throw new NotSupportedException(errorMessage);
        }
    }
    #endregion

    #region Comparing values with specified tolerance.
    /// <summary>
    /// Checks if provided values are equal taking into account specified tolerance.
    /// </summary>
    /// <param name="value1">
    /// Second value, which shall be taken as comparison subject.
    /// </param>
    /// <param name="value2">
    /// Second value, which shall be taken as comparison subject.
    /// </param>
    /// <param name="tolerance">
    /// Comparison tolerance.
    /// If difference between both given values would be equal or smaller than specified tolerance,
    /// they will be considered as equal.
    /// Specified value of tolerance shall be greater or equal to zero.
    /// </param>
    /// <returns>
    /// True or false, depending on check result.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified tolerance is smaller than zero.
    /// </exception>
    public static bool AreEqual(double value1,  double value2, double tolerance = 0.0)
    {
        if (tolerance < 0)
        {
            string argumentName = nameof(tolerance);
            string errorMessage = $"Specified tolerance is smaller than zero: {tolerance}";
            throw new ArgumentOutOfRangeException(argumentName, tolerance, errorMessage);
        }
        
        if (Math.Abs(value1 - value2) <= tolerance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if provided value is smaller than or equal to reference value,
    /// taking into account specified tolerance.
    /// </summary>
    /// <param name="value">
    /// Value, which shall be compared to provided reference value.
    /// </param>
    /// <param name="referenceValue">
    /// Reference value, to which checked value shall be compared to.
    /// </param>
    /// <param name="tolerance">
    /// If difference between both given values would be equal or smaller than specified tolerance,
    /// they will be considered as equal.
    /// Specified value of tolerance shall be greater or equal to zero.
    /// </param>
    /// <returns>
    /// True or false, depending on check result.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified tolerance is smaller than zero.
    /// </exception>
    public static bool SmallerOrEqual(double value, double referenceValue, double tolerance = 0.0)
    {
        if (tolerance < 0)
        {
            string argumentName = nameof(tolerance);
            string errorMessage = $"Specified tolerance is smaller than zero: {tolerance}";
            throw new ArgumentOutOfRangeException(argumentName, tolerance, errorMessage);
        }

        if (AreEqual(value, referenceValue, tolerance))
        {
            return true;
        }

        if (value < referenceValue)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if provided value is smaller than or equal to reference value,
    /// taking into account specified tolerance.
    /// </summary>
    /// <param name="value">
    /// Value, which shall be compared to provided reference value.
    /// </param>
    /// <param name="referenceValue">
    /// Reference value, to which checked value shall be compared to.
    /// </param>
    /// <param name="tolerance">
    /// If difference between both given values would be equal or smaller than specified tolerance,
    /// they will be considered as equal.
    /// Specified value of tolerance shall be greater or equal to zero.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified tolerance is smaller than zero.
    /// </exception>
    public static bool GreaterOrEqual(double value, double referenceValue, double tolerance = 0.0)
    {
        if (tolerance < 0)
        {
            string argumentName = nameof(tolerance);
            string errorMessage = $"Specified tolerance is smaller than zero: {tolerance}";
            throw new ArgumentOutOfRangeException(argumentName, tolerance, errorMessage);
        }

        if (AreEqual(value, referenceValue, tolerance))
        {
            return true;
        }

        if (value > referenceValue)
        {
            return true;
        }

        return false;
    }
    #endregion
}
