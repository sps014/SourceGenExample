
[AttributeUsage(AttributeTargets.Method)]
public class JSMethodAttribute : Attribute
{
    public JSMethodAttribute(Type type=null)
    {
        Type = type;
    }

    public Type Type { get; }
}