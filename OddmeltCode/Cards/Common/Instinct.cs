using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Oddmelt.OddmeltCode.Powers;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Instinct : OddmeltCard
{
    public Instinct() : base(1,
        CardType.Skill, CardRarity.Common,
        TargetType.Self)
    {
        WithPower<StrengthPower>(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CommonActions.ApplySelf<InstinctPower>(this, DynamicVars.Strength.BaseValue);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;
        
        await Cmd.Wait(0.25f.OrFast());
        await CommonActions.ApplySelf<InstinctPower>(this, DynamicVars.Strength.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1);
    }
}