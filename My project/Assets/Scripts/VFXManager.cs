using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    // Classe interne pour représenter un effet visuel (VFX)
    [Serializable]
    public class VFX
    {
        public string Id; // Identifiant unique pour le VFX
        public GameObject Value; // Référence à l'objet du VFX
    }

    private float Timer = 0f; // Un compteur, mais il n'est jamais incrémenté ou utilisé dans ce code
    public VFX[] Animations; // Tableau des VFX disponibles
    // Dictionnaire des VFX existants avec une clé basée sur le temps pour déterminer quand les détruire
    private Dictionary<double, GameObject> ExistingVFX = new Dictionary<double, GameObject>();

    public static VFXManager Instance; // Instance singleton de cette classe

    private void Start()
    {
        Instance = this; // Initialisation de l'instance singleton
    }

    private void Update()
    {
        if (ExistingVFX == null) return;

        // Récupère la liste des VFX qui doivent être détruits
        var toDelete = ExistingVFX.Where(v => v.Key >= Timer).ToList();

        // Met à jour le dictionnaire pour ne conserver que les VFX qui ne doivent pas être détruits
        ExistingVFX = ExistingVFX.Where(v => v.Key < Timer).ToDictionary(v => v.Key, v => v.Value);

        // Détruit les VFX de la liste toDelete
        foreach (var v in toDelete)
        {
            Destroy(v.Value);
        }

        Timer += Time.deltaTime;
    }

    // Crée un VFX à une position donnée en utilisant son identifiant
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
