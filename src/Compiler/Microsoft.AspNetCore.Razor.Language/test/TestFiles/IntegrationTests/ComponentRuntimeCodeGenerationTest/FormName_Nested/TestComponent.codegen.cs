﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "form");
            __builder.AddAttribute(1, "method", "post");
            __builder.AddAttribute(2, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                               () => { }

#line default
#line hidden
#nullable disable
            ));
            string __formName = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>("1");
            __builder.AddNamedEvent("onsubmit", __formName);
            __builder.CloseElement();
            __builder.AddMarkupContent(3, "\r\n");
            __builder.OpenComponent<global::Test.TestComponent>(4);
            __builder.AddAttribute(5, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenElement(6, "form");
                __builder2.AddAttribute(7, "method", "post");
                __builder2.AddAttribute(8, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
                                   () => { }

#line default
#line hidden
#nullable disable
                ));
                string __formName2_0 = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>("2");
                __builder2.AddNamedEvent("onsubmit", __formName2_0);
                __builder2.CloseElement();
                __builder2.AddMarkupContent(9, "\r\n    ");
                __builder2.OpenComponent<global::Test.TestComponent>(10);
                __builder2.AddAttribute(11, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenElement(12, "form");
                    __builder3.AddAttribute(13, "method", "post");
                    __builder3.AddAttribute(14, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 6 "x:\dir\subdir\Test\TestComponent.cshtml"
                                       () => { }

#line default
#line hidden
#nullable disable
                    ));
                    string __formName3_0 = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>("3");
                    __builder3.AddNamedEvent("onsubmit", __formName3_0);
                    __builder3.CloseElement();
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(15, "\r\n    ");
                __builder2.OpenElement(16, "form");
                __builder2.AddAttribute(17, "method", "post");
                __builder2.AddAttribute(18, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 8 "x:\dir\subdir\Test\TestComponent.cshtml"
                                   () => { }

#line default
#line hidden
#nullable disable
                ));
                string __formName2_1 = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>("4");
                __builder2.AddNamedEvent("onsubmit", __formName2_1);
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 10 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    [Parameter] public RenderFragment ChildContent { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
