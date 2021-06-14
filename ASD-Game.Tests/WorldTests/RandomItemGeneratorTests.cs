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
        [Test]
        public void ItemGeneratorReturnsItemsTests()
        {
            // Arrange
            List<Item> expected = new List<Item>();
            List<Item> result = new List<Item>();

            // Note that this is not a "pure" unit test, as a change in ItemFactory will cause these tests to fail.
            // However, considering the static nature of ItemGeneratorTest, it seems better to do it this way.
            
            expected.Add(ItemFactory.GetAK47());
            result.Add(_sut.GetRandomItem(0.999f));

            expected.Add(ItemFactory.GetBandage());
            result.Add(_sut.GetRandomItem(0.992f));
            
            expected.Add(ItemFactory.GetMorphine());
            result.Add(_sut.GetRandomItem(0.989f));

            expected.Add(ItemFactory.GetBaseballBat());
            result.Add(_sut.GetRandomItem(0.982f));

            expected.Add(ItemFactory.GetBigMac());
            result.Add(_sut.GetRandomItem(0.979f));

            expected.Add(ItemFactory.GetFlakVest());
            result.Add(_sut.GetRandomItem(0.972f));

            expected.Add(ItemFactory.GetGasMask());
            result.Add(_sut.GetRandomItem(0.969f));

            expected.Add(ItemFactory.GetGlock());
            result.Add(_sut.GetRandomItem(0.962f));

            expected.Add(ItemFactory.GetHardHat());
            result.Add(_sut.GetRandomItem(0.959f));

            expected.Add(ItemFactory.GetHazmatSuit());
            result.Add(_sut.GetRandomItem(0.952f));

            expected.Add(ItemFactory.GetIodineTablets());
            result.Add(_sut.GetRandomItem(0.949f));

            expected.Add(ItemFactory.GetJacket());
            result.Add(_sut.GetRandomItem(0.942f));

            expected.Add(ItemFactory.GetKatana());
            result.Add(_sut.GetRandomItem(0.939f));

            expected.Add(ItemFactory.GetTacticalVest());
            result.Add(_sut.GetRandomItem(0.932f));

            expected.Add(ItemFactory.GetMedkit());           
            result.Add(_sut.GetRandomItem(0.929f));

            expected.Add(ItemFactory.GetMilitaryHelmet());            
            result.Add(_sut.GetRandomItem(0.922f));

            expected.Add(ItemFactory.GetMonsterEnergy());
            result.Add(_sut.GetRandomItem(0.919f));

            expected.Add(ItemFactory.GetSuspiciousWhitePowder());
            result.Add(_sut.GetRandomItem(0.912f));

            expected.Add(ItemFactory.GetP90());
            result.Add(_sut.GetRandomItem(0.909f));

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ItemName, result[i].ItemName);
            }
        }

        [Test]
        public void ItemGeneratorResultsNullTest()
        {
            Item result = _sut.GetRandomItem(0.0001f);
            Assert.IsNull(result);
        }
        
    }
}