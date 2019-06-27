using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Buildings.Interfaces
{
    public interface IACMFInteractionItemModel : IACMFPlaceableItem
    {
        Enums.InteractionItemGroup IInteractionItemGroup { get; }
        List<InteractionOccupancyData> IInteractionPoints { get; }
    }
}
