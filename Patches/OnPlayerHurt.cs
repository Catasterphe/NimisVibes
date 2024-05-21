using HarmonyLib;
using UnityEngine;

namespace NimisVibes.Patches
{
    internal class OnPlayerHurtPatch
    {
        [HarmonyPatch(typeof(PlayerMovement), nameof(PlayerMovement.PlayerHurt))]
        private static void Postfix()
        {
            double intensity = 1f;
            Plugin.DeviceManager.VibrateWithTime(intensity, 1f);
        }
    }
}
