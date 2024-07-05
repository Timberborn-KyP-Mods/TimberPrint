using System;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.SettingsSystemUI;
using Timberborn.SingletonSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint;

public class BluePrintSharing : IPanelController, ILoadableSingleton, IInputProcessor
{
    private readonly PanelStack _panelStack;

    private readonly BlueprintService _blueprintService;

    private readonly DialogBoxShower _dialogBoxShower;

    private readonly InputService _inputService;

    private TextField _input = null!;

    private readonly VisualElementLoader _visualElementLoader;

    private readonly string _initialSettlementName = "Paste your blueprint code";

    private VisualElement _root = null!;

    public BluePrintSharing(PanelStack panelStack, DialogBoxShower dialogBoxShower, VisualElementLoader visualElementLoader, BlueprintService blueprintService, InputService inputService)
    {
        _panelStack = panelStack;
        _dialogBoxShower = dialogBoxShower;
        _visualElementLoader = visualElementLoader;
        _blueprintService = blueprintService;
        _inputService = inputService;
    }

    public VisualElement GetPanel()
    {
        return _root;
    }

    public bool OnUIConfirmed()
    {
        Confirmed();
        return true;
    }
    
    public void Confirmed()
    {
        string text = _input.text;
        if (!string.IsNullOrEmpty(text))
        {
            try
            {
                _blueprintService.AddBlueprint(BlueprintCompressor.DecodeBlueprintString(text));
                _panelStack.Pop(this);
            }
            catch (Exception e)
            {
                var dialogShower = _dialogBoxShower.Create();
                dialogShower.AddContent(new Label("Something went wrong loading the Blueprint data, are you sure you copied it correctly?")
                {
                    style = { color = Color.white}
                });
                dialogShower.Show();
                Debug.LogError(e);
            }
        }
    }
    
    public void OnUICancelled()
    {
        _panelStack.Pop(this);
    }

    private void Show()
    {
        if(_blueprintService.TryLoadBlueprint(out var blueprint))
        {
            _input.text = BlueprintCompressor.CompressToBase64(blueprint.Blueprint);
        } 
        else
        {
            _input.text = _initialSettlementName;
        }
        
        _panelStack.HideAndPushOverlay(this);
    }

    public void Load()
    {
        _root = _visualElementLoader.LoadVisualElement("Game/SettlementNameBox");
        _input = _root.Q<TextField>("Input");
        _input.style.maxWidth = 600;
        
        _root.Q<Button>("ResetStartLocation").ToggleDisplayStyle(false);

        _root.Q<Label>().text = "This is the blueprint code, you can share it with others!";
        
        _root.Q<Button>("RelocateButton").text = "Cancel";
        _root.Q<Button>("RelocateButton").clicked += OnUICancelled;
        
        _root.Q<Button>("ConfirmButton").text = "Load blueprint";
        _root.Q<Button>("ConfirmButton").clicked += Confirmed;
        
        _inputService.AddInputProcessor(this);
    }

    public bool ProcessInput()
    {
        if(_inputService.IsKeyDown("Blueprint.OpenDialog"))
        {
            Show();
        }
        
        return false;
    }
}
