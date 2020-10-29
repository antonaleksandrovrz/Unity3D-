using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuruEvenets : MonoBehaviour
{
    void CanMove()
    {
        transform.parent.GetComponent<Movement>().canMove = true;
    }
}
