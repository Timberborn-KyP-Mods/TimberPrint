using System;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.SettingsSystemUI;
using Timberborn.SingletonSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint;

public class BluePrintSharing(
    PanelStack panelStack,
    DialogBoxShower dialogBoxShower,
    VisualElementLoader visualElementLoader,
    BlueprintService blueprintService,
    InputService inputService)
    : IPanelController, ILoadableSingleton, IInputProcessor
{
    private TextField _input = null!;

    private readonly string _initialSettlementName = "Paste your blueprint code";

    private VisualElement _root = null!;

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
                blueprintService.AddBlueprint(BlueprintCompressor.DecodeBlueprintString(text));
                panelStack.Pop(this);
            }
            catch (Exception e)
            {
                var dialogShower = dialogBoxShower.Create();
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
        panelStack.Pop(this);
    }

    private void Show()
    {
        if(blueprintService.TryLoadBlueprint(out var blueprint))
        {
            _input.text = BlueprintCompressor.CompressToBase64(blueprint.Blueprint);
        } 
        else
        {
            _input.text = _initialSettlementName;
        }
        
        panelStack.HideAndPushOverlay(this);
    }

    public void Load()
    {
        _root = visualElementLoader.LoadVisualElement("Game/SettlementNameBox");
        _input = _root.Q<TextField>("Input");
        _input.style.maxWidth = 600;
        
        _root.Q<Button>("ResetStartLocation").ToggleDisplayStyle(false);

        _root.Q<Label>().text = "This is the blueprint code, you can share it with others!";
        
        _root.Q<Button>("RelocateButton").text = "Cancel";
        _root.Q<Button>("RelocateButton").clicked += OnUICancelled;
        
        _root.Q<Button>("ConfirmButton").text = "Load blueprint";
        _root.Q<Button>("ConfirmButton").clicked += Confirmed;
        
        inputService.AddInputProcessor(this);
    }

    public bool ProcessInput()
    {
        if(inputService.IsKeyDown("Blueprint.OpenDialog"))
        {
            Show();
        }
        
        return false;
    }
}
