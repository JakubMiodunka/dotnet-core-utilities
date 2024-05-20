using System.Text;


namespace Utilities.ProgressTracking;

/// <summary>
/// Implementation of CLI based-progress bar.
/// </summary>
/// <remarks>
/// Progress bar body is build using Unicode characters U+2588, U+2589, U+258A, U+258B, U+258C, U+258D,
/// U+258E and U+258F. They are used to divide each bar block (character creating bar body) into eight segments.
/// This way, bar can display eight times more steps, which makes progress bar more smooth during runtime.
/// As Unicode is not fully supported by Windows command prompt, progress bar smoothing is disabled
/// on this platform - bar is built only with U+2588 character.
/// </remarks>
internal sealed class ProgressBar
{
    private const char BarBracket = '|';
    private const char EmptyBlock = ' ';
    private const char FilledBlock = '█';
    private static readonly char[] partiallyFilledBlocks = { '▏', '▎', '▍', '▌', '▋', '▊', '▉' };

    private readonly int _blocks;
    private readonly double _stepsPerSection; 
    private readonly Process _trackedProcess;

    /// <summary>
    /// Creates a new progress bar.
    /// </summary>
    /// <param name="trackedProcess">
    /// Process, to which progress bar state shall refer.
    /// </param>
    /// <param name="blocks">
    /// Length of progress bar, expressed in number of blocks 
    /// (characters) contained by the bar body.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown, when provided process is not in its initial state.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when specified number of progress bar blocks is negative or equal to zero.
    /// </exception>
    internal ProgressBar(Process trackedProcess, int blocks)
    {
        if (trackedProcess is null)
        {
            string argumentName = nameof(trackedProcess);
            const string ErrorMessage = "Provided process to track is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
        }

        if (trackedProcess.CurrentStep != 0)
        {
            string argumentName = nameof(trackedProcess);
            const string ErrorMessage = "Provided process not in its initial state:";
            throw new ArgumentException(ErrorMessage, argumentName);
        }

        if (blocks <= 0)
        {
            string argumentName = nameof(blocks);
            string errorMessage = $"Invalid number of blocks: equal to {blocks}";
            throw new ArgumentOutOfRangeException(argumentName, blocks, errorMessage);
        }

        _trackedProcess = trackedProcess;
        _blocks = blocks;

        _stepsPerSection = _trackedProcess.TotalSteps / (_blocks * 8.0);
    }
    
    /// <summary>
    /// Generates string-based representation of current progress bar state.
    /// </summary>
    /// <returns>
    /// String, which reflects current state of progress bar.
    /// </returns>
    public override string ToString()
    {
        int filledSections = Convert.ToInt32(Math.Round(_trackedProcess.CurrentStep / _stepsPerSection));
        
        int filledBlocks = filledSections / 8;
        int emptyBlocks = _blocks - filledBlocks;
        
        var progressBar = new StringBuilder();
        progressBar.Append(BarBracket);
        progressBar.Append(FilledBlock, filledBlocks);

        if (!OperatingSystem.IsWindows())
        {
            int reminder = filledSections % 8;
            if (reminder != 0)
            {
                emptyBlocks--;

                char partiallyFilledBlock = partiallyFilledBlocks[reminder - 1];
                progressBar.Append(partiallyFilledBlock);
            }
        }

        progressBar.Append(EmptyBlock, emptyBlocks);
        progressBar.Append(BarBracket);

        return progressBar.ToString();
    }
}
