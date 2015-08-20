using UnityEngine;
using System.Collections;
[RequireComponent(typeof(NavMeshAgent))]
public class BaseUnit : MonoBehaviour
{
    [HideInInspector]
    public bool IsSelected;
    public Light SelectIndicator;
    public Color Selected;
    public Color Idle;
    [HideInInspector]
    public NavMeshAgent NavMeshAgent;
    public Transform DestinationMarker;
    public UnitType Type;
    public int MaxHp = 100;
    public int MaxEnergy = 100;
    public int Hp;
    public int Energy;
    public bool SetStatsToMaxAtStart;
    public string UnitName;

    private LineRenderer Line;
    private Transform Target;
    
	// Use this for initialization
	void Start () 
    {
        DestinationMarker.SetParent(null);
        DestinationMarker.gameObject.SetActive(false);
        SelectIndicator.color = IsSelected ? Selected : Idle;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Line = GetComponent<LineRenderer>();
        //StartCoroutine(GetPath());
        if (SetStatsToMaxAtStart)
        {
            Hp = MaxHp;
            Energy = MaxEnergy;
        }
	}

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && GeneralUnitsController.Selection.width > 0 && GeneralUnitsController.Selection.height > 0)
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = GeneralUnitsController.ScreenToRectY(camPos.y);
            IsSelected = GeneralUnitsController.Selection.Contains(camPos);
            if (IsSelected)
            {
                SendMessage("OnSelect");
            }
            else
            {
                SendMessage("OnUnselect");
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GeneralUnitsController.CleanSelections();
            SendMessage("OnSelect");
            UnitInfoPanel.Instance.ShowUnitInfo(this); 
        }
        if (Input.GetMouseButtonUp(1))
        {
            GeneralUnitsController.Attack(this);
        }
    }

    void OnSelect()
    {
        IsSelected = true;
        SelectIndicator.color = Selected;
    }

    void OnUnselect()
    {
        IsSelected = false;
        SelectIndicator.color = Idle;
    }

    IEnumerator GetPath()
    {
        do
        {
            Line.SetPosition(0, transform.position);

            yield return new WaitForEndOfFrame();

            DrawPath(NavMeshAgent.path);
        } while (NavMeshAgent.hasPath || NavMeshAgent.pathPending);
        DestinationMarker.gameObject.SetActive(false);
    }

    void DrawPath(NavMeshPath path)
    {
        if(path.corners.Length < 2)
        {
            return;
        }
        Line.SetVertexCount(path.corners.Length);
        for(var i = 1; i < path.corners.Length; i++)
        {
            Line.SetPosition(i, path.corners[i]);
        }
    }

    void OnDestinationSet()
    {
        StartCoroutine(GetPath());
        DestinationMarker.gameObject.SetActive(true);
        DestinationMarker.position = NavMeshAgent.destination;
    }
}

public enum UnitType
{
    Friendly = 0,
    Hostile = 1
}
