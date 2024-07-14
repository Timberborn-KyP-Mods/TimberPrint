using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlueprintAreaBlockObjectPicker
{
    public delegate void Callback(IEnumerable<BlockObject> blockObjects, Vector3Int start, Vector3Int end, bool selectionStarted, bool selectingArea);

	private readonly AreaSelectionController _areaSelectionController;

	private readonly AreaSelector _areaSelector;

	private readonly BlueprintBlockObjectPicker _blueprintBlockObjectPicker;

	private readonly BlockObjectPickDirection _pickDirection;

	private SelectionStart? _selectionStart;

	internal BlueprintAreaBlockObjectPicker(AreaSelectionController areaSelectionController, AreaSelector areaSelector, BlueprintBlockObjectPicker blueprintBlockObjectPicker, BlockObjectPickDirection pickDirection)
	{
		_areaSelectionController = areaSelectionController;
		_areaSelector = areaSelector;
		_blueprintBlockObjectPicker = blueprintBlockObjectPicker;
		_pickDirection = pickDirection;
	}

	public bool PickBlockObjects<T>(Callback previewCallback, Callback actionCallback, Action showNoneCallback, int height)
	{
		return _areaSelectionController.ProcessInput(delegate(Ray startRay, Ray endRay, bool selectionStarted)
		{
			var (blockObjects2, start2, end2, selectingArea2) = PickBlockObjects<T>(startRay, endRay, height); 
			
			previewCallback(blockObjects2, start2, end2, selectionStarted, selectingArea2);
		}, delegate(Ray startRay, Ray endRay, bool selectionStarted)
		{
			var (blockObjects, start, end, selectingArea) = PickBlockObjects<T>(startRay, endRay, height);
			actionCallback(blockObjects, start, end, selectionStarted, selectingArea);
		}, showNoneCallback);
	}

	private (IEnumerable<BlockObject> blockObjects, Vector3Int start, Vector3Int end, bool selectingArea) PickBlockObjects<T>(Ray startRay, Ray endRay, int height)
	{
		var flag = !startRay.Equals(endRay);
		var selectionStart = GetSelectionStart<T>(startRay, flag);
		if (!selectionStart.HasValue)
		{
			return (blockObjects: Enumerable.Empty<BlockObject>(), start: default, end: default, selectingArea: false);
		}

		var maxHeight = selectionStart.GetValueOrDefault().Coordinates.z + height;
			
		var valueOrDefault = selectionStart.GetValueOrDefault();
		var coordinates = valueOrDefault.Coordinates;
		var vector3Int = (flag ? _areaSelector.GetSelectionEnd(valueOrDefault, endRay) : coordinates);
		return (blockObjects: from blockObject in _blueprintBlockObjectPicker.PickBlockObjects(valueOrDefault, vector3Int, _pickDirection, flag, maxHeight)
			where blockObject.GetComponentFast<T>() != null
			select blockObject, start: coordinates, end: vector3Int, selectingArea: flag);
	}

	private SelectionStart? GetSelectionStart<T>(Ray startRay, bool selectingArea)
	{
		if (!selectingArea)
		{
			_selectionStart = _areaSelector.GetSelectionStart<T>(startRay);
		}
		return _selectionStart;
	}
}