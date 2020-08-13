using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {

    public float speed = 0.5f;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    //void FixedUpdate()
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);

        rend.material.mainTextureOffset = offset;
    }
}
