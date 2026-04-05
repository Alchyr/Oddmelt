using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Oddmelt.OddmeltCode.Cards.Uncommon;

public class Dredge : OddmeltCard
{
    public Dredge() : base(0, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new DissolveVar(5), new CardsVar(2));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (await Dissolve(this))
        {
            await CommonActions.Draw(this, choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}