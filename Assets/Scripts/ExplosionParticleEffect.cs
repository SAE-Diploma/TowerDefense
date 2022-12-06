using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleEffect : MonoBehaviour
{
    [SerializeField] float lifeTime = 1.0f;
    float counter = 0;

    void Update()
    {
        if (counter >= lifeTime)
        {
            Destroy(gameObject);
        }
        counter += Time.deltaTime;
    }
}
