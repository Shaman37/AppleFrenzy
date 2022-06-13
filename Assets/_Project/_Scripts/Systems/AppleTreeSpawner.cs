using UnityEngine;
using System.Collections.Generic;
using AppleFrenzy.Core;

namespace AppleFrenzy
{
    /// <summary>
    ///     Responsible for spawning new Apple Trees based on the game's current difficulty.
    ///     Also holds a reference to the spawned trees.
    /// </summary>
    public class AppleTreeSpawner : MonoBehaviour
    {   
        #region Fields
    
        [SerializeField] private Transform _applesAnchor;
        [SerializeField] private AppleTree _prefabAppleTree;
        [SerializeField] private List<AppleTree> _appleTrees;
    
        private float speedModifier = 0.5f;
    
        #endregion
    
        #region [1] - Unity Event Methods
        private void Start() 
        {
            _appleTrees = new List<AppleTree>();

            InstantiateNewTree();
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
        ///     Responsible for increasing the game's difficulty, either by increasing the 
        ///     spawned tree's current speed or by instatiating a new Apple Tree.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="level">
        ///         The current game level.
        ///     </param>
        /// </parameters>
        private void IncreaseDifficulty(int level)
        {
            for (int i = 0; i < _appleTrees.Count; i++)
            {
                AppleTree tree = _appleTrees[i];
                float newVelocity = tree.velocity + i * speedModifier;
                tree.velocity = newVelocity;
                tree.waitingTimeModifier += i * 0.5f;
            }
    
            if (level % 2 == 0) InstantiateNewTree();
        }
    
        /// <summary>
        ///     Responsible for instantiating a new Apple Tree.
        /// </summary>
        private void InstantiateNewTree()
        {   
            AppleTree newTree = Instantiate(_prefabAppleTree);
        
            // Calculate a new position offseted from an existing Apple Tree
            Vector3 pos = newTree.transform.position;
            if (_appleTrees.Count > 0)
            {
                AppleTree randomTree = _appleTrees[Random.Range(0, _appleTrees.Count)];
                pos = randomTree.transform.position;
                pos.x *= 0.5f;
            }
            
            newTree.transform.SetParent(transform);
            newTree.applesAnchor = _applesAnchor;
            newTree.waitingTimeModifier += Random.Range(0.5f ,_appleTrees.Count + 1f);
            newTree.transform.position = pos;
            
            _appleTrees.Add(newTree);
        }
    }
}