// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class PlayerInput
    {
        public bool moveTop;
        public bool moveLeft;
        public bool moveRight;
        public bool moveBottom;

        public PlayerInput()
        {
        }

        private PlayerInput(bool moveTop, bool moveLeft, bool moveBottom, bool moveRight)
        {
            this.moveTop = moveTop;
            this.moveLeft = moveLeft;
            this.moveBottom = moveBottom;
            this.moveRight = moveRight;
        }

        public static PlayerInput Deserialize(bool[] serializedPlayerInput)
        {
            return new PlayerInput(
                serializedPlayerInput[0],
                serializedPlayerInput[1],
                serializedPlayerInput[2],
                serializedPlayerInput[3]
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

        public PlayerInput Clone()
        {
            return new PlayerInput
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
                   $"w: {SerializeInputValue(moveTop)}, " +
                   $"a: {SerializeInputValue(moveLeft)}, " +
                   $"s: {SerializeInputValue(moveBottom)}, " +
                   $"d: {SerializeInputValue(moveRight)}" +
                   ")";
        }

        private static int SerializeInputValue(bool inputValue)
        {
            return inputValue ? 1 : 0;
        }
    }
}