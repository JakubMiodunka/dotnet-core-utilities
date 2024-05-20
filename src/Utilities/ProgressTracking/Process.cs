namespace Utilities.ProgressTracking;

/// <summary>
/// Representation of tracked process state.
/// </summary>
internal sealed class Process
{
    internal readonly int TotalSteps;
    internal int CurrentStep { get; private set; }

    private readonly List<Action> _subscribers;

    /// <summary>
    /// Creates a new process representation.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when given number of total process steps is invalid.
    /// </exception>
    internal Process(int totalSteps)
    {
        if (totalSteps <= 0)
        {
            string argumentName = nameof(totalSteps);
            string errorMessage = $"Invalid number of process steps: equal to {totalSteps}";
            throw new ArgumentOutOfRangeException(argumentName, totalSteps, errorMessage);
        }
        
        TotalSteps = totalSteps;

        CurrentStep = 0;
        _subscribers = new List<Action>();
    }

    /// <summary>
    /// Adds given action to subscription list.
    /// Actions from the list are invoked every time, when state of the process change.
    /// </summary>
    /// <param name="subscriber">
    /// Action, which shall be added to subscription list.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown as subscription attempt is performed, when process is not in its initial state.
    /// </exception>
    /// /// <exception cref="ArgumentNullException">
    /// Thrown, when provided subscriber is a null reference.
    /// </exception>
    internal void AddSubscriber(Action subscriber)
    {
        if (CurrentStep != 0)
        {
            const string ErrorMessage = "Subscription attempt, when process is not in its initial state:";
            throw new InvalidOperationException(ErrorMessage);
        }

        if (subscriber is null)
        {
            string argumentName = nameof(subscriber);
            const string ErrorMessage = "Specified subscriber is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
        }
        
        _subscribers.Add(subscriber);
    }

    /// <summary>
    /// Updates the state of the process.
    /// </summary>
    /// <param name="steps">
    /// Number of steps, performed by the process since last update.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when number of steps updating the process is invalid.
    /// </exception>
    internal void Update(int steps)
    {
        if (steps < 0 )
        {
            string argumentName = nameof(steps);
            string erorMessage = $"Invalid number of steps updating the process: equal to {steps}";
            throw new ArgumentOutOfRangeException(argumentName, steps, erorMessage);
        }

        if (steps != 0)
        {
            CurrentStep += steps;

            foreach (Action subscriber in _subscribers)
            {
                subscriber.Invoke();
            }
        }
    }
}