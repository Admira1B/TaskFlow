namespace TaskFlow.Shared.Messaging.Constants {
    public static class RabbitMqConstants {
        public const string TopicExchangeType = "topic";
        public const string FanoutExchangeType = "fanout";
        public const string DirectExchangeType = "direct";

        public static class IdentityService {
            public const string ExchangeName = "identity.events";
            public const string RoutingPattern = "identity.*";
            
            public static class RoutingKeys {
                public const string UserDeleted = "identity.user.deleted";
            }
            
            public static class Queues {
                public const string TasksServiceUserDeleted = "tasks-service.user.deleted";
            }
        }
    }
}
