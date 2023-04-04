using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnslavedUnit : MonoBehaviour
{
    public float life = 100f;
    public bool captured = false;

    public float health = 100f;
    public float hunger = 100f;  // added hunger bar
    public float sickness = 0f;  // added sickness stat

    private float dungRestoreAmount = 5f;  // amount of hunger restored by dung
    private float dungConsumeInterval = 10f;  // time interval between dung consumption
    private float sicknessIncreaseAmount = 5f;  // amount of sickness increased by each dung meal

    private float cryOutThreshold = 10f;  // threshold for crying out in pain
    private float cryOutInterval = 10f;  // time interval between cries
    private bool hasCried = false;  // flag to track if enemy has cried recently

    private void Start()
    {
        InvokeRepeating("ConsumeDung", dungConsumeInterval, dungConsumeInterval);
    }

    // Update is called once per frame
    private void Update()
    {
        if (hunger <= 0f)
        {
            health -= Time.deltaTime;
        }

        if (sickness >= 100f)
        {
            health = 0f;
        }

        if (!hasCried && sickness >= cryOutThreshold)
        {
            CryOut();
        }
        if (!captured)
        {
            life -= Time.deltaTime * 10f;
            if (life <= 0f)
            {
                Destroy(gameObject);
            }
            else if (life <= 75f)
            {
                Debug.Log("I'm starting to feel hopeless...");
            }
            else if (life <= 50f)
            {
                Debug.Log("Why even bother? I'm going to die anyway...");
            }
            else if (life <= 25f)
            {
                Debug.Log("I'm worthless... I should just give up...");
            }
            else if (life <= 1f)
            {
                Debug.Log("It's over... everything is over...");
            }
        }
    }

    private void ConsumeDung()
    {
        hunger += dungRestoreAmount;

        if (hunger > 100f)
        {
            hunger = 100f;
        }

        sickness += sicknessIncreaseAmount;
    }

    private void CryOut()
    {
        Debug.Log("Enemy: Aaargh! It hurts!");

        hasCried = true;

        Invoke("ResetCryOut", cryOutInterval);
    }

    private void ResetCryOut()
    {
        hasCried = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            captured = true;
            Debug.Log("I've been captured! Please don't hurt me...");
        }
    }
}