using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Rare;

public class MassExpulsion : OddmeltCard
{
    public MassExpulsion() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(8);
        WithDissolve(8);
        WithVar("DissolveCount", 2);
        WithCalculatedVar("Hits", 1, 1,
            (card, target) => (decimal)Math.Pow(2, (double)Math.Min(
                Math.Floor(card.Owner.Creature.Block / card.DynamicVars[DissolveVar.Key].BaseValue),
                card.DynamicVars["DissolveCount"].BaseValue)) - 1);
    }

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
