using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using Random = UnityEngine.Random;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Unit[] unitList;
    [SerializeField] private int numUnits = 10;
    [SerializeField] private GameObject leaderObject;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private PlayerMovement leaderMovement;
    [SerializeField] private Transform leaderObjectTransform;
    [SerializeField] private PlayerMovement playerMovement;

    private int circleRadius = 5;
    
    private InputSystem_Actions inputActions;
    private float randomRange = 10.0f;
    private Transform leaderTransform;
    private float stopDistance = 3;
    private int currentFormationID = 0;
    private int maxFormationID = 2;
    private bool activelySwitchingFormations = false;
    private bool isBasicFormationActive = true;
    
    private void Awake()
    {
        if (leaderObject == null || leaderMovement == null)
        {
            Debug.LogError("No leader object or movement attached to unit manager");
        }
        unitList = new Unit[numUnits];
        leaderTransform = leaderObject.transform;
    }

    private void Start()
    {
        inputActions = InputManager.instance.GetInputActions();
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        int i;
        for (i = 0; i < numUnits; i++)
        {
            unitList[i] = Instantiate(unitPrefab, RandomVectorAdditive(leaderTransform.position), leaderObject.transform.rotation, leaderTransform/*this.gameObject.transform*/).GetComponent<Unit>();
            unitList[i].transform.SetParent(leaderObjectTransform);
        }
    }

    private Vector3 RandomVectorAdditive(Vector3 vector)
    {
        vector.x = vector.x + RandomNumber();
        //vector.y = vector.y + RandomNumber();
        vector.z = vector.z + RandomNumber();
        return vector;
    }

    private float RandomNumber()
    {
        return Random.Range(-randomRange, randomRange);
    }

    private IEnumerator TransformDelay()
    {
        playerMovement.ToggleMovement(true);
        
        yield return new WaitForSeconds(1f);
        
        playerMovement.ToggleMovement(false);
        
        activelySwitchingFormations = false;
    }
    
    //SECTION: Updates
    #region Updates

    private void Update()
    {
        CheckInput();
    }
    
    private void FixedUpdate()
    {
        if (unitList.Length != 0)
        {
            UpdateUnits();
        }
    }
    
    private void UpdateUnits()
    {
        foreach (Unit u in unitList)
        {
            if (currentFormationID == 0)
            {
                SteerBehavior(u);
                MimicryBehavior(u);
            }
            else
            {
                MoveBehavior(u);
            }
            
        }
    }
    
    private void CheckInput()
    {
        bool interact = inputActions.Player.Interact.WasPressedThisFrame(); //inputActions.Player.Interact.triggered;
        
        if (interact)
        {
            Debug.Log("Swapping formations");
            CycleFormations();
            //SwitchToCircleFormation();
        }
    }

    #endregion
    
    //SECTION: Behaviors
    #region UnitBehavoirs
    
    private void SteerBehavior(Unit u)
    {
        u.SteerTowardsLeader(leaderTransform);
    }

    private void MimicryBehavior(Unit u)
    {
        if (leaderMovement.GetIsGrounded())
        {
            u.SetRBDrag(leaderMovement.GetGroundDrag());
        }
        else
        {
            u.SetRBDrag(0);
        }
        
        u.MoveWithLeader(leaderMovement.GetCurrentAppliedForce());
    }

    private void MoveBehavior(Unit u)
    {
        u.MoveTowardsPoint();
    }
    #endregion

    //SECTION: Formations
    #region Formations

    private void CycleFormations()
    {
        if (!activelySwitchingFormations)
        {
            currentFormationID++;

            if (currentFormationID > maxFormationID)
            {
                currentFormationID = 0;
            }

            //isBasicFormationActive = false;
            
            StartCoroutine(TransformDelay());
        
            switch(currentFormationID)
            {
                case 0: 
                    //Basic Formation
                    //Debug.Log("Switching to Basic formation");
                    SwitchToBasicFormation();
                    break;
                case 1:
                    //Square Formation
                    //Debug.Log("Switching to square formation");
                    SwitchToCircleFormation();
                    break;
                case 2:
                    SwitchToCubeFormation();
                    break;
            }
        }
    }

    private void SwitchToBasicFormation()
    {
        activelySwitchingFormations = true;

        isBasicFormationActive = true;

        int i;
        for (i = 0; i < unitList.Length; i++)
        {
            unitList[i].ResetVelocity();
            unitList[i].ToggleKinematics(false);
            unitList[i].ToggleMove(false);

        }
        
    }

    private void SwitchToCubeFormation()
    {
        activelySwitchingFormations = true;
        
        float radius = unitList[0].GetRadius();
        
        float diameter = radius * 2;
        
        float volume = unitList.Length * diameter;
        
        // Volume of a cube: V = a^3
        // Length = cube root of the volume

        int length = (int) MathF.Ceiling(Mathf.Pow(unitList.Length, 1f / 3f)); // Cube root of the volume
        //int unitLength = (int)(length / radius);
        
        float cubeUnitSize = length * diameter;
        
        Vector3 center = leaderTransform.position;
        
        //center.y = leaderTransform.position.y + (length * diameter * 0.5f);
        
        // Start at the bottom left
        Vector3 startingPosition = center;
        startingPosition.x = leaderTransform.position.x - (cubeUnitSize * 0.5f) + radius;
        startingPosition.y = leaderTransform.position.y;
        startingPosition.z = leaderTransform.position.z - (cubeUnitSize * 0.5f) + radius;
        
        
        if (unitList.Length <= 0)
        {
            return;
        }

        int i, j, k, counter = 0;
        for (i = 0; i < length; i++)
        {
            for (j = 0; j < length; j++)
            {
                for (k = 0; k < length; k++)
                {
                    Vector3 newPosition = startingPosition;
                    newPosition.x = startingPosition.x + (i * diameter); //+ (radius * (length % radius))));
                    newPosition.y = startingPosition.y + (j * diameter); // + (radius * (length % radius))); //(length / diameter)
                    newPosition.z = startingPosition.z + (k * diameter); //+ (radius * (length % radius))));
                    
                    
                    if (counter >= unitList.Length)
                    {
                        break;
                    }
                    else
                    {
                        unitList[counter].ToggleKinematics(false);
                        unitList[counter].SetDestination(newPosition);
                        unitList[counter].ToggleMove(true);
                        //unitList[counter].transform.position = newPosition;
                        counter++;
                    }
                }
                if (counter >= unitList.Length)
                {
                    break;
                }
            }
            if (counter >= unitList.Length)
            {
                break;
            }
        }
        
    }

    private void SwitchToCircleFormation()
    {
        activelySwitchingFormations = true;
        if (isBasicFormationActive)
        {
            isBasicFormationActive = false;
            int i;
            for (i = 0; i < unitList.Length; i++)
            {
                Vector3 targetPosition;
                targetPosition.x = leaderTransform.position.x + circleRadius * Mathf.Cos(2 * Mathf.PI * i / unitList.Length);
                targetPosition.y = leaderTransform.position.y;
                targetPosition.z = leaderTransform.position.z + circleRadius * Math.Sin(2 * Mathf.PI * i / unitList.Length).ConvertTo<float>();

                unitList[i].ToggleKinematics(false);
                unitList[i].ResetVelocity();
                //unitList[i].ToggleKinematics(true);
                unitList[i].SetDestination(targetPosition);
                unitList[i].ToggleMove(true);
                //unitList[i].transform.position = targetPosition;
            }
        }
        else
        {
            isBasicFormationActive = true;
        }
    }
    
    #endregion
}
