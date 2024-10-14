using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using EntranceTeleportLagFix.Patches;

namespace EntranceTeleportLagFix
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class EntranceTeleportLagFixBase : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "com.hexa0.EntranceTeleportLagFix";
        private const string PLUGIN_NAME = "EntranceTeleportLagFix";
        private const string PLUGIN_VERSION = "1.1.0";

        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        public static EntranceTeleportLagFixBase Instance;

        public ManualLogSource mls;

        private void Awake()
        {
            // Plugin startup logic
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(PLUGIN_GUID);

            mls.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(typeof(EntranceTeleportLagFixBase));
            harmony.PatchAll(typeof(EntranceTeleportPatch));
        }
    }
}