using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    // Classe interne pour repr�senter un effet visuel (VFX)
    [Serializable]
    public class VFX
    {
        public string Id; // Identifiant unique pour le VFX
        public GameObject Value; // R�f�rence � l'objet du VFX
    }

    private float Timer = 0f; // Un compteur, mais il n'est jamais incr�ment� ou utilis� dans ce code
    public VFX[] Animations; // Tableau des VFX disponibles
    // Dictionnaire des VFX existants avec une cl� bas�e sur le temps pour d�terminer quand les d�truire
    private Dictionary<double, GameObject> ExistingVFX = new Dictionary<double, GameObject>();

    public static VFXManager Instance; // Instance singleton de cette classe

    private void Start()
    {
        Instance = this; // Initialisation de l'instance singleton
    }

    private void Update()
    {
        if (ExistingVFX == null) return;

        // R�cup�re la liste des VFX qui doivent �tre d�truits
        var toDelete = ExistingVFX.Where(v => v.Key >= Timer).ToList();

        // Met � jour le dictionnaire pour ne conserver que les VFX qui ne doivent pas �tre d�truits
        ExistingVFX = ExistingVFX.Where(v => v.Key < Timer).ToDictionary(v => v.Key, v => v.Value);

        // D�truit les VFX de la liste toDelete
        foreach (var v in toDelete)
        {
            Destroy(v.Value);
        }

        Timer += Time.deltaTime;
    }

    // Cr�e un VFX � une position donn�e en utilisant son identifiant
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

        ExistingVFX.Add(Time.time + UnityEngine.Random.Range(0.85f, 1.25f), Instantiate(go, position, Quaternion.identity));
    }
}
