using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Uncommon;

public class StickySnare : OddmeltCard
{
    public StickySnare() : base(1,
        CardType.Skill, CardRarity.Uncommon,
        TargetType.Self)
    {
        WithVars(new BlockVar(3, ValueProp.Move));
        WithEnergyTip();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cardModel = await CommonActions.SelectSingleCard(this, SelectionScreenPrompt, choiceContext, PileType.Discard);
        if (cardModel != null)
        {
            await CardPileCmd.Add(cardModel, PileType.Hand);
            int amt = cardModel.EnergyCost.GetAmountToSpend();
            for (int i = 0; i < amt; ++i)
            {
                await CommonActions.CardBlock(this, play);
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}