using Harmony;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SereneGreenhouse
{
    public class ModEntry : Mod
    {
        internal static IMonitor monitor;
        internal static IModHelper modHelper;

        // ModData related
        internal static string offeringsStoredInWaterHutKey;

        public override void Entry(IModHelper helper)
        {
            // Set up the monitor, helper and config
            monitor = Monitor;
            modHelper = helper;

            // Setup our ModData keys
            offeringsStoredInWaterHutKey = $"{this.ModManifest.UniqueID}/offerings-stored-in-water-hut";

            // Load our Harmony patches
            try
            {
                var harmony = HarmonyInstance.Create(this.ModManifest.UniqueID);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Monitor.Log($"Issue with Harmony patch: {e}", LogLevel.Error);
                return;
            }

            // Hook into the player warping
            helper.Events.Player.Warped += this.OnWarped;
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation.Name == "Greenhouse")
            {
                e.NewLocation.critters = new List<Critter>();

                double mapArea = e.NewLocation.map.Layers[0].LayerWidth * e.NewLocation.map.Layers[0].LayerHeight;
                double chance = Math.Max(0.15, Math.Min(0.5, mapArea / 15000.0));
                chance = Math.Min(0.8, chance * 1.5);
                while (Game1.random.NextDouble() < 0.8)
                {
                    Vector2 v = e.NewLocation.getRandomTile();
                    if (Game1.isDarkOut())
                    {
                        e.NewLocation.critters.Add(new Firefly(v));
                    }
                    else
                    {
                        e.NewLocation.critters.Add(new Butterfly(v));
                    }
                    while (Game1.random.NextDouble() < 0.4)
                    {
                        if (Game1.isDarkOut())
                        {
                            e.NewLocation.critters.Add(new Firefly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3))));
                        }
                        else
                        {
                            e.NewLocation.critters.Add(new Butterfly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3))));
                        }
                    }
                }
            }
        }
    }
}
