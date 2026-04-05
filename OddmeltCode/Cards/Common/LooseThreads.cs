using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class LooseThreads : OddmeltCard
{
    public LooseThreads() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(new CardsVar(3));
        WithKeywords(OddmeltKeywords.Woven);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
