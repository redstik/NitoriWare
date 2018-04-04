﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Chalkboard : MonoBehaviour {

    [Header("Number of terms")]
    [SerializeField]
    private int termCount = 2;

    [Header("Minimum size of terms")]
    [SerializeField]
    private int minTerm = 1;

    [Header("Maximum size of terms")]
    [SerializeField]
    private int maxTerm = 5;

    [Header("Number of answers to generate")]
    [SerializeField]
    private int answerCount = 3;

    [Header("Operation to use")]
    [SerializeField]
    private string operation = "+";

    private List<int> termList = new List<int>();

    private GameObject term1;
    private GameObject term2;
    private GameObject term3;
    private List<GameObject> terms = new List<GameObject>();
    private GameObject answer;
    private GameObject operationSymbol;
    private int correctAnswer;
    private bool answered = false;

    // Use this for initialization
    void Start () {
        term1 = GameObject.Find("Term1");
        terms.Add(term1);
        term2 = GameObject.Find("Term2");
        terms.Add(term2);
        term3 = GameObject.Find("Term3");
        terms.Add(term3);
        operationSymbol = GameObject.Find("Operation");
        answer = GameObject.Find("Answer");
        generateProblem();
        generateAnswers();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void generateProblem()
    {
        for(int i = 0; i < termCount; i++)
        {
            termList.Add(Random.Range(minTerm, (maxTerm + 1)));
        }
        if (operation.Equals("+"))
        {
            correctAnswer = 0;
            for(int i = 0; i < terms.Count; i++)
            {
                if (i < termCount)
                {
                    terms[i].GetComponent<KeineMath_Term>().setValue(termList[i]);
                    correctAnswer += termList[i];
                } else
                {
                    terms[i].SetActive(false);
                }
            }
        } else if (operation.Equals("-"))
        {
            print("Subtraction not implemented!");
        } else
        {
            print("Invalid operation!");
        }
        float symbolx = operationSymbol.transform.position.x - (1f * Mathf.Max(termList.ToArray())) + 1;
        float symboly = operationSymbol.transform.position.y;
        operationSymbol.transform.position = new Vector3(symbolx, symboly, 0);
    }

    void generateAnswers()
    {
        int answerValue;
        int answerOffset;
        List<int> answerOffsets = new List<int>();
        for(int i = 0; i < answerCount; i++)
        {
            int sign = (Random.Range(1, 3) * 2) - 3; //Generates 1 or -1
            answerOffsets.Add(i * sign);
        }
        for(int i = 1; i <= answerCount; i++)
        {
            answerOffset = answerOffsets[Random.Range(0, answerOffsets.Count)];
            answerOffsets.Remove(answerOffset);
            answerValue = correctAnswer + answerOffset;
            float newx = answer.transform.position.x + (3.25f * i);
            float newy = answer.transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            GameObject newanswer = Object.Instantiate(answer, newposition, Quaternion.identity);
            newanswer.GetComponent<KeineMath_Answer>().value = answerValue;
        }
    }

    public void processAnswer(int answer)
    {
        if (!answered)
        {
            answered = true;
            if (answer == correctAnswer)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
            }
            else
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}
