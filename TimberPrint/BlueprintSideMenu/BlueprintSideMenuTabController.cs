using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Timberborn.AssetSystem;
using Timberborn.BatchControl;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.SingletonSystem;
using Timberborn.TooltipSystem;
using TimberPrint.BlueprintControl;
using TimberPrint.BlueprintSharing;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintSideMenu;

public class BlueprintSideMenuTabController: IUpdatableSingleton, ILateUpdatableSingleton
{
	private static readonly string SpriteDirectory = "Sprites/BatchControl";

	private static readonly string ActiveButtonClass = "batch-control-panel__tab-button--active";

	private readonly VisualElementLoader _visualElementLoader;

	private readonly ITooltipRegistrar _tooltipRegistrar;

	private readonly IAssetLoader _assetLoader;

	private readonly ILoc _loc;

	private readonly EntityRegistry _entityRegistry;

	private readonly BatchControlBoxCurrentTab _batchControlBoxCurrentTab;

	private readonly Dictionary<BatchControlTab, VisualElement> _tabs = new();

	private readonly List<EntityComponent> _entities = new();

	private readonly BlueprintControlTab _blueprintControlTab;

	private readonly BlueprintSharingTab _blueprintSharingTab;

	private VisualElement _tabButtons;

	private VisualElement _content;

	private VisualElement _middleRow;

	private Label _header;

	private VisualElement _root;

	public BatchControlTab CurrentTab { get; private set; }

	public int LastOpenedTabIndex { get; private set; }

	public IEnumerable<BatchControlTab> Tabs => _tabs.Keys;

	public BlueprintSideMenuTabController(VisualElementLoader visualElementLoader, ITooltipRegistrar tooltipRegistrar, IAssetLoader assetLoader, ILoc loc, EntityRegistry entityRegistry, EntityBadgeService entityBadgeService, BatchControlBoxCurrentTab batchControlBoxCurrentTab, BlueprintControlTab blueprintControlTab, BlueprintSharingTab blueprintSharingTab)
	{
		_visualElementLoader = visualElementLoader;
		_tooltipRegistrar = tooltipRegistrar;
		_assetLoader = assetLoader;
		_loc = loc;
		_entityRegistry = entityRegistry;
		_batchControlBoxCurrentTab = batchControlBoxCurrentTab;
		_blueprintControlTab = blueprintControlTab;
		_blueprintSharingTab = blueprintSharingTab;
	}

	public void Initialize(VisualElement root)
	{
		_tabButtons = root.Q<VisualElement>("TabButtons");
		_content = root.Q<VisualElement>("Content");
		_middleRow = root.Q<VisualElement>("MiddleRow");
		_header = root.Q<Label>("Header");
		AddTabs();

		_root = root;
	}

	public void UpdateEntities()
	{
		_entities.AddRange(_entityRegistry.Entities);
	}

	public int GetTabIndex(BatchControlTab batchControlTab)
	{
		return _tabs.Keys.IndexOf(batchControlTab);
	}

	public void ShowTab(int index)
	{
		BatchControlTab batchControlTab = _tabs.Keys.ElementAt(index);
		if (batchControlTab != CurrentTab)
		{
			SetNewTab(batchControlTab);
			UpdateActiveButtonClass(index);
			LastOpenedTabIndex = index;
			CurrentTab.UpdateRowsVisibility();
		}
	}

	public void UpdateSingleton()
	{
		BatchControlTab currentTab = CurrentTab;
		if (currentTab != null && currentTab.IsDirty)
		{
			Refresh();
		}
	}

	public void LateUpdateSingleton()
	{
		CurrentTab?.UpdateContent();
	}

	public void Clear()
	{
		foreach (BatchControlTab item in _tabs.Keys.ToList())
		{
			item.Clear();
			_tabs[item] = null;
		}
		CurrentTab?.HideTab();
		_content.Clear();
		CurrentTab = null;
		_batchControlBoxCurrentTab.CurrentTab = null;
		_entities.Clear();
	}

	private void AddTabs()
	{
		_tabs.Add(_blueprintControlTab, null);
		AddTabButton(_blueprintControlTab, 0);		
		
		_tabs.Add(_blueprintSharingTab, null);
		AddTabButton(_blueprintSharingTab, 1);
	}

	private void AddTabButton(BatchControlTab batchControlTab, int tabIndex)
	{
		string elementName = "Game/BatchControl/BatchControlTabButton";
		VisualElement visualElement = _visualElementLoader.LoadVisualElement(elementName);
		Button button = visualElement.Q<Button>("BatchControlTabButton");
		button.RegisterCallback<ClickEvent>(delegate
		{
			ShowTab(tabIndex);
		});
		_tooltipRegistrar.Register(button, _loc.T(batchControlTab.TabNameLocKey));
		string path = Path.Combine(SpriteDirectory, batchControlTab.TabImage);
		Sprite sprite = _assetLoader.Load<Sprite>(path);
		visualElement.Q<Image>("BatchControlTabImage").sprite = sprite;
		_tabButtons.Add(visualElement);
	}

	private void SetNewTab(BatchControlTab batchControlTab)
	{
		CurrentTab?.HideTab();
		_middleRow.ToggleDisplayStyle(batchControlTab.MiddleRowVisible);
		_header.text = _loc.T(batchControlTab.TabNameLocKey);
		_content.Clear();
		_content.Add(GetTabElement(batchControlTab));
		CurrentTab?.ShowTab();
		
		UxmlExtractor.ExtractToConsole(_root);
	}

	private void UpdateActiveButtonClass(int index)
	{
		foreach (VisualElement item in _tabButtons.Children())
		{
			item.RemoveFromClassList(ActiveButtonClass);
		}
		_tabButtons[index].AddToClassList(ActiveButtonClass);
	}

	private VisualElement GetTabElement(BatchControlTab batchControlTab)
	{
		VisualElement visualElement = _tabs[batchControlTab] ?? batchControlTab.GetContent(_entities.Where((EntityComponent entity) => entity));
		_tabs[batchControlTab] = visualElement;
		CurrentTab = batchControlTab;
		_batchControlBoxCurrentTab.CurrentTab = batchControlTab;
		return visualElement;
	}

	private void Refresh()
	{
		CurrentTab.Clear();
		_tabs[CurrentTab] = null;
		_content.Clear();
		_content.Add(GetTabElement(CurrentTab));
		CurrentTab.UpdateRowsVisibility();
		CurrentTab.IsDirty = false;
	}

}