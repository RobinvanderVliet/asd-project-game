namespace Player.ActionHandlers
{
    public interface IMoveHandler
    {
        public void SendMove(string directionValue, int stepsValue);
    }
}