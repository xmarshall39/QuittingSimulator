using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Linq;
using System;

[System.Serializable]
public struct Explodable
{
    public string name;
    public GameObject contianer;
    public GameObject mainBody;
    public ExplosionDriver substitue;
    public MMF_Player explodeFeedback;
}

public class MeshExploder : MonoBehaviour
{
    string[] alphanum;

    public Rigidbody templateRB;
    public MMF_Player raisePowerFeedback, lowerPowerFeedback;

    public Explodable[] explodables;
    private int currentExplodable = 0;

#if UNITY_EDITOR
    [ContextMenu("Setup Explosion Drivers")]
    public void SetupExplosionDrivers()
    {
        foreach(var explodable in explodables)
        {
            if (templateRB != null)
            {
                for (int i = 0; i < explodable.substitue.transform.childCount; ++i)
                {
                    GameObject go = explodable.substitue.transform.GetChild(i).gameObject;
                    Rigidbody rb = go.GetComponent<Rigidbody>() == null ? go.AddComponent<Rigidbody>() : go.GetComponent<Rigidbody>();
                    UnityEditor.EditorUtility.CopySerialized(templateRB, rb);
                    Settings.CopyComponent<Rigidbody>(templateRB, go, unique: true);
                }
            }

            else
            {
                Debug.LogWarning("Cannot complete setup! Must initialize all fields");
            }
        }
    }
#endif
    public void Explode(float modifier)
    {
        explodables[currentExplodable].mainBody.SetActive(false);
        explodables[currentExplodable].substitue.gameObject.SetActive(true);
        explodables[currentExplodable].substitue.Explode(transform.position, intensityMod: 1 + modifier);
        explodables[currentExplodable].explodeFeedback?.PlayFeedbacks();
    }

    public void ResetExplosion()
    {
        explodables[currentExplodable].mainBody.SetActive(true);
        explodables[currentExplodable].substitue.gameObject.SetActive(false);
    }
    
    public void ActivateCurrentExplodable()
    {
        for(int i = 0; i < explodables.Length; ++i)
        {
            if (i == currentExplodable)
            {
                explodables[i].contianer.SetActive(true);
                explodables[i].mainBody.SetActive(true);
                explodables[i].substitue.gameObject.SetActive(false);
            }

            else
            {
                explodables[i].contianer.SetActive(false);
                explodables[i].mainBody.SetActive(false);
                explodables[i].substitue.gameObject.SetActive(false);
            }
        }
        
    }

    public bool SetExplodable(int index)
    {
        if (index < explodables.Length && index >= 0) { currentExplodable = index; ActivateCurrentExplodable(); return true; }
        return false;
    }

    public bool SetExplodable(string name)
    {
        for (int i = 0; i < explodables.Length; ++i) if (explodables[i].name == name) { currentExplodable = i; ActivateCurrentExplodable(); return true; }
        return false;
    }

    private void Start()
    {
        alphanum = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => ((char)i).ToString()).ToArray();
        if (explodables.Length <= 0) Debug.LogError("No Exploables in Array!!");
        ActivateCurrentExplodable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            int hits = 0;
            foreach (string letter in alphanum)
            {
                if (Input.GetKey(letter))
                {
                    ++hits;
                }
            }

            int secretModifier = Input.GetKey(KeyCode.Tilde) ? 0 : 100;
            float inverseSecretModifier = Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) ? 0.001f : 1f;

            Explode(hits * Settings.Instance.extraKeyModifier * secretModifier * inverseSecretModifier);
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetExplosion();
        }

        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            SetExplodable(currentExplodable == 0 ? explodables.Length - 1 : currentExplodable - 1);
        }

        else if (Input.GetKeyDown(KeyCode.Period))
        {
            SetExplodable(currentExplodable == explodables.Length - 1 ? 0 : currentExplodable + 1);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Settings.Instance.explosionForce = Mathf.Clamp(Settings.Instance.explosionForce + 0.25f, 0.01f, 25);
            raisePowerFeedback?.PlayFeedbacks();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Settings.Instance.explosionForce = Mathf.Clamp(Settings.Instance.explosionForce - 0.25f, 0.01f, 25);
            lowerPowerFeedback?.PlayFeedbacks();
        }
    }
}
