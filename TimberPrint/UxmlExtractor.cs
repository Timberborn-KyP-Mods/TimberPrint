using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint;

public static class UxmlExtractor
{
    public static void ExtractToConsole(VisualElement visualElement)
    {
        Debug.LogError("-------- Starting --------");
		
        Extract(visualElement);
		
        Debug.LogError("-------- Ending ----------");
    }
    
    
    private static void Extract(VisualElement visualElement)
    {
        Debug.Log($"<{TypeConverter(visualElement.GetType())} name=\"{visualElement.name}\" class=\"{string.Join(" ", visualElement.classList)}\">");

        foreach (var element in visualElement.Children())
        {
            Extract(element);
        }
		
        Debug.Log($"</{TypeConverter(visualElement.GetType())}>");
    }
	
    private static string TypeConverter(Type type)
    {
        if (type.ToString().StartsWith("Timberborn.CoreUI"))
        {
            return type.ToString().Remove(0, "Timberborn.CoreUI.".Length).Insert(0, "tb:");
        }
        else if (type.ToString().StartsWith("Timberborn.DropdownSystem"))
        {
            return type.ToString();
        }
        else
        {
            return type.ToString().Split('.').Last().Insert(0, "ui:");
        }
    }
}