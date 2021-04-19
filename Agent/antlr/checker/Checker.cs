using System;
using System.Collections.Generic;

namespace Agent.antlr.checker
{
    public class Checker
    {
        private Dictionary<String, Assigment> symboltable;
        
        private void checkUndefinedVariables(ASTNode astnode)
        {
            for (ASTNode node : astNode.getChildren()) {
                if (node instanceof VariableReference) {
                    boolean undefined = false;
                    for (Dictionary<String, Assigment>.Entry<String, Assignment> entry : symboltable.entrySet()) {
                        if (entry.getValue().name.name.equals(((VariableReference) node).name))
                            undefined = true;
                    }
                    if (!undefined)
                        node.setError("Reference " + ((VariableReference) node).name + " is referencing an unassigned constant.");
                }
                if (!node.getChildren().isEmpty())
                    checkUndefinedVariables(node);
            }

        }
        
    }
}