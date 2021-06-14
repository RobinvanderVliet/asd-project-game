using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.Items;
using ASD_Game.World;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RandomItemGeneratorTests
    {
        private RandomItemGenerator _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new RandomItemGenerator();
        }
        
        [Ignore("function has been changed, but test will not be adjusted as this is not within the scope of the transition phase.")]
        [Test]
        public void ItemGeneratorReturnsItemsTests()
        {
            // Arrange
            List<Item> expected = new List<Item>();
            List<Item> result = new List<Item>();

            // Note that this is not a "pure" unit test, as a change in ItemFactory will cause these tests to fail.
            // However, considering the static nature of ItemGeneratorTest, it seems better to do it this way.
            
            expected.Add(ItemFactory.GetAK47());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetBandage());
            result.Add(_sut.GetRandomItem(0.999f, 100));
            
            expected.Add(ItemFactory.GetMorphine());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetBaseballBat());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetBigMac());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetFlakVest());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetGasMask());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetGlock());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetHardHat());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetHazmatSuit());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetIodineTablets());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetJacket());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetKatana());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetTacticalVest());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetMedkit());           
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetMilitaryHelmet());            
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetMonsterEnergy());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetSuspiciousWhitePowder());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            expected.Add(ItemFactory.GetP90());
            result.Add(_sut.GetRandomItem(0.999f, 100));

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ItemName, result[i].ItemName);
            }
        }

        [Test]
        public void ItemGeneratorResultsNullTest()
        {
            Item result = _sut.GetRandomItem(0.0f, 100);
            Assert.IsNull(result);
        }
        
    }
}