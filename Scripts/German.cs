using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class German : Character
{
    float blockTimer = 0;
    float blockTime = 2.5f;

    float powerTimer = 0;
    float powerTime = 1.5f;

    bool isPoweredUp;
    bool wasHitDuringPoweredUp;

    protected override void Start()
    {
        base.Start();

        if (characterSprite)
            characterSprite.onPowerUp += PowerUp;
    }
    //Empty so we don't move - as the german
    protected override void HandleMovement() { }

    protected override void Update()
    {
        base.Update();

        //If I have to make one more timer I'll make a class
        if (blockTimer != 0)
        {
            if (blockTimer > 0)
            {
                blockTimer -= Time.deltaTime;
            }
            else if (blockTimer <= 0)
            {
                animator.SetBool("blockFail", true);
                blockTimer = 0;
            }
        }

        if (powerTimer != 0)
        {
            if (powerTimer > 0)
            {
                powerTimer -= Time.deltaTime;
            }
            else if (powerTimer <= 0)
            {
                powerTimer = 0;
                isPoweredUp = false;
                Kill();
            }
        }
    }

    public override void Move(Vector2 input)
    {
        base.Move(input);

        //Not great but will do for now 
        float playerXDirection = Mathf.Sign(input.x) * -transform.localScale.x;
        if (playerXDirection == -1)
        {
            if (blockTimer == 0)
                blockTimer = blockTime;
        }
        else if (playerXDirection >= 0)
        {
            if (blockTimer != 0)
            {
                animator.SetBool("blockFail", false);
                blockTimer = 0;
            }
        }
    }

    void PowerUp()
    {
        isPoweredUp = true;
        powerTimer = powerTime;
    }


    public override void Kill()
    {
        if (isPoweredUp)
        {
            wasHitDuringPoweredUp = true;
            return;
        }
        else
        {
            base.Kill();
        }
    }
}
