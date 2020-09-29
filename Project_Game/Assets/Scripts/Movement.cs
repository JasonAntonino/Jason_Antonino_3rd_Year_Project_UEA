using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;           //Used for the Stopwatch class (for timing the keystrokes)
using UnityEngine;
using UnityEngine.Tilemaps;         //Used to access the data of the tilemap


public class Movement : MonoBehaviour
{
    Vector2 characterPos;
    public float speed;
    private Rigidbody2D rigidBody;
    Vector3 nextPos, destination;
    Vector2 currentDirection = Vector2.zero;
    bool canMove;
    bool canMoveUp, canMoveLeft, canMoveDown, canMoveRight;     //This helps identify whether the character object can move to a certain direction (prevents going past edges).
    bool wKeyPressed, aKeyPressed, sKeyPressed, dKeyPressed;    //This will help with the dwellTime timings to identify whether the right key was released.
    bool keyIsPressed;                                          //This is required to prevent multiple keys from being pressed.
    Tilemap gameTilemap;
    Stopwatch dwellTime;                                        //Duration from a key press to key release
    Stopwatch flightTime;                                       //Duration from a key release to key press
    //Stopwatch twoKeyPressTime;                                      //Duration from a key press to another key press (dwell time + flight time)
    TimeSpan dwellTimeSpan;
    TimeSpan flightTimeSpan;
    TimeSpan twoKeyPressTimeSpan;
    //string fileNameFormat = "identifierhere_data_{0}.txt";
    //string fileName;
    //int fileNameCounter = 2;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        nextPos = Vector3.up;
        destination = transform.position;
        canMoveUp = canMoveLeft = canMoveDown = canMoveRight = true;
        wKeyPressed = aKeyPressed = sKeyPressed = dKeyPressed = false;
        keyIsPressed = false;
        gameTilemap = GameObject.Find("GameTilemap").GetComponent<Tilemap>();   //Collects the game's tilemap and store it into variable called gameTilemap
        dwellTime = new Stopwatch();
        flightTime = new Stopwatch();

        addEmail(Email.emailString, SceneScript.fileName);
        UnityEngine.Debug.Log(Email.emailString);

        //twoKeyPressTime = new Stopwatch();
        //string s = Application.dataPath;
        //UnityEngine.Debug.Log(s);

        //fileName = string.Format(fileNameFormat, "1");                          //Sets the first filename to have a value of "1" added to indicate first data file for the given user.

        //while (File.Exists(fileName))                                           //On proper implementation, the filename should be the unique user id.
        //{
        //    fileName = string.Format(fileNameFormat, fileNameCounter++);        //Adds a value at the end of the filename which will create a new file for a new game.
        //}
    }

    void CharacterMovement()
    {
        //Moves Character object from current position to the destination.
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //Collects next user input for the next movement
        if (Input.GetKeyDown(KeyCode.W) && canMoveUp == true && keyIsPressed == false)
        {
            nextPos = Vector3.up;
            canMove = true;
            wKeyPressed = true;
            keyIsPressed = true;
            dwellTime.Reset();  //Resets the dwellTime Stopwatch object time
            dwellTime.Start();  //Starts the dwellTime Stopwatch object time

            flightTime.Stop();                       //flightTime is stopped
            flightTimeSpan = flightTime.Elapsed;      //Elapsed flightTime is stored into "flightTimeSpan" object.
            UnityEngine.Debug.Log("Flight Time: " + flightTimeSpan);
        }
        else if (Input.GetKeyDown(KeyCode.A) && canMoveLeft == true && keyIsPressed == false)
        {
            nextPos = Vector3.left;
            canMove = true;
            aKeyPressed = true;
            keyIsPressed = true;
            dwellTime.Reset();
            dwellTime.Start();

            flightTime.Stop();
            flightTimeSpan = flightTime.Elapsed;
            UnityEngine.Debug.Log("Flight Time: " + flightTimeSpan);
            //rigidBody.AddForce(-Vector2.right * speed * Time.deltaTime);
        }
        else if (Input.GetKeyDown(KeyCode.S) && canMoveDown == true && keyIsPressed == false)
        {
            nextPos = Vector3.down;
            canMove = true;
            sKeyPressed = true;
            keyIsPressed = true;
            dwellTime.Reset();
            dwellTime.Start();

            flightTime.Stop();
            flightTimeSpan = flightTime.Elapsed;
            UnityEngine.Debug.Log("Flight Time: " + flightTimeSpan);
        }
        else if (Input.GetKeyDown(KeyCode.D) && canMoveRight == true && keyIsPressed == false)
        {
            nextPos = Vector3.right;
            canMove = true;
            dKeyPressed = true;
            keyIsPressed = true;
            dwellTime.Reset();
            dwellTime.Start();

            flightTime.Stop();
            flightTimeSpan = flightTime.Elapsed;
            UnityEngine.Debug.Log("Flight Time: " + flightTimeSpan);
            //rigidBody.AddForce(Vector2.right * speed * Time.deltaTime);
        }

        //Code for when a key has been released (Unpressed)
        if (Input.GetKeyUp(KeyCode.W) && wKeyPressed == true)
        {
            dwellTime.Stop();                                                       //Dwelltime is stopped
            dwellTimeSpan = dwellTime.Elapsed;                                      //Elapsed dwellTime is stored into "dwellTimeSpan" object.
            UnityEngine.Debug.Log(dwellTimeSpan);

            twoKeyPressTimeSpan = dwellTimeSpan + flightTimeSpan;                   //Duration between two key presses (dwell time + flight time)
            UnityEngine.Debug.Log("TWO KEY PRESS TIME: " + twoKeyPressTimeSpan);
            addData(toStr(dwellTimeSpan), toStr(flightTimeSpan),
                toStr(twoKeyPressTimeSpan), SceneScript.fileName, 'W');             //Adds the keystroke dynamics data into the file

            flightTime.Reset();                                                     //Resets the flightTime stopwatch
            flightTime.Start();                                                     //Starts the flightTime stopwatch

            wKeyPressed = false;
            keyIsPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.A) && aKeyPressed == true)
        {
            dwellTime.Stop();   //Dwelltime 
            dwellTimeSpan = dwellTime.Elapsed;
            UnityEngine.Debug.Log(dwellTimeSpan);

            twoKeyPressTimeSpan = dwellTimeSpan + flightTimeSpan;
            UnityEngine.Debug.Log("TWO KEY PRESS TIME: " + twoKeyPressTimeSpan);
            addData(toStr(dwellTimeSpan), toStr(flightTimeSpan),
                toStr(twoKeyPressTimeSpan), SceneScript.fileName, 'A');

            flightTime.Reset();
            flightTime.Start();

            aKeyPressed = false;
            keyIsPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.S) && sKeyPressed == true)
        {
            dwellTime.Stop();   //Dwelltime 
            dwellTimeSpan = dwellTime.Elapsed;
            UnityEngine.Debug.Log(dwellTimeSpan);

            twoKeyPressTimeSpan = dwellTimeSpan + flightTimeSpan;
            UnityEngine.Debug.Log("TWO KEY PRESS TIME: " + twoKeyPressTimeSpan);
            addData(toStr(dwellTimeSpan), toStr(flightTimeSpan),
                toStr(twoKeyPressTimeSpan), SceneScript.fileName, 'S');

            flightTime.Reset();
            flightTime.Start();

            sKeyPressed = false;
            keyIsPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.D) && dKeyPressed == true)
        {
            dwellTime.Stop();   //Dwelltime 
            dwellTimeSpan = dwellTime.Elapsed;
            UnityEngine.Debug.Log(dwellTimeSpan);

            twoKeyPressTimeSpan = dwellTimeSpan + flightTimeSpan;
            UnityEngine.Debug.Log("TWO KEY PRESS TIME: " + twoKeyPressTimeSpan);
            addData(toStr(dwellTimeSpan), toStr(flightTimeSpan),
                toStr(twoKeyPressTimeSpan), SceneScript.fileName, 'D');

            flightTime.Reset();
            flightTime.Start();

            dKeyPressed = false;
            keyIsPressed = false;

        }

        //Sets the next destination of the character object
        if (Vector3.Distance(destination, transform.position) <= 0.001f)
        {
            if (canMove)
            {
                destination = transform.position + nextPos;
                canMove = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canMoveLeft = true;
        canMoveRight = true;
        canMoveUp = true;
        canMoveDown = true;
    }

    void checkCollisions()
    {
        if (destination.x == 6.5f)              //Character on right edge
        {
            if (destination.y == 0.5f)          //Character on bottom right
            {
                canMoveDown = false;
                canMoveRight = false;
            }
            else if (destination.y == 6.5f)      //Character on top left
            {
                canMoveUp = false;
                canMoveRight = false;
            }
            else                                //Character in right edge (not corners)
            {
                canMoveRight = false;
                canMoveDown = true;
                canMoveUp = true;
                canMoveLeft = true;
            }
        }
        if (destination.x == 0.5f)              //Character on left edge
        {
            if (destination.y == 0.5f)          //Character on bottom left
            {
                canMoveLeft = false;
                canMoveDown = false;
            }
            else if (destination.y == 6.5f)      //Character on top left
            {
                canMoveLeft = false;
                canMoveUp = false;
            }
            else                                //Character on left edge (not corners)
            {
                canMoveLeft = false;
                canMoveUp = true;
                canMoveDown = true;
                canMoveRight = true;
            }
        }
        if (destination.y == 6.5f)              //Character on top edge
        {
            if (destination.x == 6.5f)          //Character on top right
            {
                canMoveUp = false;
                canMoveRight = false;
            }
            else if (destination.x == 0.5f)      //Character on top left
            {
                canMoveLeft = false;
                canMoveUp = false;
            }
            else
            {
                canMoveUp = false;
                canMoveDown = true;
                canMoveLeft = true;
                canMoveRight = true;
            }

        }
        if (destination.y == 0.5f)              //Character on bottom edge
        {
            if (destination.x == 0.5f)           //Character on bottom left
            {
                canMoveLeft = false;
                canMoveDown = false;
            }
            else if (destination.x == 6.5f)      //Character on bottom right)
            {
                canMoveRight = false;
                canMoveDown = false;
            }
            else
            {
                canMoveDown = false;
                canMoveLeft = true;
                canMoveUp = true;
                canMoveRight = true;
            }
        }
    }

    //This method will change the colour of the tiles in the tilemap when the character object goes over the tile (From white to orange)
    void changeTileColour()
    {
        Vector3Int characterPosition = Vector3Int.FloorToInt(transform.position);       //Stores the character object's current position on the variable "characterPosition".

        //If the colour of the tile is not yet orange, then run this code. Else just do nothing.
        if (gameTilemap.GetColor(characterPosition) != new Color(1.0f, 0.64f, 0.0f))
        {
            gameTilemap.SetTileFlags(characterPosition, TileFlags.None);                //Allows the tile to have a different colour (removes colour lock)
            gameTilemap.SetColor(characterPosition, new Color(1.0f, 0.64f, 0.0f));      //Sets the colour of the specific tile at the character object's position to the given colour.
            Score.tilesFilled += 1;
        }
    }

    //This method will be used to add the keystroke dynamics data to a csv file.
    static void addData(string dwellTime, string flightTime, string twoKeyPress, string filePath, char aKey)
    {
        try
        {
            using (System.IO.StreamWriter aFile = new System.IO.StreamWriter(filePath, true))
            {
                aFile.WriteLine(dwellTime + ',' + flightTime + ',' + twoKeyPress + ',' + aKey);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException("Error: ", e);
        }
    }

    //Adds the email to the data file
    static void addEmail(string email, string filePath)
    {
        try
        {
            using (System.IO.StreamWriter aFile = new System.IO.StreamWriter(filePath, true))
            {
                aFile.WriteLine(email);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException("Error: ", e);
        }
    }

    //Formats the keystroke data timings to just seconds and milliseconds
    static string toStr(TimeSpan time)
    {
        return time.TotalSeconds.ToString();
    }


    //Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        checkCollisions();
        CharacterMovement();
        changeTileColour();
        SceneScript.gameSceneToEndScene();

        //Vector2 movement = new Vector2(horizontalMovement, verticalMovement);
        //rigidBody.AddForce(movement * speed);
    }


}
