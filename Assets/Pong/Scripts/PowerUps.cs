using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pong.Scripts
{
    public class PowerUps : MonoBehaviour
    {
    
        public GameObject apwerUp;
    
        private TrailRenderer _tail;
        private MeshRenderer _visible;
        
        public float minSize = 0.5f;
        public float maxSize = 1.5f;
        public float oscillationSpeed = 1.0f;
        private Vector3 _initialScale;
    
        public int ballHits;
        public bool ballEffect;
        public bool ballOscillate;
        
        public List<GameObject> spawnedPowerUps = new List<GameObject>();
    
        private void Start()
        {
            _tail = GetComponent<TrailRenderer>();
            _visible  = GetComponent<MeshRenderer>();
            _initialScale = transform.localScale;

        }

        void Update()
        {
            if (ballOscillate)
            {
                float scale = Mathf.PingPong(Time.time * oscillationSpeed, 1.0f);
                transform.localScale = Vector3.Lerp(Vector3.one * minSize, Vector3.one * maxSize, scale);
            }
        }


        private void OnCollisionEnter()
        {
            if (ballEffect)
            {
                RemoveBallEffect();
            }
        
            ballHits++;
        
            if(spawnedPowerUps.Count < 2 && !ballEffect)
            {
                ballHits = 0; // Reset ball hits
                SpawnPowerUp();
            }    
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                ballHits = 0;
                InvisiBall();
                ballOscillate = true;
                Destroy(other.gameObject);
                spawnedPowerUps.Remove(other.gameObject);
            }
        }
    
    
        void SpawnPowerUp()
        {
            float randomZ = Random.Range(4.5f, -4.5f);
            Vector3 spawn = new Vector3(0, 0, randomZ);
            GameObject powerUp = Instantiate(apwerUp, spawn, Quaternion.identity);
            spawnedPowerUps.Add(powerUp);
        }

        void InvisiBall()
        {
            _visible.enabled = false;
            _tail.enabled = false;
            ballEffect = true;
        }

        public void resetOnGoal()
        {
            ballHits = 0;
            ballOscillate = false;
            transform.localScale = _initialScale;

            if (ballEffect)
            {
                RemoveBallEffect();
            }
            foreach (var powerUp in spawnedPowerUps)
            {
                Destroy(powerUp);
            }
            spawnedPowerUps.Clear();
            
        }

        public void RemoveBallEffect()
        {
            _visible.enabled = true;
            _tail.enabled = true;
            ballEffect = false;
        }
        
    }
}

            
    

