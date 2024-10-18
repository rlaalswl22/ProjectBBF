using System;
using System.Collections.Generic;
using System.Linq;

public struct ControlStrongContext<T> : IDisposable
{
    private ControlOwner<T> _owner;
    private readonly int Number;

    public T Value
    {
        get
        {
            if (IsExpired)
            {
                throw new Exception("Expire된 Control에서 값을 얻으려 시도했습니다.");
            }
            
            return _owner.ForceAcquire();
        }
    }

    public bool IsExpired => _owner?.IsExpired(Number) ?? true;

    public void Expire()
    {
        _owner?.Expire(Number);
        _owner = null;
    }
    
    public ControlStrongContext(ControlOwner<T> owner, int number)
    {
        _owner = owner;
        Number = number;
    }

    public void Dispose()
    {
        Expire();
    }
}
public struct ControlWeakContext<T>
{
    private ControlOwner<T> _owner;
    private readonly int Number;

    public T Value
    {
        get
        {
            if (IsLocked)
            {
                throw new Exception("Expire된 Control에서 값을 얻으려 시도했습니다.");
            }
            
            return _owner.ForceAcquire();
        }
    }

    public bool IsLocked
    {
        get
        {
            if (IsExpired) return true;
            if (_owner.IsLocked()) return true;

            return false;
        }
    }
    public bool IsExpired => _owner is null;

    public void Expire()
    {
        _owner?.Expire(Number);
        _owner = null;
    }
    
    public ControlWeakContext(ControlOwner<T> owner, int number)
    {
        _owner = owner;
        Number = number;
    }
}

public class ControlOwner<T>
{
    private int _numberCount;
    private Stack<int> _numbers = new Stack<int>(10);
    
    private int _strongOwnerNumber = -1;

    private readonly T _value;

    public ControlOwner(T value)
    {
        _value = value;
    }

    public T ForceAcquire() => _value;

    private int PopNumber()
    {
        if (_numbers.Any() is false)
        {
            _numbers.Push(_numberCount++);
        }

        return _numbers.Pop();
    }

    private void PushNumber(int number)
    {
        _numbers.Push(number);
    }

    public bool IsExpired(int number)
    {
        if (number == -1) return true;

        // strong으로 획득됐으면서, number가 strong이 아닐 때 expire 처리
        return _strongOwnerNumber != -1 && number != _strongOwnerNumber;
    }

    public bool IsLocked()
    {
        return _strongOwnerNumber != -1;
    }

    public ControlStrongContext<T> AcquireStrong()
    {
        if (_strongOwnerNumber != -1)
        {
            return new ControlStrongContext<T>(this, -1);
        }
        
        int number = PopNumber();
        _strongOwnerNumber = number;
        
        return new ControlStrongContext<T>(this, number);
    }

    public bool TryAcquireStrong(out ControlStrongContext<T> context)
    {
        context = AcquireStrong();

        return context.IsExpired is false;
    }

    public ControlWeakContext<T> AcquireWeak()
    {
        int number = PopNumber();
        
        return new ControlWeakContext<T>(this, number);
    }

    public void Expire(int number)
    {
        if (_strongOwnerNumber == number)
        {
            _strongOwnerNumber = -1;
        }
        
        PushNumber(number);
    }
}