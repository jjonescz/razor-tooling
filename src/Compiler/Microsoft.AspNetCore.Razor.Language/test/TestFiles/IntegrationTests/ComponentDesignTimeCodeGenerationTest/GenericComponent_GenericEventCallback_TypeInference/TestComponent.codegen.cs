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
using Test;

#line default
#line hidden
#nullable disable
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            var __typeInference_CreateMyComponent_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateMyComponent_0(__builder, -1, -1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                   3

#line default
#line hidden
#nullable disable
            , -1, global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::Test.MyEventArgs>(this, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                               x => {}

#line default
#line hidden
#nullable disable
            ));
            #pragma warning disable BL0005
            __typeInference_CreateMyComponent_0.
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
             Item

#line default
#line hidden
#nullable disable
             = default;
            __typeInference_CreateMyComponent_0.
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                      MyEvent

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.MyComponent<>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static global::Test.MyComponent<TItem> CreateMyComponent_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TItem __arg0, int __seq1, global::Microsoft.AspNetCore.Components.EventCallback<global::Test.MyEventArgs> __arg1)
        {
        __builder.OpenComponent<global::Test.MyComponent<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Item", (object)__arg0);
        __builder.AddAttribute(__seq1, "MyEvent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
