using ASD_Game.Items;
using ASD_Game.Items.Consumables;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ASD_Game.Tests.WorldTests.Models.Characters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InventoryTest
    {
        private Inventory _sut;
        
        [SetUp]
        public void Setup()
        {
            _sut = new Inventory();
        }

        [Test]
        public void GetConsumableItem_Test()
        {
            var consumable = ItemFactory.GetMedkit();
            _sut.AddConsumableItem(consumable);

            var listItem = _sut.GetConsumableItem("Medkit");

            Assert.That(listItem != null);
            Assert.That(listItem == consumable);
        }

        [Test]
        public void CheckIfListIsEmptyAtStart_Test()
        {
            Assert.IsEmpty(_sut.ConsumableItemList);
        }

        [Test]
        public void GetEmptyItemList_Test()
        {
            var consumable = _sut.ConsumableItemList.FirstOrDefault();

            Assert.That(consumable == null);
        }

        [Test]
        public void CantAddConsumableFour()
        {
            var consumable1 = ItemFactory.GetMedkit();
            var consumable2 = ItemFactory.GetBigMac();
            var consumable3 = ItemFactory.GetMorphine();
            var consumable4 = ItemFactory.GetMonsterEnergy();
            _sut.AddConsumableItem(consumable1);
            _sut.AddConsumableItem(consumable2);
            _sut.AddConsumableItem(consumable3);

            Assert.Throws<InventoryFullException>(() =>
            {
                _sut.AddConsumableItem(consumable4);
            });

        }

        [Test]
        public void AddWeaponItemIfPlayerDoenstHaveOne()
        {
            _sut.Weapon = null;
            var weapon = ItemFactory.GetAK47();
            _sut.AddItem(weapon);

            Assert.That(_sut.Weapon == weapon);
        }

        [Test]
        public void AddWeaponItemIfPlayerHasWeapon()
        {
            var weapon = ItemFactory.GetAK47();
            var playerWeapon = _sut.Weapon;

            Assert.Throws<InventoryFullException>(() =>
            {
                _sut.AddItem(weapon);
            }, "You already have a weapon!");
        }
        [Test]
        public void AddBodyArmorIfPlayerDoenstHaveOne()
        {
            var armor = ItemFactory.GetFlakVest();
            _sut.AddItem(armor);

            Assert.That(_sut.Armor == armor);
        }

        [Test]
        public void AddBodyArmorIfPlayerHasOne()
        {
            var armor = ItemFactory.GetFlakVest();

            _sut.Armor = ItemFactory.GetTacticalVest();
            
            Assert.Throws<InventoryFullException>(() =>
            {
                _sut.AddItem(armor);
            }, "You already have body armor!");
        }

        [Test]
        public void AddHelmetArmorIfPlayerHasOne()
        {
            var helmet = ItemFactory.GetMilitaryHelmet();

            Assert.Throws<InventoryFullException>(() =>
            {
                _sut.AddItem(helmet);
            }, "You already have a helmet!");
        }

        [Test]
        public void AddHelmetIfPlayerDoesntHaveOne()
        {
            _sut.Helmet = null;

            var helmet = ItemFactory.GetMilitaryHelmet();
            _sut.AddItem(helmet);

            Assert.That(_sut.Helmet == helmet);
        }

        [Test]
        public void AddConsumableItem()
        {
            var consumable = ItemFactory.GetMorphine();

            _sut.AddItem(consumable);

            Assert.That(_sut.ConsumableItemList.Count == 1);
            Assert.That(_sut.ConsumableItemList.Where(item => item.ItemName == consumable.ItemName).Any);
        }

        [Test]
        public void RemoveConsumableItem() 
        {
            var consumable1 = ItemFactory.GetBigMac();
            var consumable2 = ItemFactory.GetMorphine();
            _sut.AddConsumableItem(consumable1);
            _sut.AddConsumableItem(consumable2);

            _sut.RemoveConsumableItem(consumable1);

            Assert.That(_sut.ConsumableItemList.Count == 1);
        }

        [Test]
        public void RemoveConsumableItemThatsNotInList()
        {
            var consumable = ItemFactory.GetBigMac();

            _sut.RemoveConsumableItem(consumable);

            Assert.That(_sut.ConsumableItemList.Count == 0);
        }

        [Test]
        public void EmptyConsumableItemListWithItems()
        {
            var consumable1 = ItemFactory.GetBigMac();
            var consumable2 = ItemFactory.GetMorphine();
            _sut.AddConsumableItem(consumable1);
            _sut.AddConsumableItem(consumable2);

            _sut.EmptyConsumableItemList();

            Assert.That(_sut.ConsumableItemList.Count == 0);
        }

        [Test]
        public void EmptyConsumableItemListWithoutItems()
        {
            _sut.EmptyConsumableItemList();

            Assert.That(_sut.ConsumableItemList.Count == 0);
        }

        [Test]
        public void GetComsumableAtIndex()
        {
            var consumable1 = ItemFactory.GetBigMac();
            var consumable2 = ItemFactory.GetMorphine();
            _sut.AddConsumableItem(consumable1);
            _sut.AddConsumableItem(consumable2);

            var consumableOnIndex =_sut.GetConsumableAtIndex(1);

            Assert.That(consumableOnIndex == consumable2);
        }

        public void GetComsumableAtIndexIfTheIndexDoesntExists()
        {
            var consumable1 = ItemFactory.GetBigMac();
            _sut.AddConsumableItem(consumable1);

            var consumableOnIndex = _sut.GetConsumableAtIndex(1);

            Assert.That(consumableOnIndex == null);
        }

        [Test]
        public void SetConsumableItemList()
        {
            var consumable1 = ItemFactory.GetBigMac();
            var consumable2 = ItemFactory.GetMorphine();
            var itemList = new List<Consumable>
            {
                consumable2,
                consumable1
            };

            _sut.ConsumableItemList = itemList;

            Assert.That(_sut.ConsumableItemList.ToArray().GetValue(0) == consumable2);
            Assert.That(_sut.ConsumableItemList.ToArray().GetValue(1) == consumable1);
        }

    }
}
