using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class MoveDTO
    {
        public string UserId { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Stamina { get; set; }

        public MoveDTO()
        {
        }

        public MoveDTO(string userId, int xPosition, int yPosition)
        {
            UserId = userId;
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public MoveDTO(string userId, int xPosition, int yPosition, int stamina)
        {
            UserId = userId;
            XPosition = xPosition;
            YPosition = yPosition;
            Stamina = stamina;
        }
    }
}