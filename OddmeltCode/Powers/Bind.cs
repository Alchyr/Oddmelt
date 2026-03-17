using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Powers;

public sealed class Bind : OddmeltPower
{
    private class Data
    {
        public AttackCommand? CommandToModify;
        public int AmountWhenAttackStarted;
    }

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || !command.DamageProps.IsPoweredAttack_())
        {
            return Task.CompletedTask;
        }

        var internalData = GetInternalData<Data>();
        if (internalData.CommandToModify != null || command.ModelSource != null && command.ModelSource is not CardModel || !command.DamageProps.IsPoweredAttack_())
        {
            return Task.CompletedTask;
        }

        internalData.CommandToModify = command;
        internalData.AmountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (Owner != dealer)
        {
            return 0m;
        }

        if (!props.IsPoweredAttack_())
        {
            return 0m;
        }

        Data internalData = GetInternalData<Data>();
        if (internalData.CommandToModify != null && cardSource != null && cardSource != internalData.CommandToModify.ModelSource)
        {
            return 0m;
        }

        if (internalData.CommandToModify != null && internalData.CommandToModify.Attacker != dealer)
        {
            return 0m;
        }

        return -Amount;
    }

    public override async Task AfterAttack(AttackCommand command)
    {
        var internalData = GetInternalData<Data>();
        if (command == internalData.CommandToModify)
        {
            await PowerCmd.ModifyAmount(this, -internalData.AmountWhenAttackStarted, null, null);
        }
    }
}
