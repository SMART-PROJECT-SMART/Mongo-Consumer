namespace MongoConsumer.Services.TailIdStorage.Interfaces;

public interface ITailIdStorageService
{
    IEnumerable<int> GetAllTailIds();
    bool ContainsTailId(int tailId);
    void AddTailId(int tailId);
    void RemoveTailId(int tailId);
    void UpdateTailId(int oldTailId, int newTailId);
}
