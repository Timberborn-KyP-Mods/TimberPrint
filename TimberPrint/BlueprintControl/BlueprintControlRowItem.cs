using TextCopy;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintControl;

public class BlueprintControlRowItem : IBatchControlRowItem
{
    public VisualElement Root { get; }

    private readonly Blueprint _blueprint;

    private readonly BlueprintManager _blueprintManager;

    private readonly BlueprintRepository _blueprintRepository;
    
    private readonly VisualElementLoader _visualElementLoader;
    
    private readonly DropdownItemsSetter _dropdownItemsSetter;

    
    public BlueprintControlRowItem(VisualElement root, Blueprint blueprint, BlueprintManager blueprintManager, BlueprintRepository blueprintRepository, VisualElementLoader visualElementLoader, DropdownItemsSetter dropdownItemsSetter)
    {
        Root = root;
        _blueprint = blueprint;
        _blueprintManager = blueprintManager;
        _blueprintRepository = blueprintRepository;
        _visualElementLoader = visualElementLoader;
        _dropdownItemsSetter = dropdownItemsSetter;
        Root.Q<NineSliceButton>("ShowButton").ToggleDisplayStyle(false);
        Root.Q<NineSliceButton>("ShareButton").clicked += SaveBlueprintToClipboard;
        Root.Q<Button>("SelectButton").clicked += SelectBlueprint;
        Root.Q<NineSliceButton>("FavoriteButton").ToggleDisplayStyle(false);
        Root.Q<NineSliceButton>("DestroyButton").clicked += RemoveBlueprint;

        var dropdown = _visualElementLoader.LoadVisualElement("TestDropdown");
        _dropdownItemsSetter.SetItems(dropdown.Q<Dropdown>("DistrictDropdown"), new TestDropdownProvider());
        Root.Add(dropdown);
    }
    
    private void SaveBlueprintToClipboard()
    {
        ClipboardService.SetText(BlueprintCompressor.CompressToBase64(_blueprint));
    }
    
    private void SelectBlueprint()
    {
        _blueprintManager.SwitchBlueprint(_blueprint);
    }
    
    private void AddBlueprintToFavorite()
    {
        _blueprintManager.TryAddFavorite(_blueprint);
    }
    
    private void RemoveBlueprint()
    {
        _blueprintRepository.Remove(_blueprint.Guid);
    }
}