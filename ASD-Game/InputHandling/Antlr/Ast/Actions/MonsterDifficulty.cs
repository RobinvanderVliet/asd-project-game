
namespace InputHandling.Antlr.Ast.Actions
{
    public class MonsterDifficulty : Command
    {
        public readonly string Difficulty;

        public MonsterDifficulty(string difficulty)
        {
            Difficulty = difficulty;
        }
    }
}