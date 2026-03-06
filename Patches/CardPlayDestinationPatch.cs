using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Oddmelt.Cards;
using System.Collections.Generic;

namespace Oddmelt.Patches;


[HarmonyPatch(typeof(CardModel), "GetResultPileType")]
public class CardPlayDestinationPatch
{   //tbh I could do this with a postfix but this is good practice
    [HarmonyTranspiler]
    static List<CodeInstruction> AltDestination(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions)
           .Match(new InstructionMatcher()
               .ldc_i4_4()
               .ret()
               .ldc_i4_3()
           )
           .Insert([
               CodeInstruction.LoadArgument(0),
               CodeInstruction.Call(() => ChangeDestination(default, default)),
            ]);
    }

    //patched to be lower priority than exhaust
    public static PileType ChangeDestination(PileType dest, CardModel model)
    {
        if (OddmeltKeywords.IsStitch(model))
        {
            return StitchPile.CustomType;
        }
        return dest;
    }
}
