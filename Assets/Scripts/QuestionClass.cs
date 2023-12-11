using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class AnswerData
{
    public string answerText;
    public bool isCorrect;
}


[System.Serializable]
public class QuestionData 
{
    public string questionText;
    public AnswerData[] answers;
}

public class GameData
{
    public QuestionData[] questionData;
}




