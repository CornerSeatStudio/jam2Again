using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ControlKey{ LeftKey, RightKey, JumpKey, AbilityKey, AttackKey}
    
    [SerializeField]
    List<PlayerControl> playerControls;

    [Serializable]
    class PlayerControl{
        public List<KeyCodeMapping> keyCodesMappings;
        public KeyCode GetKeyCode(ControlKey controlKey){
            return keyCodesMappings.Find(k => k.controlKey == controlKey).keyCode;
            
        }
    }

    [Serializable]
    class KeyCodeMapping{
        public ControlKey controlKey;
        public KeyCode keyCode;
    }



    public KeyCode GetKey(int playerID, ControlKey key){
        return playerControls[playerID].GetKeyCode(key);
    }

}
