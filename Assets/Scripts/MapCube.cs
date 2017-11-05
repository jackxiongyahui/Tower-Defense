using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {

    [HideInInspector]
    public GameObject turretGo; //保存当前cube身上的炮台
    [HideInInspector]  //代表不需要在Inspector面板上进行修改
    public TurretData turretData;   //保存当前的炮台类型   
    [HideInInspector]  
    public bool isUpgraded = false;//炮台是否升级过

    public GameObject buildEffect;

    private Renderer renderer ;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    //建造炮台
    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;
        isUpgraded = false;   //建造炮台前炮台是没有被升级过的
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);  //实例化特效
        Destroy(effect, 1);  //1秒之后销毁
    }

    public void UpgradeTurret()
    {
        if(isUpgraded == true)
        {
            return;
        }
        Destroy(turretGo);
        isUpgraded = true;   //建造炮台前炮台是没有被升级过的
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f); 
    }

    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    /// <summary>
    /// 可能与炮台的攻击范围产生矛盾
    /// </summary>
    private void OnMouseEnter()
    {
        if(turretGo==null&&EventSystem.current.IsPointerOverGameObject()==false)
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

   
}
