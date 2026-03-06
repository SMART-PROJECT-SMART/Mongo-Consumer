using MongoConsumer.Services.TailIdStorage.Interfaces;

namespace MongoConsumer.Services.TailIdStorage;

public class TailIdStorageService : ITailIdStorageService
{
    private readonly HashSet<int> _tailIds;
    private readonly Lock _lock;

    public TailIdStorageService()
    {
        _tailIds = new HashSet<int>();
        _lock = new Lock();
    }

    public IEnumerable<int> GetAllTailIds()
    {
        lock (_lock)
        {
            return [.. _tailIds];
        }
    }

    public bool ContainsTailId(int tailId)
    {
        lock (_lock)
        {
            return _tailIds.Contains(tailId);
        }
    }

    public void AddTailId(int tailId)
    {
        lock (_lock)
        {
            _tailIds.Add(tailId);
        }
    }

    public void RemoveTailId(int tailId)
    {
        lock (_lock)
        {
            _tailIds.Remove(tailId);
        }
    }

    public void UpdateTailId(int oldTailId, int newTailId)
    {
        lock (_lock)
        {
            if (_tailIds.Remove(oldTailId))
            {
                _tailIds.Add(newTailId);
            }
        }
    }
}
