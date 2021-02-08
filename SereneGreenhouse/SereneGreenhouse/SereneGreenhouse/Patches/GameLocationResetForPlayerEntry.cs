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
        }
    }
}
