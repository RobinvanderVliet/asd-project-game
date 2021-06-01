namespace InputHandling.Antlr.Ast.Actions
{
    public class MonsterDifficulty : Command
    {
        public string Difficulty;

        public MonsterDifficulty(string difficulty)
        {
            Difficulty = difficulty;
        }
    }
}