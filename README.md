MathParser
==========

An application which parses and evaluates mathematical expressions.  
Supports build-in functions and user-defined variables. Implemented using a client-server model.  

It is written in C# and uses WCF for communication and WPF for the GUI.
Uses the Shunting-yard algorithm to parser the mathematical expressions.

Description of the projects:  
MathParserLib: implements the math expression parsing and evaluation functionality.
MathParserService: exposes the functionality as a WCF service.
ParserServiceHost: hosts the service and waits for clients to connect.
ParserClient: connects to the service and sends math expression for evaluation.

![SmartFlip screenshot](http://www.gratianlup.com/documents/math_parser_1.PNG)  
