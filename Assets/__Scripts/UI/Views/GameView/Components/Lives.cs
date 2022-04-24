using UnityEngine;
using System.Collections.Generic;
public class Lives : MonoBehaviour
{
    private List<GameObject> _hearts;

    private void Start() 
    {
        _hearts = new List<GameObject>();

        foreach (Transform child in transform)
        {
            _hearts.Add(child.gameObject);
        }
    }

    public void UpdateLives(int ndx, bool addLife)
    {   
        if(addLife)
        {
            _hearts[ndx].SetActive(true);
        }
        else 
        {
            _hearts[ndx].SetActive(false);
        }
    }
}
