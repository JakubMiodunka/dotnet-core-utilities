namespace Utilities.ProgressTracking;

/// <summary>
/// Utility that visualize current state of tracked process and measures its performance.
/// </summary>
/// <remarks>
/// Tracker can operate int three modes - 'simple', 'regular' and 'advanced'.
/// Differences between modes are limited only to visual aspects - in 'simple'
/// mode only progress bar with percentage-based process state indicator is being displayed,
/// 'regular' mode adds to it a label and step ratio indicator and 'advanced' mode supplements
/// the output with time metrics.
/// Tracker prints its visual representation to std-out, so it is not recommended to perform
/// other operations in use of std-out during is runtime.
/// It is highly recommended to use progress tracker using 'using' statement.
/// </remarks>
/// <example>
/// Simple example of tracker usage.
/// <code>
/// const int Steps = 150;
/// const int Delay = 200;
/// 
/// using (var progressTracker = ProgressTracker.Reqular(Steps))
/// {
///     for (int i = 0; i < Steps; i++)
///     {
///         Thread.Sleep(Delay);
///         progressTracker.Update();
///     }
/// }
/// </code>
/// </example>
public sealed class ProgressTracker : IDisposable
{
    private enum Mode { Simple, Regular, Advanced }

    public int CurrentStep { get { return _trackedProcess.CurrentStep; } }
    public int TotalSteps { get { return _trackedProcess.TotalSteps; } }
    public bool IsProcessComplete { get { return _trackedProcess.CurrentStep == _trackedProcess.TotalSteps; } }
    public DateTime RuntimeBegin { get { return _stopwatch.RuntimeBegin; } }
    public TimeSpan EstimatedRemainingRuntime { get { return _stopwatch.EstimatedRemainingRuntime; } }
    public DateTime EstimatedRuntimeFinish { get { return _stopwatch.EstimatedRuntimeFinish; } }
    public TimeSpan AverageTimePerStep { get { return _stopwatch.AverageTimePerStep; } }

    private readonly Mode _mode;
    private readonly string _label;
    private readonly Process _trackedProcess;
    private readonly ProgressBar _progressBar;
    private readonly ProgressStopwatch _stopwatch;

    /// <summary>
    /// Creates a new progress tracker.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <param name="label">
    /// Desired tracker label.
    /// </param>
    /// <param name="progressBarBlocks">
    /// Length of displayed progress bar, expressed in number of blocks 
    /// (characters) contained by the bar body.
    /// </param>
    /// <param name="mode">Mode, in which created tracker shall operate.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when specified label is a null reference.
    /// </exception>
    private ProgressTracker(int totalSteps, string label, int progressBarBlocks, Mode mode)
    {
        if (label is null) throw new ArgumentNullException("Provided label is a null reference.");
        
        _mode = mode;
        _label = label;

        _trackedProcess = new Process(totalSteps);
        _progressBar = new ProgressBar(_trackedProcess, progressBarBlocks);
        _stopwatch = new ProgressStopwatch(_trackedProcess);

        Console.Write(this);
    }

    /// <summary>
    /// Creates a new progress tracker, which operates in 'simple' mode.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <param name="progressBarBlocks">
    /// Length of displayed progress bar, expressed in number of blocks 
    /// (characters) contained by the bar body.
    /// </param>
    /// <returns>
    /// Newly created instance of progress tracker.
    /// </returns>
    public static ProgressTracker Simple(int totalSteps, int progressBarBlocks=30)
    {
        return new ProgressTracker(totalSteps, "", progressBarBlocks, Mode.Simple);
    }

    /// <summary>
    /// Creates a new progress tracker, which operates in 'regular' mode.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <param name="label">
    /// Desired tracker label.
    /// </param>
    /// <param name="progressBarBlocks">
    /// Length of displayed progress bar, expressed in number of blocks 
    /// (characters) contained by the bar body.
    /// </param>
    /// <returns>
    /// Newly created instance of progress tracker.
    /// </returns>
    public static ProgressTracker Regular(int totalSteps, string label="Progress", int progressBarBlocks=30)
    {
        return new ProgressTracker(totalSteps, label, progressBarBlocks, Mode.Regular);
    }

    /// <summary>
    /// Creates a new progress tracker, which operates in 'advanced' mode.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <param name="label">
    /// Desired tracker label.
    /// </param>
    /// <param name="progressBarBlocks">
    /// Length of displayed progress bar, expressed in number of blocks 
    /// (characters) contained by the bar body.
    /// </param>
    /// <returns>
    /// Newly created instance of progress tracker.
    /// </returns>
    public static ProgressTracker Advanced(int totalSteps, string label = "Progress", int progressBarBlocks = 30)
    {
        return new ProgressTracker(totalSteps, label, progressBarBlocks, Mode.Advanced);
    }

    /// <summary>
    /// Generates string-based representation of current state of the tracker.
    /// Format of generated depends on current tracker mode and starts with carriage return.
    /// </summary>
    /// <returns>String representation of the tracker.</returns>
    /// <exception cref="NotImplementedException">
    /// Thrown, when currently selected mode is not supported by the method.
    /// </exception>
    public override string ToString()
    {
        double progressPercentage = Convert.ToDouble(_trackedProcess.CurrentStep) / _trackedProcess.TotalSteps * 100;
        string simpleRepresentation = $"{progressPercentage,3:F0}%{_progressBar}";

        if (_mode == Mode.Simple)
        {
            return $"\r{simpleRepresentation}";
        }

        string stepsRatio = $"{_trackedProcess.CurrentStep}/{_trackedProcess.TotalSteps}";
        string regularRepresentation = $"{_label}: {simpleRepresentation}[{stepsRatio}]";

        if (_mode == Mode.Regular)
        {
            return $"\r{regularRepresentation}";
        }

        string advancedRepresentation = $"{regularRepresentation} {_stopwatch}";

        if (_mode == Mode.Advanced)
        {
            return $"\r{advancedRepresentation}";
        }

        throw new NotImplementedException($"Selected mode not supported: equal to {_mode}");
    }

    /// <summary>
    /// Updates displayed tracker representation.
    /// </summary>
    /// <param name="steps">
    /// Number of steps, performed by the process since last update.
    /// </param>
    public void Update(int steps=1)
    {
        _trackedProcess.Update(steps);
        Console.Write(this);
    }
    
    /// <summary>
    /// Prints a newline to leave the line, in which tracker representation was being displayed.
    /// </summary>
    public void Dispose()
    {
        Console.WriteLine();
    }
}
