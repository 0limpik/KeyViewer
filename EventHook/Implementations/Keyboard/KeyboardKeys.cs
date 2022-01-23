namespace EventHook.Implementations.Keyboard
{
    ///<Summary>
    ///Клавиши клавиатуры
    ///<para><see href="https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.keys?view=windowsdesktop-5.0">Список клавиш</see></para>
    ///</Summary>
    public enum KeyboardKeys
    {
        [Key("Modifiers", "The bitmask to extract modifiers from a key value.")]
        Modifiers = -65536,

        [Key("None", "No key pressed.", false)]
        None = 0,

        [Key("LButton", "The left mouse button.", false)]
        LButton = 1,

        [Key("RButton", "The right mouse button.", false)]
        RButton = 2,

        [Key("Cancel", "The CANCEL key.")]
        Cancel = 3,

        [Key("MButton", "The middle mouse button (three-button mouse).", false)]
        MButton = 4,

        [Key("XButton1", "The first x mouse button (five-button mouse).", false)]
        XButton1 = 5,

        [Key("XButton2", "The second x mouse button (five-button mouse).", false)]
        XButton2 = 6,

        [Key("Back", "The BACKSPACE key.")]
        Back = 8,

        [Key("Tab", "The TAB key.")]
        Tab = 9,

        [Key("LineFeed", "The LINEFEED key.")]
        LineFeed = 10,

        [Key("Clear", "The CLEAR key.")]
        Clear = 12,

        [Key("Enter", "The ENTER key.")]
        Enter = 13,

        [Key("Return", "The RETURN key.")]
        Return = 13,

        [Key("Shift", "The SHIFT key.")]
        ShiftKey = 16,

        [Key("Ctrl", "The CTRL key.")]
        ControlKey = 17,

        [Key("Menu", "The ALT key.")]
        Menu = 18,

        [Key("Pause", "The PAUSE key.")]
        Pause = 19,

        [Key("CapsLock", "The CAPS LOCK key.")]
        Capital = 20,

        [Key("HanguelMode", "The IME Hanguel mode key. (maintained for compatibility; use HangulMode)")]
        HanguelMode = 21,

        [Key("JunjaMode", "The IME Junja mode key.")]
        JunjaMode = 23,

        [Key("FinalMode", "The IME final mode key.")]
        FinalMode = 24,

        [Key("HanjaMode", "The IME Hanja mode key.")]
        HanjaMode = 25,

        [Key("Esc", "The ESC key.")]
        Escape = 27,

        [Key("IMEConvert", "The IME convert key.")]
        IMEConvert = 28,

        [Key("IMENonconvert", "The IME nonconvert key.")]
        IMENonconvert = 29,

        [Key("IMEAceept", "The IME accept key. Obsolete, use IMEAccept instead.")]
        IMEAceept = 30,

        [Key("IMEModeChange", "The IME mode change key.")]
        IMEModeChange = 31,

        [Key("Space", "The SPACEBAR key.")]
        Space = 32,

        [Key("PageUp", "The PAGE UP key.")]
        PageUp = 33,

        [Key("PageDown", "The PAGE DOWN key.")]
        Next = 34,

        [Key("End", "The END key.")]
        End = 35,

        [Key("Home", "The HOME key.")]
        Home = 36,

        [Key("Left", "The LEFT ARROW key.")]
        Left = 37,

        [Key("Up", "The UP ARROW key.")]
        Up = 38,

        [Key("Right", "The RIGHT ARROW key.")]
        Right = 39,

        [Key("Down", "The DOWN ARROW key.")]
        Down = 40,

        [Key("Select", "The SELECT key.")]
        Select = 41,

        [Key("Print", "The PRINT key.")]
        Print = 42,

        [Key("Execute", "The EXECUTE key.")]
        Execute = 43,

        [Key("PrintScreen", "The PRINT SCREEN key.")]
        PrintScreen = 44,

        [Key("Insert", "The INS key.")]
        Insert = 45,

        [Key("Delete", "The DEL key.")]
        Delete = 46,

        [Key("Help", "The HELP key.")]
        Help = 47,

        [Key("0", "The 0 key.")]
        D0 = 48,

        [Key("1", "The 1 key.")]
        D1 = 49,

        [Key("2", "The 2 key.")]
        D2 = 50,

        [Key("3", "The 3 key.")]
        D3 = 51,

        [Key("4", "The 4 key.")]
        D4 = 52,

        [Key("5", "The 5 key.")]
        D5 = 53,

        [Key("6", "The 6 key.")]
        D6 = 54,

        [Key("7", "The 7 key.")]
        D7 = 55,

        [Key("8", "The 8 key.")]
        D8 = 56,

        [Key("9", "The 9 key.")]
        D9 = 57,

        [Key("A", "The A key.")]
        A = 65,

        [Key("B", "The B key.")]
        B = 66,

        [Key("C", "The C key.")]
        C = 67,

        [Key("D", "The D key.")]
        D = 68,

        [Key("E", "The E key.")]
        E = 69,

        [Key("F", "The F key.")]
        F = 70,

        [Key("G", "The G key.")]
        G = 71,

        [Key("H", "The H key.")]
        H = 72,

        [Key("I", "The I key.")]
        I = 73,

        [Key("J", "The J key.")]
        J = 74,

        [Key("K", "The K key.")]
        K = 75,

        [Key("L", "The L key.")]
        L = 76,

        [Key("M", "The M key.")]
        M = 77,

        [Key("N", "The N key.")]
        N = 78,

        [Key("O", "The O key.")]
        O = 79,

        [Key("P", "The P key.")]
        P = 80,

        [Key("Q", "The Q key.")]
        Q = 81,

        [Key("R", "The R key.")]
        R = 82,

        [Key("S", "The S key.")]
        S = 83,

        [Key("T", "The T key.")]
        T = 84,

        [Key("U", "The U key.")]
        U = 85,

        [Key("V", "The V key.")]
        V = 86,

        [Key("W", "The W key.")]
        W = 87,

        [Key("X", "The X key.")]
        X = 88,

        [Key("Y", "The Y key.")]
        Y = 89,

        [Key("Z", "The Z key.")]
        Z = 90,

        [Key("Win", "The left Windows logo key (Microsoft Natural Keyboard).")]
        LWin = 91,

        [Key("RWin", "The right Windows logo key (Microsoft Natural Keyboard).")]
        RWin = 92,

        [Key("Apps", "The application key (Microsoft Natural Keyboard).")]
        Apps = 93,

        [Key("Sleep", "The computer sleep key.")]
        Sleep = 95,

        [Key("Num0", "The 0 key on the numeric keypad.")]
        NumPad0 = 96,

        [Key("Num1", "The 1 key on the numeric keypad.")]
        NumPad1 = 97,

        [Key("Num2", "The 2 key on the numeric keypad.")]
        NumPad2 = 98,

        [Key("Num3", "The 3 key on the numeric keypad.")]
        NumPad3 = 99,

        [Key("Num4", "The 4 key on the numeric keypad.")]
        NumPad4 = 100,

        [Key("Num5", "The 5 key on the numeric keypad.")]
        NumPad5 = 101,

        [Key("Num6", "The 6 key on the numeric keypad.")]
        NumPad6 = 102,

        [Key("Num7", "The 7 key on the numeric keypad.")]
        NumPad7 = 103,

        [Key("Num8", "The 8 key on the numeric keypad.")]
        NumPad8 = 104,

        [Key("Num9", "The 9 key on the numeric keypad.")]
        NumPad9 = 105,

        [Key("Multiply", "The multiply key.")]
        Multiply = 106,

        [Key("Add", "The add key.")]
        Add = 107,

        [Key("Separator", "The separator key.")]
        Separator = 108,

        [Key("Subtract", "The subtract key.")]
        Subtract = 109,

        [Key("Decimal", "The decimal key.")]
        Decimal = 110,

        [Key("Divide", "The divide key.")]
        Divide = 111,

        [Key("F1", "The F1 key.")]
        F1 = 112,

        [Key("F2", "The F2 key.")]
        F2 = 113,

        [Key("F3", "The F3 key.")]
        F3 = 114,

        [Key("F4", "The F4 key.")]
        F4 = 115,

        [Key("F5", "The F5 key.")]
        F5 = 116,

        [Key("F6", "The F6 key.")]
        F6 = 117,

        [Key("F7", "The F7 key.")]
        F7 = 118,

        [Key("F8", "The F8 key.")]
        F8 = 119,

        [Key("F9", "The F9 key.")]
        F9 = 120,

        [Key("F10", "The F10 key.")]
        F10 = 121,

        [Key("F11", "The F11 key.")]
        F11 = 122,

        [Key("F12", "The F12 key.")]
        F12 = 123,

        [Key("F13", "The F13 key.")]
        F13 = 124,

        [Key("F14", "The F14 key.")]
        F14 = 125,

        [Key("F15", "The F15 key.")]
        F15 = 126,

        [Key("F16", "The F16 key.")]
        F16 = 127,

        [Key("F17", "The F17 key.")]
        F17 = 128,

        [Key("F18", "The F18 key.")]
        F18 = 129,

        [Key("F19", "The F19 key.")]
        F19 = 130,

        [Key("F20", "The F20 key.")]
        F20 = 131,

        [Key("F21", "The F21 key.")]
        F21 = 132,

        [Key("F22", "The F22 key.")]
        F22 = 133,

        [Key("F23", "The F23 key.")]
        F23 = 134,

        [Key("F24", "The F24 key.")]
        F24 = 135,

        [Key("NumLock", "The NUM LOCK key.")]
        NumLock = 144,

        [Key("Scroll", "The SCROLL LOCK key.")]
        Scroll = 145,

        [Key("Shift", "The left SHIFT key.")]
        LShiftKey = 160,

        [Key("RShift", "The right SHIFT key.")]
        RShiftKey = 161,

        [Key("Ctrl", "The left CTRL key.")]
        LControlKey = 162,

        [Key("RCtrl", "The right CTRL key.")]
        RControlKey = 163,

        [Key("Alt", "The left ALT key.")]
        LMenu = 164,

        [Key("RAlt", "The right ALT key.")]
        RMenu = 165,

        [Key("BrowserBack", "The browser back key.")]
        BrowserBack = 166,

        [Key("BrowserForward", "The browser forward key.")]
        BrowserForward = 167,

        [Key("BrowserRefresh", "The browser refresh key.")]
        BrowserRefresh = 168,

        [Key("BrowserStop", "The browser stop key.")]
        BrowserStop = 169,

        [Key("BrowserSearch", "The browser search key.")]
        BrowserSearch = 170,

        [Key("BrowserFavorites", "The browser favorites key.")]
        BrowserFavorites = 171,

        [Key("BrowserHome", "The browser home key.")]
        BrowserHome = 172,

        [Key("VolumeMute", "The volume mute key.")]
        VolumeMute = 173,

        [Key("VolumeDown", "The volume down key.")]
        VolumeDown = 174,

        [Key("VolumeUp", "The volume up key.")]
        VolumeUp = 175,

        [Key("MediaNextTrack", "The media next track key.")]
        MediaNextTrack = 176,

        [Key("MediaPreviousTrack", "The media previous track key.")]
        MediaPreviousTrack = 177,

        [Key("MediaStop", "The media Stop key.")]
        MediaStop = 178,

        [Key("MediaPlayPause", "The media play pause key.")]
        MediaPlayPause = 179,

        [Key("LaunchMail", "The launch mail key.")]
        LaunchMail = 180,

        [Key("SelectMedia", "The select media key.")]
        SelectMedia = 181,

        [Key("LaunchApplication1", "The start application one key.")]
        LaunchApplication1 = 182,

        [Key("LaunchApplication2", "The start application two key.")]
        LaunchApplication2 = 183,

        [Key("OemSemicolon", "The OEM Semicolon key on a US standard keyboard.")]
        OemSemicolon = 186,

        [Key("Oemplus", "The OEM plus key on any country/region keyboard.")]
        Oemplus = 187,

        [Key("Oemcomma", "The OEM comma key on any country/region keyboard.")]
        Oemcomma = 188,

        [Key("OemMinus", "The OEM minus key on any country/region keyboard.")]
        OemMinus = 189,

        [Key("OemPeriod", "The OEM period key on any country/region keyboard.")]
        OemPeriod = 190,

        [Key("OemQuestion", "The OEM question mark key on a US standard keyboard.")]
        OemQuestion = 191,

        [Key("Oemtilde", "The OEM tilde key on a US standard keyboard.")]
        Oemtilde = 192,

        [Key("OemOpenBrackets", "The OEM open bracket key on a US standard keyboard.")]
        OemOpenBrackets = 219,

        [Key("OemPipe", "The OEM pipe key on a US standard keyboard.")]
        OemPipe = 220,

        [Key("OemCloseBrackets", "The OEM close bracket key on a US standard keyboard.")]
        OemCloseBrackets = 221,

        [Key("OemQuotes", "The OEM singled/double quote key on a US standard keyboard.")]
        OemQuotes = 222,

        [Key("Oem8", "The OEM 8 key.")]
        Oem8 = 223,

        [Key("Oem102", "The OEM 102 key.")]
        Oem102 = 226,

        [Key("OemBackslash", "The OEM angle bracket or backslash key on the RT 102 key keyboard.")]
        OemBackslash = 226,

        [Key("ProcessKey", "The PROCESS KEY key.")]
        ProcessKey = 229,

        [Key("Packet", "Used to pass Unicode characters as if they were keystrokes. The Packet key value is the low word of a 32-bit virtual-key value used for non-keyboard input methods.")]
        Packet = 231,

        [Key("Attn", "The ATTN key.")]
        Attn = 246,

        [Key("Crsel", "The CRSEL key.")]
        Crsel = 247,

        [Key("Exsel", "The EXSEL key.")]
        Exsel = 248,

        [Key("EraseEof", "The ERASE EOF key.")]
        EraseEof = 249,

        [Key("Play", "The PLAY key.")]
        Play = 250,

        [Key("Zoom", "The ZOOM key.")]
        Zoom = 251,

        [Key("NoName", "A constant reserved for future use.")]
        NoName = 252,

        [Key("Pa1", "The PA1 key.")]
        Pa1 = 253,

        [Key("OemClear", "The CLEAR key.")]
        OemClear = 254,

        [Key("KeyCode", "The bitmask to extract a key code from a key value.")]
        KeyCode = 65535,

        [Key("Shift", "The SHIFT modifier key.")]
        Shift = 65536,

        [Key("Control", "The CTRL modifier key.")]
        Control = 131072,

        [Key("Alt", "The ALT modifier key")]
        Alt = 262144
    }
}
