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
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.TestComponent>(0);
            global::Microsoft.AspNetCore.Components.IComponentRenderMode __renderMode = 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                            Microsoft.AspNetCore.Components.Web.RenderMode.Server

#line default
#line hidden
#nullable disable
            ;
            __builder.AddComponentParameter(1, "@rendermode", "Value2");
            __builder.AddComponentRenderMode(__renderMode);
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
