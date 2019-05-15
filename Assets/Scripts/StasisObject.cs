using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StasisObject : MonoBehaviour
{
    private Rigidbody rb;
    private TrailRenderer trail;

    public bool activated;
    public Color particleColor;

    private float forceLimit = 100;
    public float accumulatedForce;
    public Vector3 direction;
    public Vector3 hitPoint;

    private Color normalColor;
    private Color finalColor;

    private Transform arrow;
    private Renderer renderer;

    [Header("Particles")]
    public Transform startparticleGroup;
    public Transform endParticleGroup;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        normalColor = FindObjectOfType<StasisCharacter>().normalColor;
        finalColor = FindObjectOfType<StasisCharacter>().finalColor;
        particleColor = normalColor;
        arrow = transform.GetChild(0);
        renderer = GetComponent<Renderer>();
    }

    public void SetStasis(bool state)
    {
        print("setstasis");
        activated = state;
        rb.isKinematic = state;
        float noise = state ? 1 : 0;

        renderer.material.SetFloat("_StasisAmount", .2f);
        renderer.material.SetFloat("_NoiseAmount", noise);

        if (state)
        {
            renderer.material.SetColor("_EmissionColor", normalColor);
            StartCoroutine(StasisWait());


            startparticleGroup.LookAt(FindObjectOfType<StasisCharacter>().transform);
            ParticleSystem[] particles = startparticleGroup.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem p in particles)
            {
                p.Play();
            }
        }

        if(!state)
        {
            StopAllCoroutines();
            DOTween.KillAll();
            transform.GetChild(0).gameObject.SetActive(state);
            renderer.material.SetFloat("_StasisAmount", 0);

            ParticleSystem[] particles = endParticleGroup.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in particles)
            {
                var pmain = p.main;
                pmain.startColor = particleColor;
                p.Play();
            }

            if (accumulatedForce < 0)
            {
                return;
            }

            direction = transform.position - hitPoint;
            rb.AddForceAtPosition(direction * accumulatedForce, hitPoint, ForceMode.Impulse);
            accumulatedForce = 0;

            trail.startColor = particleColor; trail.endColor = particleColor;
            trail.emitting = true;
            trail.DOTime(0, 5).OnComplete(()=>trail.emitting = false);
        }
    }

    public void AccumulateForce(float amount, Vector3 point)
    {
        if (!activated)
            return;

        arrow.gameObject.SetActive(true);
        float scale = Mathf.Min(arrow.localScale.z + .3f, 1.8f);
        transform.GetChild(0).DOScaleZ(scale, .15f).SetEase(Ease.OutBack);

        accumulatedForce = Mathf.Min(accumulatedForce += amount, forceLimit);
        hitPoint = point;

        direction = transform.position - hitPoint;
        transform.GetChild(0).rotation = Quaternion.LookRotation(direction);

        Color c = Color.Lerp(normalColor, finalColor, accumulatedForce/50);
        transform.GetChild(0).GetComponentInChildren<Renderer>().material.SetColor("_Color",c);
        renderer.material.SetColor("_EmissionColor", c);
        particleColor = c;
    }

    public IEnumerator StasisWait()
    {
        for (int i = 0; i < 20; i++)
        {
            float wait = 1;

            if (i > 4)
                wait = .5f;

            if (i > 12)
                wait = .25f;

            yield return new WaitForSeconds(wait);
            Sequence s = DOTween.Sequence();
            s.Append(renderer.material.DOFloat(.5f, "_StasisAmount", .05f));
            s.AppendInterval(.1f);
            s.Append(renderer.material.DOFloat(.2f, "_StasisAmount", .05f));
        }

        SetStasis(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - direction);
    }
}
