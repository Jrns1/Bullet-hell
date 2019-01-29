using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour {

    public static List<Transform> interactants = new List<Transform>();
    public Transform pointer;

    const float INTERACTIVE_DISTANCE = .8f;
    readonly Vector3 offset = new Vector3(0, 1, 0);

    private void Update()
    {
        if (GameManager.Ins.isInteractionAllowed)
        {
            int nearest = GetValidNearestInteractant();
            if (nearest >= 0)
            {
                pointer.gameObject.SetActive(true);
                pointer.position = interactants[nearest].position + offset;

                if (Input.GetKeyDown(KeyCode.E))
                    interactants[nearest].GetComponent<InteractiveObject>().Interact();
            }
            else
            {
                pointer.gameObject.SetActive(false);
            }
        }
    }

    int GetValidNearestInteractant()
    {
        int nearest = -1;
        float nearestSqrDis = GameManager.Sqr(INTERACTIVE_DISTANCE);
        for (int i = 0; i < interactants.Count; i++)
        {
            float currentSqrDis = (transform.position - interactants[i].position).sqrMagnitude;
            if (currentSqrDis < nearestSqrDis)
            {
                nearest = i;
                nearestSqrDis = currentSqrDis;
            }
        }

        return nearest;
    }

}
