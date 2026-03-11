namespace TaskFlow.Shared.Messaging.RabbitMQ.Constants {
    public static class RabbitMqConstants {
        public const string TopicExchangeType = "topic";
        public const string FanoutExchangeType = "fanout";
        public const string DirectExchangeType = "direct";

        public static class IdentityService {
            public const string ServiceName = "identity-service";
            public const string ExchangeName = $"{ServiceName}.events";
            public const string RoutingPattern = $"{ServiceName}.#";
            
            public static class RoutingKeys {
                public const string UserDeleted = $"{ServiceName}.user.deleted";
            }
        }

        public static class TasksService {
            public const string ServiceName = "tasks-service";
            public const string ExchangeName = $"{ServiceName}.events";
            public const string RoutingPattern = $"{ServiceName}.#";

            public static class RoutingKeys {
            }
        }
    }
}
