using System;
using System.Collections.Generic;
using System.Linq;
using BetterLeveling.Core;
using BetterLeveling.Model;
using BetterLeveling.Model.Enums;
using PoeHUD.Framework;
using PoeHUD.Hud.AdvancedTooltip;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.FilesInMemory;

namespace BetterLeveling.Methods
{
    public static class AffixSum
    {

        public static float GetSums(Item item)
        {
            Settings settings = Main.Core.Settings;
            float totalSum = 0;

            switch (item.BaseItemType)
            {
                case ItemTypeParent.None:
                    break;
                case ItemTypeParent.Amulet:
                    totalSum += TotalElementalResistance(item.Mods);
                    totalSum += TotalLife(item.Mods) * settings.LifeWeight;
                    break;
                case ItemTypeParent.Armour:
                    totalSum += TotalElementalResistance(item.Mods);
                    totalSum += TotalLife(item.Mods) * settings.LifeWeight;
                    break;
                case ItemTypeParent.Flask:
                    break;
                case ItemTypeParent.Quiver:
                    break;
                case ItemTypeParent.Ring:
                    totalSum += TotalElementalResistance(item.Mods);
                    totalSum += TotalLife(item.Mods) * settings.LifeWeight;
                    break;
                case ItemTypeParent.Weapon:
                case ItemTypeParent.OneHandedWeapon:
                case ItemTypeParent.TwoHandedWeapon:
                    totalSum += DPS(item);
                    break;
            }

            return totalSum;
        }

        public static int TotalLife(List<ModValue> mods)
        {
            return mods.Where(mod => mod.Record.Group == "IncreasedLife").Sum(mod => mod.StatValue[0]);
        }

        public static int TotalResistance(List<ModValue> mods)
        {
            int sum = 0;

            foreach (ModValue mod in mods)
            {
                if (!mod.Record.Group.Contains("Resist")) continue;

                if (mod.Record.Group == "AllResistances")
                    sum += mod.StatValue[0] * 3;
                else if (mod.Record.Group.Contains("And"))
                    sum += mod.StatValue[0] * 2;
                else
                    sum += mod.StatValue[0];
            }
            return sum;
        }

        public static float TotalElementalResistance(List<ModValue> mods)
        {
            Settings settings = Main.Core.Settings;
            float sum = 0;

            foreach (ModValue mod in mods)
            {
                if (!mod.Record.Group.Contains("Resist")) continue;
                if (mod.Record.Group.Contains("Chaos")) continue;

                if (mod.Record.Group == "AllResistances")
                {
                    sum += mod.StatValue[0] * settings.FireResWeight;
                    sum += mod.StatValue[0] * settings.ColdResWeight;
                    sum += mod.StatValue[0] * settings.LightResWeight;
                }
                else if (mod.Record.Group.Contains("And"))
                {
                    if (mod.Record.Group.Contains("Fire"))
                        sum += mod.StatValue[0] * settings.FireResWeight;
                    if (mod.Record.Group.Contains("Cold"))
                        sum += mod.StatValue[0] * settings.ColdResWeight;
                    if (mod.Record.Group.Contains("Light"))
                        sum += mod.StatValue[0] * settings.LightResWeight;
                }
                else
                {
                    if (mod.Record.Group.Contains("Fire"))
                        sum += mod.StatValue[0] * settings.FireResWeight;
                    if (mod.Record.Group.Contains("Cold"))
                        sum += mod.StatValue[0] * settings.ColdResWeight;
                    if (mod.Record.Group.Contains("Light"))
                        sum += mod.StatValue[0] * settings.LightResWeight;
                }
            }
            return sum;
        }

        public static int FireResistance(List<ModValue> mods)
        {
            return (from mod in mods where mod.Record.Group == "FireResist" select mod.StatValue[0]).Sum();
        }

        public static int ColdResistance(List<ModValue> mods)
        {
            return (from mod in mods where mod.Record.Group == "ColdResist" select mod.StatValue[0]).Sum();
        }

        public static int LightningResistance(List<ModValue> mods)
        {
            return (from mod in mods where mod.Record.Group == "LightningResist" select mod.StatValue[0]).Sum();
        }

        public static int ChaosResistance(List<ModValue> mods)
        {
            return (from mod in mods where mod.Record.Group == "ChaosResist" select mod.StatValue[0]).Sum();
        }

        public static float DPS(Item item)
        {
            Settings settings = Main.Core.Settings;
            Weapon weapon = item.Original.Item.GetComponent<Weapon>();
            float aSpd = (float)Math.Round(1000f / weapon.AttackTime, 2);
            int cntDamages = Enum.GetValues(typeof(DamageType)).Length;
            float[] doubleDpsPerStat = new float[cntDamages];
            float physDmgMultiplier = 1;
            int PhysHi = weapon.DamageMax;
            int PhysLo = weapon.DamageMin;
            foreach (ModValue mod in item.Mods)
                for (int iStat = 0; iStat < 4; iStat++)
                {
                    IntRange range = mod.Record.StatRange[iStat];
                    if (range.Min == 0 && range.Max == 0)
                        continue;

                    StatsDat.StatRecord theStat = mod.Record.StatNames[iStat];
                    int value = mod.StatValue[iStat];
                    switch (theStat.Key)
                    {
                        case "physical_damage_+%":
                        case "local_physical_damage_+%":
                            physDmgMultiplier += value / 100f;
                            break;

                        case "local_attack_speed_+%":
                            aSpd *= (100f + value) / 100;
                            break;

                        case "local_minimum_added_physical_damage":
                            PhysLo += value;
                            break;
                        case "local_maximum_added_physical_damage":
                            PhysHi += value;
                            break;

                        case "local_minimum_added_fire_damage":
                        case "local_maximum_added_fire_damage":
                        case "unique_local_minimum_added_fire_damage_when_in_main_hand":
                        case "unique_local_maximum_added_fire_damage_when_in_main_hand":
                            doubleDpsPerStat[(int)DamageType.Fire] += value;
                            break;

                        case "local_minimum_added_cold_damage":
                        case "local_maximum_added_cold_damage":
                        case "unique_local_minimum_added_cold_damage_when_in_off_hand":
                        case "unique_local_maximum_added_cold_damage_when_in_off_hand":
                            doubleDpsPerStat[(int)DamageType.Cold] += value;
                            break;

                        case "local_minimum_added_lightning_damage":
                        case "local_maximum_added_lightning_damage":
                            doubleDpsPerStat[(int)DamageType.Lightning] += value;
                            break;

                        case "unique_local_minimum_added_chaos_damage_when_in_off_hand":
                        case "unique_local_maximum_added_chaos_damage_when_in_off_hand":
                        case "local_minimum_added_chaos_damage":
                        case "local_maximum_added_chaos_damage":
                            doubleDpsPerStat[(int)DamageType.Chaos] += value;
                            break;
                    }
                }
            physDmgMultiplier += item.Original.Item.GetComponent<Quality>().ItemQuality / 100f;
            PhysLo = (int)Math.Round(PhysLo * physDmgMultiplier);
            PhysHi = (int)Math.Round(PhysHi * physDmgMultiplier);
            doubleDpsPerStat[(int)DamageType.Physical] = PhysLo + PhysHi;

            aSpd = (float)Math.Round(aSpd, 2);
            float pDps = doubleDpsPerStat[(int)DamageType.Physical] / 2 * aSpd;
            float eDps = 0;

            for (int i = 1; i < cntDamages; i++)
                eDps += doubleDpsPerStat[i] / 2 * aSpd;
            float dps = pDps * settings.PhysicalDPSWeight + eDps * settings.EleDPSWeight;

            return dps;
        }
    }
}