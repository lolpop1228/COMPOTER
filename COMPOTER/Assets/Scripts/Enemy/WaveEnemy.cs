using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemy : MonoBehaviour
{
    public GameObject nextWave;
    public float delayTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        nextWave.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            Invoke(nameof(NextWave), delayTime);
        }
    }

    void NextWave()
    {
        nextWave.SetActive(true);
    }
}
