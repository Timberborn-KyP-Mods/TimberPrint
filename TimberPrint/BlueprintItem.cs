using Timberborn.Coordinates;
using UnityEngine;

namespace TimberPrint;

public class BlueprintItem(string templateName, Vector3Int coordinates, Orientation orientation, FlipMode flipMode)
{
    public string TemplateName { get; } = templateName;

    public Placement Placement { get; } = new(coordinates, orientation, flipMode);
}