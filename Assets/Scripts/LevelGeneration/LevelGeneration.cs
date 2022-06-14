using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] private LayerMask _room;

    [Header("Scene Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _exit;
    [SerializeField] public GameObject _rooms;
    [SerializeField] private Transform[] _startingPositions;
    [SerializeField] public GameObject[] roomsArray;
    /* 0 - room with LEFT and RIGHT opening, LR
     * 1 - room with LEFT, RIGHT and DOWN opening, LRD
     * 2 - room with LEFT, RIGHT and UP opening, LRU
     * 3 - room with LEFT, RIGHT, DOWN and UP opening, LRDU
     */

    [Header("Random Generation Variables")]
    [SerializeField] private float _moveAmount; // nummber of cells the generator moves, project uses 10
    [SerializeField] private float _startTimeBetweenRoom; // time between generating new rooms
    [SerializeField] private float _minX; // left border of generation, project uses 10
    [SerializeField] private float _maxX; // right border of generation, project uses 30
    [SerializeField] private float _minY; // bottom border of generation, project uses -30

    private char _generationDirection; //an int that determines in which direction generation will move
    private float _timeBetweenRoom; //real time counter before spawning a new room
    public bool _stopGeneration; //used as a condition to stop generation - false when game starts, true when all rooms are filled

    private int _downCounter; //counts how many times generation has moved down in a row

    private GameObject _spawnRoom;
    private GameObject _currentRoom;

    void Start()
    {
        int randStartingPos = Random.Range(0, _startingPositions.Length);
        transform.position = _startingPositions[randStartingPos].position;
        _spawnRoom = Instantiate(roomsArray[0], transform.position, Quaternion.identity);
        _spawnRoom.transform.parent = _rooms.transform;

        /*Generation can move left, right or down.
         *Left and right directions are favored.
         */
        char[] directions = {'L', 'L', 'R', 'R', 'D'};
        _generationDirection = directions[Random.Range(0, directions.Length)];
    }

    private void Update()
    {
        //spawns a new room in intervals we determined, if generation isn't stopped
        if (_timeBetweenRoom <= 0 && !_stopGeneration)
        {
            Move();
            _timeBetweenRoom = _startTimeBetweenRoom;
        }
        else
        {
            _timeBetweenRoom -= Time.deltaTime; //countdown to spawning a new room
        }
    }

    private void Move()
    {
        if (_generationDirection == 'R')
        {
            //move the generator point RIGHT, if within borders, else move down
            if (transform.position.x < _maxX)
            {
                _downCounter = 0; //reset down counter because room didn't move down

                //take current position and move it right by move amount
                Vector2 newPos = new Vector2(transform.position.x + _moveAmount, transform.position.y);
                transform.position = newPos;

                //select a random room from the array of rooms and instantiate it
                int rand = Random.Range(0, roomsArray.Length);
                _currentRoom = Instantiate(roomsArray[rand], transform.position, Quaternion.identity);
                _currentRoom.transform.parent = _rooms.transform;

                char[] directions = {'R', 'R', 'R', 'D'};
                _generationDirection = directions[Random.Range(0, directions.Length)];
            }
            else
            {
                _generationDirection = 'D';
            }
        }
        else if (_generationDirection == 'L')
        {
            //move the generator point LEFT, if within borders, else move down
            if (transform.position.x > _minX)
            {
                _downCounter = 0; //reset down counter because room didn't move down

                //take current position and move it left by move amount
                Vector2 newPos = new Vector2(transform.position.x - _moveAmount, transform.position.y);
                transform.position = newPos;

                //select a random room from the array of rooms and instantiate it
                int rand = Random.Range(0, roomsArray.Length);
                _currentRoom = Instantiate(roomsArray[rand], transform.position, Quaternion.identity);
                _currentRoom.transform.parent = _rooms.transform;

                char[] directions = {'L', 'L', 'D'};
                _generationDirection = directions[Random.Range(0, directions.Length)];
            }
            else
            {
                _generationDirection = 'D';
            }

        }
        else if (_generationDirection == 'D')
        {
            _downCounter++; //increment how many times the generation moved down

            //move the generator point DOWN, if within border, else stop generation
            if (transform.position.y > _minY)
            {
                /*Before the generation moves on, a collider is created that detects whether the room has a DOWN opening.
                 * If it doesn't (isn't type 1 or 3), check how many times generation moved down.
                 */
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, _room); //layer mask to only detect objects of type room
                if (roomDetection.GetComponent<RoomTypeDetect>().type != 1 && roomDetection.GetComponent<RoomTypeDetect>().type != 3)
                {
                    bool newSpawnRoom = false;
                    // If the room being destroyed is the spawnRoom, assign the newly created room to it
                    if (GameObject.ReferenceEquals(roomDetection.gameObject.transform.parent.gameObject, _spawnRoom))
                    {
                        newSpawnRoom = true;
                    }

                    if (_downCounter >= 2)
                    {
                        /*if generation moved down 2 times or more, destroy current room and
                         *instantiate a room with opening in all directions (type 3)
                         */
                        roomDetection.GetComponent<RoomTypeDetect>().RoomDestruction();
                        _currentRoom = Instantiate(roomsArray[3], transform.position, Quaternion.identity);
                        _currentRoom.transform.parent = _rooms.transform;
                    }
                    else
                    {
                        //destroy current room, create a new random variable that will only spawn a room that does have bottom opening
                        roomDetection.GetComponent<RoomTypeDetect>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 4);

                        //because room of type 2 doesn't have a bottom opening, change it to 1
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        _currentRoom = Instantiate(roomsArray[randBottomRoom], transform.position, Quaternion.identity); //instantiate the new room
                        _currentRoom.transform.parent = _rooms.transform;
                    }

                    if (newSpawnRoom)
                    {
                        _spawnRoom = _currentRoom;
                    }
                }

                //once the old room was cleared out, move the generation
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - _moveAmount);
                transform.position = newPos;

                //instantiate one of the rooms that have top opening (2 or 3)
                int rand = Random.Range(2, 4);
                _currentRoom = Instantiate(roomsArray[rand], transform.position, Quaternion.identity);
                _currentRoom.transform.parent = _rooms.transform;

                char[] directions = {'L', 'L', 'R', 'R', 'D'};
                _generationDirection = directions[Random.Range(0, directions.Length)];
            }
            else
            {
                _stopGeneration = true;
                SpawnPlayerAndCreateExit();
            }

        }

    }

    private void SpawnPlayerAndCreateExit()
    {
        _player.transform.position = _spawnRoom.transform.GetChild(0).Find("SpawnPointPlayer").transform.position;
        Instantiate(_exit, _currentRoom.transform.GetChild(0).Find("SpawnPointExit").transform);
    }
}
