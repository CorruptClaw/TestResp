using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using UnityEngine;


namespace Corrupt.Claw
{
    [CreateAssetMenu(fileName = "List of Sprite Assets", menuName = "List of Sprite Assets", order = 0)]
    public class ListOfSpriteAssets : ScriptableObject
    {
        public List<SpriteAsset> SpriteAssets;

    }

}

