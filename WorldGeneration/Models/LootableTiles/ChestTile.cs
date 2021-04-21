namespace WorldGeneration.Models.LootableTiles
{
    public class ChestTile : LootableTile
    {
        public ChestTile() 
        {
            Symbol = "n";
            Accessible = true;
        }

        public override int GenerateLoot()
        {
            throw new System.NotImplementedException();
        }

        public override void LootItem(int Item)
        {
            throw new System.NotImplementedException();
        }
    }
}
