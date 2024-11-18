using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Corrupt.Claw
{
    [RequireComponent(typeof(Text))]

    public class NewMonoBehaviourScript : MonoBehaviour
    {

        [TextArea(2, 3)]
        [SerializeField] private string message = "Press BUTTOMPROMPT to shoot.";

        [Header("Setup for Sprites")]
        [SerializeField] private ListOfSpriteAssets listOfSpriteAssets;
        [SerializeField] private DeviceType deviceType;


        private PlayerInput _playerInput;
        private Text _textBox;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }






        private void Start()
        {
            SetText();
        }


        [ContextMenu(itemName: "Set Text")]
        private void SetText()
        {

        }




















        private enum DeviceType
        {
            Keyboard = 0

        }

    }
}

