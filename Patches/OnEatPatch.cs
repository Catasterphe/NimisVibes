using HarmonyLib;
using UnityEngine;

namespace NimisVibes.Patches
{
    internal class OnEatPatch
    {
        private static int mykills;
        [HarmonyPatch(typeof(PlayerEat), "ConsumeDead")]
        private static void Prefix()
        {
            mykills = Object.FindFirstObjectByType<PlayerEat>().legitkills;
        }
        [HarmonyPatch(typeof(PlayerEat), "ConsumeDead")]
        private static void Postfix()
        {
            var newkills = Mathf.Clamp(Object.FindFirstObjectByType<PlayerEat>().legitkills - mykills, 0, 4);
            mykills = Object.FindFirstObjectByType<PlayerEat>().legitkills;
            double intensity = newkills / 4.0f;
            Plugin.Log.LogInfo($"Vibrating at {intensity} with {newkills} new kills");
            Plugin.DeviceManager.VibrateWithTime(intensity, .45f);
        }
    }
}
