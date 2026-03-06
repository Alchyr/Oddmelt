using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using MegaCrit.Sts2.Core.Logging;
using System.Collections.Generic;
using System;
using BaseLib.Utils;
using BaseLib.Abstracts;

namespace Oddmelt.Nodes;

public partial class NStitchCardHolder : NCardHolder, IPoolable
{
    private static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new("StitchHolder", MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static readonly SpireField<NCard, NStitchCardHolder> HolderForModel = new((node)=>{
        NStitchCardHolder holder = Create();
        node.AddChild(holder);
        holder.Owner = node;
        return holder;
    });
    private static List<NStitchCardHolder> _activeHolders = [];
    
    public static NStitchCardHolder HolderForCard(CardModel model)
    {
        foreach (NStitchCardHolder holder in _activeHolders)
        {
            if (holder.CardModel == model) return holder;
        }
        return null;
    }

    private static readonly string StitchedName = "STITCHED_CARD";
    public static readonly Vector2 StitchedScale = Vector2.One * 0.87f;
    public static readonly Vector2 StitchedPosition = new(0, -74);
    public static readonly Vector2 HoverPosition = new(0, -210);

    private static readonly Rect2 BaseHitbox = new(-150, -211, 300, 422);

    protected override Vector2 HoverScale => Vector2.One;

    public static NStitchCardHolder NewInstanceForPool()
    {
        NStitchCardHolder holder = new();
        holder.Name = StitchedName;

        NButton hitbox = new()
        {
            Name = "Hitbox",
            UniqueNameInOwner = true,
            //normal card "hitbox" is 300x422
            //Just want top part of card as hitbox
            //Position based on full size, but make it smaller
            Size = BaseHitbox.Size,
            Position = BaseHitbox.Position,
            PivotOffset = BaseHitbox.Position * -1,
        };


        holder.AddChild(hitbox);
        hitbox.Owner = holder;

        return holder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static NStitchCardHolder Create()
    {
        return NodePool.Get<NStitchCardHolder>();
    }

    private bool CanHover => CardModel?.Pile is CustomPile customPile //can't hover in play, can't hover in discard/draw/etc screens
                            && customPile.CardShouldBeVisible(CardModel);

    private Vector2 _globalCardPos = Vector2.Zero;
    private Vector2 _lockedPosition = Vector2.Zero;

    public void LockPosition()
    {
        _lockPosition = true;
        _lockedPosition = GlobalPosition;
    }
    private bool _lockPosition = false;


    public NStitchCardHolder Initialize(NCard card)
    {
        Name = StitchedName;
        _globalCardPos = card.GlobalPosition;
        SetCard(card);
        _lockPosition = false;
        card.CardHighlight.AnimHide();
        Scale = StitchedScale;
        ShowBehindParent = true;
        return this;
    }

    public void SetToStitchedPosition()
    {
        Scale = StitchedScale;
        Position = StitchedPosition;
    }

    public override void _Ready()
    {
        ConnectSignals();
    }
    protected override void OnFocus()
    {
        if (CanHover)
        {
            _isHovered = true;

            _hoverTween?.Kill();
            _hoverTween = CreateTween();
            _hoverTween.TweenProperty(this, "scale", HoverScale, 0.5).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
            CreateHoverTips();

            Position = HoverPosition;
            Vector2 baseSize = BaseHitbox.Size;
            _hitbox.SetSize(new Vector2(baseSize.X, baseSize.Y * -(Position.Y / baseSize.Y)));
        }
    }

    protected override void OnUnfocus()
    {
        base.OnUnfocus();
        _hoverTween?.Kill();
        ClearHoverTips();
        if (CanHover)
        {
            _hoverTween = CreateTween();
            _hoverTween.TweenProperty(this, "scale", StitchedScale, 0.5).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);

            Position = StitchedPosition;
            Vector2 baseSize = BaseHitbox.Size;
            _hitbox.SetSize(new Vector2(baseSize.X, baseSize.Y * -(Position.Y / baseSize.Y)));
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Vector2 baseSize = BaseHitbox.Size;
        //avoiding having hitbox if no node
        _hitbox.SetSize(CardNode == null ? Vector2.Zero : new Vector2(baseSize.X, baseSize.Y * -(Position.Y / baseSize.Y)));
        //MainFile.Logger.Info($"Hitbox size: {_hitbox.Size}");

        if (CardNode == null) return;
        CardNode.SetGlobalPosition(_globalCardPos);

        if (CardNode.GetParent() != this)
        {
            //MainFile.Logger.Info("Removing CardNode due to not being parent");
            CardNode = null;
            return;
        }

        Vector2 target = _lockPosition ? _lockedPosition : GlobalPosition;
        Vector2 diff = GlobalPosition - _globalCardPos;
        float speed = Math.Max(diff.Length() * 8, 30f);
        _globalCardPos += diff.Normalized() * (float)Math.Min(delta * speed, diff.Length());
        CardNode.SetGlobalPosition(_globalCardPos);
    }

    public override void _Draw()
    {
        base._Draw();
        //this.DrawDebug(_hitbox);
    }

    protected override void CreateHoverTips()
    {
        if (CardNode != null)
        {
            NHoverTipSet nHoverTipSet = NHoverTipSet.CreateAndShow(this, CardNode.Model.HoverTips);
            nHoverTipSet.SetAlignmentForCardHolder(this);
        }
    }

    /// <summary>
    /// Called by ListenModelChange patch
    /// </summary>
    /// <param name="model"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void StitchedModelChanged(CardModel stitched)
    {
        //should only happen when NCard is created and thus can be very simple

        if (stitched == null)
        {
            //Logger.Info($"No stitched card");
            Reset();
            return;
        }
        if (stitched == CardModel)
        {
            return;
        }

        Logger.Info("NCard has stitched card, attaching new NCard");
        //Can be assumed that NCard does not exist, since this card was just obtained from pool
        //But I don't trust anything...
        NCard cardNode = NCard.FindOnTable(stitched);
        if (cardNode != null)
        {
            Logger.Info("NCard already exists, doing nothing");
            return; //I don't know what's going on here so let this handle how they will
        }
        //For the NCard to already exist it must(?) be in play or something similar

        Logger.Info($"Creating NCard {stitched.Id}");
        cardNode = NCard.Create(stitched);
        cardNode.Position = GlobalPosition;
        Initialize(cardNode);
    }

    public void Reset()
    {
        if (CardNode != null)
        {
            if (IsAncestorOf(CardNode))
            {
                CardNode.QueueFreeSafely();
            }
            CardNode = null;
        }
    }

    void IPoolable.OnInstantiated()
    {

    }

    void IPoolable.OnReturnedFromPool()
    {
        SetToStitchedPosition();
    }

    void IPoolable.OnFreedToPool()
    {
        Reset();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        _activeHolders.Add(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _activeHolders.Remove(this);
    }
}
