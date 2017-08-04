using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterController : MonoBehaviour {

    public GameObject TypewriterHead;
    public GameObject PaperPanel;
    public List<Text> TextLines;

    public bool enabled = false;
    public int hideDelay = 3;
    public string characterLetter;
    public string lastPlayerEntered;

    private Vector3 hideVector = new Vector3(0, 0, 0);
    private Vector3 typewriterHeadDefaultPosition;
    private Vector3 paperPanelDefaultPosition;
    private bool isRunning = false;

	// Use this for initialization
	void Start () {

        typewriterHeadDefaultPosition = TypewriterHead.transform.position;
        paperPanelDefaultPosition = PaperPanel.transform.position;

        Hide();

	}
	
	// Update is called once per frame
	void Update () {

        string s = Input.inputString;

        if (s != "" && enabled == false && isRunning == false)
        {
            StartCoroutine(PlayerInput(s));
        }

        if (characterLetter != "" && enabled == true && isRunning == false)
        {
            Show();
            StartCoroutine(TypeLetter(characterLetter));
        }

        if (enabled == false)
        {
            Hide();
        }

    }

    private void Hide()
    {
        TypewriterHead.transform.position = hideVector;
        PaperPanel.transform.position = hideVector;

        characterLetter = "";
        enabled = false;
        isRunning = false;
    }

    private void Show()
    {
        TypewriterHead.transform.position = typewriterHeadDefaultPosition;
        PaperPanel.transform.position = paperPanelDefaultPosition;

        enabled = true;
        isRunning = true;
    }

    private IEnumerator TypeLetter(string v)
    {
        Show();

        foreach(Text t in TextLines)
        {
            t.text = "";
        }

        for (int x = 0; x < v.Length; x++)
        {
            string s = v.Substring(x, 1);
            if (s == "|")
            {
                for (int line=0; line <= TextLines.Count - 1; line++)
                {
                    if (line != TextLines.Count-1)
                        TextLines[line].text = TextLines[line + 1].text;
                    else
                        TextLines[line].text = "";
                }

                TypewriterHead.transform.position = typewriterHeadDefaultPosition;
            }
            else
            {
                Vector3 pos = TypewriterHead.transform.position;
                pos.x -= 0.1f;
                TypewriterHead.transform.position = pos;
                TextLines[TextLines.Count - 1].text += s;
            }

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(3);

        Hide();


    }

    private IEnumerator PlayerInput(string v)
    {
        System.DateTime lastInputTime = System.DateTime.Now;
        bool done = false;
        Show();

        foreach (Text t in TextLines)
        {
            t.text = "";
        }

        while (!done)
        {
            for (int x = 0; x < v.Length; x++)
            {
                string s = v.Substring(x, 1);
                if (s == "\r")
                {
                    for (int line = 0; line <= TextLines.Count - 1; line++)
                    {
                        if (line != TextLines.Count - 1)
                            TextLines[line].text = TextLines[line + 1].text;
                        else
                            TextLines[line].text = "";
                    }

                    TypewriterHead.transform.position = typewriterHeadDefaultPosition;
                    lastPlayerEntered = TextLines[TextLines.Count - 2].text;

                    PlayerInputHandler(lastPlayerEntered);

                }
                else if(s == "\b")
                {
                    string originalText = TextLines[TextLines.Count - 1].text;
                    if(originalText.Length > 0)
                    {
                        TextLines[TextLines.Count - 1].text = originalText.Substring(0, originalText.Length - 1);

                        Vector3 pos = TypewriterHead.transform.position;
                        pos.x += 0.1f;
                        TypewriterHead.transform.position = pos;
                    }
                }
                else
                {
                    Vector3 pos = TypewriterHead.transform.position;
                    pos.x -= 0.1f;
                    TypewriterHead.transform.position = pos;
                    TextLines[TextLines.Count - 1].text += s;
                }
            }

            yield return null;

            v = Input.inputString;

            if (v != "")
                lastInputTime = System.DateTime.Now;

            System.TimeSpan timeDifference = System.DateTime.Now - lastInputTime;

            if (timeDifference.Seconds > 3)
                done = true;

        }

        yield return new WaitForSeconds(3);

        Hide();
    }

    private void PlayerInputHandler(string input)
    {
        GameObject go = GameObject.Find("character");

        input = input.ToLower();

        if (input.Contains("please"))
        {
            if(input.Contains("read"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.ReadBook);
            }

            if (input.Contains("type") || input.Contains("write") || input.Contains("letter"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Typewriter);
            }

            if (input.Contains("tv") || input.Contains("television"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.TVChair);
            }

            if (input.Contains("sleep") || input.Contains("bed") || input.Contains("bedtime"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Sleep);
            }

            if (input.Contains("sofa") || input.Contains("relax") || input.Contains("rest"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Sofa);
            }

            if (input.Contains("play") || input.Contains("piano") || input.Contains("music"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Piano);
            }

            if (input.Contains("call") || input.Contains("telephone") || input.Contains("phone"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Phone);
            }

            if (input.Contains("eat") || input.Contains("food") || input.Contains("dinner") || input.Contains("lunch") || input.Contains("breakfast"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Cupboard);
            }

            if (input.Contains("fire") || input.Contains("fireplace") || input.Contains("chimney"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.BuildFire);
            }

            if (input.Contains("dressed") || input.Contains("clothes") || input.Contains("shirt"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Closet);
            }

            if (input.Contains("shower") || input.Contains("bath"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Shower);
            }

            if (input.Contains("leave") || input.Contains("bye"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.LeaveHouse);
            }

            if (input.Contains("exercise"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.Exercise);
            }

            if (input.Contains("dishes"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.WashDishes);
            }

            if (input.Contains("teeth"))
            {
                go.GetComponent<LcpManController>().activityQueue.Push(LcpManController.Activities.BrushTeeth);
            }

        }

        if (input == "help")
        {
            TextLines[0].text = "Try saying 'please' before a command!";
            TextLines[1].text = "CTRL-A = alarm clock, wake up";
            TextLines[2].text = "CTRL-C = call";
        }
    }
}
