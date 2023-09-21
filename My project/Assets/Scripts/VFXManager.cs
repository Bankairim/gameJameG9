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

    public float Timer = 0f;
    public VFX[] Animations;
    public Dictionary<double, GameObject> ExistingVFX = new Dictionary<double, GameObject>();

    public static VFXManager Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        var toDelete = ExistingVFX.Where(v => v.Key >= Timer).ToList();
        ExistingVFX = ExistingVFX.Where(v => v.Key < Timer) as Dictionary<double, GameObject>;

        foreach(var v in toDelete)
        {
            Destroy(v.Value);
        }
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

        ExistingVFX.Add(Time.deltaTime + 1f, Instantiate(go, position, Quaternion.identity));
    }
}
