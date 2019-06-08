﻿using ACMF.ModHelper.EnumPatcher;
using System;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Procurments
{
    public abstract class ProcurmentTemplate : PatchableClass
    {
        public abstract string Title { get; }
        public abstract Enums.ProcureableProductCategory Category { get; }
        public abstract Enums.ProcureableProductSubCategory SubCategory { get; }
        public abstract string Description { get; }
        public abstract string Requirement { get; }
        public abstract PrerequisiteContainer[] Prequisite { get; }
        public abstract ProcurementController.Prerequisite[] PrerequisiteForDisplay { get; }
        public abstract float FixedCost { get; }
        public abstract float OperatingCost { get; }
        public abstract TimeSpan DeliveryTime { get; }
        public abstract bool IsQuantifiable { get; }
        public abstract bool IsPhysicalProduct { get; }
        public abstract Sprite Sprite { get; }

        public Enums.ProcureableProductType Type { get; private set; }

        public override void Patch()
        {
            Utilities.Logger.Print($"Added ProcurmentTemplate {Title}");
            Type = EnumCache<Enums.ProcureableProductType>.Instance.Patch(Title);
            ProcurmentManager.ProcureableProducts.Add(Type, this);
        }

        public abstract void SpawnProcureable();

        internal ProcureableProduct CreateProcureableProduct()
        {
            return new ProcureableProduct
            {
                type = Type,
                title = Title,
                category = Category,
                subCategory = SubCategory,
                description = Description,
                fixedCost = FixedCost,
                operatingCost = OperatingCost,
                deliveryTime = DeliveryTime,
                isQuantifiable = IsQuantifiable,
                isPhysicalProduct = IsPhysicalProduct,
                prerequisiteForDisplay = PrerequisiteForDisplay,
                prerequisite = Prequisite
            };
        }
    }
}
