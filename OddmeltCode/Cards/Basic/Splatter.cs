using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Oddmelt.OddmeltCode.Powers;

namespace Oddmelt.OddmeltCode.Cards.Basic;

public class Splatter : OddmeltCard
{
    public static SavedSpireField<Splatter, int> PlayCount = new(() => 0, "SplatterPlayCount");
    
    public Splatter() : base(0, CardType.Skill, CardRarity.Basic, TargetType.AllEnemies)
    {
        WithVars(new PowerVar<Bind>(2), new DissolveVar(5), new DynamicVar("Bonus", 3));
    }

    protected override bool ShouldGlowGoldInternal => CanDissolve;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_flying_slash");
        int amount = DynamicVars["Bind"].IntValue;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<Bind>(enemy, amount, Owner.Creature, this);
        }
        if (await Dissolve(this))
        {
            amount = DynamicVars["Bonus"].IntValue;

            foreach (var enemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<Bind>(enemy, amount, Owner.Creature, this);
            }
        }
        
        if (!(DeckVersion is Splatter deckVersion))
            return;
        PlayCount.Set(deckVersion, PlayCount.Get(deckVersion) + 1);
        MainFile.Logger.Info("Splatter play count: " + PlayCount.Get(deckVersion));
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Bind"].UpgradeValueBy(1m);
        DynamicVars["Bonus"].UpgradeValueBy(1m);
    }
}
