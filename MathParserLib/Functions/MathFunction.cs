﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParserLib {
    /// <summary>
    /// Interfata care trebuie implementata de toate functiile incluse (sin, cos, etc. ).
    /// </summary>
    public abstract class MathFunction : IMathExpression {
        public abstract string Symbol { get; }
        public abstract string HelpString { get; }
        public abstract double Evaluate(MathParser parser);
    }
}