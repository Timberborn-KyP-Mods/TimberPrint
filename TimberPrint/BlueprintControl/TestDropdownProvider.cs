using System.Collections.Generic;
using Timberborn.DropdownSystem;

namespace TimberPrint.BlueprintControl;

public class TestDropdownProvider : IDropdownProvider
{
    public IReadOnlyList<string> Items => _list.AsReadOnly();

    private readonly List<string> _list = new()
    {
        "Hello world",
        "Zoey",
        "Nala",
    };
    

    private string _selectedValue = "Nala";
    
    public string GetValue()
    {
        return _selectedValue;
    }

    public void SetValue(string value)
    {
        _selectedValue = value;
    }
}