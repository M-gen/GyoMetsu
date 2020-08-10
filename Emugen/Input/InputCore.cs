using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;

namespace Emugen.Input
{
    public class InputCore
    {
        public enum KeyEventCode
        {
            MouseLeftButton,
            MouseRightButton,
        }

        public enum KeyEventType
        {
            DownUpdateBefor, // 押下直後 Update待ち
            UpUpdateBefor, // 押下直後 Update待ち

            Down,
            DownKeep,
            Up,
            None,
        }

        public class KeyEventStatus
        {
            public KeyEventCode code;
            public KeyEventType type = KeyEventType.None;

            public KeyEventStatus(KeyEventCode code)
            {
                this.code = code;
            }
        }


        public static InputCore Instance;
        public Vector2I mousePosition = new Vector2I(0,0);

        public Dictionary<KeyEventCode, KeyEventStatus> keyValuePairs = new Dictionary<KeyEventCode, KeyEventStatus>();

        public InputCore()
        {
            Instance = this;
            keyValuePairs.Add(KeyEventCode.MouseLeftButton, new KeyEventStatus(KeyEventCode.MouseLeftButton));
            keyValuePairs.Add(KeyEventCode.MouseRightButton, new KeyEventStatus(KeyEventCode.MouseLeftButton));
        }

        public void Update()
        {
            foreach( var key in keyValuePairs)
            {
                switch (key.Value.type)
                {
                    case KeyEventType.DownUpdateBefor:
                        key.Value.type = KeyEventType.Down;
                        break;
                    case KeyEventType.Down:
                        key.Value.type = KeyEventType.DownKeep;
                        break;
                    case KeyEventType.DownKeep:
                        break;
                    case KeyEventType.UpUpdateBefor:
                        key.Value.type = KeyEventType.Up;
                        break;
                    case KeyEventType.Up:
                        key.Value.type = KeyEventType.None;
                        break;
                    case KeyEventType.None:
                        break;
                }
            }

        }

        public void DoKeyEvent(KeyEventCode code, KeyEventType type)
        {
            if (keyValuePairs.ContainsKey(code))
            {
                var key = keyValuePairs[code];
                key.type = type;
            }
        }

        public KeyEventType GetKeyEventType(KeyEventCode code)
        {
            if (keyValuePairs.ContainsKey(code))
            {
                var key = keyValuePairs[code];
                return key.type;
            }
            return KeyEventType.None;

        }

    }
}
