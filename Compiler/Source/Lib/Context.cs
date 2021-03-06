using System;
using System.Collections.Generic;
using Compiler.Source.Datatypes;

namespace Compiler.Source.Lib
{
    public class Context
    {
        public Dictionary<string, object> Variables { get; set; }
        public List<string> LockedVariables { get; set; }
        public string ContextString { get; set; }

        public Context(string contextString, Context copy = null)
        {
            if (copy != null)
            {
                Variables = copy.Variables;
                LockedVariables = copy.LockedVariables;
            }
            else
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

            ContextString = contextString;
        }

        public Context Clone(string newContext = null)
        {
            if (newContext != null)
                return new Context(ContextString, this);
            else
                return new Context(newContext, this);
        }

        public void AddVariable(string name, object value, bool locked = false)
        {
            Variables.Add(name, value);
            if (locked)
                LockedVariables.Add(name);
        }
    }
}
