using Jobs.Common;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Jobs.DOD
{
    [RequireComponent(typeof(BoxCollider))]
    public class CubesGenerator : MonoBehaviour
    {
        public GameObject cubeArchetype = null;
        public GameObject targetArea = null;

        [Range(10, 10000)] public int generationTotalNum = 500;
        [Range(1, 60)] public int generationNumPerTicktime = 5;
        [Range(0.1f, 1.0f)] public float tickTime = 0.2f;
        [HideInInspector]
        public Vector3 generatorAreaSize;
        [HideInInspector]
        public Vector3 targetAreaSize;

        //开启collectionChecks时，当外部尝试销毁池内对象时，会触发异常报错
        public bool collectionChecks = true;
        // 对象池
        private ObjectPool<GameObject> pool = null;
        private float timer = 0.0f;


        private TransformAccessArray transformsAccessArray;
        private Transform[] transforms;//需要提前生成全部的对象
        private NativeArray<float3> randTargetPosArray;

        static readonly ProfilerMarker profilerMarker = new ProfilerMarker("CubesMarchWithJob");

        private void Start()
        {
            ///创建对象池
            if (pool == null)
                pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                    OnDestroyPoolObject, collectionChecks, 10, generationTotalNum);

            generatorAreaSize = GetComponent<BoxCollider>().size;
            if (targetArea != null)
                targetAreaSize = targetArea.GetComponent<BoxCollider>().size;

            timer = 0.0f;

            //首先预先生成所有cube 记录下起始点和终点 并且添加到TransformAccessArray
            randTargetPosArray = new NativeArray<float3>(generationTotalNum, Allocator.Persistent);
            transforms = new Transform[generationTotalNum];
            for (int i = 0; i < generationTotalNum; i++)
            {
                GameObject cube = pool.Get();
                var component = cube.AddComponent<AutoReturnToPool>();
                component.pool = pool;

                Vector3 randGenerationPos = transform.position + new Vector3(Random.Range(-generatorAreaSize.x * 0.5f, generatorAreaSize.x * 0.5f),
                   0,
                   Random.Range(-generatorAreaSize.z * 0.5f, generatorAreaSize.z * 0.5f));
                component.generationPos = randGenerationPos;
                cube.transform.position = randGenerationPos;

                Vector3 randTargetPos = targetArea.transform.position + new Vector3(Random.Range(-targetAreaSize.x * 0.5f, targetAreaSize.x * 0.5f),
                   0, Random.Range(-targetAreaSize.z * 0.5f, targetAreaSize.z * 0.5f));
                component.targetPos = randTargetPos;
                randTargetPosArray[i] = randTargetPos;

                transforms[i] = cube.transform;
            }
            transformsAccessArray = new TransformAccessArray(transforms);

            for (int i = generationTotalNum - 1; i >= 0; i--)
            {
                pool.Release(transforms[i].gameObject);
            }
        }

        private void Update()
        {
            using (profilerMarker.Auto())
            {
                var autoRotateAndMoveJob = new AutoRotateAndMoveJob();
                autoRotateAndMoveJob.randTargetPosArray = randTargetPosArray;
                autoRotateAndMoveJob.deltaTime = Time.deltaTime;
                autoRotateAndMoveJob.moveSpeed = 5.0f;
                autoRotateAndMoveJob.rotateSpeed = 180.0f;
                JobHandle autoRotateAndMoveJobJobHandle =
                    autoRotateAndMoveJob.Schedule(transformsAccessArray);
                autoRotateAndMoveJobJobHandle.Complete();

                if (timer >= tickTime)
                {
                    GenerateCubes();
                    timer -= tickTime;
                }

                timer += Time.deltaTime;
            }

        }

        private void GenerateCubes()
        {
            if (!cubeArchetype || pool == null)
                return;
            for (int i = 0; i < generationNumPerTicktime; ++i)
            {
                if (pool.CountActive < generationTotalNum)
                {
                    pool.Get();
                }
                else
                {
                    timer = 0;
                    return;
                }
            }
        }

        GameObject CreatePooledItem()
        {
            return Instantiate(cubeArchetype, transform);
        }

        void OnReturnedToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        void OnTakeFromPool(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        void OnDestroyPoolObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}
