using System;
using System.Collections.Generic;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;
using Compiler.Source.Datatypes;

namespace Compiler.Source.Lib
{
    public class RuntimeResult
    {
        #nullable enable
        public dynamic? Value { get; set; }
        public Error? Err { get; set; }
        public Datatype? FuncReturn { get; set; }
        public bool LoopContinue { get; set; }
        public bool LoopBreak { get; set; }
        #nullable disable

        public RuntimeResult()
        {
            Reset();
        }

        public void Reset()
        {
            Value = null;
            Err = null;
            FuncReturn = null;
            LoopContinue = false;
            LoopBreak = false;
        }

        public dynamic Register(RuntimeResult res)
        {
            Err = res.Err;
            FuncReturn = res.FuncReturn;
            LoopBreak = res.LoopBreak;
            LoopContinue = res.LoopContinue;
            return res.Value;
        }

        public RuntimeResult Success(dynamic val)
        {
            Reset();
            Value = val;
            return this;
        }

        public RuntimeResult SuccessReturn(Datatype val)
        {
            Reset();
            FuncReturn = val;
            return this;
        }

        public RuntimeResult SuccessContinue()
        {
            Reset();
            LoopContinue = true;
            return this;
        }

        public RuntimeResult SuccessBreak()
        {
            Reset();
            LoopBreak = true;
            return this;
        }

        public RuntimeResult Failure(Error err)
        {
            Reset();
            Err = err;
            return this;
        }

        public bool ShouldReturn()
        {
            var hasError = Err != null;
            var hasReturn = FuncReturn != null;
            return hasError || hasReturn || LoopContinue || LoopBreak;
        }
    }
}
