using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CurvedUI;
using HarmonyLib;
using System;
using UnityEngine;

namespace DontTrip
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Content Warning.exe")]
    public class DontTrip : BaseUnityPlugin
    {

        public static DontTrip instance;


        public static readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        public static ConfigEntry<float> Duration { get; set; }
        public static ConfigEntry<float> ChanceToTrip { get; set; }
        public static ConfigEntry<float> DamageAmount { get; set; }
        public static ConfigEntry<bool> DoesDamage { get; set; }
        public static ConfigEntry<bool> DropItem { get; set; }

        private void Awake()
        {

            instance = this;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Duration = ((BaseUnityPlugin)this).Config.Bind<float>("DontTrip", "Duration of ragdoll", 2f, "The amount of time to ragdoll for");
            ChanceToTrip = ((BaseUnityPlugin)this).Config.Bind<float>("DontTrip", "Tripping Chance", 5f, new ConfigDescription("The Chance to Trip in percent 0-100", new AcceptableValueRange<float>(0f, 100f)));
            DamageAmount = ((BaseUnityPlugin)this).Config.Bind<float>("DontTrip", "Damage Amount", 5f, "The Damage to Take on Tripping");
            DoesDamage = ((BaseUnityPlugin)this).Config.Bind<bool>("DontTrip", "Does Damage", true, "If True you get Damage on Tripping");
            DropItem = ((BaseUnityPlugin)this).Config.Bind<bool>("DontTrip", "Drop Item", true, "If True you drop Item on Tripping");


            harmony.PatchAll();

        }


    }


}
