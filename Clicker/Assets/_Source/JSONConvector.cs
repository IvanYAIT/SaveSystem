using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONConvector
{
    public static string Convert(string name,object obj)
    {
        return $"\"{name}\":\"{obj.ToString()}\"";
    }
}
