
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace VRSaber
{
    public class TriggerPoint : UdonSharpBehaviour
    {
        public GameObject SaberPrefab;
        public Transform SpawnPoint;

        private GameObject[] _saberPool = new GameObject[3];

        void Start()
        {

        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            base.OnPlayerTriggerEnter(player);
            int index = -1;
            if ((SaberPrefab != null) && (IsPoolOpen(out index)))
            {
                var saberPrefab = Object.Instantiate(SaberPrefab);
                saberPrefab.name = $"Saber_0{index}";
                saberPrefab.transform.position = SpawnPoint.position;
                _saberPool[index] = saberPrefab;
            }
            else if ((SaberPrefab != null) && (!IsPoolOpen(out index)))
            {
                var playerPosition = player.GetPosition();
                var saberToReturn = FindFurthestSaber(playerPosition);
                saberToReturn.transform.position = SpawnPoint.position;
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            base.OnPlayerTriggerExit(player);
        }

        private bool IsPoolOpen(out int objectIndex)
        {
            for (int index = 0; index < _saberPool.Length; ++index)
            {
                if (_saberPool[index] == null)
                {
                    objectIndex = index;
                    return true;
                }
            }

            objectIndex = -1;
            return false;
        }

        private GameObject FindFurthestSaber(Vector3 playerPosition)
        {
            GameObject furthestSaber = null;
            for (int index = 0; index < _saberPool.Length; ++index)
            {
                if (furthestSaber == null)
                {
                    furthestSaber = _saberPool[index];
                }
                else
                {
                    furthestSaber = GetFurtherGameObject(furthestSaber, _saberPool[index], playerPosition);
                }
            }

            return furthestSaber;
        }

        private GameObject GetFurtherGameObject(GameObject current, GameObject prospect, Vector3 referencePoint)
        {
            GameObject further = Vector3.Distance(current.transform.position, referencePoint) >= Vector3.Distance(prospect.transform.position, referencePoint) ? current : prospect;
            return further;
        }
    }
}