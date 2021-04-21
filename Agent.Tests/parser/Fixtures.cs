using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables.subjects;

namespace Agent.Tests.parser
{
    public static class Fixtures
    {

        public static AST GetFixture(string name)
        {
            switch (name) {
                case "test1.txt":
                    return GetTest1Fixture();
                default:
                    return new AST();
            }
        }
        
        
        private static AST GetTest1Fixture()
        {
            var configuration = new Configuration();

            configuration.AddChild((new Setting("combat"))
                .AddChild((new When())
                    .AddChild(new Player("player"))
                    .AddChild(new Comparison("nearby"))
                    .AddChild(new Player("player"))
                    .AddChild(new Action("attack"))
                )
            );
            return new AST(configuration);
        }
        
        
        
    }
}