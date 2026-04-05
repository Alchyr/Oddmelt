using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Token;

public class Weave : OddmeltCard
{
    public Weave() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithKeywords(OddmeltKeywords.Woven);
        WithBlock(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}
