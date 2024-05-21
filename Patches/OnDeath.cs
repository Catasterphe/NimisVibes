using HarmonyLib;
using UnityEngine;

namespace NimisVibes.Patches
{
    internal class OnDeathPatch
    {
        [HarmonyPatch(typeof(PlayerMovement), "gameoverscreen")]
        private static void Prefix()
        {
            float gameoverscreen_time = 3.5f;
            double intensity = 1f;
            Plugin.DeviceManager.VibrateWithTime(intensity, gameoverscreen_time);
        }

    }
}
