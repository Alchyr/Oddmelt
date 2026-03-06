using BaseLib.Abstracts;
using BaseLib.Extensions;

namespace Oddmelt.Powers;

public abstract class OddmeltPower : CustomPowerModel
{
    public override string CustomPackedIconPath => "res://images/oddmelt/powers/" + Id.Entry.RemovePrefix().ToLowerInvariant() + ".png";
    public override string CustomBigIconPath => "res://images/oddmelt/powers/big/" + Id.Entry.RemovePrefix().ToLowerInvariant() + ".png";
}