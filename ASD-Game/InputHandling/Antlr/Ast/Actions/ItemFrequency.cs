namespace InputHandling.Antlr.Ast.Actions
{
    public class ItemFrequency : Command
    {
        public string Frequency;

        public ItemFrequency(string frequency)
        {
            Frequency = frequency;
        }
    }
}