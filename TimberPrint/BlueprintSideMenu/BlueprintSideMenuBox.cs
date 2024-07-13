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

public class BlueprintSideMenuBox(
	VisualElementLoader visualElementLoader,
	UILayout uiLayout,
	PanelStack panelStack,
	CameraHorizontalShifter cameraHorizontalShifter,
	InputService inputService,
	EventBus eventBus,
	BlueprintSideMenuTabController blueprintSideMenuTabController,
	IHideableByBatchControl hideableByBatchControl)
	: ILoadableSingleton, IPanelController, IInputProcessor, IPostLoadableSingleton
{
	private static readonly string ToggleBatchControlBoxKey = "ToggleBatchControlBox";

	private static readonly float CameraOffset = -0.2f;

	private VisualElement _root = null!;

	public void Load()
	{
		_root = visualElementLoader.LoadVisualElement("Game/BatchControl/BatchControlBox");
		
		blueprintSideMenuTabController.Initialize(_root);

		_root.Q<Button>("CancelButton").RegisterCallback<ClickEvent>(delegate
		{
			Close();
		});
		_root.Q<Button>("SettlementButton").RegisterCallback<ClickEvent>(delegate
		{
			Close();
		});
		
		_root.Q<Dropdown>("DistrictDropdown").ToggleDisplayStyle(false);
		
		eventBus.Register(this);
	}

	public VisualElement GetPanel()
	{
		uiLayout.HideLeftAndCenterItems();
		hideableByBatchControl.Hide();
		cameraHorizontalShifter.EnableHorizontalCameraShift(CameraOffset);
		blueprintSideMenuTabController.UpdateEntities();
		inputService.AddInputProcessor(this);
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
		if (inputService.IsKeyDown(ToggleBatchControlBoxKey))
		{
			Close();
			return true;
		}
		return false;
	}

	public void OpenBatchControlBox()
	{
		OpenTab(blueprintSideMenuTabController.LastOpenedTabIndex);
	}

	public void OpenTab(int index)
	{
		if (blueprintSideMenuTabController.CurrentTab != null)
		{
			blueprintSideMenuTabController.ShowTab(index);
			return;
		}
		panelStack.HideAndPushWithoutPause(this);
		blueprintSideMenuTabController.ShowTab(index);
		eventBus.Post(new BatchControlBoxShownEvent());
	}

	private void Close()
	{
		blueprintSideMenuTabController.Clear();
		uiLayout.ShowLeftAndCenterItems();
		hideableByBatchControl.Show();
		panelStack.Pop(this);
		cameraHorizontalShifter.DisableCameraShift();
		inputService.RemoveInputProcessor(this);
		eventBus.Post(new BatchControlBoxHiddenEvent());
	}

	public void PostLoad()
	{
		var blueprintButton = visualElementLoader.LoadVisualElement("OpenBlueprintTab");
		blueprintButton.Q<NineSliceButton>("OpenBlueprint").clicked += OpenBatchControlBox;
		uiLayout.AddTopLeft(blueprintButton, 100);
	}
}