MathParser
==========

An application which parses and evaluates mathematical expressions.  
Supports build-in functions and user-defined variables. Implemented using a client-server model.  

It is written in *C#* and uses *WCF* for communication and *WPF* for the GUI.  
Uses the [Shunting-yard algorithm](http://en.wikipedia.org/wiki/Shunting-yard_algorithm) to parse the mathematical expressions.  

The following video demonstrates the use of the client application:  
**[View demonstration video](http://youtu.be/7wdAIWGz_kA)**

### Component roles:  

- **MathParserLib**: implements the math expression parsing and evaluation functionality.  
- **MathParserService**: exposes the functionality as a *WCF* service.  
- **ParserServiceHost**: hosts the service and waits for clients to connect.  
- **ParserClient**: GUI which connects to the service and sends math expression for evaluation.    

### Screenshots:

![SmartFlip screenshot](http://www.gratianlup.com/documents/math_parser_1.PNG)  
