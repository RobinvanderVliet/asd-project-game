namespace Player
{
    public interface IPlayerModel
    {
        void HandleDirection(string direction, int steps);
        int[] SendNewPosition(int[] newMovement);
    }
}