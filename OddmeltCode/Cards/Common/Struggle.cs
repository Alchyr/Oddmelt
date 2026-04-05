using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.OddmeltCode.Powers;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Struggle : OddmeltCard
{
    public Struggle() : base(2,
        CardType.Attack, CardRarity.Common,
        TargetType.AnyEnemy)
    {
        WithDamage(12);
        WithPower<Bind>(6);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: 2).Execute(choiceContext);
        await CommonActions.ApplySelf<Bind>(this, DynamicVars["Bind"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}