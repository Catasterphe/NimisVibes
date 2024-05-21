using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NimisVibes;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static DeviceManager DeviceManager { get; private set; }
    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        DeviceManager = new DeviceManager("NIMIS");
        DeviceManager.ConnectDevices();
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(Patches.OnEatPatch));
        harmony.PatchAll(typeof(Patches.OnDeathPatch));
        harmony.PatchAll(typeof(Patches.OnPlayerHurtPatch));

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
