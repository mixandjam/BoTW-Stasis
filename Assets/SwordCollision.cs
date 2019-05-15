using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [Header("Settings")]
    public float collisionForce = 15;
    public LayerMask layerMask;
    public Vector3 hitPoint;
    [Space]
    [Header("Particle")]
    public GameObject hitParticle;
    public GameObject stasisHitParticle;


    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (transform.forward * 1f), -transform.forward, out hit,8, layerMask))
        {
            print("hit");
            if (hit.transform.GetComponent<StasisObject>() == null)
                return;

            hitPoint = hit.point;
            hit.transform.GetComponent<StasisObject>().AccumulateForce(collisionForce, hit.point);

            //Hit Particle
            Instantiate(hitParticle, hit.point, Quaternion.identity);

            if (!hit.transform.GetComponent<StasisObject>().activated)
                return;

            //Stasis Hit Particle
            GameObject stasisHit = Instantiate(stasisHitParticle, hit.point, Quaternion.identity);
            ParticleSystem[] stasisParticles = stasisHit.GetComponentsInChildren<ParticleSystem>();

            //Particle color
            foreach(ParticleSystem p in stasisParticles)
            {
                var pmain = p.main;
                pmain.startColor = hit.transform.GetComponent<StasisObject>().particleColor;
            }
            var main = stasisHit.GetComponent<ParticleSystem>().main;
            main.startColor = hit.transform.GetComponent<StasisObject>().particleColor;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new Ray(transform.position + (transform.forward * 1f), -transform.forward);
        Gizmos.DrawRay(ray);

        Gizmos.DrawSphere(hitPoint, .2f);
    }
}
