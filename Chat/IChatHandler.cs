namespace Chat
{
    public interface IChatHandler
    {
        public void SendSay(string message);
        public void SendShout(string message);
    }
}
