using Player.Services;

namespace Player.DTO
{
    public class MoveDTO
    {
        private IPlayerService _player;
        public IPlayerService Player{ get => _player; set => _player = value; }
        private int _x;
        private int _y;


        public MoveDTO(IPlayerService player, int x, int y)
        {
            _player = player;
            _x = x;
            _y = y;

        }
    }
}