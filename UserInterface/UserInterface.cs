using System;

namespace UserInterface
{
    public class UserInterface
    {
        private ScreenState _screenState;

        public UserInterface(ScreenState screenState)
        {
            _screenState = screenState;
        }

        public void SetSettings() 
        {
            _screenState = ScreenState.Settings;
        }
    }
}
