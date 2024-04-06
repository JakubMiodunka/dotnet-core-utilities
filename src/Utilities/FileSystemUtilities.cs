using Utilities.XmlValidation;


namespace Utilities;

/// <summary>
/// Set of static methods helpful during interactions with a file system.
/// </summary>
public static class FileSystemUtilities
{
    /// <summary>
    /// Checks if given file system entry exists and is a directory.
    /// If either of above cases would not be fulfilled, according exception will be raised.
    /// </summary>
    /// <param name="path">
    /// Path to file system entry, that is required to be validated.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when given path is null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given path is an empty string.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown, when provided file system entry does not exist in file system as directory.
    /// </exception>
    public static void ValidateExistingDirectory(string path)
    {
        if (path is null)
        {
            throw new ArgumentNullException("Entry path is a null reference:");
        }

        if (path == string.Empty)
        {
            throw new ArgumentOutOfRangeException("Entry path is an empty string:");
        }

        if (!Directory.Exists(path))
        {
            if (File.Exists(path))
            {
                throw new DirectoryNotFoundException($"Given file system entry is a file: {path}");
            }

            throw new DirectoryNotFoundException($"Directory does not exist: {path}");
        }
    }

    /// <summary>
    /// Checks if given file system entry does not exists as directory in file system.
    /// If validation fails, according exception will be thrown.
    /// </summary>
    /// <param name="path">
    /// Path to file system entry, that is required to be validated.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when given path is null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given path is an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown, when provided file system entry exists in file system as directory.
    /// </exception>
    public static void ValidateNonExistingDirectory(string path)
    {
        if (path is null)
        {
            throw new ArgumentNullException("Entry path is a null reference:");
        }

        if (path == string.Empty)
        {
            throw new ArgumentOutOfRangeException("Entry path is an empty string:");
        }

        if (Directory.Exists(path))
        {
            throw new IOException($"Directory already exists: {path}");
        }
    }

    /// <summary>
    /// Checks if given file system entry has valid extension.
    /// If validation fails, according exception will be thrown.
    /// </summary>
    /// <param name="path">
    /// Path, that is required to be validated.
    /// </param>
    /// <param name="extensions">
    /// File extensions, that shall be considered as valid during the process.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when given path or at least one of given extensions is null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given path or at least one of given extension is an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown, when validation fails.
    /// </exception>
    public static void ValidateExtension(string path, params string[] extensions)
    {
        if (path is null)
        {
            throw new ArgumentNullException("Entry path is a null reference:");
        }

        if (path == string.Empty)
        {
            throw new ArgumentOutOfRangeException("Entry path is an empty string:");
        }

        if (extensions is null)
        {
            throw new ArgumentNullException("Extensions array is a null reference:");
        }

        if (extensions.Contains(null))
        {
            throw new ArgumentNullException("One of given extensions is a null reference:");
        }

        if (extensions.Contains(string.Empty))
        {
            throw new ArgumentOutOfRangeException("One of given extensions is an empty string:");
        }


        if (extensions.Any())
        {
            string extension = Path.GetExtension(path);

            if (!extensions.Contains(extension))
            {
                throw new IOException($"Invalid extension: is '{extension}', but shall be one of '{string.Join(", ", extensions)}'");
            }
        }

    }

    /// <summary>
    /// Checks if given file system entry exists, is a file and its extension is among given ones.
    /// If either of above cases would not be fulfilled, according exception will be thrown.
    /// </summary>
    /// <param name="path">
    /// Path to file system entry, that is required to be validated.
    /// </param>
    /// <param name="extensions">
    /// File extensions, that shall be considered as valid during the process.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when given path or one of file extensions is null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given path is an empty string.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown, when given file system entry is not a file or does not exists.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown, when provided file system entry has invalid extension.
    /// </exception>
    public static void ValidateExistingFile(string path, params string[] extensions)
    {
        if (path is null)
        {
            throw new ArgumentNullException("Entry path is a null reference:");
        }

        if (path == string.Empty)
        {
            throw new ArgumentOutOfRangeException("Entry path is an empty string:");
        }

        if (!File.Exists(path))
        {
            if (Directory.Exists(path))
            {
                throw new FileNotFoundException($"Given file system entry is a directory: {path}");
            }

            throw new FileNotFoundException($"File does not exist: {path}");
        }

        ValidateExtension(path, extensions);
    }

    /// <summary>
    /// Checks if given file system entry does not exists as a file in file system and its extension is among given ones.
    /// If either of above cases would not be fulfilled, according exception will be thrown.
    /// </summary>
    /// <param name="path">
    /// Path to file system entry, that is required to be validated.
    /// </param>
    /// <param name="extensions">
    /// File extensions, that shall be considered as valid during the process.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when given path or one of file extensions is null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given path is an empty string.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown, when provided file system entry has invalid extension or when it exists in file system as file.
    /// </exception>
    public static void ValidateNonExistingFile(string path, params string[] extensions)
    {
        if (path is null)
        {
            throw new ArgumentNullException("Entry path is a null reference:");
        }

        if (path == string.Empty)
        {
            throw new ArgumentOutOfRangeException("Entry path is an empty string:");
        }

        if (File.Exists(path))
        {
            throw new IOException($"Given file already exists: {path}");
        }

        ValidateExtension(path, extensions);
    }

    /// <summary>
    /// Checks if specified XML document and XML schema exist within a file system as *.xml and *.xsd file respectively,
    /// then checks if document is compatible with the schema.
    /// </summary>
    /// <param name="document">
    /// Path to *.xml file, which shall be validated.
    /// </param>
    /// <param name="schema">
    /// Path to *.xsd file, against which provided XML document shall be validated against.
    /// </param>
    /// <exception cref="FormatException">
    /// Thrown, when specified XML document not compatible with specified schema.
    /// </exception>
    public static void ValidateXmlDocument(string document, string schema)
    {
        ValidateExistingFile(document, ".xml");
        ValidateExistingFile(schema, ".xsd");

        var validation = new XmlDocumentValidation(document, schema);

        if (!validation.Execute())
        {
            string errorMessage = $"XML document not compatible with specified schema: {document}";
            throw new FormatException(errorMessage);
        }
    }
}
