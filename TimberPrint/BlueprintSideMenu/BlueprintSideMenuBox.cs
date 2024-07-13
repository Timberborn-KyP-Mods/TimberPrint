using System;
using System.Linq;
using Timberborn.BatchControl;
using Timberborn.CameraSystem;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using Timberborn.UILayoutSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintSideMenu;

public class BlueprintSideMenuBox : ILoadableSingleton, IPanelController, IInputProcessor, IPostLoadableSingleton
{
	private static readonly string ToggleBatchControlBoxKey = "ToggleBatchControlBox";

	private static readonly float CameraOffset = -0.2f;

	private readonly VisualElementLoader _visualElementLoader;

	private readonly UILayout _uiLayout;

	private readonly PanelStack _panelStack;

	private readonly CameraHorizontalShifter _cameraHorizontalShifter;

	private readonly InputService _inputService;

	private readonly EventBus _eventBus;

	private readonly BlueprintSideMenuTabController _blueprintSideMenuTabController;

	private readonly IHideableByBatchControl _hideableByBatchControl;

	private VisualElement _root = null!;

	public BlueprintSideMenuBox(VisualElementLoader visualElementLoader, UILayout uiLayout, PanelStack panelStack, CameraHorizontalShifter cameraHorizontalShifter, InputService inputService, EventBus eventBus, BlueprintSideMenuTabController blueprintSideMenuTabController, IHideableByBatchControl hideableByBatchControl)
	{
		_visualElementLoader = visualElementLoader;
		_uiLayout = uiLayout;
		_panelStack = panelStack;
		_cameraHorizontalShifter = cameraHorizontalShifter;
		_inputService = inputService;
		_eventBus = eventBus;
		_blueprintSideMenuTabController = blueprintSideMenuTabController;
		_hideableByBatchControl = hideableByBatchControl;
	}
	
	public void Load()
	{
		_root = _visualElementLoader.LoadVisualElement("Game/BatchControl/BatchControlBox");
		
		_blueprintSideMenuTabController.Initialize(_root);

		_root.Q<Button>("CancelButton").RegisterCallback<ClickEvent>(delegate
		{
			Close();
		});
		_root.Q<Button>("SettlementButton").RegisterCallback<ClickEvent>(delegate
		{
			Close();
		});
		
		_root.Q<Dropdown>("DistrictDropdown").ToggleDisplayStyle(false);
		
		_eventBus.Register(this);
	}

	public VisualElement GetPanel()
	{
		_uiLayout.HideLeftAndCenterItems();
		_hideableByBatchControl.Hide();
		_cameraHorizontalShifter.EnableHorizontalCameraShift(CameraOffset);
		_blueprintSideMenuTabController.UpdateEntities();
		_inputService.AddInputProcessor(this);
		return _root;
	}

	public bool OnUIConfirmed()
	{
		return false;
	}

	public void OnUICancelled()
	{
		Close();
	}

	public bool ProcessInput()
	{
		if (_inputService.IsKeyDown(ToggleBatchControlBoxKey))
		{
			Close();
			return true;
		}
		return false;
	}

	public void OpenBatchControlBox()
	{
		OpenTab(_blueprintSideMenuTabController.LastOpenedTabIndex);
	}

	public void OpenTab(int index)
	{
		if (_blueprintSideMenuTabController.CurrentTab != null)
		{
			_blueprintSideMenuTabController.ShowTab(index);
			return;
		}
		_panelStack.HideAndPushWithoutPause(this);
		_blueprintSideMenuTabController.ShowTab(index);
		_eventBus.Post(new BatchControlBoxShownEvent());
	}

	private void Close()
	{
		_blueprintSideMenuTabController.Clear();
		_uiLayout.ShowLeftAndCenterItems();
		_hideableByBatchControl.Show();
		_panelStack.Pop(this);
		_cameraHorizontalShifter.DisableCameraShift();
		_inputService.RemoveInputProcessor(this);
		_eventBus.Post(new BatchControlBoxHiddenEvent());
	}

	public void PostLoad()
	{
		var blueprintButton = _visualElementLoader.LoadVisualElement("OpenBlueprintTab");
		blueprintButton.Q<NineSliceButton>("OpenBlueprint").clicked += OpenBatchControlBox;
		_uiLayout.AddTopLeft(blueprintButton, 100);
	}
}