using System;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }
        private Bounds mapBounds;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
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
            //mapBounds = new Bounds((min + max) / 2, max - min + new Vector3(5, 0, 5));
            Vector3 center = (min + max) / 2;
            Vector3 size = max - min;
            
            mapBounds = new Bounds(center, size);
            //Debug.Log($"✅ Map Bounds Calculated: Center = {mapBounds.center}, Size = {mapBounds.size}");
        }
        
        public Bounds GetMapBounds() => mapBounds;
    }
}
