using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretManager : MonoBehaviour
{
    [SerializeField] GameObject bodyAttachArea;
    [Tooltip("The turret body attach area (this needs to have a TurretAttachmentPoint component)")]
    [SerializeField] Transform bodyAttachTransform;
    [Tooltip("The turret body attach transfrom")]
    [SerializeField] private TextMeshProUGUI debugScreenText;
    private float rotationSpeed = 5.0f;
    public bool isAssembled = false;
    private bool wasAssembled = false;
    private TurretAttachmentPoint checkBody;
    private GameObject turretBody;
    private GameObject armAttachAreaL;
    private Transform armAttachPointL;
    private GameObject armAttachAreaR;
    private Transform armAttachPointR;
    private GameObject turretArmL;
    private GameObject turretArmR;

    public void Start()
    {
        // Checks if something is attached to the base
        checkBody = bodyAttachArea?.GetComponent<TurretAttachmentPoint>();
    }

    private void Update()
    {
        wasAssembled = isAssembled;
        // Check if turret is assembled
        isAssembled = CheckAssembled();
        // Debug screen
        if (debugScreenText != null)
        {
            DebugTurretManager();
        }
        // When assembled
        if (isAssembled)
        {
            // turnLights("green");
            turnAndShoot();
        }
        else
        {
            // Turret is not fully assembled
            // turnLights("red");
            turretBody = null;
            armAttachAreaL = null;
            armAttachPointL = null;
            turretArmL = null;
            armAttachAreaR = null;
            armAttachPointR = null;
            turretArmR = null;
        }
    }

    // Checks if turret is assembled
    public bool CheckAssembled()
    {
        // Check if the AttachedObjectRef script is attached
        if (checkBody != null)
        {
            // Check for the turretBody
            turretBody = checkBody?.attachedObject;
            if (turretBody != null)
            {
                // Check for left and right arm attach point scripts
                armAttachAreaL = turretBody?.transform.Find("Arm Attach Area L")?.gameObject;
                armAttachPointL = turretBody?.transform.Find("Arm Attach Point L");
                TurretAttachmentPoint checkArmL = armAttachAreaL?.GetComponent<TurretAttachmentPoint>();
                armAttachAreaR = turretBody?.transform.Find("Arm Attach Area R")?.gameObject;
                armAttachPointR = turretBody?.transform.Find("Arm Attach Point R");
                TurretAttachmentPoint checkArmR = armAttachAreaR?.GetComponent<TurretAttachmentPoint>();
                // Check for left and right arms
                if (checkArmL != null && checkArmR != null)
                {
                    turretArmL = checkArmL?.attachedObject;
                    turretArmR = checkArmR?.attachedObject;
                    // If both left and right arms are found
                    if (turretArmL != null && turretArmR != null)
                    {
                        // Turret assembled (both body and arms attached)
                        return true;
                    }
                }
            }
        }
        // Turret is not fully assembled
        return false;
    }

    private void RotateBodyToTarget(Transform target)
    {
        // Rotate body to point at target (only on y axis)
        Vector3 directionToTarget = target.position - transform.position;
        // Ignore vertical rotation
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        // Rotate based on rotation speed
        bodyAttachTransform.rotation = Quaternion.Slerp(bodyAttachTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void RotateArmsToTarget(Transform target)
    {
        // Rotate the arms attach transforms (in the body)
        Quaternion targetRotationL = Quaternion.LookRotation(target.position - armAttachPointL.position);
        armAttachPointL.rotation = Quaternion.Slerp(armAttachPointL.rotation, targetRotationL, rotationSpeed * Time.deltaTime);
        Quaternion targetRotationR = Quaternion.LookRotation(target.position - armAttachPointR.position);
        armAttachPointR.rotation = Quaternion.Slerp(armAttachPointR.rotation, targetRotationR, rotationSpeed * Time.deltaTime);
    }

    private void turnAndShoot()
    {
        Transform target = findClosestEnemy();
        if (target != null)
        {
            // Aim at closest enemy
            RotateBodyToTarget(target);
            RotateArmsToTarget(target);
            // Shoot at enemy
            TurretGun gunScriptL = turretArmL.GetComponent<TurretGun>();
            TurretGun gunScriptR = turretArmR.GetComponent<TurretGun>();
            // Add randomness
            if (Random.Range(0.0f, 1.0f) > 0.97f)
            {
                gunScriptL.FireBullet();
            }
            if (Random.Range(0.0f, 1.0f) > 0.97f)
            {
                gunScriptR.FireBullet();
            }
        }
    }

    private Transform findClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            return null;
        }
        Transform closestEnemy = enemies[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.position);
        for (int i = 1; i < enemies.Length; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestEnemy = enemies[i].transform;
                closestDistance = distanceToEnemy;
            }
        }
        return closestEnemy;
    }

    // private void turnLights(string status)
    // {
    //     // Get all lights
    //     List<Transform> lightContainers = new List<Transform>();
    //     Transform turretBaseLights = transform.Find("Lights");
    //     if (turretBaseLights != null)
    //     {
    //         lightContainers.Add(turretBaseLights);
    //     }
    //     if (turretBody != null) {
    //         Transform turretBodyLights = turretBody?.transform.Find("Lights");
    //         if (turretBodyLights != null)
    //         {
    //             lightContainers.Add(turretBodyLights);
    //         }
    //     }
    //     // For each light group
    //     foreach (Transform lightsGroup in lightContainers)
    //     {
    //         if (lightsGroup != null)
    //         {
    //             // Go trhough each light inside the group
    //             foreach (Transform light in lightsGroup)
    //             {
    //                 TurretLightController lightController = light.GetComponent<TurretLightController>();
    //                 if (lightController != null && status == "green")
    //                 {
    //                    lightController.TurnLightGreen(); 
    //                 }
    //                 if (lightController != null && status == "red")
    //                 {
    //                    lightController.TurnLightRed(); 
    //                 } 
    //             }
    //         }
    //     }

    // }

    private void DebugTurretManager()
    {
        if (debugScreenText != null)
        {
            string debuggingText = "Debug:";
            debuggingText += "\n";
            debuggingText += "\nisAssembled: " + isAssembled.ToString();
            debuggingText += "\nbodyAttached: " + (turretBody != null).ToString();
            debuggingText += "\narmLAttached: " + (turretArmL != null).ToString();
            debuggingText += "\narmRAttached: " + (turretArmR != null).ToString();
            debuggingText += "\n";
            debugScreenText.text = debuggingText;
        }
    }
    
}
