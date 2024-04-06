using System.Xml.Schema;


namespace Utilities.XmlValidation;

/// <summary>
/// Creates a new representation of deviation detected during XML validation process.
/// </summary>
/// <param name="LineNumber">
/// Ordinal number of line, in which deviation was detected.
/// </param>
/// <param name="Severity">
/// Severity of detected deviation.
/// </param>
/// <param name="Description">
/// Detailed description of detected deviation.
/// </param>
public record XmlDeviation(int LineNumber, XmlSeverityType Severity, string Description)
{
    /// <summary>
    /// Generates string-based representation of represented deviation.
    /// </summary>
    /// <returns>
    /// String-based representation of represented deviation.
    /// </returns>
    public override string ToString()
    {
        return $"Line {LineNumber}: {Description}";
    }
}
