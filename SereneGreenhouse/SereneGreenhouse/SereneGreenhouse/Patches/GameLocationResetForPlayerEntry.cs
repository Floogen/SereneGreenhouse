using Harmony;
using StardewValley;
using System.Reflection;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Linq;
using xTile.Dimensions;
using xTile.Tiles;
using System.Collections.Generic;
using StardewValley.BellsAndWhistles;
using System;

namespace SereneGreenhouse.Patches
{
    [HarmonyPatch]
    public class GameLocationResetForPlayerEntry
    {
        private static IMonitor monitor = ModEntry.monitor;

        internal static MethodInfo TargetMethod()
        {
            return AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.resetForPlayerEntry));
        }

        internal static void Postfix(GameLocation __instance)
        {
            if (__instance.Name != "Greenhouse")
            {
                return;
            }

            if (Game1.isRaining)
            {
                Game1.changeMusicTrack("rain");
            }
            else if (Game1.timeOfDay < 1800)
            {
                Game1.changeMusicTrack("woodsTheme");
            }

            __instance.critters = new List<Critter>();

            double mapArea = __instance.map.Layers[0].LayerWidth * __instance.map.Layers[0].LayerHeight;
            double chance = Math.Max(0.15, Math.Min(0.5, mapArea / 1500.0));
            chance = Math.Min(0.8, chance * 1.5);
            while (Game1.random.NextDouble() < 0.8)
            {
                Vector2 v = __instance.getRandomTile();
                if (Game1.isDarkOut())
                {
                    __instance.critters.Add(new Firefly(v));
                }
                else
                {
                    __instance.critters.Add(new Butterfly(v));
                }
                while (Game1.random.NextDouble() < 0.4)
                {
                    if (Game1.isDarkOut())
                    {
                        __instance.critters.Add(new Firefly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3))));
                    }
                    else
                    {
                        __instance.critters.Add(new Butterfly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3))));
                    }
                }
            }
        }
    }
}
