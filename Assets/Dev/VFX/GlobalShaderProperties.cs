using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderProperties : MonoBehaviour
{
    public Vector2 meshOffsetPan = new Vector2(0, 0);

    private float timer;

    // Start is called before the first frame update
    void Start() { }

    public void RetrieveProperties()
    {
        meshOffsetPan = Shader.GetGlobalVector("meshOffsetPan");
    }

    public void UpdateProperties()
    {
        Shader.SetGlobalVector("meshOffsetPan", meshOffsetPan);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > .2)
        {
            timer = 0;
            meshOffsetPan.x = UnityEngine.Random.value;
            meshOffsetPan.y = UnityEngine.Random.value;
            UpdateProperties();
        }
    }
}
