using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Framework;
using UnityEngine;

public partial class DebugOptions
{
    private void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public static void AssertNotNull(object value, string message = null, MonoBehaviour instance = null)
    {
        if (!EqualityComparer<object>.Default.Equals(value, null))
        {
            return;
        }

        message = message != null ? string.Format("NotNullAssert Failed: {0}", message) : "Assert Failed";

        DebugUtil.LogError(message, instance);

        if (instance != null)
        {
            instance.enabled = false;
        }

        throw new NullReferenceException(message);
    }
}
public static class DebugReflection
{
    public static void SetPropertyValue(object obj, PropertyInfo p, object value)
    {
#if NETFX_CORE
			p.SetValue(obj, value, null);
#else
        p.GetSetMethod().Invoke(obj, new[] { value });
#endif
    }

    public static object GetPropertyValue(object obj, PropertyInfo p)
    {
#if NETFX_CORE
			return p.GetValue(obj, null);
#else
        return p.GetGetMethod().Invoke(obj, null);
#endif
    }

    public static T GetAttribute<T>(MemberInfo t) where T : Attribute
    {
#if !NETFX_CORE
        return Attribute.GetCustomAttribute(t, typeof(T)) as T;
#else
			return t.GetCustomAttribute(typeof (T), true) as T;
#endif
    }

#if NETFX_CORE

		public static T GetAttribute<T>(Type t) where T : Attribute
		{
			
			return GetAttribute<T>(t.GetTypeInfo());

		}

#endif
}
public class OptionDefinition
{
    private OptionDefinition(string name, string category, int sortPriority)
    {
        Name = name;
        Category = category;
        SortPriority = sortPriority;
    }

    public OptionDefinition(string name, string category, int sortPriority, MethodReference method)
        : this(name, category, sortPriority)
    {
        Method = method;
    }

    public OptionDefinition(string name, string category, int sortPriority, PropertyReference property)
        : this(name, category, sortPriority)
    {
        Property = property;
    }

    public string Name { get; private set; }
    public string Category { get; private set; }
    public int SortPriority { get; private set; }
    public MethodReference Method { get; private set; }
    public PropertyReference Property { get; private set; }
}
public class MethodReference
{
    private MethodInfo _method;
    private object _target;

    public MethodReference(object target, MethodInfo method)
    {
        DebugOptions.AssertNotNull(target);

        _target = target;
        _method = method;
    }

    public string MethodName
    {
        get { return _method.Name; }
    }

    public object Invoke(object[] parameters)
    {
        return _method.Invoke(_target, parameters);
    }
}
public class PropertyReference
{
    private readonly PropertyInfo _property;
    private readonly object _target;

    public PropertyReference(object target, PropertyInfo property)
    {
        DebugOptions.AssertNotNull(target);

        _target = target;
        _property = property;
    }

    public string PropertyName
    {
        get { return _property.Name; }
    }

    public Type PropertyType
    {
        get { return _property.PropertyType; }
    }

    public bool CanRead
    {
        get
        {
#if NETFX_CORE
				return _property.GetMethod != null && _property.GetMethod.IsPublic;
#else
            return _property.GetGetMethod() != null;
#endif
        }
    }

    public bool CanWrite
    {
        get
        {
#if NETFX_CORE
			return _property.SetMethod != null && _property.SetMethod.IsPublic;
#else
            return _property.GetSetMethod() != null;
#endif
        }
    }

    public object GetValue()
    {
        if (_property.CanRead)
        {
            return DebugReflection.GetPropertyValue(_target, _property);
        }

        return null;
    }

    public void SetValue(object value)
    {
        if (_property.CanWrite)
        {
            DebugReflection.SetPropertyValue(_target, _property, value);
        }
        else
        {
            throw new InvalidOperationException("Can not write to property");
        }
    }

    public T GetAttribute<T>() where T : Attribute
    {
        var attributes = _property.GetCustomAttributes(typeof(T), true).FirstOrDefault();

        return attributes as T;
    }
}