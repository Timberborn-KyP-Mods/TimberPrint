using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Timberborn.SingletonSystem;
using TimberPrint.BlueprintControl;

namespace TimberPrint;

public class BlueprintRepository(EventBus eventBus)
{
    public IEnumerable<Blueprint> Blueprints => _blueprints.Values;
    
    private readonly Dictionary<Guid, Blueprint> _blueprints = new();

    public bool TryGet(Guid guid, [NotNullWhen(true)] out Blueprint? blueprint)
    {
        return _blueprints.TryGetValue(guid, out blueprint);
    }

    public void Add(Blueprint blueprint)
    {
        _blueprints.Add(blueprint.Guid, blueprint);
        eventBus.Post(new BlueprintRepositoryChangedEvent());
    }

    public void Remove(Guid blueprintGuid)
    {
        _blueprints.Remove(blueprintGuid);
    }
}