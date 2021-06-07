namespace ASD_project.InputHandling.Antlr.Ast.Actions
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