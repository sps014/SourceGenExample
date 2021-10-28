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
