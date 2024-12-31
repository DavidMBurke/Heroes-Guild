using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Random methods that could be used in various places
/// </summary>
public static class Methods
{
    /// <summary>
    /// Shuffle the order of a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int RandomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[RandomIndex];
            list[RandomIndex] = temp;
            
        }
    }

    /// <summary>
    /// Logs all fields of a given object along with their values.
    /// </summary>
    /// <param name="obj">The object to log fields for.</param>
    public static void LogAllFields(object obj)
    {
        if (obj == null)
        {
            Debug.Log("Null object provided.");
            return;
        }

        Type type = obj.GetType();
        Debug.Log($"Class: {type.Name}");

        // Get all fields in the class (public and private)
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(obj);
            Debug.Log($"Field: {field.Name}  Value: {value}");
        }

        // Get all properties in the class (public only by default)
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanRead)
            {
                object value = property.GetValue(obj);
                Debug.Log($"Property: {property.Name}  Value: {value}");
            }
        }
    }
}
