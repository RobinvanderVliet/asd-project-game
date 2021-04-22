namespace WorldGeneration.Models.LootableTiles
{
    public class ChestTile : LootAbleTile
    {
        public ChestTile() 
        {
            Symbol = TileSymbol.Chest;
            IsAccessible = true;
        }

        public override int GenerateLoot()
        {
            throw new System.NotImplementedException();
        }

        public override void LootItem(int item)
        {
            throw new System.NotImplementedException();
        }
    }
}
