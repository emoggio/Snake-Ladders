using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    public GameObject Explosion;
   
    //particle explosion effect at start
    public void StartPlaying()
    {
        Invoke("FXExplosion", 1f);
    }

    //destroy once is played
    public void FXExplosion()
    {
        GameObject explostion = Instantiate(Explosion, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 0.25f);
        Destroy(explostion, 1.5f);
    }

}
