﻿// <auto-generated/>
#pragma warning disable 1591
namespace MyApp.Components
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((global::System.Action)(() => {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
global::System.Object __typeHelper = nameof(MyApp.Components);

#line default
#line hidden
#nullable disable
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            {
                global::__Blazor.MyApp.Components.TestComponent.TypeInference.CreateParentComponent_0_CaptureParameters(
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                            new MyClass<string>()

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg0);
                var __typeInference_CreateParentComponent_0 = global::__Blazor.MyApp.Components.TestComponent.TypeInference.CreateParentComponent_0(__builder, -1, -1, __typeInferenceArg_0___arg0, -1, (__builder2) => {
                    var __typeInference_CreateChildComponent_1 = global::__Blazor.MyApp.Components.TestComponent.TypeInference.CreateChildComponent_1(__builder2, -1, __typeInferenceArg_0___arg0);
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::MyApp.Components.ChildComponent<>);

#line default
#line hidden
#nullable disable
                }
                );
                #pragma warning disable BL0005
                __typeInference_CreateParentComponent_0.
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                 Parameter

#line default
#line hidden
#nullable disable
                 = default;
                #pragma warning restore BL0005
            }
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::MyApp.Components.ParentComponent<>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.MyApp.Components.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static global::MyApp.Components.ParentComponent<T> CreateParentComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::MyApp.MyClass<T> __arg0, int __seq1, Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::MyApp.Components.ParentComponent<T>>(seq);
        __builder.AddAttribute(__seq0, "Parameter", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateParentComponent_0_CaptureParameters<T>(global::MyApp.MyClass<T> __arg0, out global::MyApp.MyClass<T> __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::MyApp.Components.ChildComponent<T> CreateChildComponent_1<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, global::MyApp.MyClass<T> __syntheticArg0)
        {
        __builder.OpenComponent<global::MyApp.Components.ChildComponent<T>>(seq);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
