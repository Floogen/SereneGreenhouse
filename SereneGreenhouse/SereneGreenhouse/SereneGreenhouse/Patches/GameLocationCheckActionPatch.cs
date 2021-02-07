using Harmony;
using StardewValley;
using System.Reflection;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Linq;
using xTile.Dimensions;
using xTile.Tiles;

namespace SereneGreenhouse.Patches
{
    [HarmonyPatch]
    public class GameLocationCheckActionPatch
    {
        private static IMonitor monitor = ModEntry.monitor;

        internal static MethodInfo TargetMethod()
        {
            return AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.checkAction));
        }

        internal static bool Prefix(GameLocation __instance, ref bool __result, Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
        {
            if (__instance.Name != "Greenhouse")
            {
                return true;
            }

            Tile tile = __instance.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * 64, tileLocation.Y * 64), viewport.Size);
            if (tile != null && tile.Properties.ContainsKey("CustomAction"))
            {
                if (tile.Properties["CustomAction"] == "Treehouse")
                {
                    if (bool.Parse(tile.Properties["HasGivenOfferingToday"]) is true)
                    {
                        Game1.drawObjectDialogue("Fruits, fruits! Come back tomorrow, forest will change!");
                    }
                    else if (who.ActiveObject is null)
                    {
                        if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_1"))
                        {
                            Game1.drawObjectDialogue("An odd tree that seems to have a door fused to it.#From behind the door you can hear a tiny voice...#Gibe 100 Starfruit, we shape forest for more plants!");
                        }
                        else if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_2"))
                        {
                            Game1.drawObjectDialogue("An odd tree that seems to have a door fused to it.#From behind the door you can hear a tiny voice...#Gibe 100 Sweet Gem Berries, we shape forest for more plants!");
                            //Game1.MasterPlayer.mailReceived.Add("SG_Treehouse_Expansion_2");
                        }
                        else if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_3"))
                        {
                            Game1.drawObjectDialogue("An odd tree that seems to have a door fused to it.#From behind the door you can hear a tiny voice...#Gibe 100 Ancient Fruit, we shape forest for more plants!");
                            //Game1.MasterPlayer.mailReceived.Add("SG_Treehouse_Expansion_3");
                        }
                        else
                        {
                            Game1.drawObjectDialogue("An odd tree that seems to have a door fused to it.#From behind the door you hear only silence.");
                        }
                    }
                    else
                    {
                        if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_1") && who.ActiveObject.ParentSheetIndex == 268 && who.ActiveObject.Stack >= 100)
                        {
                            AcceptOffering(who, tile);
                            //Game1.MasterPlayer.mailReceived.Add("SG_Treehouse_Expansion_1");
                        }
                        else if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_2") && who.ActiveObject.ParentSheetIndex == 417 && who.ActiveObject.Stack >= 100)
                        {
                            AcceptOffering(who, tile);
                            //Game1.MasterPlayer.mailReceived.Add("SG_Treehouse_Expansion_2");
                        }
                        else if (!Game1.MasterPlayer.mailReceived.Contains("SG_Treehouse_Expansion_3") && who.ActiveObject.ParentSheetIndex == 454 && who.ActiveObject.Stack >= 100)
                        {
                            AcceptOffering(who, tile);
                            //Game1.MasterPlayer.mailReceived.Add("SG_Treehouse_Expansion_3");
                        }
                        else
                        {
                            Game1.drawObjectDialogue("Nothing interesting happens.");
                        }
                    }

                    __result = true;
                    return false;
                }
            return true;
        }

        private static void AcceptOffering(Farmer who, Tile tile)
        {
            Game1.drawObjectDialogue("For us? Thank you, thank you!#Come back tomorrow, forest will change!");
            RemoveActiveItemByCount(who, 100);

            tile.Properties["HasGivenOfferingToday"] = true;
        }

        private static void RemoveActiveItemByCount(Farmer farmer, int countToRemove)
        {
            if (farmer.CurrentItem != null && (farmer.CurrentItem.Stack -= countToRemove) <= 0)
            {
                farmer.removeItemFromInventory(farmer.CurrentItem);
                farmer.showNotCarrying();
            }
        }
    }
}
