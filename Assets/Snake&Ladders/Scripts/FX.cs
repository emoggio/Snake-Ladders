using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    public GameObject Explosion;
   
    public void StartPlaying()
    {
        Invoke("FXExplosion", 2.5f);
    }

    public void FXExplosion()
    {
        GameObject explostion = Instantiate(Explosion, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 0.25f);
        Destroy(explostion, 1.5f);
    }

}
