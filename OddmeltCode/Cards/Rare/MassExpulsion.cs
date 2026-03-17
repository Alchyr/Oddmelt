using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Rare;

public class MassExpulsion() : OddmeltCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8, ValueProp.Move),
        new DissolveVar(8),
        new DynamicVar("DissolveCount", 2)
    ];
    protected override bool ShouldGlowGoldInternal => CanDissolve;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        int amt = 0;
        while (amt < DynamicVars["DissolveCount"].IntValue && await Dissolve(this))
        {
            for (int i = 0; i < Math.Pow(2, amt); ++i)
            {
                await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
            }
            ++amt;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DissolveCount"].UpgradeValueBy(1m);
    }
}
