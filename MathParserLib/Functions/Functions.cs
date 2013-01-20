// Copyright (c) Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
//       copyright notice, this list of conditions and the following
//       disclaimer in the documentation and/or other materials provided
//       with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParserLib {
    public class SinFunction : MathFunction {
        public override string Symbol {
            get { return "sin"; }
        }

        public override string HelpString {
            get { return "sin(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Sin(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class CosFunction : MathFunction {
        public override string Symbol {
            get { return "cos"; }
        }

        public override string HelpString {
            get { return "cos(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Cos(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class TanFunction : MathFunction {
        public override string Symbol {
            get { return "tan"; }
        }

        public override string HelpString {
            get { return "tan(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Tan(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class AtanFunction : MathFunction {
        public override string Symbol {
            get { return "atan"; }
        }

        public override string HelpString {
            get { return "atan(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Atan(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class AbsFunction : MathFunction {
        public override string Symbol {
            get { return "abs"; }
        }

        public override string HelpString {
            get { return "abs(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Abs(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class SqrtFunction : MathFunction {
        public override string Symbol {
            get { return "sqrt"; }
        }

        public override string HelpString {
            get { return "sqrt(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Sqrt(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class MinFunction : MathFunction {
        public override string Symbol {
            get { return "min"; }
        }

        public override string HelpString {
            get { return "min(x,y)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Min(parser.Pop(), parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class MaxFunction : MathFunction {
        public override string Symbol {
            get { return "max"; }
        }

        public override string HelpString {
            get { return "max(x,y)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Max(parser.Pop(), parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class LogFunction : MathFunction {
        public override string Symbol {
            get { return "log"; }
        }

        public override string HelpString {
            get { return "log(x, base)"; }
        }

        public override double Evaluate(MathParser parser) {
            double newBase = parser.Pop();
            return Math.Log(parser.Pop(), newBase);
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class LnFunction : MathFunction {
        public override string Symbol {
            get { return "ln"; }
        }

        public override string HelpString {
            get { return "ln(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Log(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class Log10Function : MathFunction {
        public override string Symbol {
            get { return "log10"; }
        }

        public override string HelpString {
            get { return "log10(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Log10(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class SignFunction : MathFunction {
        public override string Symbol {
            get { return "sign"; }
        }

        public override string HelpString {
            get { return "sign(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Sign(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class CeilFunction : MathFunction {
        public override string Symbol {
            get { return "ceil"; }
        }

        public override string HelpString {
            get { return "ceil(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Ceiling(parser.Pop());
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class FloorFunction : MathFunction {
        public override string Symbol {
            get { return "floor"; }
        }

        public override string HelpString {
            get { return "floor(x)"; }
        }

        public override double Evaluate(MathParser parser) {
            return Math.Floor(parser.Pop());
        }
    }
}
