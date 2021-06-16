using ActionHandling.DTO;

namespace ActionHandling
{
    public interface IAttackHandler
    {
        public void SendAttack(string direction);

        public void SendAttackDTO(AttackDTO attackDto);
    }
}