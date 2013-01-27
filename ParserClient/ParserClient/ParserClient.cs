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


namespace MathParserService {
    using System.Runtime.Serialization;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "VariableInfo", Namespace = "http://schemas.datacontract.org/2004/07/MathParserService")]
    public partial class VariableInfo : object, System.Runtime.Serialization.IExtensibleDataObject {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string NameField;

        private double ValueField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                this.NameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Value {
            get {
                return this.ValueField;
            }
            set {
                this.ValueField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "FunctionInfo", Namespace = "http://schemas.datacontract.org/2004/07/MathParserService")]
    public partial class FunctionInfo : object, System.Runtime.Serialization.IExtensibleDataObject {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string HelpStringField;

        private string NameField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HelpString {
            get {
                return this.HelpStringField;
            }
            set {
                this.HelpStringField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                this.NameField = value;
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "OperatorInfo", Namespace = "http://schemas.datacontract.org/2004/07/MathParserService")]
    public partial class OperatorInfo : object, System.Runtime.Serialization.IExtensibleDataObject {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string HelpStringField;

        private char SymbolField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HelpString {
            get {
                return this.HelpStringField;
            }
            set {
                this.HelpStringField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Symbol {
            get {
                return this.SymbolField;
            }
            set {
                this.SymbolField = value;
            }
        }
    }
}
namespace MathParserLib {
    using System.Runtime.Serialization;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ParseError", Namespace = "http://schemas.datacontract.org/2004/07/MathParserLib")]
    public partial class ParseError : object, System.Runtime.Serialization.IExtensibleDataObject {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string TargetField;

        private MathParserLib.TargetType TargetTypeField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string Target {
            get {
                return this.TargetField;
            }
            set {
                this.TargetField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public MathParserLib.TargetType TargetType {
            get {
                return this.TargetTypeField;
            }
            set {
                this.TargetTypeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "TargetType", Namespace = "http://schemas.datacontract.org/2004/07/MathParserLib")]
    public enum TargetType : int {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Function = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Variable = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Number = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        FunctionVariable = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Other = 4,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 5,
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IParserService")]
public interface IParserService {

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IParserService/EvaluateExpression", ReplyAction = "http://tempuri.org/IParserService/EvaluateExpressionResponse")]
    MathParserLib.ParseError EvaluateExpression(out double result, string expression, MathParserService.VariableInfo[] variables);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IParserService/GetAvailableVariables", ReplyAction = "http://tempuri.org/IParserService/GetAvailableVariablesResponse")]
    MathParserService.VariableInfo[] GetAvailableVariables();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IParserService/GetAvailableFunctions", ReplyAction = "http://tempuri.org/IParserService/GetAvailableFunctionsResponse")]
    MathParserService.FunctionInfo[] GetAvailableFunctions();

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IParserService/GetAvailableOperators", ReplyAction = "http://tempuri.org/IParserService/GetAvailableOperatorsResponse")]
    MathParserService.OperatorInfo[] GetAvailableOperators();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IParserServiceChannel : IParserService, System.ServiceModel.IClientChannel {
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class ParserServiceClient : System.ServiceModel.ClientBase<IParserService>, IParserService {

    public ParserServiceClient() {
    }

    public ParserServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName) {
    }

    public ParserServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress) {
    }

    public ParserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress) {
    }

    public ParserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress) {
    }

    public MathParserLib.ParseError EvaluateExpression(out double result, string expression, MathParserService.VariableInfo[] variables) {
        return base.Channel.EvaluateExpression(out result, expression, variables);
    }

    public MathParserService.VariableInfo[] GetAvailableVariables() {
        return base.Channel.GetAvailableVariables();
    }

    public MathParserService.FunctionInfo[] GetAvailableFunctions() {
        return base.Channel.GetAvailableFunctions();
    }

    public MathParserService.OperatorInfo[] GetAvailableOperators() {
        return base.Channel.GetAvailableOperators();
    }
}
