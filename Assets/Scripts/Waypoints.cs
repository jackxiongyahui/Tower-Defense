using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    public static Transform[] positions;

	/// <summary>
    /// 此处取-1源于自己的失误，自已又太懒，不想修改waypoints的值
    /// </summary>
	void Awake () {
        positions = new Transform[transform.childCount];
        for(int i = 0; i<positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);
        }
	}
	
}
