using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class SaveGame : MonoBehaviour
    {
        [SerializeField] private bool _clearSaveOnLoad = false;

        private GameSaveDataStructure _gameSaveData;
        private Button _btnSaveGame;

        private string _gameSaveKey = "GameSave";

        private BaseBuildingItems _baseBuildingItemComponent;

        private static SaveGame _instance;
        public static SaveGame GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
            _baseBuildingItemComponent = GetComponent<BaseBuildingItems>();
            _gameSaveData = new GameSaveDataStructure();
            _btnSaveGame = GameObject.Find("BtnSaveGame").GetComponent<Button>();
            _btnSaveGame.onClick.AddListener(OnSaveBtnClick);
        }


        private void OnSaveBtnClick()
        {
            SaveGameToPlayerPrefs();
        }


        //The player basebuilding component calls this when a player is placing a block. Increment the cound for that block type
        public void AddBlock(GameObject prefab)
        {
            _gameSaveData.AddItem(prefab);
        }


        public void SaveGameToPlayerPrefs()
        {
            string saveDataJson = JsonUtility.ToJson(_gameSaveData);
            Debug.LogError(saveDataJson);
            PlayerPrefs.SetString(_gameSaveKey, saveDataJson);
            Debug.LogError("saving");
        }


        public void LoadGameFromPlayerPrefs()
        {
            if (_clearSaveOnLoad)
            {
                PlayerPrefs.SetString(_gameSaveKey, null);
                _gameSaveData = new GameSaveDataStructure();
                return;
            }

            //Load the saved data from the player prefs and deserialize it
            GameSaveDataStructure save = new GameSaveDataStructure();
            string saveJson = PlayerPrefs.GetString(_gameSaveKey);
            _gameSaveData = JsonUtility.FromJson<GameSaveDataStructure>(saveJson);

            //spawn the game items. 
            _SpawnSavedItems();
        }


        private void _SpawnSavedItems()
        {            
            if (_gameSaveData == null)
            {
                _gameSaveData = new GameSaveDataStructure();
                return;
            }

            BaseBuildingItems definedItems =  BaseBuildingItems.GetInstance();
            foreach (var itemTypeToSpawnFrom in _gameSaveData.baseBlocks)
            {
                var item = Instantiate(_baseBuildingItemComponent.GetBaseBuildingPrefabByName(itemTypeToSpawnFrom.name), 
                             itemTypeToSpawnFrom.position,
                             Quaternion.Euler(itemTypeToSpawnFrom.eulers));
                item.GetComponent<Collider>().isTrigger = false;   
            }
        }
    }




    [System.Serializable]
    public class GameSaveDataStructure
    {
       [SerializeField] public List<VirtualBaseBlock> baseBlocks;

        public GameSaveDataStructure()
        {
            baseBlocks = new List<VirtualBaseBlock>();
        }

        public void AddItem(GameObject baseBlockToSave)
        {
            baseBlocks.Add(new VirtualBaseBlock
            {
                name = baseBlockToSave.name,
                position = baseBlockToSave.transform.position,
                eulers = baseBlockToSave.transform.eulerAngles
            });           
        }
    }


    [System.Serializable]
    public class VirtualBaseBlock
    {
      [SerializeField] public string name;
      [SerializeField] public Vector3 position;
      [SerializeField] public Vector3 eulers;
    }

}

