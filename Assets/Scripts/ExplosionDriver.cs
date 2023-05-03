using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDriver : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Vector3[] originalPositions;
    Vector3[] originalRotations;

    public bool Exploded { get; private set; } = false;
    public bool chainExplosion;
    public List<ExplosionDriver> explosionChain;
    public MoreMountains.Feedbacks.MMF_Player miniSplosion;
    private Coroutine explosionChainCoroutine = null;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        ResetExplosion();
    }

    public void Initialize()
    {
        rigidbodies = new Rigidbody[transform.childCount];
        originalPositions = new Vector3[transform.childCount];
        originalRotations = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            Rigidbody x = transform.GetChild(i).GetComponent<Rigidbody>();
            rigidbodies[i] = x;
            originalPositions[i] = x != null ? x.transform.position : Vector3.zero;
            originalRotations[i] = x != null ? x.transform.rotation.eulerAngles : Vector3.zero;
        }

        Exploded = false;
    }

    public void Explode(Vector3 explosionOrigin, float intensityMod = 1f)
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            if (rigidbodies[i] != null)
            {
                rigidbodies[i].isKinematic = false;
                rigidbodies[i].AddExplosionForce(Settings.Instance.explosionForce * intensityMod, explosionOrigin, Settings.Instance.explosionRadius, Settings.Instance.explosionUpwardForce, ForceMode.Impulse);
            }
        }

        if (chainExplosion)
        {
            if (explosionChainCoroutine != null) StopCoroutine(explosionChainCoroutine);
            explosionChainCoroutine = StartCoroutine(ChainExplosion());
        }

        Exploded = true;
    }

    public void ResetExplosion()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (rigidbodies[i] != null)
            {
                rigidbodies[i].isKinematic = true;
                rigidbodies[i].transform.position = originalPositions[i];
                rigidbodies[i].transform.eulerAngles = originalRotations[i];
            }
        }

        if (chainExplosion && explosionChainCoroutine != null)
        {
            StopCoroutine(explosionChainCoroutine);
            foreach (var ex in explosionChain) ex.ResetExplosion();
        }

        Exploded = false;
    }

    public IEnumerator ChainExplosion()
    {
        yield return new WaitForSeconds(1.8f);
        foreach(var driver in explosionChain)
        {
            driver.Explode(transform.position);
            miniSplosion?.PlayFeedbacks();
            yield return new WaitForSeconds(.15f);
        }
    }
}
