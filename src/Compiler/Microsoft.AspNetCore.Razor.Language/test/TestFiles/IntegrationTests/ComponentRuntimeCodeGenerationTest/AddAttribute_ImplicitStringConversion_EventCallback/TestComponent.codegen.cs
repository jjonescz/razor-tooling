﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.TestComponent.TypeInference.CreateMyComponent_0(__builder, 0, 1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                   true

#line default
#line hidden
#nullable disable
            , 2, "str", 3, 
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
                       () => { }

#line default
#line hidden
#nullable disable
            , 4, 
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
                     c

#line default
#line hidden
#nullable disable
            , 5, 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                c

#line default
#line hidden
#nullable disable
            , 6, global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => c = __value, c)));
        }
        #pragma warning restore 1998
#nullable restore
#line 7 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    private MyClass<string> c = new();

#line default
#line hidden
#nullable disable
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateMyComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Boolean __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.Delegate __arg2, int __seq3, global::System.Object __arg3, int __seq4, global::Test.MyClass<T> __arg4, int __seq5, global::Microsoft.AspNetCore.Components.EventCallback<global::Test.MyClass<T>> __arg5)
        {
        __builder.OpenComponent<global::Test.MyComponent<T>>(seq);
        __builder.AddAttribute(__seq0, "BoolParameter", __arg0);
        __builder.AddAttribute(__seq1, "StringParameter", __arg1);
        __builder.AddAttribute(__seq2, "DelegateParameter", __arg2);
        __builder.AddAttribute(__seq3, "ObjectParameter", __arg3);
        __builder.AddAttribute(__seq4, "MyParameter", __arg4);
        __builder.AddAttribute(__seq5, "MyParameterChanged", __arg5);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
