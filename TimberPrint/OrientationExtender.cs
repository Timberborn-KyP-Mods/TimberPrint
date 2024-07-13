using Timberborn.Coordinates;

namespace TimberPrint;

public static class OrientationExtender
{
    public static Orientation Rotate(this Orientation orientation, Orientation additive)
    {
        return (Orientation)(((int)orientation + (int)additive) % 4);
    }
}