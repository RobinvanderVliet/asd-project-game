namespace Network
{
    public interface IWebSocketConnection
    {
        public void Send(string message);
        public UserSettingsConfig UserSettingsConfig { get; }
        public void AddOrUpdateConfigVariables<T>(string key, T value);
    }
}
