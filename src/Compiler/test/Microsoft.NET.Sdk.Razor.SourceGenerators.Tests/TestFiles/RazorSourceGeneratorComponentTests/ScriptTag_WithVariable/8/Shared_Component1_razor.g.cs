﻿#pragma checksum "Shared/Component1.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b72b3eb93cc4a906714929a4849d9f7aac2ef62a"
// <auto-generated/>
#pragma warning disable 1591
namespace MyApp.Shared
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
    #nullable restore
    public partial class Component1 : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "Component:\r\n");
#nullable restore
#line 2 "Shared/Component1.razor"
   var msg = "What's up"; 

#line default
#line hidden
#nullable disable
            __builder.OpenElement(1, "script");
            __builder.AddMarkupContent(2, "console.log(\'");
#nullable restore
#line (3,23)-(3,26) 24 "Shared/Component1.razor"
__builder.AddContent(3, msg);

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(4, "\');");
            __builder.CloseElement();
            __builder.AddMarkupContent(5, "\r\n");
            __builder.OpenElement(6, "div");
            __builder.AddContent(7, "console.log(\'");
#nullable restore
#line (4,20)-(4,23) 24 "Shared/Component1.razor"
__builder.AddContent(8, msg);

#line default
#line hidden
#nullable disable
            __builder.AddContent(9, "\');");
            __builder.CloseElement();
            __builder.AddMarkupContent(10, "\r\n");
            __builder.AddMarkupContent(11, "<script>console.log(\'No variable\');</script>\r\n");
            __builder.AddMarkupContent(12, "<div>console.log(\'No variable\');</div>\r\n");
            __builder.OpenElement(13, "script");
            __builder.AddMarkupContent(14, "\r\n    console.log(\'");
#nullable restore
#line (8,19)-(8,22) 25 "Shared/Component1.razor"
__builder.AddContent(15, msg);

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(16, "\');\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n");
            __builder.OpenElement(18, "div");
            __builder.AddMarkupContent(19, "\r\n    console.log(\'");
#nullable restore
#line (11,19)-(11,22) 25 "Shared/Component1.razor"
__builder.AddContent(20, msg);

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(21, "\');\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n");
            __builder.AddMarkupContent(23, "<script>\r\n    console.log(\'No variable\');\r\n</script>\r\n");
            __builder.AddMarkupContent(24, "<div>\r\n    console.log(\'No variable\');\r\n</div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
