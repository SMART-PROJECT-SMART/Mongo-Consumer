namespace MongoConsumer.Common
{
    public static class MongoConsumerConstants
    {
        public static class Configuration
        {
            public const string DEVICE_MANAGER_CONFIG_SECTION = "DeviceManager";
        }

        public static class HttpClients
        {
            public const string DEVICE_MANAGER_HTTP_CLIENT = "DeviceManagerHttpClient";
        }

        public static class DeviceManagerApiEndpoints
        {
            public const string GET_ALL_UAVS = "api/uav";
        }

        public static class Kafka
        {
            public const string KAFKA_CONFIG_SECTION = "Kafka";
            public const int TELEMETRY_PARTITION = 0;
        }
    }
}
