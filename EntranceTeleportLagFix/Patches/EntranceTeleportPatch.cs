using HarmonyLib;
using UnityEngine;

namespace EntranceTeleportLagFix.Patches
{
    [HarmonyPatch(typeof(EntranceTeleport))]
    internal class EntranceTeleportPatch
    {
        // reset checkForEnemiesInterval to 1f
        [HarmonyPatch("FindExitPoint")]
        [HarmonyPostfix]
        static void FindExitPointPatch(ref EntranceTeleport __instance, ref float ___checkForEnemiesInterval)
        {
            EntrancePerformancePatchData data = __instance.gameObject.GetComponent<EntrancePerformancePatchData>();

            if (data == null)
            {
                data = __instance.gameObject.AddComponent<EntrancePerformancePatchData>();
            }
            if (___checkForEnemiesInterval <= 0f)
            {
                if (data.attemptsToFindExit >= 5)
                {
                    EntranceTeleportLagFixBase.Instance.mls.LogError($"Entrance with ID {__instance.entranceId} has failed to find an exitPoint after {data.attemptsToFindExit} attempts.");
                    __instance.enabled = false;
                    ___checkForEnemiesInterval = float.MaxValue; // in-case this gets re-enabled
                    // forcibly destroy it because this keeps on re-enabling itself and causing MAJOR lag spikes
                    Object.Destroy(__instance);
                    Object.Destroy(data);
                    return;
                }

                ___checkForEnemiesInterval = 1f;
                data.attemptsToFindExit++;

                EntranceTeleportLagFixBase.Instance.mls.LogDebug($"Entrance with ID {__instance.entranceId} tries to find exitPoint, attempt #{data.attemptsToFindExit}");
            }
        }
    }
}