using BaseLib.Extensions;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Oddmelt.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Oddmelt.Patches;

[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.ShuffleIfNecessary))]
class ShuffleNotNecessary
{
    [HarmonyPrefix]
    static bool SkipIfOnlyWoven(PlayerChoiceContext choiceContext, Player player, ref Task __result)
    {
        CardPile discard = PileType.Discard.GetPile(player);
        if (discard.Cards.All((card) => card.Keywords.Contains(OddmeltKeywords.Woven)))
        {
            __result = Task.CompletedTask;
            return false;
        }
        return true;
    }
}

public class IgnoreWovenCards
{
    private static Type stateMachineType;
    public static void Patch(Harmony harmony)
    {
        MethodInfo transpiler = AccessTools.Method(typeof(IgnoreWovenCards), nameof(RemoveWoven));
        harmony.PatchAsyncMoveNext(AccessTools.Method(typeof(CardPileCmd), nameof(CardPileCmd.Shuffle)), out stateMachineType, transpiler: new HarmonyMethod(transpiler));
    }

    static List<CodeInstruction> RemoveWoven(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions)
           .Match(new InstructionMatcher()
               .ldc_i4_3()
               .ldarg_0()
               .ldfld(stateMachineType, "player")
               .call(typeof(PileTypeExtensions), nameof(PileTypeExtensions.GetPile))
               .callvirt(AccessTools.PropertyGetter(typeof(CardPile), "Cards"))
               .call(typeof(IEnumerable<CardModel>), "ToList")
           ) //next instruction moves list from stack to local var 1
           .Insert(CodeInstruction.Call(()=>Filter(default)));
    }

    public static List<CardModel> Filter(List<CardModel> list)
    {
        list.RemoveAll(card => card.IsWoven());
        return list;
    }
}