using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Oddmelt.OddmeltCode.Cards;

public class DissolveVar : DynamicVar
{
    public const string Key = "Dissolve";

    public DissolveVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip();
    }
}
