using UnityEngine;
using Bolt;
using TheForest;
using ModAPI;


namespace Simple_Player_Markers
{
    public class xScene : TheForest.Utils.Scene
    {
        protected override void Awake()
        {
            Simple_Player_Markers.Init();
            base.Awake();
        }
    }

    class Simple_Player_Markers : MonoBehaviour
    {
        public static void Init()
        {
            ModAPI.Log.Write("Application Version : " + Application.version + "  Unity Version: " + Application.unityVersion);
            new GameObject("__Simple_Player_Markers__").AddComponent<Simple_Player_Markers>();
        }

        private void OnEnable()
        {

            ModAPI.Log.Write("Simple_Player_Markers has been enabled");
        }

        private void OnDisable()
        {
            ModAPI.Log.Write("Simple_Player_Markers has been disabled");
        }
        void Start()
        {
            ModAPI.Log.Write("Simple_Player_Markers has been started");
        }
        void Update()
        {
            if (BoltNetwork.isRunning) {
                markerListener();
            }
        }
        private void markerListener()
        {
            if (UnityEngine.Input.GetMouseButtonDown(2))
            {
                RemoveMarker();

                GameObject camera = GameObject.FindWithTag("MainCamera");
                var hits = Physics.RaycastAll(camera.gameObject.transform.position, camera.gameObject.transform.forward, 100.0F);
                // make sure I do not hit the "grabber collision"
                if (hits.Length == 1 && hits[hits.Length - 1].collider.name == "Grabber")
                {
                    return;
                }

                foreach (var _hit in hits)
                {
                    if (_hit.collider.tag.ToLower() == "terrainmain")
                    {
                        ModAPI.Log.Write("Position: " + _hit.transform.position + " - Collider: " + _hit.collider.name + " - Hit Position: " + _hit.point + " - Distance " + _hit.distance + " - Tag " + _hit.collider.tag);
                        PlaceMarker(_hit.point);
                        break;
                    }
                }
            }
        }

        PrefabId markerObject = BoltPrefabs.StickMarkerBuilt;
        BoltEntity marker = null;
        private void PlaceMarker(Vector3 position)
        {

            if (marker != null) return;
            ModAPI.Log.Write("Simple_Player_Marker is Being Set");
            marker = BoltNetwork.Instantiate(markerObject, position, Quaternion.identity);
        }

        private void RemoveMarker()
        {
            if (marker == null) return;
            ModAPI.Log.Write("Simple_Player_Marker is Being Removed");
            BoltNetwork.Destroy(marker);
            marker = null;
        }
    }
}
