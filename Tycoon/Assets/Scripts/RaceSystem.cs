using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CodeMonkey.Utils;

public class RaceSystem : MonoBehaviour
{
    public static RaceSystem instance;

    public List<Vector3> roads;
    public Vector3 startFinish;
    public PlacedObjectTypeSO.Dir trackDir;
    public PlacedObjectTypeSO.Dir startFinishDir;

    public GameObject raceCar;

    public float timeForOneMove;

    void Awake() { instance = this; }

    public void Race()
    {
        if(RaceTrackIsComplete())
        {
            raceCar.SetActive(true);
            raceCar.transform.position = startFinish;
            StartCoroutine(RaceStakRoutine());
        }
        else
        {
            UtilsClass.CreateWorldTextPopup("Race Track Is Not Complete", startFinish);
        }
    }

    public bool RaceTrackIsComplete()
    {
        if(roads.Count() > 1 && Vector3.Distance(startFinish, roads[0]) == 10 && Vector3.Distance(startFinish, roads.LastOrDefault()) == 10)
        {
            return true;
        }
        return false;
    }

    IEnumerator RaceStakRoutine ()
    {
        int count = 0;
        foreach(var i in roads)
        {
            StartCoroutine(MoveOverSeconds(raceCar, roads[count], timeForOneMove));
            count += 1;
            yield return new WaitForSeconds(timeForOneMove);
        }
        StartCoroutine(MoveOverSeconds(raceCar, startFinish, timeForOneMove));
    }

    IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}
