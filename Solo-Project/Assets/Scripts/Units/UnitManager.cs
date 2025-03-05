using UnityEngine;

public class UnitManager : MonoBehaviour
{
   [SerializeField] private Unit[] unitList;
    [SerializeField] private int numUnits = 10;
    [SerializeField] private GameObject leaderObject;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private PlayerMovement leaderMovement;
    
    
    private float randomRange = 10.0f;
    private Transform leaderTransform;
    private float stopDistance = 3;
    private int currentFormationID = 0;
    private int maxFormationID = 1;
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
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        int i;
        for (i = 0; i < numUnits; i++)
        {
            unitList[i] = Instantiate(unitPrefab, RandomVectorAdditive(leaderTransform.position), leaderObject.transform.rotation, leaderTransform/*this.gameObject.transform*/).GetComponent<Unit>();
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
            if (isBasicFormationActive)
            {
                SteerBehavior(u);
            }
            MimicryBehavior(u);
        }
    }
    
    private void CheckInput()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     CycleFormations();
        // }
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

            isBasicFormationActive = false;
        
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
                    SwitchToSquareFormation();
                    break;
            }
        }
    }

    private void SwitchToBasicFormation()
    {
        activelySwitchingFormations = true;

        isBasicFormationActive = true;

        activelySwitchingFormations = false; //TODO: Temporarily here
    }

    private void SwitchToSquareFormation()
    {
        activelySwitchingFormations = true;



        float perimeter = (numUnits * (unitList[0].GetRadius() * 2)) - (4 * (2 * unitList[0].GetRadius()));

        float sideWidth = perimeter / 4;

        float sideHeight = (perimeter / 4) + ((unitList[0].GetRadius() * 2) * 2); 
        
        

        
        
        activelySwitchingFormations = false; //TODO: Temporarily here
    }
    
    #endregion
}
