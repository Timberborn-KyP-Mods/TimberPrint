using System.Diagnostics.CodeAnalysis;

namespace TimberPrint;

public class BlueprintRepository
{
    private Blueprint? _blueprint;

    public bool TryGet([NotNullWhen(true)] out Blueprint? blueprint)
    {
        if (_blueprint != null)
        {
            blueprint = _blueprint;
            return true;
        }
        
        blueprint = null;
        return false;
    }

    public void Add(Blueprint blueprint)
    {
        _blueprint = blueprint;
    }
}