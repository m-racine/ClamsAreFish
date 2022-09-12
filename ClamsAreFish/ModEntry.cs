using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewValley.Objects;

namespace ClamsAreFish
{
    // <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        internal Harmony Harmony;
        public static Random Random = new();

        // <summary>The mod entry point, called after the mod is first loaded.</summary>
        // <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Harmony = new Harmony(ModManifest.UniqueID);
            Harmony.Patch(AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.performObjectDropInAction)),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(ModEntry), nameof(Postfix))));
        }

        public static void Postfix(ref bool __result, ref StardewValley.Object __instance, ref Item dropInItem, ref bool probe, ref Farmer who)
        {
            if (__instance?.Name is "Recycling Machine")
            {
                if (dropInItem?.Name is "Clam Shell")
                {
                    __instance.heldObject.Value = new StardewValley.Object(542, 1);
                    if (!probe)
                    {
                        who.currentLocation.playSound("trashcan");
                        __instance.MinutesUntilReady = 60;
                        Game1.stats.PiecesOfTrashRecycled++;
                    }
                    __result = true;
                }
            }
        }

        // /*********
        // ** Private methods
        // *********/
        // /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        // /// <param name="sender">The event sender.</param>
        // /// <param name="e">The event data.</param>
        // private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        // {
        //     if (e.NameWithoutLocale.IsEquivalentTo("Data/BigCraftableInformation"))
        //     {
        //         e.Edit(asset =>
        //         {
        //             var data = asset.AsDictionary<int, string>().Data;
        //             string[] fields = data[CLAM_ID].Split('/');
        //             // Fields at index 3 is type/category
        //             /*fields[3] = TRUE_FISH;
        //             data[CLAM_ID] = string.Join("/", fields);*/
        //             Monitor.Log($"Loaded {string.Join(", ", fields)}.", LogLevel.Debug);
        //         });
        //     }
        // }
    }
}

// Name, Price, Edibility, Type & Category, Display Name (translation), Description (translated), misc, misc2, buff duration
// Initial
// Clam, 50, -300, Basic -23, Clam, Someone lived here once.. 
// Goal
// Clam, 50, -300, Fish -4, Clam, Someone lived here once..

/*
 * Next Steps:
 * clam vs. clam shell? (optional)
 * Change description?
 * Fish Pond entry -- currently its caught in the catch all, which isn't exactly exciting.
 * Add "Pearl" as a fish pond item?
 */

/*
 * Credit: https://github.com/XxHarvzBackxX/recyclableCola

*/