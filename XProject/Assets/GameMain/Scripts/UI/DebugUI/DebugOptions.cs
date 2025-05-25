
using System;

public partial class DebugOptions
{
    private static readonly DebugOptions _current = new DebugOptions();

    public static DebugOptions Current
    {
        get { return _current; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public readonly string Name;

        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class SortAttribute : Attribute
    {
        public readonly int SortPriority;

        public SortAttribute(int priority)
        {
            SortPriority = priority;
        }
    }

}