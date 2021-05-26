using System;

namespace UserInterface
{
    public class ConfigurationScreen : Screen
    {
        public override void DrawScreen()
        {
            Console.Write("configuration");
        }

        public override void HandleInput()
        {
            throw new System.NotImplementedException();
        }
    }
}