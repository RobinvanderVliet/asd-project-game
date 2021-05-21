using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Session.Tests
{
    class HeartbeatHandlerTest
    {
        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class SessionHandlerTests
        {
            //Declaration and initialisation of constant variables
            private HeartbeatHandler _sut;
            private StringWriter stringWriter;
            private TextWriter originalOutput;
            //Declaration of mocks


            [SetUp]
            public void Setup()
            {
                _sut = new HeartbeatHandler();
                stringWriter = new StringWriter();
                originalOutput = Console.Out;
                Console.SetOut(stringWriter);

            }

            public string GetOuput()
            {
                return stringWriter.ToString();
            }

            public void Dispose()
            {
                Console.SetOut(originalOutput);
                stringWriter.Dispose();
            }

            [Test]
            public void Test_Heartbeat_Success()
            {
                // Arrange ------------

                //Act ---------

                //Assert ---------
                string expected = String.Empty;

                using (StringWriter sw = new StringWriter())
                {
                    //Act ---------
                    Console.SetOut(sw);
                    _sut.ReceiveHeartbeat("test");
                    //Assert ---------
                    Assert.AreEqual(expected, sw.ToString());
                }

            }

            [Test]
            public void Test_Heartbeat_Fail()
            {
                // Arrange ------------

                //Act ---------


                //Assert ---------
                string expected = string.Format("Agents are enabled\r\n", Environment.NewLine); ;
                
                using (StringWriter sw = new StringWriter())
                {
                    //Act ---------
                    Console.SetOut(sw);
                    _sut.ReceiveHeartbeat("test");
                    _sut.ReceiveHeartbeat("test2");
                    Task.Delay(2000);
                    _sut.ReceiveHeartbeat("test2");

                    //Assert ---------
                    Assert.AreEqual(expected, sw.ToString());
                }

            }
        }
    }
}
