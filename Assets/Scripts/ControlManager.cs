using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ControlKey{ LeftKey, RightKey, JumpKey, AbilityKey, AttackKey}
    
    class PlayerControl{
        public List<KeyCodeMapping> keyCodesMappings;
        public KeyCode GetKeyCode(ControlKey controlKey){
            return keyCodesMappings.Find(k => k.controlKey == controlKey).keyCode;
            
        }
    }

    class KeyCodeMapping{
        public ControlKey controlKey;
        public KeyCode keyCode;
    }

    [SerializeField] List<PlayerControl> playerControls;

    public KeyCode GetKey(int playerID, ControlKey key){
        return playerControls[playerID].GetKeyCode(key);
    }

}
