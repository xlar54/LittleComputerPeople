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
}
