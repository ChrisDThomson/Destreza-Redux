using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour
{
    protected Vector2 pInput;
    protected Vector3 velocity;
    protected float moveSpeed = 5;
    protected bool canAct = true;
    protected Rigidbody2D body;
    protected Collider2D col;
    protected AudioSource source;
    public Animator animator;
    public CharacterSprite characterSprite;

    public delegate void OnCharacterStateChanged(int playerIndex);
    public OnCharacterStateChanged onKilled;
    public OnCharacterStateChanged onPlayerHitWall;

    public bool disableInput;
    public bool isDead;

    protected int playerIndex;
    protected PlayerInputInfo playerInputInfo;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        characterSprite = GetComponentInChildren<CharacterSprite>();
        if (characterSprite)
        {
            characterSprite.onFatalHit += Kill;
            characterSprite.onCanActChanged += CanAct;
            characterSprite.onMoveAnimationEvent += MoveAnimationEvent;
        }
    }

    private void OnDisable()
    {
        characterSprite.onFatalHit -= Kill;
        characterSprite.onCanActChanged -= CanAct;
        characterSprite.onMoveAnimationEvent -= MoveAnimationEvent;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead)
            return;
        // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length * 60);

        HandleMovement();

    }

    protected void FixedUpdate()
    {
        body.velocity = velocity;
    }

    public virtual void Kill()
    {
        col.enabled = false;
        body.isKinematic = true;

        isDead = true;
        pInput = Vector3.zero;
        velocity = Vector3.zero;

        animator.SetTrigger("dead");

        if (onKilled != null)
            onKilled(playerIndex);
    }

    protected virtual void HitWallFront() { }
    protected virtual void HitWallBack() { }

    protected virtual void HandleMovement()
    {
        velocity = Vector3.zero;
        if (Mathf.Abs(pInput.x) > 0.8f && canAct && !disableInput)
            velocity.x = pInput.x * moveSpeed;
    }

    public void CanAct(bool isAbleToAct)
    {
        canAct = isAbleToAct;
    }

    public void MoveAnimationEvent(MoveAnimationInfo info)
    {
        Vector3 moveAmount = info.moveAmount;
        if (moveAmount.x != 0)
            moveAmount.x *= -transform.localScale.x;

        transform.position += moveAmount;

        //This may look bad on the surface but this has actually been a blessing
        //The less we use the physics the higher change that rollback will be easier
        //This can easily be refactored to check the stage limits with a scriptable object - it's fine for now
        float frontDir = Mathf.Sign(transform.localScale.x);
        if (transform.position.x > 7)
        {
            //Check which side the character is collidSing on
            if (frontDir == 1)
                HitWallBack();
            else if (frontDir == -1)
                HitWallFront();
        }
        else if (transform.position.x < -7)
        {
            //Check which side the character is colliding on
            if (frontDir == 1)
                HitWallFront();
            else if (frontDir == -1)
                HitWallBack();
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -7, 7);
        transform.position = pos;
    }

    void PlaySound(SoundClip clip)
    {
        if (source)
        {
            AudioClip c = clip.GetClip();
            source.PlayOneShot(c);
        }
    }

    #region Inputs

    public void BindPlayerInputs(ref PlayerInputInfo inputInfo, int index)
    {
        inputInfo.players[index].onPlayerMoved += Move;

        inputInfo.players[index].onUpButtonPressed += OnUpButton;
        inputInfo.players[index].onUpButtonReleased += OnUpButtonReleased;

        inputInfo.players[index].onDownButtonPressed += OnDownButton;
        inputInfo.players[index].onDownButtonReleased += OnDownButtonReleased;

        inputInfo.players[index].onLeftButtonPressed += OnLeftButton;
        inputInfo.players[index].onLeftButtonReleased += OnLeftButtonReleased;

        inputInfo.players[index].onRightButtonPressed += OnRightButton;
        inputInfo.players[index].onRightButtonReleased += OnRightButtonReleased;

        playerIndex = index;
        playerInputInfo = inputInfo;
    }

    public void UnBindPlayerInputs(ref PlayerInputInfo inputInfo, int index)
    {
        playerInputInfo.players[playerIndex].onPlayerMoved -= Move;

        playerInputInfo.players[playerIndex].onUpButtonPressed -= OnUpButton;
        playerInputInfo.players[playerIndex].onUpButtonReleased -= OnUpButtonReleased;

        playerInputInfo.players[playerIndex].onDownButtonPressed -= OnDownButton;
        playerInputInfo.players[playerIndex].onDownButtonReleased -= OnDownButtonReleased;

        playerInputInfo.players[playerIndex].onLeftButtonPressed -= OnLeftButton;
        playerInputInfo.players[playerIndex].onLeftButtonReleased -= OnLeftButtonReleased;

        playerInputInfo.players[playerIndex].onRightButtonPressed -= OnRightButton;
        playerInputInfo.players[playerIndex].onRightButtonReleased -= OnRightButtonReleased;

        ResetStates();
    }

    public virtual void Move(Vector2 input)
    {
        if (disableInput)
            return;

        pInput = input;

        //A little Hacky but will do for now - this is due to the character naturally facing in the P2 dir
        float directionMod = Mathf.Sign(transform.localScale.x) * -1;

        animator.SetInteger("horizontal", Mathf.RoundToInt(pInput.x * directionMod));
        animator.SetInteger("vertical", Mathf.RoundToInt(pInput.y));
    }

    public virtual void OnUpButton()
    {
        if (disableInput)
            return;

        animator.SetBool("upButton", true);
    }

    public void OnUpButtonReleased()
    {
        animator.SetBool("upButton", false);
    }

    public void OnDownButton()
    {
        if (disableInput)
            return;

        animator.SetBool("downButton", true);
    }

    public void OnDownButtonReleased()
    {
        animator.SetBool("downButton", false);
    }

    public virtual void OnLeftButton()
    {
        if (disableInput)
            return;

        animator.SetBool("leftButton", true);
    }

    public void OnLeftButtonReleased()
    {
        animator.SetBool("leftButton", false);
    }

    public void OnRightButton()
    {
        if (disableInput)
            return;

        animator.SetBool("rightButton", true);
    }
    public void OnRightButtonReleased()
    {
        animator.SetBool("rightButton", false);
    }

    #endregion

    void ResetStates()
    {
        body.velocity = pInput = Vector3.zero;

        animator.SetInteger("horizontal", 0);
        animator.SetInteger("vertical", 0);

        animator.SetBool("rightButton", false);
        animator.SetBool("leftButton", false);
        animator.SetBool("upButton", false);
        animator.SetBool("downButton", false);
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}

[System.Serializable]
public enum Characters
{
    Bornu,
    Spaniard,
    Turc,
    Chinese,
    Indian,
    Japanese,
    German,
    Random

}
