using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存每一波敌人生成所需要的属性
/// </summary>
[System.Serializable]
public class Wave  {

    public GameObject enemyPrefab;
    public int count;
    public float rate;
}
