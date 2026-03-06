using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Oddmelt.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oddmelt.Cards.Basic;

public class Splatter() : OddmeltCard(0, CardType.Skill, CardRarity.Basic, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Debuff", 2), new DissolveVar(5), new DynamicVar("Debuff2", 3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<Bind>()
    ];

    protected override bool ShouldGlowGoldInternal => CanDissolve;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_flying_slash");
        int amount = DynamicVars["Debuff"].IntValue;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<Bind>(enemy, amount, Owner.Creature, this);
        }
        if (await Dissolve(this))
        {
            amount = DynamicVars["Debuff2"].IntValue;

            foreach (var enemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<Bind>(enemy, amount, Owner.Creature, this);
            }
        }
    }


    protected override void OnUpgrade()
    {
        DynamicVars["Debuff"].UpgradeValueBy(1m);
        DynamicVars["Debuff2"].UpgradeValueBy(1m);
    }
}
