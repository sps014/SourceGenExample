using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorSamples
{
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // begin creating the source we'll inject into the users compilation
            var sourceBuilder = new StringBuilder();

            // using the context, get a list of syntax trees in the users compilation
            var syntaxTrees = context.Compilation.SyntaxTrees.Where(t => t.GetText().ToString().Contains("[JSMethod"));

            sourceBuilder.AppendLine(@"using System;");
            // add the filepath of each tree to the class we're building
            foreach (SyntaxTree tree in syntaxTrees)
            {
                var v=tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
                var arr = v.Where(g => g.GetText().ToString().Contains("[JSMethod"));
                foreach (var item in arr)
                {
                    var str = item.Identifier.ValueText;
                    sourceBuilder.AppendLine("partial class " + str + "{");

                    var methods = item.Members.OfType<MethodDeclarationSyntax>()
                        .Where(x => x.AttributeLists.Count(l=>l.GetText().ToString().Contains("[JSMethod"))>0);

                    foreach (var method in methods)
                    {
                        var access = method.Modifiers.FirstOrDefault(g=>g.ValueText!="partial");
                        sourceBuilder.AppendLine($"\r\n\t{access.ValueText} partial {method.ReturnType } {method.Identifier.ValueText}(){{");
                        sourceBuilder.AppendLine("\tConsole.Write(\"OK\");\r\n\t}\r\n");
                    }
                    sourceBuilder.AppendLine("}");

                }
                //sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            }

            // finish creating the source to inject

            // inject the created source into the users compilation
            context.AddSource("SourceGen", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }


        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if(!Debugger.IsAttached)
                //Debugger.Launch();
#endif
        }
    }
}