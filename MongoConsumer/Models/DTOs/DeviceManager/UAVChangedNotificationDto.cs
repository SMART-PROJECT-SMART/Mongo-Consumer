using MongoConsumer.Common.Enums;

namespace MongoConsumer.Models.DTOs.DeviceManager;

public record UAVChangedNotificationDto(CrudOperation Operation, int TailId, int? NewTailId = null);
