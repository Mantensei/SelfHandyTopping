using System;
using System.Collections;
using System.Collections.Generic;
using MantenseiLib;
using Unity.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace MedalGame
{
    [CommonReference]
    public class MedalLoader : MonoBehaviour
    {
        MedalGameReferenceHub Hub => MedalGameReferenceHub.Instance;

        MedalManager manager => Hub.MedalManager;

        [SerializeField] Transform _spawnPoint;
        float randomForce => Random.Range(-1f, 1f);
        Vector2 RandomVec => new Vector2(randomForce, randomForce);
        Queue<Medal> _medalPool = new();
        [SerializeField] float _delay = 0.5f;

        public event Action<Medal> OnGenerateAnyMedal;
        public event Action<Medal> OnPoolAnyMedal;
        public event Action<Medal> OnDeleteAnyMedal;
        public event Action OnPoolAllMedal;
        // public event Action OnDeleteAllMedal;

        public void Pool(Medal medal)
        {
            medal.gameObject.SetActive(false);
            _medalPool.Enqueue(medal);
            OnPoolAnyMedal?.Invoke(medal);

            CheckAllMedalsCompleted();
        }

        public void DestroyMedal(Medal medal)
        {
            manager.UnregisterMedal(medal);
            Destroy(medal.gameObject);
            OnDeleteAnyMedal?.Invoke(medal);
            CheckAllMedalsCompleted();
        }

        void CheckAllMedalsCompleted()
        {
            if (manager.Medals.IsNullOrEmptyArray())
            {
                // OnDeleteAllMedal?.Invoke();
                Hub.GameManager.GameOver();
            }
            else if (manager.Medals.All(x => !x.IsSafe() || !x.gameObject.activeSelf))
            {
                OnPoolAllMedal?.Invoke();
            }
        }

        public void ShootPooledMedals()
        {
            int count = _medalPool.Count;
            for (int i = 0; i < count; i++)
            {
                ShootDelayed(_medalPool.Dequeue(), i * _delay);
            }
        }

        void ShootDelayed(Medal medal, float delay)
        {
            IEnumerator ShootDelayedCoroutine(Medal medal, float delay)
            {
                yield return new WaitForSeconds(delay);
                InitializeMedal(medal);
            }
            StartCoroutine(ShootDelayedCoroutine(medal, delay));
        }

        void InitializeMedal(Medal medal)
        {
            if (!medal.IsSafe()) return;

            medal.transform.position = _spawnPoint.position;
            medal.gameObject.SetActive(true);

            if (medal.TryGetComponent<IMoverProvider>(out var mover))
            {
                var vec = RandomVec;
                var moveCommand = new MoveCommand(vec);
                mover.Reference.Move(moveCommand);
            }
        }

        public void GenerateDelayed(Medal medalPrefab, Action<Medal> onComplete = null)
            => GenerateDelayed(medalPrefab, _delay, onComplete);
        public void GenerateDelayed(Medal medalPrefab, float delay, Action<Medal> onComplete = null)
        {
            IEnumerator GenerateDelayedCoroutine(Medal medalPrefab, float delay)
            {
                yield return new WaitForSeconds(delay);
                var medal = Generate(medalPrefab);
                onComplete?.Invoke(medal);
            }
            StartCoroutine(GenerateDelayedCoroutine(medalPrefab, delay));
        }

        public Medal Generate(Medal medalPrefab)
        {
            var medal = Instantiate(medalPrefab, _spawnPoint.position, Quaternion.identity);
            InitializeMedal(medal);
            OnGenerateAnyMedal?.Invoke(medal);

            return medal;
        }
    }
}
