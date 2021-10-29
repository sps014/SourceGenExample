# SourceGenExample

Metaprogramming in C#.<br>
Generate Partial method code at compile time on the fly.
 
 Sample shows how to generate code for scenario given below
```cs
new JSAsm().Create();
partial class JSAsm
{
	 [JSMethod]
	 public partial void Create();
}
```

Traversing Syntax Tree to generate compile time injectable code
```cs

   // using the context, get a list of syntax trees in the users compilation
   var syntaxTrees = context.Compilation.SyntaxTrees.Where(t => t.GetText().ToString().Contains("[JSMethod"));
   sourceBuilder.AppendLine(@"using System;");
   // add the filepath of each tree to the class we're building
   foreach (SyntaxTree tree in syntaxTrees)
   {
       var v=tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
   }
```
