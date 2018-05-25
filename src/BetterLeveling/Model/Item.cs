using System.Collections.Generic;
using System.Linq;
using BetterLeveling.Model.Enums;
using PoeHUD.Controllers;
using PoeHUD.Hud.AdvancedTooltip;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace BetterLeveling.Model
{
    public class Item
    {
        public ItemTypeParent BaseItemType;
        public string BaseName;
        public string ClassName;
        public bool IsIdentified;
        public bool IsWeapon;
        public int ItemLevel;
        public int LargestLink;
        public NormalInventoryItem Original;
        public string Path;
        public int RequiredLevel;
        public int Sockets;
        public ItemTypeChild SubItemType;
        public ItemRarity Rarity;
        public List<ModValue> Mods;
        public bool IsUpgrading { get; set; } = false;
        public bool IsDrawn { get; set; } = false;

        public Item()
        {
        }

        public Item(NormalInventoryItem item)
        {
            Original = item;
            Path = item.Item.Path;
            BaseItemType baseItemType = BasePlugin.API.GameController.Files.BaseItemTypes.Translate(Path);
            ClassName = baseItemType.ClassName;
            BaseName = baseItemType.BaseName;
            List<string> weaponClass = new List<string>
            {
                "One Hand Mace",
                "Two Hand Mace",
                "One Hand Axe",
                "Two Hand Axe",
                "One Hand Sword",
                "Two Hand Sword",
                "Thrusting One Hand Sword",
                "Bow",
                "Claw",
                "Dagger",
                "Sceptre",
                "Staff",
                "Wand"
            };
            if (item.Item.HasComponent<Base>())
            {
                Base @base = item.Item.GetComponent<Base>();
            }

            if (item.Item.HasComponent<Mods>())
            {
                Mods mods = item.Item.GetComponent<Mods>();
                Rarity = mods.ItemRarity;
                IsIdentified = mods.Identified;
                ItemLevel = mods.ItemLevel;
                RequiredLevel = mods.RequiredLevel;
                if (Rarity != ItemRarity.Normal && IsIdentified && !string.IsNullOrEmpty(item.Item.Path))
                {
                    List<ItemMod> itemMods = mods.ItemMods;
                    Mods = itemMods.Select(it => new ModValue(it, BasePlugin.API.GameController.Files, ItemLevel, baseItemType)).ToList();
                }
            }

            if (item.Item.HasComponent<Sockets>())
            {
                Sockets sockets = item.Item.GetComponent<Sockets>();
                Sockets = sockets.NumberOfSockets;
                LargestLink = sockets.LargestLinkSize;
            }

            if (weaponClass.Any(ClassName.Equals))
                IsWeapon = true;

            if (Path.StartsWith("Metadata/Items/Amulets"))
            {
                BaseItemType = ItemTypeParent.Amulet;
            }
            else if (Path.StartsWith("Metadata/Items/Rings"))
            {
                BaseItemType = ItemTypeParent.Ring;
            }
            else if (Path.StartsWith("Metadata/Items/Belts"))
            {
                BaseItemType = ItemTypeParent.Armour;
                SubItemType = ItemTypeChild.Belt;
            }
            else if (Path.StartsWith("Metadata/Items/Armours"))
            {
                BaseItemType = ItemTypeParent.Armour;
                if (Path.StartsWith("Metadata/Items/Armours/BodyArmours"))
                    SubItemType = ItemTypeChild.BodyArmor;
                else if (Path.StartsWith("Metadata/Items/Armours/Boots"))
                    SubItemType = ItemTypeChild.Boots;
                else if (Path.StartsWith("Metadata/Items/Armours/Gloves"))
                    SubItemType = ItemTypeChild.Gloves;
                else if (Path.StartsWith("Metadata/Items/Armours/Helmets"))
                    SubItemType = ItemTypeChild.Helmet;
                else if (Path.StartsWith("Metadata/Items/Armours/Shields"))
                    SubItemType = ItemTypeChild.Shield;
            }
            else if (Path.StartsWith("Metadata/Items/Flasks"))
            {
                BaseItemType = ItemTypeParent.Flask;
            }
            else if (Path.StartsWith("Metadata/Items/Quivers"))
            {
                BaseItemType = ItemTypeParent.Quiver;
            }
            else if (Path.StartsWith("Metadata/Items/Weapons"))
            {
                //BaseItemType = ItemTypeParent.Weapon;
                if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons"))
                {
                    BaseItemType = ItemTypeParent.OneHandedWeapon;
                    //if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/Claws"))
                    //    SubItemType = ItemTypeChild.Claw;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/Daggers"))
                    //    SubItemType = ItemTypeChild.Dagger;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/OneHandAxes"))
                    //    SubItemType = ItemTypeChild.OneHandedAxe;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/OneHandMaces"))
                    //    SubItemType = ItemTypeChild.OneHandedMace;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/OneHandSwords"))
                    //    SubItemType = ItemTypeChild.OneHandedSword;
                    //if (Path.StartsWith("Metadata/Items/Weapons/OneHandWeapons/Wands"))
                    //    SubItemType = ItemTypeChild.Wand;
                }
                else if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons"))
                {
                    BaseItemType = ItemTypeParent.TwoHandedWeapon;
                    if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons/Bows"))
                        SubItemType = ItemTypeChild.Bow;
                    else if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons/Staves"))
                        SubItemType = ItemTypeChild.Stave;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons/TwoHandAxes"))
                    //    SubItemType = ItemTypeChild.TwoHandedAxe;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons/TwoHandMaces"))
                    //    SubItemType = ItemTypeChild.TwoHandedMace;
                    //else if (Path.StartsWith("Metadata/Items/Weapons/TwoHandWeapons/TwoHandSwords"))
                    //    SubItemType = ItemTypeChild.TwoHandedSword;
                }
            }
        }
    }
}