
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class FancyWandController : UdonSharpBehaviour
{
    public GameObject Wand;
    private Material _wandMaterial;
    private Color _wandColorDefault;
    void Start()
    {
        if ((Wand != null) && (Wand.activeInHierarchy))
        {
            _wandMaterial = Wand.GetComponent<MeshRenderer>().material;
            _wandColorDefault = _wandMaterial.color;
        }
    }

    public override void OnPickupUseUp()
    {
        if (Wand.activeInHierarchy)
        {
            //Wand.SetActive(false);
            _wandMaterial.SetColor(Wand.GetInstanceID(), Color.green);
        }
    }

    //public override void Interact()
    //{
    //    if (Wand.activeInHierarchy)
    //    {
    //        Wand.SetActive(false);
    //    }
    //}
}
