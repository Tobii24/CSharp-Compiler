using System;
using System.Collections.Generic;
using Compiler.Source.Datatypes;

namespace Compiler.Source.Lib
{
    public class Context
    {
        public Dictionary<string, object> Variables;
        public List<string> LockedVariables;

        public Context()
        {
            Variables = new Dictionary<string, object>();
            LockedVariables = new List<string>();

            #region Setup built-in vars
            AddVariable("null",
                new NullType(),
                true
                );
            AddVariable("true",
                new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true)),
                true
                );
            AddVariable("false",
                new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)),
                true
                );
            #endregion
        }

        public void AddVariable(string name, object value, bool locked = false)
        {
            Variables.Add(name, value);
            if (locked)
                LockedVariables.Add(name);
        }
    }
}
