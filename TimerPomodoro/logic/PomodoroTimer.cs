using System;
using System.Timers;
using Timer = System.Timers.Timer;

public class PomodoroTimer
{
    private Timer _timer;
    public bool IsWorkTime { get; private set; } = true;
    public bool IsPaused { get; private set; } = false;
    public int WorkTimeSeconds { get; set; }
    public int BreakTimeSeconds { get; set; }
    public int CurrentTimeSeconds { get; private set; }
    
    public event Action<int> OnTimerTick;
    public event Action OnWorkTimeStart;
    public event Action OnBreakTimeStart;

    public PomodoroTimer(int workTimeSeconds, int breakTimeSeconds)
    {
        WorkTimeSeconds = workTimeSeconds;
        BreakTimeSeconds = breakTimeSeconds;
        CurrentTimeSeconds = WorkTimeSeconds;
        _timer = new Timer(1000);
        _timer.Elapsed += TimerElapsed;
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (!IsPaused)
        {
            CurrentTimeSeconds--;
            
            if (CurrentTimeSeconds <= 0)
            {
                IsWorkTime = !IsWorkTime;
                CurrentTimeSeconds = IsWorkTime ? WorkTimeSeconds : BreakTimeSeconds;
                
                if (IsWorkTime)
                {
                    OnWorkTimeStart?.Invoke();
                }
                else
                {
                    OnBreakTimeStart?.Invoke();
                }
            }
            
            OnTimerTick?.Invoke(CurrentTimeSeconds);
        }
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Pause()
    {
        IsPaused = !IsPaused;
    }
}