using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jobs.DOD
{
    public class WaveCubesWithJobs : MonoBehaviour
    {
        public GameObject cubeAchetype = null;
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int zHalfCount = 40;
        private List<Transform> cubesList;
    }
}
