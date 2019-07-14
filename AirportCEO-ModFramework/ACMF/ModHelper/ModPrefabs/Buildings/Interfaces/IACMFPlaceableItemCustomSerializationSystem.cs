namespace ACMF.ModHelper.ModPrefabs.Buildings.Interfaces
{
    public interface IACMFPlaceableItemCustomSerializationSystem
    {
        PlaceableItemSerializable SerializeItem(PlaceableItem placeableItem);
        bool DeserializeItem(PlaceableItemSerializable placeableItemSerializable);
    }
}
