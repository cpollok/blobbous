using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "fileName.asset", menuName = "WaveConfig")]
public class WaveConfig : ScriptableObject {
    [System.Serializable]
    public class WaveConfigEntry {
        public EnemyType type;
        public float spawnChance;
    }

    public float maxDuration;
    public WaveConfigEntry[] waveConfigs;
}
