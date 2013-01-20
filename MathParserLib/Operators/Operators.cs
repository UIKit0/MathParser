﻿// Copyright (c) Gratian Lup. All rights reserved.
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
    public class AddOperator : MathOperator {
        public override OperatorPrecedence Precedence {
            get { return OperatorPrecedence.Add; }
        }

        public override char Symbol {
            get { return '+'; }
        }

        public override string HelpString {
            get { return "val1 + val2"; }
        }

        public override double Evaluate(MathParser parser) {
            double a = parser.Pop();
            double b = parser.Pop();
            return a + b;
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class SubstractOperator : MathOperator {
        public override OperatorPrecedence Precedence {
            get { return OperatorPrecedence.Add; }
        }

        public override char Symbol {
            get { return '-'; }
        }

        public override string HelpString {
            get { return "val1 - val2"; }
        }

        public override double Evaluate(MathParser parser) {
            double a = parser.Pop();
            double b = parser.Pop();
            return b - a;
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class MultiplyOperator : MathOperator {
        public override OperatorPrecedence Precedence {
            get { return OperatorPrecedence.Multiply; }
        }

        public override char Symbol {
            get { return '*'; }
        }

        public override string HelpString {
            get { return "val1 * val2"; }
        }

        public override double Evaluate(MathParser parser) {
            double a = parser.Pop();
            double b = parser.Pop();
            return b * a;
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class DivideOperator : MathOperator {
        public override OperatorPrecedence Precedence {
            get { return OperatorPrecedence.Multiply; }
        }

        public override char Symbol {
            get { return '/'; }
        }

        public override string HelpString {
            get { return "val1 / val2"; }
        }

        public override double Evaluate(MathParser parser) {
            double a = parser.Pop();
            double b = parser.Pop();
            return b / a;
        }
    }

    // ------------------------------------------------------------------------------------------------------
    public class ExponentialOperator : MathOperator {
        public override OperatorPrecedence Precedence {
            get { return OperatorPrecedence.Exponential; }
        }

        public override char Symbol {
            get { return '^'; }
        }

        public override string HelpString {
            get { return "val1 ^ val2"; }
        }

        public override double Evaluate(MathParser parser) {
            double a = parser.Pop();
            double b = parser.Pop();
            return Math.Pow(b, a);
        }
    }
}