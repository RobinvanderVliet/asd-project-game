using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.World;
using Items;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RandomItemGeneratorTests
    {
        [Test]
        public void ItemGeneratorReturnsItemsTests()
        {
            // Arrange
            List<Item> expected = new List<Item>();
            List<Item> result = new List<Item>();

            // Note that this is not a "pure" unit test, as a change in ItemFactory will cause these tests to fail.
            // However, considering the static nature of ItemGeneratorTest, it seems better to do it this way.
            
            expected.Add(ItemFactory.GetAK47());
            result.Add(RandomItemGenerator.GetRandomItem(0.91f));

            expected.Add(ItemFactory.GetBandage());
            result.Add(RandomItemGenerator.GetRandomItem(0.85f));
            
            expected.Add(ItemFactory.GetMorphine());
            result.Add(RandomItemGenerator.GetRandomItem(0.75f));

            expected.Add(ItemFactory.GetBaseballBat());
            result.Add(RandomItemGenerator.GetRandomItem(0.65f));

            expected.Add(ItemFactory.GetBigMac());
            result.Add(RandomItemGenerator.GetRandomItem(0.53f));

            expected.Add(ItemFactory.GetFlakVest());
            result.Add(RandomItemGenerator.GetRandomItem(0.42f));

            expected.Add(ItemFactory.GetGasMask());
            result.Add(RandomItemGenerator.GetRandomItem(0.31f));

            expected.Add(ItemFactory.GetGlock());
            result.Add(RandomItemGenerator.GetRandomItem(0.2f));

            expected.Add(ItemFactory.GetHardHat());
            result.Add(RandomItemGenerator.GetRandomItem(0.1f));

            expected.Add(ItemFactory.GetHazmatSuit());
            result.Add(RandomItemGenerator.GetRandomItem(-0.1f));

            expected.Add(ItemFactory.GetIodineTablets());
            result.Add(RandomItemGenerator.GetRandomItem(-0.15f));

            expected.Add(ItemFactory.GetJacket());
            result.Add(RandomItemGenerator.GetRandomItem(-0.24f));

            expected.Add(ItemFactory.GetKatana());
            result.Add(RandomItemGenerator.GetRandomItem(-0.35f));

            expected.Add(ItemFactory.GetTacticalVest());
            result.Add(RandomItemGenerator.GetRandomItem(-0.48f));

            expected.Add(ItemFactory.GetMedkit());           
            result.Add(RandomItemGenerator.GetRandomItem(-0.58f));

            expected.Add(ItemFactory.GetMilitaryHelmet());            
            result.Add(RandomItemGenerator.GetRandomItem(-0.7f));

            expected.Add(ItemFactory.GetMonsterEnergy());
            result.Add(RandomItemGenerator.GetRandomItem(-0.81f));

            expected.Add(ItemFactory.GetSuspiciousWhitePowder());
            result.Add(RandomItemGenerator.GetRandomItem(-0.91f));

            expected.Add(ItemFactory.GetP90());
            result.Add(RandomItemGenerator.GetRandomItem(-0.94f));

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ItemName, result[i].ItemName);
            }
        }

        [Test]
        public void ItemGeneratorResultsNullTest()
        {
            Item result = RandomItemGenerator.GetRandomItem(1f);
            Assert.IsNull(result);
        }
        
    }
}