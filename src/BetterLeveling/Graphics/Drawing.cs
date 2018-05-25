using System.Collections.Generic;
using BetterLeveling.Core;
using BetterLeveling.Graphics.Model;
using BetterLeveling.Methods;
using BetterLeveling.Model;
using BetterLeveling.Model.Enums;
using PoeHUD.Models.Enums;
using SharpDX;
using SharpDX.Direct3D9;
using static PoeHUD.Plugins.BasePlugin;

namespace BetterLeveling.Graphics
{
    public class Drawing
    {
        public static void DrawLineToItem(Item from, Item to, bool isUpgrade)
        {
            RectangleF recTo = to.Original.GetClientRect();
            recTo.Right -= 1;
            recTo.Bottom -= 1;
            API.Graphics.DrawLine(new Vector2(from.Original.GetClientRect().Center.X, from.Original.GetClientRect().Top), new Vector2(recTo.Center.X, recTo.Bottom), Main.Core.Settings.LineThickness, Main.Core.Settings.ConnectorColor);
        }

        public static void DrawAllValidItems(List<Item> inventoryList, List<Item> characterList)
        {
            foreach (Item inventoryItem in inventoryList)
            {
                bool foundUpgrade = false;
                if (inventoryItem.BaseItemType != ItemTypeParent.None && inventoryItem?.Rarity != ItemRarity.Normal)
                {
                    float invSum = AffixSum.GetSums(inventoryItem);

                    foreach (Item characterItem in characterList)
                        if (characterItem.BaseItemType != ItemTypeParent.None && inventoryItem.BaseItemType != ItemTypeParent.None)
                        {
                            bool isUpgrade;
                            float charSum = 0;
                            if (inventoryItem.SubItemType != ItemTypeChild.None)
                            {
                                if (inventoryItem.SubItemType != characterItem.SubItemType) continue;

                                charSum = AffixSum.GetSums(characterItem);

                                isUpgrade = charSum < invSum;

                                if (!isUpgrade) continue;

                                if (!characterItem.IsUpgrading)
                                    characterItem.IsUpgrading = true;
                                foundUpgrade = true;

                                DrawLineToItem(inventoryItem, characterItem, isUpgrade);
                                DrawBoxOnItem(characterItem, charSum, characterItem.IsUpgrading, characterItem.IsDrawn);
                                characterItem.IsDrawn = true;
                            }
                            else if (inventoryItem.BaseItemType != ItemTypeParent.None)
                            {
                                if (inventoryItem.BaseItemType != characterItem.BaseItemType) continue;

                                charSum = AffixSum.GetSums(characterItem);

                                isUpgrade = charSum < invSum;

                                if (!isUpgrade) continue;

                                if (!characterItem.IsUpgrading)
                                    characterItem.IsUpgrading = true;
                                foundUpgrade = true;

                                DrawLineToItem(inventoryItem, characterItem, isUpgrade);
                                DrawBoxOnItem(characterItem, charSum, characterItem.IsUpgrading, characterItem.IsDrawn);
                                characterItem.IsDrawn = true;
                            }
                        }

                    if (foundUpgrade)
                        DrawBoxOnItem(inventoryItem, invSum, true);
                }
            }
        }

        public static void DrawBoxOnItem(Item inventoryItem, bool isUpgrade, bool isDrawn = false)
        {
            if (isDrawn) return;
            RectangleF rec = inventoryItem.Original.GetClientRect();
            rec.Right -= 1;
            rec.Bottom -= 1;
            API.Graphics.DrawBox(rec, Main.Core.Settings.FillColor);
            API.Graphics.DrawFrame(rec, Main.Core.Settings.OutlineThickness, Main.Core.Settings.FrameColor);

            // Draw sum on the item for quickly seeing what has a high value
            if (Main.Core.Settings.SumShow)
            {
                API.Graphics.DrawText(
                $"{AffixSum.GetSums(inventoryItem)}"
                , Main.Core.Settings.SumSize, rec.Center, Main.Core.Settings.SumColor, FontDrawFlags.VerticalCenter | FontDrawFlags.Center);
            }
        }

        public static void DrawBoxOnItem(Item inventoryItem, float sum, bool isUpgrade, bool isDrawn = false)
        {
            if (isDrawn) return;
            RectangleF rec = inventoryItem.Original.GetClientRect();
            rec.Right -= 1;
            rec.Bottom -= 1;
            API.Graphics.DrawBox(rec, Main.Core.Settings.FillColor);
            API.Graphics.DrawFrame(rec, Main.Core.Settings.OutlineThickness, Main.Core.Settings.FrameColor);

            // Draw sum on the item for quickly seeing what has a high value
            if (Main.Core.Settings.SumShow)
            {
                API.Graphics.DrawText(
                $"{sum}"
                , Main.Core.Settings.SumSize, rec.Center, Main.Core.Settings.SumColor, FontDrawFlags.VerticalCenter | FontDrawFlags.Center);
            }
        }
    }
}