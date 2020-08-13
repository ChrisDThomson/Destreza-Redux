using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPose : MonoBehaviour
{
    public SpriteRenderer characterArt;
    public SpriteRenderer characterKilledX;

    public ParticleSystem winFlower;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 flowerOffset = Vector3.up * 5;
        winFlower = Instantiate(winFlower, transform.position + flowerOffset, Quaternion.identity, transform);
        
    }


    public void SetCharacterWinPoseArt(Sprite sprite)
    {
        characterArt.sprite = sprite;
    }

    public void CharacterKilledXVisibility(bool isVisible)
    {
        characterKilledX.enabled = isVisible;
    }

    public void SetWinFlowerParticle(Sprite sprite)
    {
        winFlower.textureSheetAnimation.SetSprite(0, sprite);
    }
}
