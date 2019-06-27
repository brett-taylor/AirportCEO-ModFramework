using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Buildings.Interfaces
{
    public interface IACMFPlaceableItem
    {
        string IObjectNameKey { get; }
        string IObjectName { get; }
        string IObjectDescription { get; }
        Enums.ThreeStepScale IObjectSize { get; }
        Enums.ObjectType IObjectType { get; }
        float IObjectCost { get; }
        float IOperationsCost { get; }
        float IConditionReductionRate { get; }
        float ICleanlinessReductionRate { get; }
        Enums.PlacementType IPlacementType { get; }
        Vector2 IObjectGridSize { get; }
        float ISnapSize { get; }
        Vector2 ISnapOffset { get; }
        bool IAffectsPassengerGrid { get; }
        bool IAffectsAreas { get; }
        bool IAffectsRoadNodes { get; }
        bool IShouldNotConstruct { get; }
        int IConstructionEnergyRequired { get; }
        int INbrOfContractorsPossible { get; }
        bool IHasOverlaySprite { get; }
        bool IIsClickable { get; }
        bool IIsNotLeftClickable { get; }
        bool IIsColorable { get; }
        Enums.QualityType IObjectQuality { get; }
        Enums.ItemType ItemTypeEnum { get; }
        Enums.ItemPlacementArea IItemPlacementArea { get; }
        Enums.GenericZoneType[] IAllowedGenericZones { get; }
        Enums.SpecificZoneType[] IAllowedSpecificZones { get; }
        Enums.RoomType[] IAllowedRooms { get; }
        bool IMustBeWithinGenericZone { get; }
        bool IMustBeWithinSpecificZone { get; }
        bool IMustBeWithinRoom { get; }
    }
}
