// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

public partial class RazorSourceGeneratorTests
{
    public class TagHelpers
    {
        [Fact]
        public async Task CustomTagHelper()
        {
            // Arrange
            var project = CreateTestProject(new()
            {
                ["Views/Home/Index.cshtml"] = """
                    @addTagHelper *, TestProject

                    <email>
                        custom tag helper
                        <email>nested tag helper</email>
                    </email>
                    """
            }, new()
            {
                ["EmailTagHelper.cs"] = """
                    using Microsoft.AspNetCore.Razor.TagHelpers;

                    public class EmailTagHelper : TagHelper
                    {
                        public override void Process(TagHelperContext context, TagHelperOutput output)
                        {
                            output.TagName = "a";
                        }
                    }
                    """
            });
            var compilation = await project.GetCompilationAsync();
            var driver = await GetDriverAsync(project);

            // Act
            var result = RunGenerator(compilation!, ref driver, out compilation);

            // Assert
            Assert.Contains("EmailTagHelper", result.GeneratedSources.Single().SourceText.ToString());
            result.VerifyOutputsMatchBaseline();

            //

            Assembly assembly;
            using (var peStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(peStream);
                Assert.True(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));
                assembly = Assembly.Load(peStream.ToArray());
            }

            var pageType = assembly.GetType("AspNetCoreGeneratedDocument.Views_Home_Index");
            var page = (RazorPage)Activator.CreateInstance(pageType);

            var writer = new StringWriter();
            var httpContext = new DefaultHttpContext();
            var services = new ServiceCollection();
            services.AddRazorPages();
            httpContext.RequestServices = services.BuildServiceProvider();
            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor());
            var viewMock = new Mock<IView>();
            var viewContext = new ViewContext(
                actionContext,
                viewMock.Object,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
                Mock.Of<ITempDataDictionary>(),
                writer,
                new HtmlHelperOptions());

            page.ViewContext = viewContext;
            page.HtmlEncoder = HtmlEncoder.Default;

            await page.ExecuteAsync();

            AssertEx.EqualOrDiff("""

                <a>
                    custom tag helper
                    <a>nested tag helper</a>
                </a>
                """, writer.ToString());
        }

        [Fact]
        public async Task ViewComponent()
        {
            // Arrange
            var project = CreateTestProject(new()
            {
                ["Views/Home/Index.cshtml"] = """
                    @addTagHelper *, TestProject
                    @{
                        var num = 42;
                    }

                    <vc:test text="Razor" number="@num" flag />
                    """,
            }, new()
            {
                ["TestViewComponent.cs"] = """
                    public class TestViewComponent
                    {
                        public string Invoke(string text, int number, bool flag)
                        {
                            return text;
                        }
                    }
                    """,
            });
            var compilation = await project.GetCompilationAsync();
            var driver = await GetDriverAsync(project);

            // Act
            var result = RunGenerator(compilation!, ref driver);

            // Assert
            Assert.Contains("HtmlTargetElementAttribute(\"vc:test\")", result.GeneratedSources.Single().SourceText.ToString());
            result.VerifyOutputsMatchBaseline();
        }
    }
}
