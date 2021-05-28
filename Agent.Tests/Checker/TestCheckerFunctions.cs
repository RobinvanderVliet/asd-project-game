// using System;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using Agent.Antlr.Ast;
// using Agent.Antlr.Ast.Comparables;
// using Moq;
// using NUnit.Framework;
//
// namespace Agent.Tests.Checker
// {
//     [ExcludeFromCodeCoverage]
//     public class TestCheckerFunctions
//     {
//         private Antlr.Checker.Checking _sut;
//
//         [SetUp]
//         public void Setup()
//         {
//             Mock<AST> ast = new Mock<AST>();
//             _sut = new Antlr.Checker.Checking(ast.Object);
//         }
//
//
//         [Test]
//         public void Test_CheckItemAndAllowedStat_1()
//         {
//             //Arrange
//             Item item = new Item("Weapon");
//             Stat stat = new Stat("Health");
//             item.AddChild(stat);
//
//             //Act
//             bool result = _sut.CheckItem(item);
//             
//             //Assert
//             Assert.False(result);
//
//         }
//         
//         [Test]
//         public void Test_CheckStatCombination_2()
//         {
//             //Arrange
//             List<Node> testNodes = new List<Node>();
//             Item item = new Item("Weapon");
//             Stat stat = new Stat("Health");
//             item.AddChild(stat);
//             
//             When whenNode = new When();
//             whenNode.AddChild(item);
//
//             testNodes.Add(whenNode);
//
//             //Act
//             _sut.CheckStatCombination(testNodes);
//             
//             //Assert
//             Assert.AreNotEqual(String.Empty, item.GetError().ToString());
//             Assert.AreEqual(new ASTError("There is an invalid combination of item and stat").ToString(), item.GetError().ToString());
//
//         }
//     }
// }