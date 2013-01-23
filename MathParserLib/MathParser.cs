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
using System.Text;

namespace MathParserLib {
    /// <summary>
    /// Contains information about a parsed expression element 
    /// (function, variable or operator).
    /// </summary>
    class ExpressionStackItem {
        public IMathExpression Expression;
        public double Value;
        public bool IsEvaluated;

        public ExpressionStackItem(IMathExpression expression, double value, bool evaluated) {
            Expression = expression;
            Value = value;
            IsEvaluated = evaluated;
        }

        public ExpressionStackItem(IMathExpression expression) : this(expression, 0, false) { }

        public ExpressionStackItem() : this(null, 0, true) { }
    }


    /// <summary>
    // Contains information about an expression found on the operator stack.
    /// </summary>
    class OperatorStackItem {
        public IMathExpression Expression;
        public OperatorPrecedence Precedence;

        public OperatorStackItem(IMathExpression expression, OperatorPrecedence precedence) {
            Expression = expression;
            Precedence = precedence;
        }

        public OperatorStackItem(OperatorPrecedence precedence) : this(null, precedence) { }

        public OperatorStackItem() { }
    }


    /// <summary>
    /// Defines the type of elements that can generate parsing errors.
    /// </summary>
    [Serializable]
    public enum TargetType {
        Function,
        Variable,
        Number,
        FunctionVariable,
        Other,
        None
    }


    /// <summary>
    /// Describes an error that appeared during expression parsing.
    /// </summary>
    [Serializable]
    public class ParseError {
        public TargetType TargetType;
        public string Target;

        public ParseError(TargetType type, string target) {
            TargetType = type;
            Target = target;
        }
    }

    /// <summary>
    /// Describes an expection thrown because the parsed expression is invalid.
    /// </summary>
    [Serializable]
    public class ParseException : Exception {
        private ParseError _error;
        public ParseError Error {
            get { return _error; }
            set { _error = value; }
        }

        public ParseException() { }

        public ParseException(ParseError error) {
            _error = error;
        }
    }


    /// <summary>
    /// Implements the math expression parsing and evaluation.
    /// Based on the Shunting-yard algorithm.
    /// </summary>
    public class MathParser {
        #region Constants

        const char OpenBracket = '(';
        const char CloseBracket = ')';
        const char VariableSeparator = ',';
        const char NoOperator = '?';

        #endregion

        #region Fields

        private Dictionary<char, MathOperator> _operators;
        private Dictionary<string, MathFunction> _functions;
        private Dictionary<string, double> _variables;
        private List<ParseError> _errors;
        private Stack<ExpressionStackItem> expressionStack;

        #endregion

        #region Constructor

        public MathParser() {
            _operators = new Dictionary<char, MathOperator>();
            _functions = new Dictionary<string, MathFunction>();
            _variables = new Dictionary<string, double>();
            _errors = new List<ParseError>();
            expressionStack = new Stack<ExpressionStackItem>();
        }

        #endregion

        #region Properties

        public Dictionary<char, MathOperator> Operators {
            get { return _operators; }
            set { _operators = value; }
        }

        public Dictionary<string, MathFunction> Functions {
            get { return _functions; }
            set { _functions = value; }
        }

        public Dictionary<string, double> Variables {
            get { return _variables; }
            set { _variables = value; }
        }

        public List<ParseError> Errors {
            get { return _errors; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Removes all white-space characters from a string.
        /// </summary>
        private string RemoveSpaces(string expression) {
            StringBuilder builder = new StringBuilder();

            int length = expression.Length;
            for(int i = 0; i < length; i++) {
                if(char.IsWhiteSpace(expression[i]) == false) {
                    builder.Append(expression[i]);
                }
            }

            return builder.ToString();
        }

        private bool IsOperator(char c) {
            return _operators.ContainsKey(c);
        }

        private bool IsFunction(string name) {
            return _functions.ContainsKey(name);
        }

        private bool IsVariable(string word) {
            return _variables.ContainsKey(word);
        }

        /// <summary>
        /// Checks if the specified words represents a valid number.
        /// Allows both integer and floating numbers, including the ones
        /// which start with the + or - symbols.
        /// </summary>
        private bool IsNumber(string word) {
            int length = word.Length;

            for(int i = 0; i < length; i++) {
                char c = word[i];

                if((c != '.') &&
                   (char.IsDigit(c) == false) &&
                   (IsOperator(c) == false)) {
                    // Definitely not a valid number.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines the type of the word in the expression.
        /// </summary>
        private ExpressionStackItem ParseWord(string word, char nextChar, out bool isFunction) {
            isFunction = false;

            if(nextChar == OpenBracket) {
                // ( means that a function call starts.
                if(IsFunction(word)) {
                    isFunction = true;
                    return new ExpressionStackItem(_functions[word]);
                }
                else {
                    // The function could not be found, report the error.
                    _errors.Add(new ParseError(TargetType.Function, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }
            else if(IsNumber(word)) {
                double number;

                if(double.TryParse(word, out number)) {
                    return new ExpressionStackItem(null, number, true /* evaluated */);
                }
                else {
                    // The number is invalid, report the error.
                    _errors.Add(new ParseError(TargetType.Number, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }
            else {
                if(IsVariable(word)) {
                    return new ExpressionStackItem(null, _variables[word], true /* evaluated */);
                }
                else {
                    // The variable could not be found, report the error.
                    _errors.Add(new ParseError(TargetType.Variable, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }
        }

        /// <summary>
        /// Adds an operator to the operator stack.
        /// If required, the operators are reordered according to their priority.
        /// </summary>
        private void AddOperator(Stack<OperatorStackItem> operatorStack, char c) {
            if(operatorStack.Count > 0) {
                // The operands might need to be reordered.
                OperatorPrecedence prevPrecedence = operatorStack.Peek().Precedence;
                OperatorPrecedence currentPrecedence;

                if(prevPrecedence != OperatorPrecedence.OpenBracket) {
                    currentPrecedence = _operators[c].Precedence;
                    while(currentPrecedence <= prevPrecedence) {
                        IMathExpression expression = operatorStack.Pop().Expression;
                        expressionStack.Push(new ExpressionStackItem(expression));

                        if(operatorStack.Count > 0) {
                            prevPrecedence = operatorStack.Peek().Precedence;
                            if(prevPrecedence == OperatorPrecedence.OpenBracket) {
                                // Stop when the an open paren ( is found.
                                break;
                            }
                        }
                        else break;
                    }
                }
            }

            // Add the operator to the stack.
            MathOperator op = _operators[c];
            operatorStack.Push(new OperatorStackItem(op, op.Precedence));
        }

        /// <summary>
        /// Parses and evaluates the specified expression.
        /// </summary>
        private void ParseExpression(string expression) {
            bool wasOperator = false;
            int operatorCount = 0;
            char currentChar;
            string currentWord = string.Empty;
            Stack<OperatorStackItem> operatorStack = new Stack<OperatorStackItem>();
            int length = expression.Length;

            for(int i = 0; i < length; i++) {
                currentChar = expression[i];
                bool isOperator = IsOperator(currentChar);

                if(isOperator && (wasOperator || i == 0)) {
                    if(operatorCount > 1) {
                        // Not enought operands available, report error.
                        _errors.Add(new ParseError(TargetType.Other, string.Empty));
                        throw new ParseException(_errors[_errors.Count - 1]);
                    }

                    isOperator = false;
                }

                if(isOperator || (currentChar == OpenBracket)  ||
                                 (currentChar == CloseBracket) ||
                                 (currentChar == VariableSeparator)) {
                    // A word separator was found, check what the previous word has been.
                    if(string.IsNullOrEmpty(currentWord) == false) {
                        bool isFunction;
                        ExpressionStackItem exprItem = ParseWord(currentWord, currentChar, out isFunction);

                        if(isFunction) {
                            operatorStack.Push(new OperatorStackItem(exprItem.Expression, OperatorPrecedence.Function));
                        }
                        else {
                            expressionStack.Push(exprItem);
                        }

                        // Reset the current word.
                        currentWord = string.Empty;
                        wasOperator = false;
                        operatorCount = 0;
                    }

                    if(isOperator) {
                        AddOperator(operatorStack, currentChar);
                        wasOperator = true;
                        operatorCount++;
                    }
                    else if((currentChar == VariableSeparator) ||
                             (currentChar == CloseBracket)) {
                        if(operatorStack.Count > 0) {
                            // Extract all operators until the open paren ( is found.
                            OperatorPrecedence precedence = operatorStack.Peek().Precedence;

                            while(precedence != OperatorPrecedence.OpenBracket) {
                                expressionStack.Push(new ExpressionStackItem(operatorStack.Pop().Expression));

                                if(operatorStack.Count == 0) {
                                    // The open paren could not be found, report error.
                                    _errors.Add(new ParseError(TargetType.Other, string.Empty));
                                    throw new ParseException(_errors[_errors.Count - 1]);
                                }

                                precedence = operatorStack.Peek().Precedence;
                            }

                            // Remove the close paren ) if found.
                            if(currentChar == CloseBracket) {
                                operatorStack.Pop();
                            }
                        }

                        wasOperator = false;
                        operatorCount = 0;
                    }
                    else if(currentChar == OpenBracket) {
                        // Push the open paren ( to the stack.
                        operatorStack.Push(new OperatorStackItem(OperatorPrecedence.OpenBracket));
                        wasOperator = true;
                        operatorCount++;
                    }
                }
                else {
                    // Extend the current word with the character.
                    currentWord += currentChar;
                    wasOperator = false;
                    operatorCount = 0;
                }
            }

            // Check the type of the last word.
            if(string.IsNullOrEmpty(currentWord) == false) {
                bool isFunction;
                expressionStack.Push(ParseWord(currentWord, ' ', out isFunction));

                if(isFunction) {
                    // The word should not be a function name, it cannot be followed
                    // by any other symbol, making it invalid.
                    _errors.Add(new ParseError(TargetType.FunctionVariable, currentWord));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }

            // Extract the remaining operators.
            while(operatorStack.Count > 0) {
                if(operatorStack.Peek().Precedence == OperatorPrecedence.OpenBracket) {
                    // In a valid expression no more open parens should remain.
                    _errors.Add(new ParseError(TargetType.Other, string.Empty));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }

                expressionStack.Push(new ExpressionStackItem(operatorStack.Pop().Expression));
            }
        }

        #endregion

        #region Public methods

        public void AddOperator(MathOperator op) {
            _operators.Add(op.Symbol, op);
        }

        public void AddFunction(MathFunction f) {
            _functions.Add(f.Symbol, f);
        }

        public void AddVariable(string name, double value) {
            _variables.Add(name, value);
        }

        /// <summary>
        /// Transforms the specified expression string in an expression stack.
        /// </summary>
        public void BuildExpression(string expression) {
            expression = RemoveSpaces(expression);
            ParseExpression(expression);
        }

        /// <summary>
        /// Evaluates the expression found in the epxression stack.
        /// </summary>
        public double Evaluate() {
            ExpressionStackItem item = expressionStack.Pop();

            if(item.IsEvaluated) {
                return item.Value;
            }
            else {
                item.Value = item.Expression.Evaluate(this);
                item.IsEvaluated = true;
                return item.Value;
            }
        }

        /// <summary>
        /// Method used by IMathExpression objects to extract operands from the stack.
        /// </summary>
        /// <returns></returns>
        internal double Pop() {
            return Evaluate();
        }

        #endregion
    }
}
