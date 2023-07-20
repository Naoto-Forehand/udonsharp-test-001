
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
//using System.Collections;

namespace VRSaber
{
    public enum SABER_STATE
    {
        INACTIVE = 0,
        ACTIVE = 1
    }

    public class FancyWandController : UdonSharpBehaviour
    {
        public GameObject Wand;
        public GameObject Saber;
        public float Duration = 1f;
        private const float DEFAULT_DURATION = 1f;
        private Material _wandMaterial;
        private Material _saberMaterial;
        private Color _wandColorDefault;
        //private Coroutine _colorLerp;

        private bool _colorToInvoked = false;
        private Color _targetColor = Color.clear;
        private float _targetIncrement = 0f;
        private float _targetDuration = DEFAULT_DURATION;
        private SABER_STATE _saberState = SABER_STATE.INACTIVE;

        void Start()
        {
            Debug.Log($"[FancyWand] Wand Exists ? {(Wand != null)}");
            if ((Wand != null) && (Wand.activeInHierarchy))
            {
                _wandMaterial = this.Wand.GetComponent<MeshRenderer>().material;
                _wandColorDefault = this._wandMaterial.color;
                Debug.Log($"[FancyWand] default color is {_wandColorDefault}");
            }

            if (Saber != null)
            {
                _saberMaterial = this.Saber.GetComponent<MeshRenderer>().material;
                _saberMaterial.color = Color.green;
            }
        }

        void Update()
        {
            if (_colorToInvoked && _targetIncrement < _targetDuration && _wandMaterial.color != _targetColor)
            {
                //while (_targetIncrement < _targetDuration)
                //{
                Debug.Log($"[FancyWand] {_targetColor}");
                //var currentColor = _wandMaterial.color;
                //_wandMaterial.color = Color.Lerp(currentColor, _targetColor, (_targetIncrement / _targetDuration));
                StepColor(_targetColor, _targetIncrement, _targetDuration);
                _targetIncrement += Time.fixedDeltaTime;
                Debug.Log($"[FancyWand] {_targetIncrement}");
                //if (_wandMaterial.color == _targetColor)
                //{
                //    ResetLoop();
                //}

                //}

                //if (_wandMaterial.color != _targetColor)
                //{
                //    ResetLoop();
                //}
            }
            else if (_colorToInvoked && _targetIncrement >= _targetDuration)
            {
                Debug.Log("[FancyWand] Time to cut off loop");
                _wandMaterial.color = _targetColor;
                ResetLoop();
            }
        }

        void StartLoop(Color targetColor, float duration = DEFAULT_DURATION)
        {
            if (!_colorToInvoked)
            {
                Debug.Log("[FancyWand] color loop invoked");
                _targetColor = targetColor;
                _targetDuration = duration;
                _colorToInvoked = true;
            }
            else
            {
                Debug.LogWarning("[FancyWand] already invoking color loop");
                if (_targetColor != targetColor)
                {

                }
            }
        }

        void StepColor(Color targetColor, float step, float duration)
        {
            var currentColor = _wandMaterial.color;
            Debug.Log($"[FancyWand] stepping color {currentColor} {step} / {duration}");
            _wandMaterial.color = Color.Lerp(currentColor, targetColor, (step / duration));
        }

        void ResetLoop()
        {
            Debug.Log("[FancyWand] color loop reset");
            _colorToInvoked = false;
            _targetColor = Color.clear;
            _targetIncrement = 0f;
            _targetDuration = DEFAULT_DURATION;
        }

        void UpdateSaber()
        {
            switch (this._saberState)
            {
                case SABER_STATE.INACTIVE:
                    this.Saber.SetActive(true);
                    this._saberState = SABER_STATE.ACTIVE;
                    break;
                case SABER_STATE.ACTIVE:
                    this.Saber.SetActive(false);
                    this._saberState = SABER_STATE.INACTIVE;
                    break;
            }
        }

        public override void InputGrab(bool value, UdonInputEventArgs args)
        {
            Debug.Log($"[FancyWand] value {value} args {args.ToString()} --- InputGrab");
            if (value)
            {
                this._wandMaterial.color = Color.grey;
                //StartLoop(Color.green);
                //DoCoroutine(ColorToLoop(Color.green));
                //_colorLerp = StartCoroutine(ColorToLoop(Color.green));
            }
            else
            {
                Debug.Log("[FancyWand] grab false --- InputGrab");
                if (this._saberState == SABER_STATE.ACTIVE)
                {
                    UpdateSaber();
                }
            }
            base.InputGrab(value, args);
        }

        public override void InputUse(bool value, UdonInputEventArgs args)
        {
            Debug.Log($"[FancyWand] value {value} args {args.ToString()} --- InputUse");
            if (value)
            {
                this._wandMaterial.color = Color.black;
                UpdateSaber();
                //Saber.SetActive(true);
                //StartLoop(Color.black, 0.25f);
                //DoCoroutine(ColorToLoop(Color.black, 0.5f));
                //_colorLerp = StartCoroutine(ColorToLoop(Color.black, 0.5f));
            }
            else
            {
                Debug.Log("[FancyWand] Use false --- InputUse");
                this._wandMaterial.color = Color.cyan;
            }

            base.InputUse(value, args);
        }

        public override void InputDrop(bool value, UdonInputEventArgs args)
        {
            Debug.Log($"[FancyWand] value {value} args {args.ToString()} --- InputDrop");
            if (value)
            {
                this._wandMaterial.color = _wandColorDefault;
                UpdateSaber();
                //Saber.SetActive(false);
                //StartLoop(_wandColorDefault, 0.25f);
                //DoCoroutine(ColorToLoop(_wandColorDefault, 0.5f));
                //_colorLerp = StartCoroutine(ColorToLoop(_wandColorDefault, 0.5f));
            }
            else
            {
                Debug.Log("[FancyWand] drop false --- InputDrop");
            }
            base.InputDrop(value, args);
        }

        public override void OnDrop()
        {
            Debug.Log("[FancyWand] OnDrop -- called");
            this._wandMaterial.color = _wandColorDefault;
            this.Saber.SetActive(false);
            base.OnDrop();
        }

        public override void Interact()
        {
            Debug.Log("[FancyWand] Interact --- called");
            base.Interact();
        }

        public override void OnPickupUseUp()
        {
            Debug.Log("[FancyWand] OnPickupUseUp --- called");
            this._wandMaterial.SetColor(Wand.GetInstanceID(), Color.green);
            base.OnPickupUseUp();
        }

        public override void OnPickup()
        {
            Debug.Log("[FancyWand] OnPickup --- called");
            base.OnPickup();
        }

        public override void OnPickupUseDown()
        {
            Debug.Log("[FancyWand] OnPickupUseDown --- called");
            base.OnPickupUseDown();
        }

        //private void DoCoroutine(IEnumerator coroutine)
        //{
        //    while (coroutine.MoveNext())
        //    {
        //        Debug.Log($"[FancyWand] {coroutine?.Current}");
        //    }
        //}

        //private IEnumerator ColorToLoop(Color colorTarget, float duration = DEFAULT_DURATION)
        //{
        //    float increment = 0f;
        //    while (increment < duration)
        //    {
        //        var currentColor = _wandMaterial.color;
        //        _wandMaterial.color = Color.Lerp(currentColor, colorTarget, (increment / duration));
        //        increment += Time.fixedDeltaTime;
        //        yield return _wandMaterial.color;
        //    }

        //    if (_wandMaterial.color != colorTarget)
        //    {
        //        _wandMaterial.color = colorTarget;
        //    }

        //    yield return null;
        //}

        //public override void Interact()
        //{
        //    if (Wand.activeInHierarchy)
        //    {
        //        Wand.SetActive(false);
        //    }
        //}
    }

}
