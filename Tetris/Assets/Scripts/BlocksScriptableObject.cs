using UnityEngine;

namespace Tetris.Models
{
    [CreateAssetMenu(fileName = "Blocks Scriptable Object", menuName = "Scriptable Objects/Blocks Scriptable Object", order = -100)]
    public class BlocksScriptableObject : ScriptableObject
    {
        // Serialized Fields
        [Header("Tetrominos Colors")]
        [SerializeField] private Material _background;                                             // BACKGROUND is the color of the empty block
        [SerializeField] private Material _lightBlue;                                              // LIGHT BLUE is the color of the I piece
        [SerializeField] private Material _darkBlue;                                               // DARK BLUE is the color of the J piece
        [SerializeField] private Material _orange;                                                 // ORANGE is the color of the L piece
        [SerializeField] private Material _yellow;                                                 // YELLOW is the color of the O piece
        [SerializeField] private Material _green;                                                  // GREEN is the color of the S piece
        [SerializeField] private Material _purple;                                                 // PURPLE is the color of the T piece
        [SerializeField] private Material _red;                                                    // RED is the color of the Z piece
        [SerializeField] private Material _gray;

        // Properties
        public Material Background { get => _background; set => _background = value; }
        public Material LightBlue { get => _lightBlue; set => _lightBlue = value; }
        public Material DarkBlue { get => _darkBlue; set => _darkBlue = value; }
        public Material Orange { get => _orange; set => _orange = value; }
        public Material Yellow { get => _yellow; set => _yellow = value; }
        public Material Green { get => _green; set => _green = value; }
        public Material Purple { get => _purple; set => _purple = value; }
        public Material Red { get => _red; set => _red = value; }
        public Material Gray { get => _gray; set => _gray = value; }
        public Material[] Materials
        {
            get
            {
                return new Material[8] { _background, _lightBlue, _darkBlue, _orange, _yellow, _green, _purple, _red };
            }
        }

    };
}
