namespace InputHandling.Antlr
{
    public interface IPipeline
    {
        void ParseCommand(string input);
        void Transform();
    }
}