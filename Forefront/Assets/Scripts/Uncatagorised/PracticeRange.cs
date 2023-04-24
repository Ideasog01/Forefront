using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeRange : MonoBehaviour
{
    //Manages the resetting of defeated bots in the main menu practice range

    public void RespawnBot(DroneEntity entity)
    {
        StartCoroutine(DelayRespawnBot(entity));
    }

    private IEnumerator DelayRespawnBot(DroneEntity entity)
    {
        yield return new WaitForSeconds(3);
        entity.gameObject.SetActive(true);
        entity.ResetEnemy(entity.transform.position);
    }
}
