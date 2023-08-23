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
                var parent = reference.Parent;

                if (parent is not MarkupElementIntermediateNode { TagName: "form" })
                {
                    node.Diagnostics.Add(ComponentDiagnosticFactory.CreateFormName_NotAForm(node.Source));
                    reference.Replace(RewriteForErrorRecovery(node, parent));
                    continue;
                }

                if (!parent.Children.Any(c => c is HtmlAttributeIntermediateNode { AttributeName: "@onsubmit" }))
                {
                    node.Diagnostics.Add(ComponentDiagnosticFactory.CreateFormName_MissingOnSubmit(node.Source));
                    reference.Replace(RewriteForErrorRecovery(node, parent));
                    continue;
                }

                reference.Replace(Rewrite(node));
            }
        }

        static IntermediateNode Rewrite(TagHelperDirectiveAttributeIntermediateNode node)
        {
            return RewriteCore(node, new FormNameIntermediateNode
            {
                Source = node.Source
            });
        }

        static IntermediateNode RewriteForErrorRecovery(TagHelperDirectiveAttributeIntermediateNode node, IntermediateNode parent)
        {
            if (parent is ComponentIntermediateNode)
            {
                return RewriteCore(node, new ComponentAttributeIntermediateNode
                {
                    AttributeName = "@formname",
                    Source = node.Source,
                });
            }

            // If not a component, we can emit FormName node normally, codegen will handle it.
            return Rewrite(node);
        }

        static IntermediateNode RewriteCore(TagHelperDirectiveAttributeIntermediateNode node, IntermediateNode replacement)
        {
            replacement.Children.AddRange(node.Children);
            replacement.Diagnostics.AddRange(node.Diagnostics);
            return replacement;
        }
    }
}
