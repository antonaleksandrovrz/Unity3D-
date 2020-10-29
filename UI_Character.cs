using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    [SerializeField] public bool alive = true;
    [SerializeField] public int lifeRemaining = 4;
    [SerializeField] GameObject[] lifes;
    [SerializeField] public Text score;
    [SerializeField] GameObject spawn;
    GameObject character;
    Animator anim;
    void Start()
    {
        character = transform.GetChild(0).gameObject;
        anim = character.GetComponent<Animator>();

        if (spawn == null)
        {
            spawn = GameObject.FindGameObjectWithTag("Spawn");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "GoldCoin")
        {
            score.text = (int.Parse(score.text) + 5).ToString();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SilverCoin")
        {
            score.text = (int.Parse(score.text) + 1).ToString();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            GetDamage(1);
        }
    }

    public void GetDamage(int damage)
    {
        character.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(ReviveFromDamage(0.2f));
        lifeRemaining -= damage;

        if (lifeRemaining >= 0)
        {
            RefreshUI();
        }

        else
        {
            anim.SetTrigger("death");
            alive = false;
            GetComponent<Movement>().enabled = false;
            spawn.GetComponent<Spawn>().Respawn();
        }
    }

    public void RefreshUI()
    {
        
        for (int i = lifeRemaining; i < lifes.Length; i++)
        {
            lifes[i].active = false;
        }
    }
    private IEnumerator ReviveFromDamage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        character.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
