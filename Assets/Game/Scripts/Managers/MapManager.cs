using System;
using Game.Scripts.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }
        private Bounds mapBounds;

        [SerializeField] private MapConfig mapConfig;
        public MapConfig MapConfig => mapConfig;
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            string sceneName = SceneManager.GetActiveScene().name;
            string configPath = "MapConfigs/" + sceneName + "Config";
            mapConfig = Resources.Load<MapConfig>(configPath);
            if (mapConfig is null)
            {
                Debug.LogError($"Map config not found: {configPath}");
            }
            
            CalculateMapBounds();
        }

        private void CalculateMapBounds()
        {
            GameObject[] mapsObjects = GameObject.FindGameObjectsWithTag("Map");
            if (mapsObjects.Length == 0)
            {
                mapBounds = new Bounds(Vector3.zero, new Vector3(100, 1, 100));
                return;
            }
            
            Vector3 min = Vector3.one * float.MaxValue;
            Vector3 max = Vector3.one * float.MinValue;

            foreach (GameObject mapObject in mapsObjects)
            {
                Renderer renderer = mapObject.GetComponent<Renderer>();
                Collider collider = mapObject.GetComponent<Collider>();
                
                if (renderer != null)
                {
                    min = Vector3.Min(min, renderer.bounds.min);
                    max = Vector3.Max(max, renderer.bounds.max);
                }
                else
                {
                    min = Vector3.Min(min, mapObject.transform.position);
                    max = Vector3.Max(max, mapObject.transform.position);
                }
            }
            Vector3 center = (min + max) / 2;
            Vector3 size = max - min;
            
            mapBounds = new Bounds(center, size);
        }
        
        public Bounds GetMapBounds() => mapBounds;
    }
}
