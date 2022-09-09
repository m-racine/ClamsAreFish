using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClamsAreFish
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private readonly int CLAM_ID = 372;
        private readonly string TRUE_FISH = "Fish -4";

        /*********
        ** Public methods
        *********/
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }


        /*********
        ** Private methods
        *********/
        /// <inheritdoc cref="IContentEvents.AssetRequested"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/ObjectInformation"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<int, string>().Data;
                    string[] fields = data[CLAM_ID].Split('/');
                    // Fields at index 3 is type/category
                    fields[3] = TRUE_FISH;
                    data[CLAM_ID] = string.Join("/", fields);
                });
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/FishPondData"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<int, string>().Data;
                    foreach (int itemID in data.Keys.ToArray())
                    {
                        string[] fields = data[itemID].Split('/');
                        Monitor.Log($"Loaded {string.Join(", ", fields)}.", LogLevel.Debug);
                    }

                });
            }

        }
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
