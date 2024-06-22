using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLightController : MonoBehaviour
{
    public Material materialLightRed;
    public Material materialLightGreen;
    public GameObject lightObject;
    private Renderer lightRenderer;

    void Start()
    {
        lightRenderer = lightObject.GetComponent<Renderer>();
    }

    public void TurnLightRed()
    {
        lightRenderer.material = materialLightRed;
    }

    public void TurnLightGreen()
    {
        lightRenderer.material = materialLightGreen;
    }
}

