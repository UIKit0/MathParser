// Copyright (c) Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "MathParser" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "MathParser" nor
// may "MathParser" appear in their names without prior written
// permission of the author.
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
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MathParserLib;

namespace MathParserService {
    public class ParserService : IParserService {
        private void InitializeParser(MathParser parser) {
            // Add the available operators.
            parser.AddOperator(new AddOperator());
            parser.AddOperator(new SubstractOperator());
            parser.AddOperator(new MultiplyOperator());
            parser.AddOperator(new DivideOperator());
            parser.AddOperator(new ExponentialOperator());

            // Add the available functions.
            parser.AddFunction(new SinFunction());
            parser.AddFunction(new CosFunction());
            parser.AddFunction(new TanFunction());
            parser.AddFunction(new AtanFunction());
            parser.AddFunction(new AbsFunction());
            parser.AddFunction(new MinFunction());
            parser.AddFunction(new MaxFunction());
            parser.AddFunction(new SqrtFunction());
            parser.AddFunction(new LogFunction());
            parser.AddFunction(new LnFunction());
            parser.AddFunction(new Log10Function());
            parser.AddFunction(new SignFunction());
            parser.AddFunction(new CeilFunction());
            parser.AddFunction(new FloorFunction());

            // Add the available constants.
            parser.AddVariable("PI", Math.PI);
            parser.AddVariable("E", Math.E);
        }

        public ParseError EvaluateExpression(string expression, List<VariableInfo> variables, out double result) {
            if(string.IsNullOrEmpty(expression)) {
                throw new ArgumentNullException("Expression was not defined.");
            }
            // ------------------------------------------------------- */
            result = 0;

            try {
                MathParser parser = new MathParser();

                // Add the received variables before starting the evaluation.
                foreach(VariableInfo varInfo in variables) {
                    parser.Variables.Add(varInfo.Name, varInfo.Value);
                }

                // Parse and evalute the received expression.
                InitializeParser(parser);
                parser.BuildExpression(expression);
                result = parser.Evaluate();
            }
            catch(ParseException pe) {
                return pe.Error;
            }
            catch(Exception e) {
                return new ParseError(TargetType.Other, string.Empty);
            }

            return new ParseError(TargetType.None, string.Empty);
        }

        public List<VariableInfo> GetAvailableVariables() {
            List<VariableInfo> list = new List<VariableInfo>();
            MathParser parser = new MathParser();
            InitializeParser(parser);

            foreach(KeyValuePair<string, double> kvp in parser.Variables) {
                list.Add(new VariableInfo() { Value = kvp.Value, Name = kvp.Key });
            }

            return list;
        }

        public List<FunctionInfo> GetAvailableFunctions() {
            List<FunctionInfo> list = new List<FunctionInfo>();
            MathParser parser = new MathParser();
            InitializeParser(parser);

            foreach(KeyValuePair<string, MathFunction> kvp in parser.Functions) {
                list.Add(new FunctionInfo() { HelpString = kvp.Value.HelpString, Name = kvp.Key });
            }

            return list;
        }

        public List<OperatorInfo> GetAvailableOperators() {
            List<OperatorInfo> list = new List<OperatorInfo>();
            MathParser parser = new MathParser();
            InitializeParser(parser);

            foreach(KeyValuePair<char, MathOperator> kvp in parser.Operators) {
                list.Add(new OperatorInfo() { HelpString = kvp.Value.HelpString, Symbol = kvp.Key });
            }

            return list;
        }
    }
}
