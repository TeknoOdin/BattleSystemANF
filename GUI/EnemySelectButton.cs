using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// remember to attach target button to selector then apply that to the when clicked section in the inspector
public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab;
    BattleManager BattleManager = BattleManager.instance;
    public void selectEnemy()
    {
       GameObject.Find("BattleCanvas").GetComponent<UIManager>().EnemySelection(EnemyPrefab);
        
    }
    public void HideSelector()
    {
        if (EnemyPrefab != null)
        {
            Transform selector = FindChildByName(EnemyPrefab.transform, "Selector");
            if (selector != null)
            {
                selector.gameObject.SetActive(false);
            }
        }
    }

    public void ShowSelector()
    {
        if (EnemyPrefab != null)
        {
            Transform selector = FindChildByName(EnemyPrefab.transform, "Selector");
            if (selector != null)
            {
                selector.gameObject.SetActive(true);
            }
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            Transform result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
