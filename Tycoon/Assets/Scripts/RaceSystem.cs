using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSystem : MonoBehaviour
{
    public static RaceSystem instance;

    public List<Vector3> roads;
    public Vector3 startfinish;

    public GameObject raceCar;

    void Awake() { instance = this; }

    public void Race()
    {
        raceCar.SetActive(true);
        raceCar.transform.position = startfinish;
        StartCoroutine(RaceStakRoutine());
    }

    IEnumerator RaceStakRoutine ()
    {
        int count = 0;
        foreach(var i in roads)
        {
            StartCoroutine(MoveOverSeconds(raceCar, roads[count], 0.5f));
            count += 1;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(MoveOverSeconds(raceCar, startfinish, 0.5f));
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
