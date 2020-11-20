namespace AmongUsClone.Shared
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
    }
}
