using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.OddmeltCode.Stitch;

namespace Oddmelt.OddmeltCode.Cards.Rare;

public class Strangle : OddmeltCard
{
    public Strangle() : base(3,
        CardType.Attack, CardRarity.Rare,
        TargetType.AnyEnemy)
    {
        WithDamage(20);
        WithKeywords(OddmeltKeywords.Stitch);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await StitchHelper.StitchOnPlay(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}