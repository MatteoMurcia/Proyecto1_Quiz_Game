using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] List<Dificulty> difficulties = new List<Dificulty>();

    [Header("Screens")]

    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject gameOverScreen;

    [Header("Questions")]
    [SerializeField] TMPro.TextMeshProUGUI questionText;
    [SerializeField] Button tipButton;
    [SerializeField] Button[] buttons;
    [SerializeField] TMPro.TextMeshProUGUI[] options;
    [SerializeField] CanvasGroup canvas;

    [Header("Buttons State")]
    [SerializeField] Sprite defaultButton;
    [SerializeField] Sprite wrongButton;
    [SerializeField] Sprite correctButton;

    [Header("Score")]
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] TMPro.TextMeshProUGUI bestScoreText;

    [Header("Score Game Over")]
    [SerializeField] TMPro.TextMeshProUGUI scoreGameOverText;
    [SerializeField] TMPro.TextMeshProUGUI bestScoreGameOverText;

    List<Question> questions = new List<Question>();
    Dificulty currentDifficulty;
    Question currentQuestion;
    int roundsLeft;
    int score = 0;
    int bestScore;
    int tips = 3;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var difficulty in difficulties)
        {
            if (difficulty.name.ToLower() == "easy")
            {
                currentDifficulty = difficulty;
                questions = difficulty.questions;
                roundsLeft = questions.Count;
                break;
            }
        }

        bestScore = PlayerPrefs.GetInt(currentDifficulty.name, 0);
        //bestScoreText.text = $"BEST: {bestScore}";

        NextQuestion();
    }

    void NextQuestion()
    {
        if (roundsLeft <= 0) {
            GameOver();
            return;
        }

        foreach (var button in buttons)
        {
            button.interactable = true;
        }

        //tipButton.interactable = true;

        currentQuestion = questions[Random.Range(0, questions.Count)];

        questionText.text = currentQuestion.question;

        List<string> answers = new List<string>();
        answers.Add(currentQuestion.correctAnswer);
        answers.Add(currentQuestion.wrongAnswer1);
        answers.Add(currentQuestion.wrongAnswer2);
        answers.Add(currentQuestion.wrongAnswer3);

        foreach (var option in options)
        {
            int answerIndex = Random.Range(0, answers.Count);
            option.text = answers[answerIndex];
            answers.Remove(answers[answerIndex]);
        }

        questions.Remove(currentQuestion);
        roundsLeft--;
    }

    public void SelectOption(GameObject button)
    {
        TMPro.TextMeshProUGUI option = button.transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();

        if (option.text == currentQuestion.correctAnswer)
        {
            button.GetComponent<Image>().sprite = correctButton;
            score += currentDifficulty.score;
            scoreText.text = $"PUNTOS: {score}";
        } else
        {
            button.GetComponent<Image>().sprite = wrongButton;
            foreach(var b in buttons){
                TMPro.TextMeshProUGUI temp = b.transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();
                if(temp.text == currentQuestion.correctAnswer){
                    b.GetComponent<Image>().sprite = correctButton;
                }
            }
        }

        canvas.blocksRaycasts = false;

        StartCoroutine("WaitForNextQuestion");
    }

    IEnumerator WaitForNextQuestion()
    {
        yield return new WaitForSeconds(1);

        canvas.blocksRaycasts = true;

        foreach (var button in buttons)
        {
            button.gameObject.GetComponent<Image>().sprite = defaultButton;
        }

        NextQuestion();
    }

    void GameOver()
    {
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        scoreGameOverText.text = score.ToString();

        if (score > bestScore)
        {
            PlayerPrefs.SetInt(currentDifficulty.name, score);
            bestScoreGameOverText.text = score.ToString();
        } else
        {
            bestScoreGameOverText.text = bestScore.ToString();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Tip(Text tipText)
    {
        if (tips <= 0) {
            return;
        }

        int removedOptions = 0;
        int index;

        while(removedOptions < 2)
        {
            index = Random.Range(0, options.Length);
            Button button = options[index].gameObject.transform.parent.GetComponent<Button>();

            if (options[index].text != currentQuestion.correctAnswer && button.interactable)
            {
                button.interactable = false;
                removedOptions++;
            }
        }

        tips--;
        tipText.text = tips.ToString();
        tipButton.interactable = false;
    }
}
