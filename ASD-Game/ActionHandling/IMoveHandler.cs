namespace ASD_Game.ActionHandling
{
    public interface IMoveHandler
    {
        public void SendMove(string directionValue, int stepsValue);

        public void SearchNearestPlayer();

        public void CheckAITimer();

        public void SendAIMove(string id, int x, int y);
    }
}