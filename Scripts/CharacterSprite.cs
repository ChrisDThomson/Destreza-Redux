using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    protected ChestHitCheck chestHitCheck;
    public SpriteRenderer spriteRenderer;

    public delegate void OnFatalHit();
    public OnFatalHit onFatalHit;

    public delegate void OnCanActChanged(bool canAct);
    public OnCanActChanged onCanActChanged;
    protected bool canAct;

    public delegate void OnMoveAnimationEvent(MoveAnimationInfo moveAnimationInfo);
    public OnMoveAnimationEvent onMoveAnimationEvent;

    public delegate void OnSoundPlayAnimationEvent(SoundClip soundClip);
    public OnSoundPlayAnimationEvent onPlaySoundClip;

    public delegate void OnPowerUp();
    public OnPowerUp onPowerUp;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        chestHitCheck = GetComponentInChildren<ChestHitCheck>();
        if (chestHitCheck)
            chestHitCheck.onFatalHit += WasFatallyHit;

    }

    public void CanAct(bool isAbleToAct)
    {
        canAct = isAbleToAct;
        if (onCanActChanged != null)
            onCanActChanged(canAct);
    }


    // I don't like this but all the animation events have already been placed
    // so it's a huge timesaver
    public void CanActFalse()
    {
        CanAct(false);
    }

    public void CanActTrue()
    {
        CanAct(true);
    }

    public void WasFatallyHit()
    {
        spriteRenderer.sortingLayerName = "Dead";
        if (onFatalHit != null)
            onFatalHit();
    }

    protected virtual void CreatePoof(AnimationEvent myEvent)
    {
        GameObject p;

        if (myEvent.objectReferenceParameter is PoofHandler)
        {
            PoofHandler poof = myEvent.objectReferenceParameter as PoofHandler;
            Poofs poofID = poof.poofID;

            string poofPrefabName = "I have not been set";

            switch (poofID)
            {
                case (Poofs.Poof_Small):
                    poofPrefabName = "SmallPoof";
                    break;
                case (Poofs.Poof_Dash):
                    poofPrefabName = "DashPoof";
                    break;
                case (Poofs.Poof_Run):
                    poofPrefabName = "RunPoof";
                    break;
                case (Poofs.Poof_Jump):
                    poofPrefabName = "JumpPoof";
                    break;
                case (Poofs.Poof_Puff):
                    poofPrefabName = "PuffPoof";
                    break;
            }

            p = Instantiate(Resources.Load(poofPrefabName, typeof(GameObject)), transform.position, transform.rotation) as GameObject;


            Vector3 newScale = p.transform.localScale;
            newScale.x = -transform.localScale.x;

            Vector3 offset = new Vector3(poof.position.x * Mathf.Sign(newScale.x), poof.position.y, poof.position.z);
            p.transform.position = p.transform.position + offset;

            p.transform.localScale = new Vector3(newScale.x * poof.scale, newScale.y * Mathf.Abs(poof.scale), newScale.z * Mathf.Abs(poof.scale));

            p.transform.parent = null;
            p.transform.parent = null;
        }
        else
        {
            //Debug.Assert(myEvent.objectReferenceParameter is PoofHandler);
        }
    }

    public void Move(AnimationEvent myEvent)
    {

        if (myEvent.objectReferenceParameter is MoveAnimationInfo)
        {
            MoveAnimationInfo m = (MoveAnimationInfo)myEvent.objectReferenceParameter;
            {
                if (onMoveAnimationEvent != null)
                    onMoveAnimationEvent(m);
            }
        }
    }

    public void PlaySound(AnimationEvent myEvent)
    {

        if (myEvent.objectReferenceParameter is SoundClip)
        {
            SoundClip s = (SoundClip)myEvent.objectReferenceParameter;
            {
                if (onPlaySoundClip != null)
                    onPlaySoundClip(s);
            }
        }
    }


    //Kept this generic in case in arcade mode we want some enemies to have this property
    public void PowerUp()
    {
        if (onPowerUp != null)
            onPowerUp();
    }
}
