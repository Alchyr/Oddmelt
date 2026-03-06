using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;

namespace Oddmelt.Relics;

public class ImpureCoil : OddmeltRelic
{
    private const string _extraDamageKey = "ExtraDamage";

    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(_extraDamageKey, 2)];

    public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
    {
        if (!props.IsPoweredAttack_())
        {
            return 0m;
        }

        if (cardSource == null)
        {
            return 0m;
        }

        if (target == null)
        {
            return 0m;
        }

        if (dealer != Owner.Creature && cardSource.Owner != Owner)
        {
            return 0m;
        }

        return target.Powers.Count((power) => power.Type == PowerType.Debuff) * DynamicVars[_extraDamageKey].BaseValue;
    }
}
