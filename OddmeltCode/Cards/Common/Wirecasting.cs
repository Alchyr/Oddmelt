using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.OddmeltCode.Stitch;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Wirecasting : OddmeltCard
{
    public Wirecasting() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithKeywords(OddmeltKeywords.Stitch);
        WithDamage(4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await StitchHelper.StitchOnPlay(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
