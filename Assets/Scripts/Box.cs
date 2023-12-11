using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Box : EventTrigger
{
    public TextMeshProUGUI dialogText;
    public GameObject dialogBox;
    GameManager gameManager;
   
    private void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
     
      
        if (gameManager)
        {
            dialogBox = gameManager.dialogBox;
            dialogText = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

 


    public override void OnPointerEnter(PointerEventData data)
    {
        
        if (dialogBox != null)
        {
                dialogBox.SetActive(true);
          
        }
        if (dialogText)
        {
            ShowDialog(int.Parse(gameObject.name));
        }
        else
        {
            Debug.Log("Dialog Text can not be found");
        }


    }

    public override void OnPointerExit(PointerEventData data)
    {
     
        if (dialogBox )
        {
            dialogBox.SetActive(false);
        }
      
    }

    public override void OnPointerUp(PointerEventData data)
    {
        Debug.Log("OnPointerUp called.");
    }

    public override void OnScroll(PointerEventData data)
    {
        Debug.Log("OnScroll called.");
    }

    public override void OnSelect(BaseEventData data)
    {
        Debug.Log("OnSelect called.");
    }

    public override void OnSubmit(BaseEventData data)
    {
        Debug.Log("OnSubmit called.");
    }

    public override void OnUpdateSelected(BaseEventData data)
    {
        Debug.Log("OnUpdateSelected called.");
    }

    public void ShowDialog(int field)
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(true);
        }
        switch (field)
        {
            case 0:// game data ui field 1
                dialogText.text = "How many questions do you want in your app? Type a number greater than zero and press <b>Next</b> to type questions.";
                break;
            case 1: //game data ui field 2
                dialogText.text = "Type a number and click on the <b>Go To Questions</b> button.\n(Reducing this number deletes the last entry!)";
                break;

            case 2: //roundData subject name field
                dialogText.text = "The name of your subject e.g English";
                break;

            case 3: //roundData timelimit field
                dialogText.text = "Time given to complete the round in seconds e.g 600 = 10 minutes";
                break;
            case 4: //roundData question limit field
                dialogText.text = "Number of questions in this round e.g 10 ";
                break;
            case 5: //roundData points added field
                dialogText.text = "Points awarded when user gets a question right.";
                break;
            case 6: //roundData group data size field
                dialogText.text = "Sections are a group of questions. Examples:\n1. An English Passage and its corresponding questions\n2. 100 Maths questions\n3. An Labelled Image with its questions.\nType a number and press Enter.";
                break;
            case 7: //group Data passage instruction
                dialogText.text = "Use this field to write a passage instruction if needed. E.g Read this passage carefully and answer the next 5 questions.";
                break;
            case 8: //group Data passage
                dialogText.text = "If present, Comprehensions (Passages) or Lengthy Example Texts for all the questions in this section go here.";
                break;
            case 9: //group Data instruction
                dialogText.text = "If you have an instruction for the questions in this section, enter it here. E.g <i>Find the word that is exactly opposite to the word outlined.</i> The questions in this section will have this instruction attached to them.";
                break;
            case 10: //group Data example image
                dialogText.text = "Some sections have Example Images e.g Labelled Diagrams. If present, enter the name of the image here.";
                break;
            case 11: //question data size field.
                dialogText.text = "Enter the number of questions for this section.";
                break;
            case 12: //question  field
                dialogText.text = "Example 1:\nAccording to the passage, what is the king's name?\n Example 2:\n 2 + 2 = ?";
                break;
            case 13: //question  image field
                dialogText.text = "Some questions come in form of, or with images. If present, enter image name here.";
                break;
            case 14: //answer size field
                dialogText.text = "How many answer options are there for this question?";
                break;
            case 15: //answer field
                dialogText.text = "Write your answer here and click the <b>isCorrect</b> box if it's the right one.";
                break;
            case 100: //answer field
                dialogText.text = "Type a name for your file.";
                break;


        }
    }
}
