using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Difficulty")]
public class Dificulty : ScriptableObject
{
    public string name;
    public int score;
    public List<Question> questions = new List<Question>();
}
