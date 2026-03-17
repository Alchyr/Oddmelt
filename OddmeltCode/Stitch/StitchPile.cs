using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Oddmelt.OddmeltCode.Nodes;

namespace Oddmelt.OddmeltCode.Stitch;

//First part of stitch works fine
//currently is jank when card is played though

//TODO:
//Cards stitched to stitched cards have some jank
//Don't update properly when re-generated
//Card play process is weird

public class StitchPile() : CustomPile(CustomType)
{
    [CustomEnum]
    public static PileType CustomType;

    public override NCard? GetNCard(CardModel card)
    {
        var holder = NStitchCardHolder.HolderForCard(card);

        return holder?.CardNode;
    }

    public override bool CardShouldBeVisible(CardModel card)
    {
        var stitchParent = StitchHelper.StitchedParent.Get(card);
        return stitchParent == null ? throw new Exception("Card in stitch pile without parent") : PileShouldBeVisible(stitchParent);
    }

    private bool PileShouldBeVisible(CardModel card)
    {
        if (card.Pile == null) return false;

        CardPile pile = card.Pile;
        return pile.Type switch
        {
            PileType.None => false,
            PileType.Draw => false,
            PileType.Discard => false,
            PileType.Exhaust => false,
            PileType.Deck => false,
            PileType.Play => true,
            PileType.Hand => true,
            _ => pile is CustomPile customPile && customPile.CardShouldBeVisible(card)
        };
    }

    public override bool CustomTween(Tween tween, CardModel card, NCard cardNode, CardPile oldPile)
    {
        var stitchParent = StitchHelper.StitchedParent.Get(card);
        var parent = stitchParent != null ? NCard.FindOnTable(stitchParent) : null;

        if (parent == null)
        {
            MainFile.Logger.Info($"Parent not visible, no custom tween");
            //default tween actually doesn't move much, it transfers to NCardFlyVfx while reparenting to vfxContainer
            return false;
        }

        //Normal tween adding to hand - 
        //first cardnode is parented in NCombatRoom.Instance.Ui.AddToPlayContainer
        //on tween callback handNode.Add -> ends up at NCardHolder.SetCard which reparents

        //immediately reparent preserving global position
        var holder = NStitchCardHolder.HolderForModel[parent];
        holder!.Initialize(cardNode);
        holder.SetToStitchedPosition();

        float num = SaveManager.Instance.PrefsSave.FastMode switch
        {
            FastModeType.Instant => 0.05f,
            FastModeType.Fast => 0.4f,
            _ => 0.7f,
        }; //relatively slow tween

        tween.TweenProperty(cardNode, "rotation", 0, num).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.Parallel().TweenProperty(cardNode, "scale", Vector2.One, num * 0.75f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        return true;
    }

    public override Vector2 GetTargetPosition(CardModel card, Vector2 size)
    {
        var stitchParent = StitchHelper.StitchedParent.Get(card);
        var parentModel = stitchParent != null ? NCard.FindOnTable(stitchParent) : null;

        if (parentModel == null) return stitchParent!.Pile?.Type.GetTargetPosition(null) ?? Vector2.Zero;

        return Vector2.Zero; //should be position relative to parent NCard
    }
}
