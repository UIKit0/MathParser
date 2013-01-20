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
    /// <summary>
    /// Contine informatii despre o expresie (functie, variabila, operator).
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
    /// Contine informatii despre o expresie aflata pe stiva (temporara) de operatori.
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
    /// Elementul care a generat eroarea.
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
    /// Reprezinta o eroare care a avut loc in timpul parcurgerii expresiei.
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
    /// Implementeaza evaluarea unei expresii.
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
        private Stack<ExpressionStackItem> expressionStack; // stiva cu expresii

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
        /// Elimina toate spatiile dintr-o expresie.
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
        /// Verifica daca cuvantul gasit este un numar. Suporta semnele + si -.
        /// </summary>
        private bool IsNumber(string word) {
            int length = word.Length;
            for(int i = 0; i < length; i++) {
                char c = word[i];
                if((char.IsDigit(c) == false) && (c != '.') && (IsOperator(c) == false)) return false; // nu este numar
            }

            return true;
        }

        /// <summary>
        /// Determina tipul cuvantului gasit (functie, variabila sau numar).
        /// </summary>
        /// <param name="word"></param>
        /// <param name="nextChar"></param>
        /// <param name="isFunction"></param>
        /// <returns></returns>
        private ExpressionStackItem ParseWord(string word, char nextChar, out bool isFunction) {
            isFunction = false; // presupunem ca nu-i functie

            if(nextChar == OpenBracket) {
                // doar functiile pot avea paranteza '('
                if(IsFunction(word)) {
                    isFunction = true;
                    return new ExpressionStackItem(_functions[word]);
                }
                else {
                    // functia nu este definita
                    _errors.Add(new ParseError(TargetType.Function, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }
            else if(IsNumber(word)) {
                double number;
                if(double.TryParse(word, out number)) {
                    // numarul este valid
                    return new ExpressionStackItem(null, number, true /* evaluated */);
                }
                else {
                    // numarul nu este valid
                    _errors.Add(new ParseError(TargetType.Number, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }
            else {
                if(IsVariable(word)) {
                    // este o variabila
                    return new ExpressionStackItem(null, _variables[word], true /* evaluated */);
                }
                else {
                    // variabia nu este definita
                    _errors.Add(new ParseError(TargetType.Variable, word));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }

            System.Diagnostics.Debug.Assert(false); // nu ar trebui sa ajunga aici!
        }

        void Test() { }

        /// <summary>
        /// Adauga un operator in stiva temporara cu operatori.
        /// Daca este necesar, operatorii vor fi reordonati in functie de prioritate.
        /// </summary>
        private void AddOperator(Stack<OperatorStackItem> operatorStack, char c) {
            if(operatorStack.Count > 0) {
                // trebuie sa reordonam operatorii
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
                                break; // ne oprim daca am dat de paranteza deschisa '('
                            }
                        }
                        else break; // nu mai sunt elemente in stiva
                    }
                }
            }

            // adauga operatorul in stiva
            MathOperator op = _operators[c];
            operatorStack.Push(new OperatorStackItem(op, op.Precedence));
        }

        /// <summary>
        /// Parcurge expresia si o transforma intr-o stiva cu expresii bazate pe forma poloneza postfixata.
        /// </summary>
        private void ParseExpression(string expression) {
            bool wasOperator = false; // folosit pt. numere cu semn (- sau +)
            int operatorCount = 0;
            char currentChar;
            string currentWord = string.Empty;
            Stack<OperatorStackItem> operatorStack = new Stack<OperatorStackItem>(); // stiva temporara a operatorilor

            int length = expression.Length;
            for(int i = 0; i < length; i++) {
                currentChar = expression[i];

                bool isOperator = IsOperator(currentChar);
                if(isOperator && (wasOperator || i == 0)) {
                    if(operatorCount > 1) {
                        // nu ar trebui sa fie mai mult de doi operatori consecutivi
                        _errors.Add(new ParseError(TargetType.Other, string.Empty));
                        throw new ParseException(_errors[_errors.Count - 1]);
                    }

                    // trateaza ca pe un semn (+ sau -)
                    isOperator = false;
                }

                if(isOperator || (currentChar == OpenBracket) ||
                                  (currentChar == CloseBracket) ||
                                  (currentChar == VariableSeparator)) {
                    // caracter special (operator, paranteza sau separator de variabile in functii)

                    if(string.IsNullOrEmpty(currentWord) == false) {
                        // inainte a fost un cuvant, trebuie sa-i determinam tipul
                        bool isFunction;
                        ExpressionStackItem exprItem = ParseWord(currentWord, currentChar, out isFunction);

                        if(isFunction) {
                            // adauga in stiva cu operatori
                            operatorStack.Push(new OperatorStackItem(exprItem.Expression, OperatorPrecedence.Function));
                        }
                        else {
                            // poate fi adaugat direct in stiva cu expresii
                            expressionStack.Push(exprItem);
                        }

                        // reseteaza cuvantul
                        currentWord = string.Empty;
                        wasOperator = false;
                        operatorCount = 0;
                    }

                    if(isOperator) {
                        // adauga operatorul  in stiva
                        AddOperator(operatorStack, currentChar);
                        wasOperator = true;
                        operatorCount++;
                    }
                    else if((currentChar == VariableSeparator) ||
                             (currentChar == CloseBracket)) {
                        if(operatorStack.Count > 0) {
                            // extrage din stiva cu operatori pana intalnim paranteza '('
                            OperatorPrecedence precedence = operatorStack.Peek().Precedence;
                            while(precedence != OperatorPrecedence.OpenBracket) {
                                expressionStack.Push(new ExpressionStackItem(operatorStack.Pop().Expression));

                                if(operatorStack.Count == 0) {
                                    // paranteza '(' nu a fost gasita
                                    _errors.Add(new ParseError(TargetType.Other, string.Empty));
                                    throw new ParseException(_errors[_errors.Count - 1]);
                                }

                                precedence = operatorStack.Peek().Precedence;
                            }

                            // daca avem o paranteza inchisa, sterge-o
                            if(currentChar == CloseBracket) {
                                operatorStack.Pop();
                            }
                        }

                        wasOperator = false;
                        operatorCount = 0;
                    }
                    else if(currentChar == OpenBracket) {
                        // adauga o paranteza deschisa
                        operatorStack.Push(new OperatorStackItem(OperatorPrecedence.OpenBracket));
                        wasOperator = true;
                        operatorCount++;
                    }
                }
                else {
                    // adauga litera la cuvant
                    currentWord += currentChar;
                    wasOperator = false;
                    operatorCount = 0;
                }
            }

            // analizeaza ultimul cuvant
            if(string.IsNullOrEmpty(currentWord) == false) {
                bool isFunction;
                expressionStack.Push(ParseWord(currentWord, ' ', out isFunction));

                if(isFunction) {
                    // expresia nu se poate termina cu o functie (nu are operatori)
                    _errors.Add(new ParseError(TargetType.FunctionVariable, currentWord));
                    throw new ParseException(_errors[_errors.Count - 1]);
                }
            }

            // extrage operatorii ramasi
            while(operatorStack.Count > 0) {
                if(operatorStack.Peek().Precedence == OperatorPrecedence.OpenBracket) {
                    // nu ar fi trebuit sa mai ramana paranteze deschise in stiva
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
        /// Transforma expresia data in stiva de evaluare.
        /// </summary>
        public void BuildExpression(string expression) {
            expression = RemoveSpaces(expression); // sterge spatiile de la inceput/sfarsit
            ParseExpression(expression);
        }

        /// <summary>
        /// Evalueaza expresia din stiva.
        /// </summary>
        public double Evaluate() {
            ExpressionStackItem item = expressionStack.Pop();

            if(item.IsEvaluated) return item.Value;
            else {
                item.Value = item.Expression.Evaluate(this);
                item.IsEvaluated = true;

                return item.Value;
            }
        }

        /// <summary>
        /// Folosita de obiecte IMathExpression pentru a extrage valori din stiva.
        /// </summary>
        /// <returns></returns>
        internal double Pop() {
            return Evaluate();
        }

        #endregion
    }
}
