using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] GameObject bodyAttachPoint;
    [SerializeField] Transform bodyAttachTransform;
    private float rotationSpeed = 5.0f;
    private bool isAssembled = false;
    private bool wasAssembled = false;
    private TurretAttachedObjectRef checkBody;
    private GameObject turretBody;
    private GameObject armAttachPointL;
    private Transform armAttachTransformL;
    private GameObject armAttachPointR;
    private Transform armAttachTransformR;
    private GameObject turretArmL;
    private GameObject turretArmR;

    public void Start()
    {
        // Checks if something is attached to the base
        checkBody = bodyAttachPoint?.GetComponent<TurretAttachedObjectRef>();
    }

    private void Update()
    {
        wasAssembled = isAssembled;
        // Check if turret is assembled
        isAssembled = CheckAssembled();
        // When assembled
        if (isAssembled)
        {
            turnLights("green");
            turnAndShoot();
        }
        else
        {
            // Turret is not fully assembled
            turnLights("red");
            turretBody = null;
            armAttachPointL = null;
            armAttachTransformL = null;
            turretArmL = null;
            armAttachPointR = null;
            armAttachTransformR = null;
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
                armAttachPointL = turretBody?.transform.Find("Arm attach point L")?.gameObject;
                armAttachTransformL = turretBody?.transform.Find("Arm attach transform L");
                TurretAttachedObjectRef checkArmL = armAttachPointL?.GetComponent<TurretAttachedObjectRef>();
                armAttachPointR = turretBody?.transform.Find("Arm attach point R")?.gameObject;
                armAttachTransformR = turretBody?.transform.Find("Arm attach transform R");
                TurretAttachedObjectRef checkArmR = armAttachPointR?.GetComponent<TurretAttachedObjectRef>();
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
        Quaternion targetRotationL = Quaternion.LookRotation(target.position - armAttachTransformL.position);
        armAttachTransformL.rotation = Quaternion.Slerp(armAttachTransformL.rotation, targetRotationL, rotationSpeed * Time.deltaTime);
        Quaternion targetRotationR = Quaternion.LookRotation(target.position - armAttachTransformR.position);
        armAttachTransformR.rotation = Quaternion.Slerp(armAttachTransformR.rotation, targetRotationR, rotationSpeed * Time.deltaTime);
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

    private void turnLights(string status)
    {
        // Get all lights
        List<Transform> lightContainers = new List<Transform>();
        Transform turretBaseLights = transform.Find("Lights");
        if (turretBaseLights != null)
        {
            lightContainers.Add(turretBaseLights);
        }
        if (turretBody != null) {
            Transform turretBodyLights = turretBody?.transform.Find("Lights");
            if (turretBodyLights != null)
            {
                lightContainers.Add(turretBodyLights);
            }
        }
        // For each light group
        foreach (Transform lightsGroup in lightContainers)
        {
            if (lightsGroup != null)
            {
                // Go trhough each light inside the group
                foreach (Transform light in lightsGroup)
                {
                    TurretLightController lightController = light.GetComponent<TurretLightController>();
                    if (lightController != null && status == "green")
                    {
                       lightController.TurnLightGreen(); 
                    }
                    if (lightController != null && status == "red")
                    {
                       lightController.TurnLightRed(); 
                    } 
                }
            }
        }

    }
    
}
