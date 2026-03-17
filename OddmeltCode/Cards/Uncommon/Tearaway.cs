using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Uncommon;

public class Tearaway() : OddmeltCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move), new DissolveVar(3)];

    protected override bool ShouldGlowGoldInternal => CanDissolve;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);

        if (await Dissolve(this))
        {
            var cardModel = await CommonActions.SelectSingleCard(this, SelectionScreenPrompt, choiceContext, PileType.Discard);
            if (cardModel != null)
            {
                await CardPileCmd.Add(cardModel, PileType.Hand);
            }
        }
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
