#if HDPipeline && UNITY_2021_2_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ProceduralWorlds.HDRPTOD
{
    [ExecuteAlways]
    public class HDRPTimeOfDayReflectionProbeSync : MonoBehaviour
    {
        public float m_waitBeforeRefreshTime = 4.5f;
        public ReflectionProbe m_reflectionProbe;
        public HDAdditionalReflectionData m_reflectionProbeData;

        [SerializeField] private HDRPTimeOfDay m_timeOfDaySystem;
        private float m_generatedWaitTime = 1f;

        private void Start()
        {
            Setup();
        }
        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                Setup();
            }
        }
        private void OnDisable()
        {
            if (m_timeOfDaySystem)
            {
                m_timeOfDaySystem.UnRegisterReflectionProbe(this);
            }
        }
        private void OnDestroy()
        {
            if (m_timeOfDaySystem)
            {
                m_timeOfDaySystem.UnRegisterReflectionProbe(this);
            }
        }

        public void StartSync()
        {
            StopSync();
            if (m_reflectionProbe != null && m_reflectionProbeData != null)
            {
                m_generatedWaitTime = UnityEngine.Random.Range(m_waitBeforeRefreshTime / 2f, m_waitBeforeRefreshTime);
                m_reflectionProbeData.mode = ProbeSettings.Mode.Realtime;
                m_reflectionProbeData.realtimeMode = ProbeSettings.RealtimeMode.OnDemand;
                StartCoroutine(RefreshAllProbes(m_reflectionProbe));
            }
        }
        public void StopSync()
        {
            StopAllCoroutines();
        }

        public void Setup()
        {
            if (m_reflectionProbe == null)
            {
                m_reflectionProbe = GetComponent<ReflectionProbe>();
            }

            if (m_reflectionProbe != null)
            {
                if (m_reflectionProbeData == null)
                {
                    m_reflectionProbeData = m_reflectionProbe.GetComponent<HDAdditionalReflectionData>();
                }

                m_reflectionProbe.RequestRenderNextUpdate();
            }

            if (m_timeOfDaySystem == null)
            {
                m_timeOfDaySystem = HDRPTimeOfDay.Instance;
            }
            if (m_timeOfDaySystem)
            {
                m_timeOfDaySystem.RegisterReflectionProbe(this);
            }
        }
        /// <summary>
        /// Refreshes all the probes over time
        /// </summary>
        /// <param name="reflectionProbes"></param>
        /// <returns></returns>
        private IEnumerator RefreshAllProbes(ReflectionProbe reflectionProbe)
        {
            while (true)
            {
                yield return new WaitForSeconds(m_generatedWaitTime);
                if (reflectionProbe != null)
                {
                    reflectionProbe.RequestRenderNextUpdate();
                }
                yield return new WaitForEndOfFrame();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(reflectionProbe);
#endif

                StopSync();
            }
        }
    }
}
#endif