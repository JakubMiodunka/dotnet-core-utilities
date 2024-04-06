// Ignore Spelling: Timestamp

using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;


namespace Utilities.XmlValidation;

/// <summary>
/// Entity, which represents the validation process of XML-formatted document against XML schema.
/// </summary>
/// <example>
/// Simple example of tracker usage.
/// <code>
/// const string Document = @"sample.xml";
/// const string Schema = @"sample.xsd";
/// const string Report = @"report.xml";
/// 
/// var xmlValidation = new XmlValidation(Document, Schema);
/// bool validationResult = xmlValidation.Execute();
/// 
/// var message = (validationResult) ? "Success!" : "Failure...";
/// Console.WriteLine(message);
/// 
/// xmlValidation.SaveReportAs(Report);
/// </code>
/// </example>
public sealed class XmlDocumentValidation
{
    public readonly string XmlDocument;
    public readonly string XmlSchema;
    public bool WasExecuted
    {
        get
        {
            return _validationTimestamp != DateTime.MinValue;
        }
    }
    public bool WasSuccessful
    {
        get
        {
            if (!WasExecuted)
            {
                const string ErrorMessage = "Attempt to access validation status before its execution:";
                throw new InvalidOperationException(ErrorMessage);
            }

            return !_deviations.Any();
        }
    }
    public XmlDeviation[] Deviations
    {
        get
        {
            if (!WasExecuted)
            {
                const string ErrorMessage = "Attempt to access detected deviations before its execution:";
                throw new InvalidOperationException(ErrorMessage);
            }

            return _deviations.ToArray();
        }
    }
    public DateTime ValidationTimestamp
    {
        get
        {
            if (!WasExecuted)
            {
                const string ErrorMessage = "Attempt to access validation timestamp before its execution:";
                throw new InvalidOperationException(ErrorMessage);
            }

            return _validationTimestamp;
        }
    }

    private DateTime _validationTimestamp;
    private readonly List<XmlDeviation> _deviations;

    /// <summary>
    /// Creates a new representation of XML validation process.
    /// </summary>
    /// <param name="xmlDocument">
    /// Path to *.xml file, which shall be validated.
    /// </param>
    /// <param name="xmlSchema">
    /// Path to *.xsd file, against which specified XML document shall be validated.
    /// </param>
    public XmlDocumentValidation(string xmlDocument, string xmlSchema)
    {
        FileSystemUtilities.ValidateExistingFile(xmlDocument, ".xml");
        FileSystemUtilities.ValidateExistingFile(xmlSchema, ".xsd");

        XmlDocument = xmlDocument;
        XmlSchema = xmlSchema;

        _validationTimestamp = DateTime.MinValue;
        _deviations = new List<XmlDeviation>();
    }

    /// <summary>
    /// Executes validation of selected *.xml file against provided schema.
    /// </summary>
    /// <returns>
    /// True or false, depending on validation outcome.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown, when validation was already performed.
    /// </exception>
    public bool Execute()
    {
        if (WasExecuted)
        {
            var errorMessage = "Unable to perform validation: validation already performed";
            throw new InvalidOperationException(errorMessage);
        }

        void ValidationCallback(object? sender, ValidationEventArgs eventArgs)
        {
            var deviation = new XmlDeviation(eventArgs.Exception.LineNumber, eventArgs.Severity, eventArgs.Message);
            _deviations.Add(deviation);
        }

        _validationTimestamp = DateTime.Now;

        var readerSettings = new XmlReaderSettings();
        readerSettings.Schemas.Add(null, XmlSchema);
        readerSettings.ValidationType = ValidationType.Schema;
        readerSettings.ValidationEventHandler += ValidationCallback;

        using (var reader = XmlReader.Create(XmlDocument, readerSettings))
        {
            while (reader.Read()) ;
        }

        return WasSuccessful;
    }

    /// <summary>
    /// Prepares XML-formatted report from performed validation.
    /// </summary>
    /// <param name="reportPath">
    /// Path, under which report file shall be saved.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown, when report preparation is not possible.
    /// </exception>
    public void SaveReportAs(string reportPath)
    {
        if (WasExecuted)
        {
            var errorMessage = "Unable to prepare validation report: validation not performed yet";
            throw new InvalidOperationException(errorMessage);
        }

        FileSystemUtilities.ValidateNonExistingFile(reportPath, ".xml");

        string validationTimestamp = _validationTimestamp.ToString(@"yyyy-MM-ddTHH\:mm\:ss");
        string validationResult = WasSuccessful ? "passed" : "failed";

        var rootNode = new XElement("XmlValidation",
            new XAttribute("Result", validationResult),
            new XAttribute("Timestamp", validationTimestamp));

        var configurationNode = new XElement("Configuration",
            new XElement("XmlDocument",
                new XAttribute("Path", XmlDocument)),
            new XElement("XmlSchema",
                new XAttribute("Path", XmlSchema)));

        rootNode.Add(configurationNode);

        var deviationsNode = new XElement("Deviations",
            new XAttribute("Quantity", _deviations.Count));

        foreach (XmlDeviation deviation in _deviations)
        {
            string severity = deviation.Severity.ToString().ToLower();

            var deviationNode = new XElement("Deviation",
                new XAttribute("LineNumber", deviation.LineNumber),
                new XAttribute("Severity", severity),
                new XText(deviation.Description));

            deviationsNode.Add(deviationNode);
        }

        rootNode.Add(deviationsNode);

        var report = new XDocument(rootNode);
        report.Save(reportPath);
    }
}
