// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal sealed class ComponentFormNameLoweringPass : ComponentIntermediateNodePassBase, IRazorOptimizationPass
{
    // Run after component lowering pass
    public override int Order => 50;

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (!IsComponentDocument(documentNode))
        {
            return;
        }

        var references = documentNode.FindDescendantReferences<TagHelperDirectiveAttributeIntermediateNode>();
        foreach (var reference in references)
        {
            var node = (TagHelperDirectiveAttributeIntermediateNode)reference.Node;
            if (node.TagHelper.IsFormNameTagHelper())
            {
                var replacement = new FormNameIntermediateNode
                {
                    Source = node.Source
                };

                replacement.Children.AddRange(node.Children);
                replacement.Diagnostics.AddRange(node.Diagnostics);

                var parent = reference.Parent;

                if (!parent.Children.Any(c => c is HtmlAttributeIntermediateNode { AttributeName: "@onsubmit" }))
                {
                    replacement.Diagnostics.Add(ComponentDiagnosticFactory.CreateFormName_MissingOnSubmit(node.Source));
                }

                if (parent is not MarkupElementIntermediateNode { TagName: "form" })
                {
                    replacement.Diagnostics.Add(ComponentDiagnosticFactory.CreateFormName_NotAForm(node.Source));
                }

                reference.Replace(replacement);
            }
        }
    }
}
