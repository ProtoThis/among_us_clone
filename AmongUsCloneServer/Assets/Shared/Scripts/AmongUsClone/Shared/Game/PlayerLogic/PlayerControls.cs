// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class PlayerControls
    {
        public bool moveTop;
        public bool moveLeft;
        public bool moveRight;
        public bool moveBottom;

        public PlayerControls()
        {
        }

        private PlayerControls(bool moveTop, bool moveLeft, bool moveBottom, bool moveRight)
        {
            this.moveTop = moveTop;
            this.moveLeft = moveLeft;
            this.moveBottom = moveBottom;
            this.moveRight = moveRight;
        }

        public static PlayerControls Deserialize(bool[] serializedPlayerControls)
        {
            return new PlayerControls(
                serializedPlayerControls[0],
                serializedPlayerControls[1],
                serializedPlayerControls[2],
                serializedPlayerControls[3]
            );
        }

        public bool[] Serialize()
        {
            return new[]
            {
                moveTop,
                moveLeft,
                moveBottom,
                moveRight
            };
        }

        public PlayerControls Clone()
        {
            return new PlayerControls
            {
                moveTop = moveTop,
                moveBottom = moveBottom,
                moveLeft = moveLeft,
                moveRight = moveRight
            };
        }

        public override string ToString()
        {
            return "(" +
                   $"w: {SerializeControlValue(moveTop)}, " +
                   $"a: {SerializeControlValue(moveLeft)}, " +
                   $"s: {SerializeControlValue(moveBottom)}, " +
                   $"d: {SerializeControlValue(moveRight)}" +
                   ")";
        }

        private static int SerializeControlValue(bool controlValue)
        {
            return controlValue ? 1 : 0;
        }
    }
}
