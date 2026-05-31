using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;
    [SerializeField] private List<GameObject> hearts;
    public int health;

    void Start()
    {
        instance = this;
        health = hearts.Count;
    }

    public void TakeDamage()
    {
        if (health == 0) return;

        health--;
        hearts[health].SetActive(false);
        if (health == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        PlayerMovement.instance.SetPlayerAlive(false);
        PlayerMovement.instance.GetAnimator().SetTrigger("Dying");
        Invoke(nameof(ShowGameOverScreen), 0.5f);
    }

    void ShowGameOverScreen()
    {
        GameManager.instance.GameOver();    
    }

    public void ResetHealth()
    {
        health = hearts.Count;
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(true);
        }
    }
}
