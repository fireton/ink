﻿using System;

namespace Inklewriter
{
	public partial class InkParser : StringParser
	{
        public InkParser(string str, string filenameForMetadata = null) : base(str) { 
            _filename = filenameForMetadata;
			RegisterExpressionOperators ();
            GenerateStatementLevelRules ();
            this.errorHandler = OnError;
		}

        protected override string PreProcessInputString(string str)
        {
            var inputWithCommentsRemoved = (new CommentEliminator (str)).Process();
            return inputWithCommentsRemoved;
        }

        protected override void RuleDidSucceed(object result, StringParserState.Element stateAtStart, StringParserState.Element stateAtEnd)
        {
            // Apply DebugMetadata based on the state at the start of the rule
            // (i.e. use line number as it was at the start of the rule)
            var parsedObj = result as Parsed.Object;
            if ( parsedObj != null) {
                var md = new Runtime.DebugMetadata ();
                md.startLineNumber = stateAtStart.lineIndex + 1;
                md.endLineNumber = stateAtEnd.lineIndex + 1;
                md.fileName = _filename;
                parsedObj.debugMetadata = md;
            }
        }

        public void OnError(string message, int index, int lineIndex)
        {
            if (_filename != null) {
                Console.WriteLine ("Error in '{0}' line {1}: {2}", _filename, (lineIndex+1), message);
            } else {
                Console.WriteLine ("Error on line {0}: {1}", (lineIndex+1), message);
            }
        }

        string _filename;
	}
}

