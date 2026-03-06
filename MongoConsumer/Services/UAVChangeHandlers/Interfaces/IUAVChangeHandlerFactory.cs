using MongoConsumer.Common.Enums;

namespace MongoConsumer.Services.UAVChangeHandlers.Interfaces;

public interface IUAVChangeHandlerFactory
{
    IUAVChangeHandler CreateHandler(CrudOperation operation);
}
