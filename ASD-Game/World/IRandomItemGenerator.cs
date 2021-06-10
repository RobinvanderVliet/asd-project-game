using ASD_Game.Items;

namespace ASD_Game.World
{
    public interface IRandomItemGenerator
    {
        Item GetRandomItem(float noise);
    }
}