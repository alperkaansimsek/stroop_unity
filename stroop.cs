using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class stroop : MonoBehaviour
{
    public TMP_Text mText;


    [SerializeField]
    string[] wordList = {"Blue", "Red", "Green" "Yellow", "  " };
    public Color[] colors = { Color.red, Color.yellow, Color.cyan, Color.green };
    private int num = 0;
    private float startTime;
    private float totalTime;
    private int counter = 0;
    private Color previousColor;
    private List<int> availableColors; // List to track remaining colors
    public int randomColorIndex;
    private int correctPresses = 0;
    private int wrongPresses = 0;
    private string path = @"C:\\Users\\YOURNAME\\LOCATION\\DATA\\Data.txt"; //Use this path to specify desired data location. Do not forget to create a txt file with the same name in the script Do not remove the @

    //Do not forget to change the name of the previous file and generate new one to collect new data. 

    public void CreateText()
    {
        Debug.Log("Attempting to create file at: " + path);
        File.WriteAllText(path, "Trial ReactionTime \n\n");

    }

    void Start()
    {
        totalTime = Time.time;
        mText = gameObject.GetComponent<TextMeshProUGUI>();
        availableColors = new List<int>(colors.Length); // Initialize list 
        for (int i = 0; i < colors.Length; i++)
        {
            availableColors.Add(i);
        }
        CreateText();
        WordGenerator();
        startTime = Time.time; // Set the start time initially
    }

    void Update()
    {
        totalTime = Time.time;
        if (Input.GetKeyDown(KeyCode.H)) //Red
        {
            CheckReaction(KeyCode.H, colors[0]);
        }
        else if (Input.GetKeyDown(KeyCode.J)) //Yellow
        {
            CheckReaction(KeyCode.J, colors[1]);
        }
        else if (Input.GetKeyDown(KeyCode.K)) //Blue
        {
            CheckReaction(KeyCode.K, colors[2]);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            CheckReaction(KeyCode.L, colors[3]); //Green
        }
    }


    private void CheckReaction(KeyCode keyPressed, Color expectedColor)
    {
        if (mText.faceColor == expectedColor)
        {
            correctPresses++;
            counter++;
            num++;
            MeasureReactionTime();
            WordGenerator();
        }
        else
        {
            wrongPresses++;
            counter++;
            string falseString = "Skipped the word due to wrong response.\n\n";
            num++;
            MeasureReactionTime();

            File.AppendAllText(path, falseString);
            WordGenerator();
        }
    }

    private void MeasureReactionTime()
    {
        float reactionTime = Time.time - startTime;
        string reactionString = "Reaction Time for word " + wordList[num] + " is: " + reactionTime + "\n";
        File.AppendAllText(path, reactionString);
        //counter++;
        startTime = Time.time; // Update start time for the next measurement
    }

    private void WordGenerator()
    {
        if (availableColors.Count == 0) // If all colors used, reset the list
        {
            availableColors = new List<int>(colors.Length);
            for (int i = 0; i < colors.Length; i++)
            {
                availableColors.Add(i);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, availableColors.Count);
        randomColorIndex = availableColors[randomIndex];
        previousColor = mText.faceColor;
        mText.faceColor = colors[randomColorIndex];
        //num = UnityEngine.Random.Range(0, wordList.Length);
        availableColors.RemoveAt(randomIndex); // Remove used color from available list
        mText.text = wordList[num];
        //num++;
        Debug.Log(num);

        if (num == 10)
        {
            File.AppendAllText(path, "Trial ended in " + totalTime + " seconds." + "with total of " + correctPresses + " correct and total of " + wrongPresses + " wrong presses");

            SceneManager.LoadScene("end");
            return;
        }
    }
}
