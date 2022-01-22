namespace EventHook.Implementations.Mouse
{
    public enum MouseKeys
    {
        [Key("MouseLeft", "The left mouse button was pressed")]
        Left = 1048576,

        [Key("MouseMiddle", "The middle mouse button was pressed")]
        Middle = 4194304,

        [Key("WheelDown", "The mouse wheel was scroll dowm")]
        MiddleDown = 4194301,

        [Key("WheelUp", "The mouse wheel  was scroll up")]
        MiddleUp = 4194302,

        [Key("None", "No mouse button was pressed")]
        None = 0,

        [Key("MouseRight", "The right mouse button was pressed")]
        Right = 2097152,

        [Key("XButton1", "The first XButton (XBUTTON1) on Microsoft IntelliMouse Explorer was pressed")]
        XButton1 = 8388608,

        [Key("XButton2", "The second XButton (XBUTTON2) on Microsoft IntelliMouse Explorer was pressed")]
        XButton2 = 16777216,

    }
}
