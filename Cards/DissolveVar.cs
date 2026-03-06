using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Oddmelt.Cards;

public class DissolveVar : DynamicVar
{
    public const string KEY = "Dissolve";

    public DissolveVar(decimal baseValue) : base(KEY, baseValue)
    {
        this.WithTooltip();
    }
}
