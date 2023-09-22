using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Serializable]
    public class VFX
    {
        public string Id;
        public GameObject Value;
    }

    public VFX[] Animations;
    public List<GameObject> AnimationsList = new List<GameObject>();

    public static VFXManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public void Create(string Id, Vector3 position)
    {
        GameObject go = null;
        foreach (VFX vfx in Animations)
        {
            if (vfx.Id == Id)
            {
                go = vfx.Value; break;
            }
        }

        if (go == null)
        {
            return;
        }

        AnimationsList.Add(go);
    }

    private void Update()
    {
        foreach(GameObject g in AnimationsList)
        {
            var animator = g.GetComponent<Animator>();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Blocked") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                g.SetActive(false);
                Destroy(g);
            }
        }


    }
}
