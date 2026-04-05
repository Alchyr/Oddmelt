using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class ImpactPrep : OddmeltCard
{
    public ImpactPrep() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(new BlockVar(7, ValueProp.Move), new DissolveVar(4), new BlockVar("Block2", 7, ValueProp.Move));
    }

    protected override bool ShouldGlowGoldInternal => CanDissolve;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        BlockVar block = DynamicVars.Block;
        decimal amount = block.CalculateBlock(Owner.Creature, block.Props, play);
        await CommonActions.ApplySelf<BlockNextTurnPower>(this, amount);
        if (await Dissolve(this))
        {
            block = (BlockVar) DynamicVars["Block2"];
            amount = block.CalculateBlock(Owner.Creature, block.Props, play);
            await CommonActions.ApplySelf<BlockNextTurnPower>(this, amount);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(2m);
        DynamicVars["Block2"].UpgradeValueBy(2m);
    }
}
