namespace ASD_Game.ActionHandling
{
    public interface IMoveHandler
    {
        public void SendMove(string directionValue, int stepsValue);
    }
}