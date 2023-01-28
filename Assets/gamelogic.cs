using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class gamelogic : MonoBehaviour
{

    public TMP_InputField leftInput, rightInput;
    public TextMeshProUGUI numbers, result, textResult, corrects;

    public Canvas gameCanvas;
    public Canvas finishCanvas;

    Game[] games;

    int correct = 6;
    int numberGame = 0;
    public void validate()
    {
        print(leftInput.text);
        print(rightInput.text);
        int validate = System.Convert.ToInt32(leftInput.text) + System.Convert.ToInt32(rightInput.text);
        print(validate);

        if(validate == System.Convert.ToInt32(result.text))
        {
            correct++;
        }

        numberGame++;

        resetGame();
    }

    void resetGame ()
    {
        if(games.Length == numberGame)
        {
            print("ganaste");
            gameCanvas.enabled = false;
            finishCanvas.enabled = true;
            testResult();
            return;
        }

        leftInput.text = "";
        rightInput.text = "";

        numbers.text = games[numberGame].numbers;
        result.text = games[numberGame].result.ToString();
    }

    void testResult()
    {
        corrects.text = "Correctas: " + correct + " de " + games.Length + " preguntas";
        if(correct < 6)
        {
            textResult.text = "“Ups, Esfuérzate un poco más, intenta nuevamente para que obtengas más puntos”";
        } else if (correct >= 6 && correct < 9)
        {
            textResult.text = "“¡Lo has hecho muy bien, pero puedes volverlo a intentar si deseas obtener un puntaje mayor!”";
        } else
        {
            textResult.text = "“¡Wow, no te equivocaste, felicidades, lograste el puntaje máximo posible!”";
        }
    }


    void Start()
    {
        games = new Game[] { 
            new Game(10, "1,2,6,4"),
            new Game(18, "10, 2, 3, 8"),
            new Game(20, "10, 2, 3, 10"),
            new Game(5, "10, 2, 3, 8"),
            new Game(12, "10, 2, 3, 8"),
            new Game(13, "10, 2, 3, 8"),
            new Game(78, "70, 2, 3, 8"),
            new Game(25, "15, 2, 3, 10"),
            new Game(9, "10, 2, 1, 8"),
            new Game(4, "10, 1, 3, 8"),
            new Game(7, "10, 2, 5, 8") };
        numbers.text = games[0].numbers;
        result.text = games[0].result.ToString();
        numberGame = 10;
        gameCanvas.enabled = true;
        finishCanvas.enabled = false;
    }
}

class Game
{
    public int result;
    public string numbers;
    public Game(int result, string numbers)
    {
        this.result = result;
        this.numbers = numbers;
    }

}