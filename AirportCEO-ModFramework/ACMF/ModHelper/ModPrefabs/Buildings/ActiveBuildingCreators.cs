using ACMF.ModHelper.ModPrefabs.Buildings.Interfaces;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Buildings
{
    public static class ActiveBuildingCreators
    {
        private static readonly Dictionary<Type, IBuildingCreator> BuildingCreators = new Dictionary<Type, IBuildingCreator>();
        private static readonly Dictionary<Enums.ItemType, IBuildingCreator> ActiveEnums = new Dictionary<Enums.ItemType, IBuildingCreator>(); 

        public static PlaceableItemCreator<T> GetCreator<T>() where T : PlaceableItemCreator<T>
        {
            if (HasCreatorFromType(typeof(T)) && BuildingCreators[typeof(T)] is PlaceableItemCreator<T> result)
                return result;

            Utilities.Logger.Error($"BuildingCreator attempted to get creator from controller for {typeof(T)} which does not exist.");
            return null;
        }

        internal static void Add<T>(IBuildingCreator o, Enums.ItemType itemType) where T : IBuildingCreator
        {
            BuildingCreators.Add(typeof(T), o);
            ActiveEnums.Add(itemType, o);
        }

        internal static IBuildingCreator GetCreatorFromType(Type type)
        {
            return HasCreatorFromType(type) ? BuildingCreators[type] : null;
        }

        internal static bool HasCreatorFromType(Type type)
        {
            return BuildingCreators.ContainsKey(type);
        }

        internal static bool HasCreatorFromEnum(Enums.ItemType itemType)
        {
            return ActiveEnums.ContainsKey(itemType);
        }

        internal static IBuildingCreator GetCreatorFromEnum(Enums.ItemType itemType)
        {
            return HasCreatorFromEnum(itemType) ? ActiveEnums[itemType] : null;
        }
    }
}
