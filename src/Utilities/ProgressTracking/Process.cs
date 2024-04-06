namespace Utilities.ProgressTracking;

/// <summary>
/// Representation of tracked process state.
/// </summary>
internal class Process
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
            throw new ArgumentOutOfRangeException($"Invalid number of process steps: equal to {totalSteps}");
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
            throw new InvalidOperationException("Subscription attempt, when process is not in its initial state:");
        }

        if (subscriber is null) throw new ArgumentNullException("Specified subscriber is a null reference:");
        
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
            throw new ArgumentOutOfRangeException($"Invalid number of steps updating the process: equal to {steps}");
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