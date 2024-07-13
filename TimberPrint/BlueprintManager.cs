using System.Collections.Generic;
using Timberborn.SingletonSystem;
using TimberPrint.BlueprintControl;

namespace TimberPrint;

public class BlueprintManager
{
    private readonly EventBus _eventBus;

    public BlueprintManager(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Blueprint? ActiveBlueprint { get; private set; }
    
    public Blueprint[] RecentBlueprints { get; private set; } = new Blueprint[3];

    public List<Blueprint> FavoriteBlueprints { get; set; } = new();

    public void SwitchBlueprint(Blueprint blueprint)
    {
        ActiveBlueprint = blueprint;
        _eventBus.Post(new ActiveBlueprintChanged());
    }

    public bool TryAddFavorite(Blueprint blueprint)
    {
        if (FavoriteBlueprints.Count >= 6)
        {
            return false;
        }
        
        FavoriteBlueprints.Add(blueprint);

        return true;
    }

    public void RemoveFavorite(Blueprint blueprint)
    {
        
    }
}