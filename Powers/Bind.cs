using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Threading.Tasks;
using BaseLib.Extensions;

namespace Oddmelt.Powers;

public sealed class Bind : OddmeltPower
{
    private class Data
    {
        public AttackCommand commandToModify;
        public int amountWhenAttackStarted;
    }

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner)
        {
            return Task.CompletedTask;
        }

        if (!command.DamageProps.IsPoweredAttack_())
        {
            return Task.CompletedTask;
        }

        Data internalData = GetInternalData<Data>();
        if (internalData.commandToModify != null)
        {
            return Task.CompletedTask;
        }

        if (command.ModelSource != null && command.ModelSource is not CardModel)
        {
            return Task.CompletedTask;
        }

        if (!command.DamageProps.IsPoweredAttack_())
        {
            return Task.CompletedTask;
        }

        internalData.commandToModify = command;
        internalData.amountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
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
        if (internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource)
        {
            return 0m;
        }

        if (internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer)
        {
            return 0m;
        }

        return -Amount;
    }

    public override async Task AfterAttack(AttackCommand command)
    {
        Data internalData = GetInternalData<Data>();
        if (command == internalData.commandToModify)
        {
            await PowerCmd.ModifyAmount(this, -internalData.amountWhenAttackStarted, null, null);
        }
    }
}
