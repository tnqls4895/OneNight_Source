using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedManager : MonoBehaviour
{

    public RectTransform ClearBed;
    public RectTransform DirtyBed;
    public RectTransform BloodBed;
    public RectTransform LiquidBed;
    public RectTransform Liquid2Bed;
    public RectTransform OneDeadBody;
    public RectTransform TwoDeadBody;

    public RectTransform ClearTrashCan;
    public RectTransform DirtyTrashCan;

    public RectTransform OpenBlind;
    public RectTransform CloseBlind;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClearBed.localScale = Vector2.zero;
        DirtyBed.localScale = Vector2.zero;
        BloodBed.localScale = Vector2.zero;
        LiquidBed.localScale = Vector2.zero;
        Liquid2Bed.localScale = Vector2.zero;
        OneDeadBody.localScale = Vector2.zero;
        TwoDeadBody.localScale = Vector2.zero;

        EBedType currentBedType = GameEventManager.instance.GetBedType();
        switch(currentBedType)
        {

            case EBedType.clear:
                ClearBed.localScale = Vector2.one;
                break;

            case EBedType.dirty:
                DirtyBed.localScale = Vector2.one;
                break;

            case EBedType.liquid:
                LiquidBed.localScale = Vector2.one;
                DirtyBed.localScale = Vector2.one;
                break;

            case EBedType.liquid2:
                Liquid2Bed.localScale = Vector2.one;
                DirtyBed.localScale = Vector2.one;
                break;

            case EBedType.oneDeadBody:
                OneDeadBody.localScale = Vector2.one; 
                DirtyBed.localScale = Vector2.one;
                break;

            case EBedType.twoDeadBody:
                TwoDeadBody.localScale = Vector2.one;
                DirtyBed.localScale = Vector2.one;
                break;
        }

        if (GameEventManager.instance.IsOpenBlind())
        {
           OpenBlind.localScale = Vector2.one;
            CloseBlind.localScale = Vector2.up;
        }
        else
        {
            OpenBlind.localScale = Vector2.up;
            CloseBlind.localScale = Vector2.one;
        }

        if(GameEventManager.instance.isClearTrashCan())
        {
            ClearTrashCan.localScale = Vector2.one; 
            DirtyTrashCan.localScale = Vector2.up;
        }
        else
        {
            ClearTrashCan.localScale = Vector2.up;
            DirtyTrashCan.localScale = Vector2.one;
        }
    }

    public void OnClickedBed()
    {
        GameEventManager.instance.OnClickedBed();
    }

    public void OnClickedBlind()
    {
        GameEventManager.instance.OnClickedBlind();
    }

    public void OnClickedDirtyTrashCan()
    {
        GameEventManager.instance.OnClickedTrashCan();
    }
    public void OnClickedDeadBody()
    {
        GameEventManager.instance.OnClickedBed();
    }
}
