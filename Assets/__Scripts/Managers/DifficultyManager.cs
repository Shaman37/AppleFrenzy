using UnityEngine;
using System.Collections.Generic;

/// <summary>
///     Class which handles the Apple Tree behaviour.
///     It can move randomly along the X axis and randomly drop Apples.
/// </summary>
public class DifficultyManager : MonoBehaviour
{   
    #region Fields

    [SerializeField] private AppleTree _prefabAppleTree;
    [SerializeField] private List<AppleTree> _appleTrees;

    private float speedModifier = 0.5f;

    #endregion

    #region [1] - Unity Event Methods
    private void Start() 
    {
        _appleTrees = new List<AppleTree>();

        AppleTree tree = Instantiate(_prefabAppleTree);
        tree.transform.SetParent(transform);
        _appleTrees.Add(tree);
    } 

    private void OnEnable()
    {
        Messenger<int>.AddListener(GameEvents.INC_DIFFICULTY, IncreaseDifficulty);
    }
    
    private void OnDisable()
    {
        Messenger<int>.RemoveListener(GameEvents.INC_DIFFICULTY, IncreaseDifficulty);
    }
    
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    private void IncreaseDifficulty(int level)
    {
        for (int i = 0; i < _appleTrees.Count; i++)
        {
            AppleTree tree = _appleTrees[i];
            float newVelocity = tree.velocity + i * speedModifier;
            tree.velocity = newVelocity;
            tree.waitingTimeModifier += i/2;
        }

        if (level % 2 == 0) InstantiateNewTree();
    }

    /// <summary>
    /// 
    /// </summary>
    private void InstantiateNewTree()
    {   
        Vector3 pos = Vector3.zero;
        // Calculate a new position offseted from an existing Apple Tree
        if (_appleTrees.Count > 0)
        {
            AppleTree randomTree = _appleTrees[Random.Range(0, _appleTrees.Count)];
            pos = randomTree.transform.position;
            pos.x /= 2;
        }

        // Instantiate new Tree
        AppleTree newTree = Instantiate(_prefabAppleTree);
        newTree.transform.SetParent(transform);
        newTree.transform.position = pos;
        newTree.waitingTimeModifier += Random.Range(0.5f ,_appleTrees.Count + 1f);
        
        _appleTrees.Add(newTree);
    }
}