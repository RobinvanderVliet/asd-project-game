using System;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class Username : ASTNode, IEquatable<Username>
    {
        private string _username;
        public string UsernameValue { get => _username; private set => _username = value; }

        public Username()
        {
        }

        public Username(string username)
        {
            _username = username.Replace("\"", "");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Username);
        }

        public bool Equals(Username other)
        {
            if (other == null)
            {
                return false;
            }

            return _username == other.UsernameValue;
        }
    }
}