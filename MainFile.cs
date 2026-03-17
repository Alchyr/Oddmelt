using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using Oddmelt.OddmeltCode.Nodes;
using Oddmelt.OddmeltCode.Patches;

namespace Oddmelt;

/**
 * Ideas
 * 
 * Self Bind
 * 
 * Bind effect - square texture based on model size, lines random generated (amount equal to bind amount)
 * shader of transparency of line based on average of point spread of the model
 * colored
 * 
 * Bind... rename? Necrobinder kinda overlaps.
 * */

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "Oddmelt"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        
        IgnoreWovenCards.Patch(harmony);
        
        harmony.PatchAll();

        GeneratedNodePool.Init(NStitchCardHolder.NewInstanceForPool, 25);
    }
}