
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace VRSaber
{
    public class BreakableBox : UdonSharpBehaviour
    {
        public GameObject BoxObject;
        private Material _boxMaterial;
        private Color _defaultColor;

        public GameObject TextObject;
        //private TMPro.TMP_Text _textMesh;

        void Start()
        {
            Debug.Log("[BreakableBox] Box is here");
            _boxMaterial = BoxObject.GetComponent<MeshRenderer>().material;
            _defaultColor = _boxMaterial.color;

            //_textMesh = TextObject.GetComponent<TMPro.TMP_Text>();
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"[BreakableBox] enter {collision.gameObject.name}");
            if (collision.gameObject.name == "Capsule")
            {
                _boxMaterial.color = Color.red;
                //_textMesh.text = ;
                //_textMesh.SetText($"HIT BY {collision}");
            }
            
        }

        void OnCollisionExit(Collision collision)
        {
            Debug.Log($"[BreakableBox] exit {collision.gameObject.name}");
            if (collision.gameObject.name == "Capsule")
            {
                _boxMaterial.color = _defaultColor;
                //_textMesh.text = "";
                //_textMesh.SetText("");
            }
        }
    }
}