using System;
using Agent.antlr.ast.comparables;
using Agent.antlr.ast.implementation;
using Agent.antlr.checker;
using Moq;
using NUnit.Framework;

namespace Agent.Tests.checker
{
    public class TestCheckerFunctions
    {
        private Checker sut;

        [SetUp]
        public void Setup()
        {
            Mock<AST> ast = new Mock<AST>();
            sut = new Checker(ast.Object);
        }

        [Test]
        public void Test_CheckItemAndAllowedStat_1()
        {
            //Arrange
            String testItem = "Weapon";
            String testAllowedStat = "Power";
            
            
            Item item = new Item("Weapon");
            Stat stat = new Stat("Power");
            item.AddChild(stat);

            //Act
            bool result = sut.CheckItemAndAllowedStat(testItem, testAllowedStat, item);
            
            //Assert
            Assert.True(result);

        }
    }
}