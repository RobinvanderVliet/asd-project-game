using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using NUnit.Framework;

namespace Agent.Tests.ast
{
    [TestFixture]
    public class ComparableTest
    {
        private Comparable comparable;

        /*
         * GetNodeType() of Item
         *
         * Test If Item is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfItem()
        {
            //Arrange
            comparable = new Item("");
            //Act

            //Assert
            Assert.IsTrue(comparable is Comparable);
        }

        /*
         * GetNodeTypeInt() of Item
         *
         * Test If Int is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfInt()
        {
            //Arrange
            comparable = new Int(1);
            //Act

            //Assert
            Assert.IsTrue(comparable is Comparable);
        }

        /*
         * GetNodeTypeOfStat() of Item
         *
         * Test If Stat is a comparable
         * @author Abdul     
        */
        [Test]
        public void GetNodeTypeOfStat()
        {
            //Arrange
            comparable = new Stat("");
            //Act

            //Assert
            Assert.IsTrue(comparable is Comparable);
        }

        /*
        * GetNodeTypeOfSubject() of Item
        *
        * Test If Stat is a comparable
        * @author Abdul     
       */
        [Test]
        public void GetNodeTypeOfSubject()
        {
            //Arrange
            comparable = new Subject("");
            //Act
            
            //Assert
            Assert.IsTrue(comparable is Comparable);
        }
    }
}