using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FlyPath : MonoBehaviour
{
    public float speed = 0.5f;
    public float rotationSpeed = 0.5f;

    public Transform target;
    public int currentPath;
    public List<Transform> flyPathPositions;
    public GameObject player;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) > 1)
        {
            if (Vector3.Distance(transform.position, flyPathPositions[currentPath].position) > 5)
            {
                transform.Translate(Vector3.forward * speed);

                Vector3 targetDirection = flyPathPositions[currentPath].position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = rotationSpeed * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

            else
            {
                CalculateNearestFlyPath();
            }
        }

        else
        {
            player.transform.position = transform.position;
            player.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<FirstPersonController>().enabled = true;
            this.gameObject.SetActive(false);
        }
    }
    void CalculateNearestFlyPath()
    {

        if (currentPath+1 < flyPathPositions.Count && Vector3.Distance(target.position, flyPathPositions[currentPath + 1].position) < Vector3.Distance(transform.position, target.position))
        {
            currentPath += 1;
        }

        else if (currentPath - 1 > 0)
        {
            currentPath -= 1;
        }

        else
        {
            currentPath = flyPathPositions.Count - 1;
        }
    }


}
