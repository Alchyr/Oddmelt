using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using Oddmelt.OddmeltCode.Extensions;

namespace Oddmelt.OddmeltCode.Powers;

public abstract class OddmeltPower : CustomPowerModel
{
    //Loads from Oddmelt/images/powers/your_power.png
    
    //Not working?
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}