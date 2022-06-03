using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] private LayerMask _room;

    [Header("Scene Components")]
    [SerializeField]  private Transform[] _startingPositions;
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

    private int _generationDirection; //an int that determines in which direction generation will move
    private float _timeBetweenRoom; //real time counter before spawning a new room
    public bool _stopGeneration; //used as a condition to stop generation - false when game starts, true when all rooms are filled

    private int _downCounter; //counts how many times generation has moved down in a row
   

    void Start()
    {
        int randStartingPos = Random.Range(0, _startingPositions.Length);
        transform.position = _startingPositions[randStartingPos].position;
        Instantiate(roomsArray[0], transform.position, Quaternion.identity);

        /*Generation can move left, right or down;
         *left and right directions are favored so they are assigned to 1-2 and 3-4, while
         *down is assigned only to 5. 
         */
       _generationDirection = Random.Range(1, 6);
    }

    private void Update()
    {
        //spawns a new room in intervals we determined, if generation isn't stopped
        if (_timeBetweenRoom <= 0 && !_stopGeneration)
        {
            Move();
            _timeBetweenRoom = _startTimeBetweenRoom;
        } else
        {
            _timeBetweenRoom -= Time.deltaTime; //countdown to spawning a new room
        }
    }

    private void Move()
    {
        if (_generationDirection == 1 || _generationDirection == 2)
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
                Instantiate(roomsArray[rand], transform.position, Quaternion.identity);

                /*Generate tje next direction, but only 1, 2 and 5 are allowed.
                 *If 3 is genrated, change it to 2 (right). 
                 *If 4 is generated,change it to 5 (down),
                 */
                _generationDirection = Random.Range(1, 5);

                if (_generationDirection == 3)
                {
                    _generationDirection = 2;
                } else if (_generationDirection == 4)
                {
                    _generationDirection = 5;
                }
            } else
            {
                _generationDirection = 5;
            }
        } else if (_generationDirection == 3 || _generationDirection == 4)
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
                Instantiate(roomsArray[rand], transform.position, Quaternion.identity);

                //choose the next direction, only 3,4 and 5 are allowed (left and down)
                _generationDirection = Random.Range(3, 5);
            } else
            {
                _generationDirection = 5;
            }
           
        } else if (_generationDirection == 5)
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
                    if (_downCounter >= 2)
                    {
                        /*if generation moved down 2 times or more, destroy current room and 
                         *instantiate a room with opening in all directions (type 3)
                         */
                        roomDetection.GetComponent<RoomTypeDetect>().RoomDestruction();
                        Instantiate(roomsArray[3], transform.position, Quaternion.identity);
                    } else
                    {
                        //destroy current room, create a new random variable that will only spawn a room that does have bottom opening
                        roomDetection.GetComponent<RoomTypeDetect>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 4);

                        //because room of type 2 doesn't have a bottom opening, change it to 1
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(roomsArray[randBottomRoom], transform.position, Quaternion.identity); //instantiate the new room
                    }                    
                }

                //once the old room was cleared out, move the generation
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - _moveAmount);
                transform.position = newPos;

                //instantiate one of the rooms that have top opening (2 or 3)
                int rand = Random.Range(2, 4);
                Instantiate(roomsArray[rand], transform.position, Quaternion.identity);

                _generationDirection = Random.Range(1, 6);
            } else
            {
                _stopGeneration = true;
            }
           
        }
        
    }
}
