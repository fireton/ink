﻿
namespace Inklewriter.Parsed
{
    public class VariableAssignment : Parsed.Object
    {
        public string variableName { get; protected set; }
        public Expression expression { get; protected set; }
        public bool isNewDeclaration { get; protected set; }

        public VariableAssignment (string variableName, Expression assignedExpression, bool isNewDeclaration)
        {
            this.variableName = variableName;
            this.expression = AddContent(assignedExpression);
            this.isNewDeclaration = isNewDeclaration;
        }

        public override Runtime.Object GenerateRuntimeObject ()
        {
            var container = new Runtime.Container ();

            // The expression's runtimeObject is actually another nested container
            container.AddContent (expression.runtimeObject);

            container.AddContent (new Runtime.VariableAssignment (variableName, isNewDeclaration));

            return container;
        }

        public override void ResolveReferences (Story context)
        {
            base.ResolveReferences (context);

            if (!this.isNewDeclaration) {
                if (!context.ResolveVariableWithName (this.variableName, fromNode:this)) {
                    Error ("variable could not be found to assign to: '" + this.variableName + "'", this);
                }
            }

            if (IsReservedKeyword (variableName)) {
                Error ("cannot use '" + variableName + "' as a variable since it's a reserved ink keyword");
            }
        }

        // TODO: Move this somewhere more general?
        bool IsReservedKeyword(string name)
        {
            return name == "true"   || name == "false" 
                || name == "on"     || name == "off" 
                || name == "yes"    || name == "no"
                || name == "return" || name == "done";
        }
    }
}

