using System.Diagnostics;


namespace Utilities.OsManagement;

/// <summary>
/// Representation of a process, which functionalities
/// are handled through a pool of arguments passed to it using CLI.
/// </summary>
public sealed class CliProcess
{
    private int? _exitCode;
    public int ExitCode => _exitCode ?? -1;
    public bool WasExecuted => _exitCode is null ? false : true;

    private string? _stdOut;
    public string StdOut => _stdOut ?? string.Empty;

    private string? _stdErr;
    public string StdErr => _stdErr ?? string.Empty;

    private TimeSpan _timeout;
    public TimeSpan Timeout
    {
        get
        {
            return _timeout;
        }
        set
        {
            if (value <= TimeSpan.Zero)
            {
                string errorMesage = $"Invalid timeout assignment: {value}";
                throw new ArgumentOutOfRangeException(errorMesage);
            }

            _timeout = value;
        }
    }

    private readonly ProcessStartInfo _processConfiguration;
    public string Executable => _processConfiguration.FileName;

    /// <summary>
    /// Basing on given *.exe file path and arguments array, prepares CLI-based process configuration.
    /// </summary>
    /// <param name="executable">
    /// Path to *.exe file, which shall be executed by the process.
    /// </param>
    /// <param name="arguments">
    /// Array containing arguments, which shall be specified for given executable.
    /// </param>
    /// <returns>
    /// Prepared process configuration.
    /// </returns>
    private static ProcessStartInfo PrepareProcessConfiguration(string executable, string[] arguments)
    {
        var processConfiguration = new ProcessStartInfo();

        processConfiguration.FileName = executable;
        processConfiguration.UseShellExecute = false;
        processConfiguration.CreateNoWindow = true;
        processConfiguration.RedirectStandardOutput = true;
        processConfiguration.RedirectStandardError = true;

        foreach (string argument in arguments)
        {
            processConfiguration.ArgumentList.Add(argument);
        }

        return processConfiguration;
    }

    /// <summary>
    /// Creates a new representation of CLI-based process.
    /// </summary>
    /// <param name="executable">
    /// Path to *.exe file, which shall be executed by the process.
    /// </param>
    /// <param name="arguments">
    /// Array containing arguments, which shall be specified for given executable.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when path for executable, arguments array or one of its elements is a null reference.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when at least one element of arguments array is an empty string.
    /// </exception>
    public CliProcess(string executable, params string[] arguments)
    {
        FileSystemUtilities.ValidateExistingFile(executable, ".exe");

        if (arguments is null)
        {
            throw new ArgumentNullException("Arguments array is a null reference:");
        }

        if (arguments.Contains(null))
        {
            throw new ArgumentNullException("One of given argument is a null reference:");
        }

        if (arguments.Contains(string.Empty))
        {
            throw new ArgumentOutOfRangeException("One of given argument is an empty string:");
        }

        /* 
         * Maximal value of timeout allowed by System.Diagnostics.Process class.
         * Equals to 24 days (596 h).
         */
        Timeout = new TimeSpan(0, 0, 0, 0, int.MaxValue);

        _processConfiguration = PrepareProcessConfiguration(executable, arguments);
    }

    /// <summary>
    /// Executes the process.
    /// </summary>
    /// <exception cref="TimeoutException">
    /// Thrown, when process execution does not finish within desired time frame.
    /// </exception>
    /// <exception cref="SystemException">
    /// Thrown, when process exits with negative exit code.
    /// </exception>
    public void Execute()
    {
        using (var process = new Process())
        {
            process.StartInfo = _processConfiguration;

            process.Start();

            _stdOut = process.StandardOutput.ReadToEnd();
            _stdErr = process.StandardError.ReadToEnd();

            if (!process.WaitForExit(Timeout))
            {
                string errorMessage = $"Failed to execute the process within desired time frame: {Timeout}";
                throw new TimeoutException(errorMessage);
            }

            _exitCode = process.ExitCode;
        }

        if (ExitCode < 0)
        {
            string errorMessage = $"Failed to execute the process: exit code {ExitCode}";
            throw new SystemException(errorMessage);
        }
    }
}
