namespace Mcc.Timers;

public class DailyTimer
{
    private readonly TimeSpan _timeOfDay;
    private readonly Action _action;
    private Timer? _timer = null;

    public DailyTimer(TimeSpan timeOfDay, Action action)
    {
        _timeOfDay = timeOfDay;
        _action = action;
    }

    public void Start()
    {
        var now = DateTime.Now;
        var scheduledTime = new DateTime(now.Year, now.Month, now.Day, _timeOfDay.Hours, _timeOfDay.Minutes, _timeOfDay.Seconds);
        if (scheduledTime <= now)
            scheduledTime = scheduledTime.AddDays(1);
        var timeUntilNextRun = scheduledTime - now;

        _timer = new Timer(TimerCallback, null, timeUntilNextRun, TimeSpan.FromDays(1));
    }

    private void TimerCallback(object state)
    {
        // Schedule the next run
        _timer.Change(TimeSpan.FromDays(1), TimeSpan.Zero);

        // Run the action
        _action();
    }
}