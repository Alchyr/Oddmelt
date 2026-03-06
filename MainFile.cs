using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using Oddmelt.Patches;
using Oddmelt.Nodes;

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
 * 
 * Rename png to tres works but may result in parse error warning in pack process.
 * Alternatively, import png, create atlastexture, set image as png
 * */

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModID = "Oddmelt";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModID, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModID);

        IgnoreWovenCards.Patch(harmony);

        harmony.PatchAll();

        GeneratedNodePool.Init(NStitchCardHolder.NewInstanceForPool, 25);
    }
}
