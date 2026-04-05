using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.OddmeltCode.Stitch;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Forming : OddmeltCard
{
    public Forming() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithKeywords(OddmeltKeywords.Stitch);
        WithBlock(4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await StitchHelper.StitchOnPlay(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}
