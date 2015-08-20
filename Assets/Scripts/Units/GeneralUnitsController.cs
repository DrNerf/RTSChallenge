using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralUnitsController : MonoBehaviour 
{
    public static GeneralUnitsController Instance;
    private static List<BaseUnit> AllUnits = null;
    private static List<BaseUnit> FriendlyUnits = null;
    private static List<BaseUnit> HostileUnits = null;
    public Texture2D SelectionBox;
    public static Rect Selection = new Rect(0, 0, 0, 0);
    private Vector3 StartSelectPos = -Vector3.one;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        AllUnits = new List<BaseUnit>(FindObjectsOfType<BaseUnit>());
        FriendlyUnits = AllUnits.Where(x => x.Type == UnitType.Friendly).ToList();
        HostileUnits = AllUnits.Where(x => x.Type == UnitType.Hostile).ToList();
	}

    void Update()
    {
        CheckCamera();
        CheckForCommand();
    }

    void CheckForCommand()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var unitsForMovement = FriendlyUnits.Where(x => x.IsSelected).ToList();
            if(unitsForMovement.Any())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                Vector3 destination = Vector3.zero;
                if (Physics.Raycast (ray, out hit)) {
                    destination = hit.point;
                }
                //foreach (var item in AllUnits)
                //{
                //    if (item.IsSelected && destination != Vector3.zero)
                //    {
                //        item.NavMeshAgent.SetDestination(destination);
                //        item.SendMessage("OnDestinationSet");
                //    }
                //}
                if (unitsForMovement.Count > 1)
                {
                    var leader = unitsForMovement[0];
                    Vector3 leaderStartingPosition = leader.transform.position;
                    leader.NavMeshAgent.SetDestination(destination);
                    leader.SendMessage("OnDestinationSet");
                    for (int i = 1; i < unitsForMovement.Count; i++)
                    {
                        Vector3 relativePositionToLeader = unitsForMovement[i].transform.position - leaderStartingPosition;
                        unitsForMovement[i].NavMeshAgent.SetDestination(destination + relativePositionToLeader);
                        unitsForMovement[0].NavMeshAgent.Resume();
                        unitsForMovement[i].SendMessage("OnDestinationSet");
                    }
                }
                else
                {
                    unitsForMovement[0].NavMeshAgent.SetDestination(destination);
                    unitsForMovement[0].NavMeshAgent.Resume();
                    unitsForMovement[0].SendMessage("OnDestinationSet");
                }
            }
        }
    }

    void OnGUI()
    {
        if (StartSelectPos != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.2f);
            GUI.DrawTexture(Selection, SelectionBox);
        }
    }

    void CheckCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSelectPos = Input.mousePosition;
            //UnitInfoPanel.Instance.HideUnitInfo();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (Selection.width < 0)
            {
                Selection.x += Selection.width;
                Selection.width = -Selection.width;
            }
            if (Selection.height < 0)
            {
                Selection.y += Selection.height;
                Selection.height = -Selection.height;
            }
            StartSelectPos = -Vector3.one;
        }

        if (Input.GetMouseButton(0))
        {
            Selection = new Rect(StartSelectPos.x, ScreenToRectY(StartSelectPos.y),
                Input.mousePosition.x - StartSelectPos.x,
                ScreenToRectY(Input.mousePosition.y) - ScreenToRectY(StartSelectPos.y));
        }
    }
	
	// Update is called once per frame
	void OnMouseDown() 
    {
        CleanSelections();
        UnitInfoPanel.Instance.HideUnitInfo();
	}

    public static void CleanSelections()
    {
        if (AllUnits != null)
        {
            foreach (var item in AllUnits)
            {
                item.SendMessage("OnUnselect");
            }
        }
    }

    public static float ScreenToRectY(float y)
    {
        return Screen.height - y;
    }

    public static void Attack(BaseUnit unit)
    {
        foreach (var item in FriendlyUnits.Where(x => x.IsSelected))
        {
            //item.NavMeshAgent.Stop();
            item.SendMessage("OnAttack", unit);
        }
    }
}
