using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Oddmelt.OddmeltCode.Nodes;

namespace Oddmelt.OddmeltCode.Stitch;

public class StitchHelper
{
    internal static readonly SpireField<CardModel, CardModel> StitchedCard = new(() => null);
    internal static readonly SpireField<CardModel, CardModel> StitchedParent = new(() => null);

    public static async Task<CardPileAddResult> AddToStitchPile(CardModel card)
    {
        return await CardPileCmd.Add(card, StitchPile.CustomType);
    }

    public static async Task StitchOnPlay(CardModel card)
    {
        if (CombatManager.Instance.IsEnding || !CombatManager.Instance.IsInProgress)
        {
            return;
        }
        if (card.Keywords.Contains(OddmeltKeywords.Stitched))
        {
            return;
        }
        if (card.Owner.Creature.IsDead)
        {
            return;
        }

        CardPile pile = PileType.Hand.GetPile(card.Owner);
        List<CardModel> validCards = [.. pile.Cards];
        if (validCards.Remove(card))
        {
            MainFile.Logger.Info("Removed self from possible stitch targets");
        }

        if (validCards.Count == 0)
        {
            return;
        }

        var handCard = card.Owner.RunState.Rng.CombatCardSelection.NextItem(validCards);

        var stitched = handCard != null ? StitchedCard.Get(handCard) : null;
        while (stitched != null)
        {
            handCard = stitched;
            stitched = StitchedCard.Get(handCard);
        }

        MainFile.Logger.Info($"Stitching {card.Id} to {handCard!.Id}");

        StitchedCard.Set(handCard, card);
        StitchedParent.Set(card, handCard);

        card.AddKeyword(OddmeltKeywords.Stitched);

        //No need to do visuals; card's destination will be set to stitch pile which will resolve through CardCmd.Add -> StitchPile
    }

    public static async Task Stitch(CardModel target, CardModel toStitch)
    {
        StitchedCard.Set(target, toStitch);
        StitchedParent.Set(toStitch, target);

        await CardPileCmd.Add(toStitch, StitchPile.CustomType);

        //generate visual if necessary
        var parent = NCard.FindOnTable(target);
        if (parent == null) return;

        //parent.AddChildSafely();
    }
}


//When card is played, it is removed from its parent and added to Play pile.

//Move to play pile early to avoid weird card movement
[HarmonyPatch(typeof(Hook), nameof(Hook.BeforeCardPlayed))]
class MoveStitchedToPlay
{
    [HarmonyPostfix]
    static void PrePlay(CombatState combatState, CardPlay cardPlay)
    {
        var stitched = StitchHelper.StitchedCard.Get(cardPlay.Card);
        if (stitched == null) return;

        var holder = NStitchCardHolder.HolderForCard(stitched);

        holder?.LockPosition();
    }
}

//Patch to trigger at the end of play process rather than during it
[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
class PlayStitchedCard
{
    [HarmonyPostfix]
    static async void PlayStitched(CardModel __instance, PlayerChoiceContext choiceContext, Creature target, bool isAutoPlay, ResourceInfo resources)
    {
        var stitched = StitchHelper.StitchedCard.Get(__instance);
        if (stitched == null) return;

        float num = SaveManager.Instance.PrefsSave.FastMode switch
        {
            FastModeType.Instant => 0,
            FastModeType.Fast => 0.3f,
            _ => 0.7f
        };
        if (num > 0) await Cmd.Wait(num);
        await CardCmd.AutoPlay(choiceContext, stitched, stitched.TargetType == __instance.TargetType ? target : null);
    }
}

//CHANGE:
//All NCards always have an NStitchCardHolder attached? Just the card is null if not needed
//Holder is attached when the Spirefield is first referenced.
//Avoids issues with removing it

//-----NStitchCardHolder patches-----
[HarmonyPatch(typeof(NCard), "SubscribeToModel")]
class ListenModelChange
{
    [HarmonyPostfix]
    static void CheckChangedCard(NCard __instance, CardModel? model)
    {
        if (model != null && __instance.IsInsideTree())
        {
            var holder = NStitchCardHolder.HolderForModel[__instance];
            var stitched = StitchHelper.StitchedCard[model];
            holder?.StitchedModelChanged(stitched);
        }
    }
}


[HarmonyPatch(typeof(NCard), nameof(NCard.OnFreedToPool))]
class ReparentOnParentRemoval
{
    [HarmonyPostfix]
    static void ResetWhenNodeFreed(NCard __instance)
    {
        try
        {
            var holder = NStitchCardHolder.HolderForModel[__instance];
            holder?.Reset();
        }
        catch (Exception) { }

    }
}