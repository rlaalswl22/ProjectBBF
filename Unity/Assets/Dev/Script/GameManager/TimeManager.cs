using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using ProjectBBF.Singleton;
using UnityEngine;

[Serializable]
public enum GameTimeOfDay
{
    AM,
    PM,
    Night,
    EndOfDay
} 

[Serializable]
public struct GameTime
{
    public int Hour;
    public int Min;

    public GameTimeOfDay? TimeOfDay { get; set; }

    public GameTime(int hour, int min)
    {
        Hour = hour;
        Min = min;
        TimeOfDay = null;
    }
    
    public override string ToString()
    {
        return $"Game time((h){Hour} : (m){Min}, ({TimeOfDay}))";
    }

    private int TotalMinutes()
    {
        return Hour * 60 + Min;
    }

    public override bool Equals(object obj)
    {
        if (obj is GameTime)
        {
            return Equals((GameTime)obj);
        }
        return false;
    }

    public bool Equals(GameTime other)
    {
        return TotalMinutes() == other.TotalMinutes() &&
               TimeOfDay == other.TimeOfDay;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TotalMinutes(), TimeOfDay);
    }

    public static bool operator ==(GameTime lhs, GameTime rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(GameTime lhs, GameTime rhs)
    {
        return !lhs.Equals(rhs);
    }

    public static bool operator <(GameTime lhs, GameTime rhs)
    {
        if (lhs.TotalMinutes() == rhs.TotalMinutes())
        {
            // If times are equal, compare TimeOfDay
            if (lhs.TimeOfDay.HasValue && rhs.TimeOfDay.HasValue)
            {
                return lhs.TimeOfDay.Value < rhs.TimeOfDay.Value;
            }
            return false;
        }
        return lhs.TotalMinutes() < rhs.TotalMinutes();
    }

    public static bool operator <=(GameTime lhs, GameTime rhs)
    {
        return lhs < rhs || lhs == rhs;
    }

    public static bool operator >(GameTime lhs, GameTime rhs)
    {
        return !(lhs <= rhs);
    }


    public static bool operator >=(GameTime lhs, GameTime rhs)
    {
        return !(lhs < rhs);
    }
}

public struct RealTime
{
    public int Min { get; private set; }
    public int Sec{ get; private set; }

    public float RealTimeStamp{ get; private set; }

    public RealTime(int min, int sec, float realTimeStamp)
    {
        Min = min;
        Sec = sec;
        RealTimeStamp = realTimeStamp;
    }

    public RealTime(int min, int sec)
    {
        Min = min;
        Sec = sec;
        RealTimeStamp = min * 60 + sec;
    }

    public override string ToString()
    {
        return $"Real time((m){Min} : (s){Sec}, total: {RealTimeStamp})";
    }
}

[Singleton(ESingletonType.Global)]
public class TimeManager : MonoBehaviourSingleton<TimeManager>
{
    private const string PATH = "Data/TimeData";
    private const string EVENT_PATH = "Event/Time";

    private TimeData _timeData;
    private float _realTimer;
    private GameTime _beforeTime;

    public bool IsRunning { get; private set; }
    private List<ESOGameTimeEvent> _esoEvents = new List<ESOGameTimeEvent>(10);

    public override void PostInitialize()
    {
        _timeData = Resources.Load<TimeData>(PATH);
        var events = Resources.LoadAll<ESOGameTimeEvent>(EVENT_PATH);
        
        _esoEvents.AddRange(events);
        
        //test
        Begin();
    }

    public override void PostRelease()
    {
        _esoEvents.ForEach(x=>x.Release());
    }

    public void Begin()
    {
        Reset();
        
        IsRunning = true;
        SetTime(_timeData.MorningTime);
    }

    public void End()
    {
        Reset();
        IsRunning = false;
    }

    public void Reset()
    {
        _realTimer = 0f;
        _beforeTime = new GameTime(-1, -1);
    }
    
    private void Update()
    {
        if (IsRunning is false) return;
        
        _realTimer += Time.deltaTime;

        var newGameTime = GetGameTime();
        if (_beforeTime != newGameTime)
        {
            _beforeTime = newGameTime;

            foreach (ESOGameTimeEvent eso in _esoEvents)
            {
                if (eso.IsTriggered) continue;

                eso.IsTriggered = false;

                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.Equal               && newGameTime == eso.TargetGameTime;
                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.NotEqual            && newGameTime != eso.TargetGameTime;
                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.GreaterThenEqual    && newGameTime >= eso.TargetGameTime;
                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.Greater             && newGameTime >  eso.TargetGameTime;
                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.Less                && newGameTime <  eso.TargetGameTime;
                eso.IsTriggered |= eso.OperationType == ESOGameTimeEvent.Operation.LessThenEqual       && newGameTime <= eso.TargetGameTime;
                
                if (eso.IsTriggered)
                {
                    eso.Signal(newGameTime);
                }
            }
        }
    }

    public GameTimeOfDay GetTimeOfDay()
    {
        GameTime currentTime = GetGameTime();
        GameTimeOfDay value = GetTimeOfDay(currentTime);
        
        return value;
    }

    public GameTimeOfDay GetTimeOfDay(GameTime currentTime)
    {
        GameTimeOfDay value = GameTimeOfDay.AM;
        
        if (_timeData.DefinitionPm <= currentTime)
        {
            value = GameTimeOfDay.PM;
        }
        if (_timeData.DefinitionNight <= currentTime)
        {
            value = GameTimeOfDay.Night;
        }
        if (_timeData.DefinitionEndOfDay <= currentTime)
        {
            value = GameTimeOfDay.EndOfDay;
        }

        return value;
    }
    public GameTime GetGameTime()
    {
        /*
         * r: realTime, x: totalTime of game time, a: totalTime of real time
         * 10 : r = x : a
         * rx = 10a
         * x = 10a / r
         * or
         * a = rx / 10
         */

        float x = (10f * _realTimer) / _timeData.Scale;

        int total = Mathf.FloorToInt(x);
        total = total - total % 10;
        
        int hour = total / 60;
        int min = total % 60;

        GameTime time = new GameTime(hour, min);
        time.TimeOfDay = GetTimeOfDay(time);

        return time;
    }

    public RealTime GetRealTime()
    {
        int hour = (int)_realTimer / 60;
        int min = (int)_realTimer % 60;
        return new RealTime(hour, min, _realTimer);
    }

    public void SetTime(GameTime time)
    {
        int gameTime = time.Hour * 60 + time.Min;

        float a = (_timeData.Scale * gameTime) / 10f;

        _realTimer = a;
    }
    
    public void SetTime(RealTime time)
    {
        _realTimer = time.RealTimeStamp;
    }
}