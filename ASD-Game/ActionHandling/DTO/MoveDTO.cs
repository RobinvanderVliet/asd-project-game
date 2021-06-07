using System.Diagnostics.CodeAnalysis;

namespace ASD_project.ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class MoveDTO
    {
        public string UserId;
        public int XPosition;
        public int YPosition;

        public MoveDTO(string userId, int xPosition, int yPosition)
        {
            UserId = userId;
            XPosition = xPosition;
            YPosition = yPosition;
        }
    }
}