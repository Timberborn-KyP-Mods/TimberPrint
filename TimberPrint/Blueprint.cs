using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.Coordinates;
using UnityEngine;

namespace TimberPrint;

public class Blueprint
{
    public Guid Guid { get; }

    public string Name { get; }
    
    public BlueprintItem[] BlueprintItems { get; }

    public Blueprint(string name, BlueprintItem[] blueprintItems)
    {
        Guid = Guid.NewGuid();
        Name = name;
        BlueprintItems = blueprintItems;
    }
    
    public Blueprint(string blueprintString)
    {
        var items = new List<BlueprintItem>();
        
        foreach (var blueprintItemString in blueprintString.Split('|'))
        {
            var values = blueprintItemString.Split(';');

            var coords = values[1].Split(',');
            
            Enum.TryParse(values[2], out Orientation orientation);
            
            items.Add(new BlueprintItem(
                values[0],
                new Vector3Int(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])),
                orientation,
                values[3] == "1" ? FlipMode.Flipped : FlipMode.Unflipped
                ));
        }

        BlueprintItems = items.ToArray();
        Name = "test";
    }

    public string ConvertToString()
    {
        var stringBuilder = new StringBuilder();
        
        foreach (var blueprintItem in BlueprintItems)
        {
            stringBuilder.AppendJoin(';', new List<string>()
            {
                blueprintItem.TemplateName,
                $"{blueprintItem.Placement.Coordinates.x},{blueprintItem.Placement.Coordinates.y},{blueprintItem.Placement.Coordinates.z}",
                blueprintItem.Placement.Orientation.ToString(),
                blueprintItem.Placement.FlipMode.IsFlipped ? "1" : "0"
            });
            stringBuilder.Append("|");
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        return stringBuilder.ToString();
    }
}