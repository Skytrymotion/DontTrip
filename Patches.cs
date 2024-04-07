using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DontTrip
{
    [HarmonyPatch(typeof(Player))]
    public static class PlayerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start(Player __instance)
        {
            if (__instance.IsLocal && !((Object)(object)((Component)__instance).gameObject.GetComponent<Tripper>() != (Object)null))
            {
                Tripper tripper = ((Component)__instance).gameObject.AddComponent<Tripper>();
                tripper.player = __instance;
            }
        }
    }
}
