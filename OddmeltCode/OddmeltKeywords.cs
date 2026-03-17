using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Oddmelt.OddmeltCode;

public static class OddmeltKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Woven; //WovenPatches to Shuffle/ShuffleIfNecessary
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Stitch;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Stitched; //applied to cards to mark them as being stitched already

    
    public static bool IsStitch(this CardModel card)
    {
        return card.Keywords.Contains(Stitch);
    }

    public static bool IsWoven(this CardModel card)
    {
        return card.Keywords.Contains(Woven);
    }

}
