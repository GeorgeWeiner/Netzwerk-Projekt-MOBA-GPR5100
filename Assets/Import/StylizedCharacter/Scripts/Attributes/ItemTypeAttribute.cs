using System;
using NHance.Assets.Scripts.Enums;

namespace NHance.Assets.Scripts.Attributes
{  
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal sealed class ItemTypeAttribute : Attribute
    {
        public ItemTypeEnum TypeEnum;

        public ItemTypeAttribute(ItemTypeEnum typeEnum)
        {
            TypeEnum = typeEnum;
        }
    }
}