namespace ASD_project.InputHandling.Antlr
{
    public interface IPipeline
    {
        void ParseCommand(string input);
        void Transform();
    }
}