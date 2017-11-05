using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;

    //表示当前选择的炮台（要建造的炮台）
    private TurretData selectedTurretData;

    //表示当前选择的炮台（场景中的游戏物体）
    private MapCube selectedMapCube;

    public Text moneyText;

    public Animator moneyanimator;

    private  int money = 1000;

    //炮台升级按钮的显示与隐藏
    public GameObject upgradeCanvas;
    public Button buttonUpgrade;

    //升级按钮播放动画
    private Animator upgaradeCanvasAnimator;

    

    public void OnLaserSelected(bool isOn)
    {
        if(isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }

    public void OnMissleSelected(bool isOn)
    {
        if(isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }

    public void OnStandardSelected(bool isOn)
    {
        if(isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "$" + money;
    }

	// Use this for initialization
	void Start () {
        upgaradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>(); //得到升级的动画
	}
	
	// Update is called once per frame
	void Update () {
        //鼠标点击
		if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject()==false )
            {
                //开发炮台的建造
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray,out hit, 1000, LayerMask.GetMask("MapCube"));
                if(isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    if(mapCube.turretGo==null && selectedTurretData != null)
                    {
                        //可以创建
                        if(money>selectedTurretData.cost)
                        {
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);
                            
                        }
                        else
                        {
                            //提示钱不够了
                            moneyanimator.SetTrigger("Flicker");
                        }
                    }
                    else if(mapCube.turretGo!=null)  //当mapCube上有炮台时
                    {
                        //升级处理
                       
                        if(mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI()); //播放隐藏动画
                        }else
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
                }
            }
        }
	}

    //显示升级按钮
    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade=false)
    {
        //StopCoroutine(HideUpgradeUI());
        StopCoroutine("HideUpgradeUI");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);  //状态机重新进行初始化
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;  //设置按钮为可以显示的
    }

    //隐藏升级按钮
    IEnumerator HideUpgradeUI()
    {
        upgaradeCanvasAnimator.SetTrigger("Hide");    
        yield return new WaitForSeconds(0.8f);
        upgradeCanvas.SetActive(false);
    }

    //点击升级按钮时进行的操作
    public void OnUpgradeButtonDown()
    {
        if(money>=selectedMapCube.turretData.costUpgraded)
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);  //减去升级所用的金钱
            selectedMapCube.UpgradeTurret();
            StartCoroutine(HideUpgradeUI());   //隐藏升级按钮
        }
        else
        {
            //提示钱不够了
            moneyanimator.SetTrigger("Flicker");
            StartCoroutine(HideUpgradeUI());
        }
        
    }

    public void OnDestroyButtonDown()
    {
        selectedMapCube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());
    }
}
