using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, BuffSpawner> buffSpawners = new Dictionary<int, BuffSpawner>();
    public static Dictionary<int, FinishLine> FinishLines= new Dictionary<int, FinishLine>();
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemPrefab;
    public GameObject finishPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void SpawnPlayer(int _id, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == GameClient.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        _player.GetComponent<PlayerManager>().Initialize(_id,_position);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
    public void CreateBuffSpawn(int _spawnId, Vector3 pos, bool _hasItem)
    {
        GameObject spawner = Instantiate(itemPrefab, pos, itemPrefab.transform.rotation);
        spawner.GetComponent<BuffSpawner>().Initialize(_spawnId, _hasItem);
        buffSpawners.Add(_spawnId,spawner.GetComponent<BuffSpawner>());
    }
    public void CreateFinishLine(int _finishId, Vector3 pos, bool _hasItem)
    {
        GameObject finish = Instantiate(finishPrefab, pos, finishPrefab.transform.rotation);
        finish.GetComponent<FinishLine>().Initialize(_finishId, _hasItem);
        FinishLines.Add(_finishId, finish.GetComponent<FinishLine>());
    }
}
