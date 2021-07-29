using System;
using System.Collections.Generic;
using Compiler.Source.Errors;

namespace Compiler.Source.Lib
{
    public sealed class DiagnosticBag
    {
        public List<Error> Diagnostics;

        public DiagnosticBag()
        {
            Diagnostics = new List<Error>();
        }

        public void Append(Error err)
        {
            Diagnostics.Add(err);
        }

        public void Extend(DiagnosticBag extendor)
        {
            Diagnostics.AddRange(extendor.Diagnostics);
        }

        #nullable enable
        public Error? GetIfError()
        {
            if (Diagnostics.Count <= 0)
                return null;
            return Diagnostics[0];
        }
    }
}
