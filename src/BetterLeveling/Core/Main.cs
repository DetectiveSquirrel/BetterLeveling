using System.Collections.Generic;
using BetterLeveling.Graphics;
using BetterLeveling.Model;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace BetterLeveling.Core
{
    public class Main : BaseSettingsPlugin<Settings>
    {
        public List<Item> CharacterItems = new List<Item>();
        public List<Item> VisabableNonEquippedItems = new List<Item>();

        public static Main Core;

        public Main()
        {
            PluginName = "Better Leveling";
        }

        public override void Initialise()
        {
            Core = this;
        }

        public override void Render()
        {

            if (!GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible) return;
            if (Settings.ShowHotkey.PressedOnce())
            {
                Settings.ShowToggle = !Settings.ShowToggle;
            }
            if (!Settings.ShowToggle) return;

            // Inventory
            VisabableNonEquippedItems = GetInventoryItems(InventoryIndex.PlayerInventory);

            // Player Items
            for (int index = 0; index < 15; index++)
            {
                if (index == 0 || index == 6 || index == 7 || index == 13 || index == 14)
                    continue;

                CharacterItems.AddRange(GetInventoryItems((InventoryIndex) index));
            }

            Drawing.DrawAllValidItems(VisabableNonEquippedItems, CharacterItems);

            VisabableNonEquippedItems.Clear();
            CharacterItems.Clear();
        }

        public List<Item> GetInventoryItems(InventoryIndex index)
        {
            Inventory inventory = GameController.Game.IngameState.IngameUi.InventoryPanel[index];
            List<NormalInventoryItem> inventoryItems = inventory.VisibleInventoryItems;

            List<Item> itemList = new List<Item>();

            if (inventoryItems == null)
            {
                LogMessage("Player inventory->VisibleInventoryItems is null!", 5);
                return itemList;
            }

            foreach (NormalInventoryItem inventoryItem in inventoryItems)
            {
                if (inventoryItem.Item == null)
                    continue;

                if (string.IsNullOrEmpty(inventoryItem.Item.Path))
                {
                    LogMessage($"Bugged item detected on X:{inventoryItem.InventPosX}, Y:{inventoryItem.InventPosY}, skipping.. Change location to fix this or restart the game (exit to character selection).", 5);
                    continue;
                }
                itemList.Add(new Item(inventoryItem));
            }
            return itemList;
        }
    }
}