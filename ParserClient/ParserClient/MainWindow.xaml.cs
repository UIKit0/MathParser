// Copyright (c) 2009 Gratian Lup. All rights reserved.
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ServiceModel;
using MathParserService;
using MathParserLib;

namespace ParserClient {
    public partial class MainWindow : Window {
        ParserServiceClient service;
        List<VariableInfo> userVariables;
        VariableInfo currentVariable;

        public MainWindow() {
            this.InitializeComponent();
            userVariables = new List<VariableInfo>();

            try {
                service = new ParserServiceClient();

                // Get the lists with the built-in functions and constants.
                FunctionInfo[] functions = service.GetAvailableFunctions();
                FunctionsListView.ItemsSource = functions;

                VariableInfo[] variables = service.GetAvailableVariables();
                AvailableVariablesListView.ItemsSource = variables;
            }
            catch(Exception ex) {
                MessageBox.Show("Service not started! Click OK to close the application.");
                Application.Current.Shutdown();
            }
        }

        private void EvaluateButton_Click(object sender, RoutedEventArgs e) {
            if(string.IsNullOrEmpty(ExpressionTextBox.Text.Trim())) {
                // The expression has not been defined yet.
                return;
            }

            try {
                double result;
                ParseError error = service.EvaluateExpression(out result, ExpressionTextBox.Text, 
                                                              userVariables.ToArray());
                // Validate the recived result.
                switch(error.TargetType) {
                    case TargetType.None: {
                        ResultLabel.Text = string.Format("{0:f6}", result);
                        ResultLabel.FontSize = 48;
                        break;
                    }
                    case TargetType.Function: {
                        ResultLabel.Text = "Function '" + error.Target + "' could not be found.";
                        ResultLabel.FontSize = 24;
                        break;
                    }
                    case TargetType.Number: {
                        ResultLabel.Text = "Invalid number: " + error.Target;
                        ResultLabel.FontSize = 24;
                        break;
                    }
                    case TargetType.Variable: {
                        ResultLabel.Text = "Variable '" + error.Target + "' could not be found.";
                        ResultLabel.FontSize = 24;
                        break;
                    }
                    case TargetType.FunctionVariable: {
                        ResultLabel.Text = "Function '" + error.Target + "' doesn't have operators.";
                        ResultLabel.FontSize = 24;
                        break;
                    }
                    case TargetType.Other: {
                        ResultLabel.Text = "Invalid expression!";
                        ResultLabel.FontSize = 24;
                        break;
                    }
                }
            }
            catch(Exception ex) {
                ResultLabel.Text = "Failed to connect to service!";
                ResultLabel.FontSize = 24;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            userVariables.Add(new VariableInfo());
            VariablesListView.ItemsSource = null;
            VariablesListView.ItemsSource = userVariables;
            VariablesListView.SelectedIndex = userVariables.Count - 1;
        }

        private void RemoveButtoon_Click(object sender, RoutedEventArgs e) {
            if(VariablesListView.SelectedIndex >= 0 && 
               VariablesListView.SelectedIndex < userVariables.Count) {
                userVariables.RemoveAt(VariablesListView.SelectedIndex);
                VariablesListView.ItemsSource = null;
                VariablesListView.ItemsSource = userVariables;
            }
        }

        private void VariablesListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(VariablesListView.SelectedItem != null) {
                currentVariable = VariablesListView.SelectedItem as VariableInfo;
                NameTextBox.Text = currentVariable.Name;
                ValueTextBox.Text = currentVariable.Value.ToString();

                if(string.IsNullOrEmpty(currentVariable.Name)) {
                    NameTextBox.Text = "Untitled";
                    NameTextBox.Focus();
                    NameTextBox.SelectAll();
                }
            }
        }

        private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e) {
            if(currentVariable != null) {
                double value;
                
                if(double.TryParse(ValueTextBox.Text.Trim(), out value)) {
                    currentVariable.Value = value;
                    VariablesListView.ItemsSource = null;
                    VariablesListView.ItemsSource = userVariables;
                }
            }
        }

        private bool ContainsVariable(string name, VariableInfo currentInfo) {
            foreach(VariableInfo vi in userVariables) {
                if((vi.Name == name) && (vi != currentInfo)) return true;
            }

            return false;
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e) {
            if(currentVariable != null) {
                // Validate the variable name.
                if(string.IsNullOrEmpty(NameTextBox.Text.Trim()) ||
                   ContainsVariable(NameTextBox.Text.Trim(), currentVariable)) {
                    MessageBox.Show("Variable name invalid or already defined!");
                    NameTextBox.SelectAll();
                    return;
                }

                currentVariable.Name = NameTextBox.Text;
                VariablesListView.ItemsSource = null;
                VariablesListView.ItemsSource = userVariables;
            }
        }

        private void FunctionsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(FunctionsListView.SelectedItem != null) {
                ExpressionTextBox.Text += ((FunctionInfo)FunctionsListView.SelectedItem).Name + "(";
                ExpressionTextBox.Focus();
                ExpressionTextBox.CaretIndex = ExpressionTextBox.Text.Length;
            }
        }

        private void AvailableVariablesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(VariablesListView.SelectedItem != null) {
                ExpressionTextBox.Text += ((VariableInfo)AvailableVariablesListView.SelectedItem).Name;
                ExpressionTextBox.Focus();
                ExpressionTextBox.CaretIndex = ExpressionTextBox.Text.Length;
            }
        }

        private void ExpressionTextBox_KeyDown(object sender, KeyEventArgs e) {
            // Unused yet.
        }

        private void VariablesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(VariablesListView.SelectedItem != null) {
                ExpressionTextBox.Text += ((VariableInfo)VariablesListView.SelectedItem).Name;
                ExpressionTextBox.Focus();
                ExpressionTextBox.CaretIndex = ExpressionTextBox.Text.Length;
            }
        }

        private void ExpressionTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                EvaluateButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
