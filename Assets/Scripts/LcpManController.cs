using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LcpManController : MonoBehaviour {

    public GameObject characterModel;
    public float speed;
    public int currentFloor = 1;
    public System.DateTime arrivalTime = System.DateTime.Now;

    public enum Activities
    {
        Kitchen,
        FrontDoor,
        ComputerRoom,
        Stove,
        TV,
        BathroomSink,
        KitchenTable,
        ComputerDesk,
        Shower,
        TVChair,
        Typewriter,
        Piano,
        Sofa,
        Sleep,
        LeaveHouse,
        BuildFire,
        Cupboard,
        Toilet,
        ReadBook,
        Exercise,
        Phone,
        Attic,
        Closet,
        Refrigerator,
        WashDishes
    }
    public enum CharacterStates
    {
        none,
        idle,
        walking,
        startactivity,
        doingactivity,
        finishedactivity
    }

    public Stack<Activities> activityQueue = new Stack<Activities>();
    public Activities currentActivity;

    public CharacterStates state;
    private Path path;
    private string lastSound = "";
    private bool hasStarted = false;

    private Vector3 tempVec1;
    private Vector3 tempVec2;
    private Vector3 panelVector3;
    private Vector3 typewriterHeadVector3;

    // Use this for initialization
    void Start () {

        currentActivity = Activities.FrontDoor;
        state = CharacterStates.idle;

        panelVector3 = GameObject.Find("Panel").transform.position;
        GameObject.Find("Panel").transform.position = new Vector3(0, 0, 0);

        typewriterHeadVector3 = GameObject.Find("TypewriterHead").transform.position;
        GameObject.Find("TypewriterHead").transform.position = new Vector3(0, 0, 0);

        StartCoroutine(BlinkCoroutine());

        activityQueue.Push(Activities.FrontDoor);
	}
    

    private void Update()
    {
        HandleInput();

        if (state == CharacterStates.idle)
        {
            currentActivity = GetNextActivity();
            GoToActivity(currentActivity);
        }

        if (state == CharacterStates.startactivity)
        {
            StartActivity(currentActivity);
        }

        if (state == CharacterStates.doingactivity)
        {
            DoingActivity(currentActivity);
        }

        if (state == CharacterStates.finishedactivity)
        {
            FinishedActivity(currentActivity);
        }

    }

    private void HandleInput()
    {
        if (isControlKeyPressed() && Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("Alarm_clock").GetComponent<AudioSource>().Play();
            if (currentActivity == Activities.Sleep)
            {
                activityQueue.Push(Activities.Toilet);
                FinishedActivity(currentActivity);
            }
        }

        if (isControlKeyPressed() && Input.GetKeyDown(KeyCode.C))
        {
            GameObject.Find("Phone_Base").GetComponent<PhoneController>().state = PhoneController.States.ringing;
            activityQueue.Push(Activities.Phone);
            FinishedActivity(currentActivity);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private bool isControlKeyPressed()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    void GoToActivity(Activities activity)
    {
        if (activity == Activities.TV)
            StartWalk(3, new Vector3(6.67f, 8.7f, 2.41f), Path.FacingDirection.right, 1);

        if (activity == Activities.BathroomSink)
            StartWalk(2, new Vector3(0.65f, 5.55f, 2.41f), Path.FacingDirection.backward, 4);

        if (activity == Activities.Stove)
            StartWalk(1, new Vector3(8.05f, 2.12f, 2.41f), Path.FacingDirection.right, 4);

        if (activity == Activities.ComputerRoom)
            StartWalk(2, new Vector3(-4.78f, 5.41f, 2.62f), Path.FacingDirection.forward, 3);

        if (activity == Activities.KitchenTable)
            StartWalk(1, new Vector3(3.66f, 2.27f, 1.75f), Path.FacingDirection.forward, 10, true);

        if (activity == Activities.ComputerDesk)
            StartWalk(2, new Vector3(-6.04f, 5.32f, 1.64f), Path.FacingDirection.backward, 20);

        if (activity == Activities.Shower)
            StartWalk(2, new Vector3(-2.39f, 5.32f, 0.86f), Path.FacingDirection.left, 8);

        if (activity == Activities.TVChair)
            StartWalk(3, new Vector3(4.8f, 8.35f, 1.75f), Path.FacingDirection.right, 20);

        if (activity == Activities.FrontDoor)
            StartWalk(1, new Vector3(-7.24f, 2.12f, 2.62f), Path.FacingDirection.forward, 2);

        if (activity == Activities.Typewriter)
            StartWalk(3, new Vector3(-2.327f, 8.62f, 1.137f), Path.FacingDirection.forward, 0);

        if (activity == Activities.Kitchen)
            StartWalk(1, new Vector3(5.56f, 2.17f, 2.62f), Path.FacingDirection.forward, 5);

        if (activity == Activities.Piano)
            StartWalk(3, new Vector3(1.99f, 8.54f, 1.767f), Path.FacingDirection.backward, 10);

        if (activity == Activities.Sofa)
            StartWalk(1, new Vector3(-2.997f, 1.993f, 1.34f), Path.FacingDirection.forward, 8);

        if (activity == Activities.Sleep)
            StartWalk(2, new Vector3(7.22f, 5.43f, 2.62f), Path.FacingDirection.up, 3600);

        if (activity == Activities.LeaveHouse)
            StartWalk(1, new Vector3(-7.83f, 2.16f, 1.15f), Path.FacingDirection.backward, 20);

        if (activity == Activities.BuildFire)
            StartWalk(1, new Vector3(-5.9f, 2.16f, 1.54f), Path.FacingDirection.backward, 2);

        if (activity == Activities.Cupboard)
            StartWalk(1, new Vector3(6.24f, 2.16f, 1.79f), Path.FacingDirection.backward, 2);

        if (activity == Activities.Toilet)
            StartWalk(2, new Vector3(-1.11f, 5.53f, 1.43f), Path.FacingDirection.backward, 10);

        if (activity == Activities.ReadBook)
            StartWalk(2, new Vector3(-4.249f, 5.392f, 1.43f), Path.FacingDirection.backward, 1);

        if (activity == Activities.Exercise)
            StartWalk(2, new Vector3(5.77f, 5.392f, 2.17f), Path.FacingDirection.forward, 10);

        if (activity == Activities.Phone)
            StartWalk(1, new Vector3(-2.997f, 1.993f, 1.34f), Path.FacingDirection.forward, 10);

        if (activity == Activities.Attic)
            StartWalk(3, new Vector3(-0.79f, 8.66f, 1.15f), Path.FacingDirection.backward, 10);

        if (activity == Activities.Closet)
            StartWalk(2, new Vector3(5.19f, 5.43f, 1.15f), Path.FacingDirection.backward, 8);

        if (activity == Activities.Refrigerator)
            StartWalk(1, new Vector3(7.11f, 2.1f, 1.95f), Path.FacingDirection.backward, 3);

        if (activity == Activities.WashDishes)
            StartWalk(1, new Vector3(5.22f, 2.1f, 1.63f), Path.FacingDirection.backward, 5);
    }

    void StartActivity(Activities activity)
    {
        StopAltSound();

        if (currentActivity == Activities.Shower)
        {
            GameObject.Find("Shower").GetComponent<ParticleSystem>().Play();
            PlaySound("shower", true);
        }

        if (currentActivity == Activities.ComputerRoom || currentActivity == Activities.Kitchen || currentActivity == Activities.FrontDoor)
        {
            PlaySound("speak1", true);
        }

        if (currentActivity == Activities.BathroomSink || currentActivity == Activities.WashDishes)
        {
            PlaySound("sink", true);
        }

        if (currentActivity == Activities.ComputerDesk)
        {
            PlaySound("computerkeyboard", true);
        }

        if (currentActivity == Activities.Typewriter)
        {
            //GameObject.Find("Panel").transform.position = panelVector3;
            PlaySound("typewriter", true);
        }

        if (currentActivity == Activities.Piano)
        {
            PlaySound("piano1", true);
        }

        if (currentActivity == Activities.Sofa)
        {
            if (GameObject.Find("Book").GetComponent<MeshRenderer>().enabled == true)
                characterModel.GetComponent<Animator>().SetBool("isReading", true);
            else
            {
                int r = UnityEngine.Random.Range(1, 3);

                if (r == 1)
                    characterModel.GetComponent<Animator>().SetBool("isSitting", true);
                else
                    characterModel.GetComponent<Animator>().SetBool("isRelaxing", true);
            }
                
        }

        if (currentActivity == Activities.Phone)
        {
            GameObject.Find("Phone_Handset").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Phone_Base").GetComponent<PhoneController>().state = PhoneController.States.normal;
            PlaySound("speak1", true);
            GameObject.Find("Phone").GetComponent<MeshRenderer>().enabled = true;
            characterModel.GetComponent<Animator>().SetBool("isUsingPhone", true);
        }

        if (currentActivity == Activities.BuildFire || currentActivity == Activities.TV)
        {
            characterModel.GetComponent<Animator>().SetBool("isBending", true);
        }

        if (currentActivity == Activities.Exercise)
        {
            PlaySound("exercise", true);
            characterModel.GetComponent<Animator>().SetBool("isExercising", true);
        }

        if (currentActivity == Activities.Cupboard || currentActivity == Activities.ComputerDesk 
            || currentActivity == Activities.Typewriter || currentActivity == Activities.BathroomSink 
            || currentActivity == Activities.Piano || currentActivity == Activities.WashDishes)
        {
            characterModel.GetComponent<Animator>().SetBool("isDoing", true);
        }

        if (currentActivity == Activities.KitchenTable)
        {
            PlaySound("eating", true);
            characterModel.GetComponent<Animator>().SetBool("isEating", true);
        }

        if (currentActivity == Activities.Sleep)
        {
            transform.position = new Vector3(7.948f, 5.97f, 2.62f);
            transform.forward = new Vector3(0f, 1f, 0f);
            PlaySound("snore", true);

            GameObject.Find("Bed_Sheet").GetComponent<MeshRenderer>().enabled = true;
        }

        if (currentActivity == Activities.LeaveHouse)
        {
            GameObject.Find("FrontDoor").GetComponent<DoorController>().state = DoorController.DoorStates.open; 
            transform.position = new Vector3(-7.24f, 2.12f, -1.4f);
        }

        if (currentActivity == Activities.Toilet)
        {
            GameObject.Find("Toilet Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
            transform.position = new Vector3(-7.24f, 2.12f, -1.4f);
        }

        if (currentActivity == Activities.Attic)
        {
            GameObject.Find("Attic Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
            transform.position = new Vector3(-7.24f, 2.12f, -1.4f);
        }

        if (currentActivity == Activities.Closet)
        {
            GameObject.Find("Closet Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
        }

        if (currentActivity == Activities.Refrigerator)
        {
            GameObject.Find("Refrigerator Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
        }

        if (currentActivity == Activities.ReadBook)
        {
            GameObject.Find("Book").GetComponent<MeshRenderer>().enabled = true;
            activityQueue.Push(Activities.Sofa);
        }

        if (activity == Activities.TVChair)
        {
            if (GameObject.Find("TVScreen").GetComponent<TVController>().state == TVController.States.off)
            {
                FinishedActivity(Activities.TV);
            }
        }

        state = CharacterStates.doingactivity;

    }

    void DoingActivity(Activities activity)
    {
        System.TimeSpan timeDifference = System.DateTime.Now - arrivalTime;

        if (path.stayMaxTime > 0)
        {
            if (timeDifference.Seconds > path.stayMaxTime)
                state = CharacterStates.finishedactivity;
        }


        if (currentActivity == Activities.LeaveHouse)
        {
            if(timeDifference.Seconds > 19)
            {
                GameObject.Find("FrontDoor").GetComponent<DoorController>().state = DoorController.DoorStates.open;
            }
            else if (timeDifference.Seconds > 0.5f)
            {
                GameObject.Find("FrontDoor").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            }
        }

        if (currentActivity == Activities.Toilet)
        {
            if (timeDifference.Seconds > 8)
            {
                GameObject.Find("Toilet Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
            }
            else if (timeDifference.Seconds > 2)
            {
                if (lastSound != "toilet") PlaySound("toilet", false);
            }
            else if (timeDifference.Seconds > 0.5f)
            {
                GameObject.Find("Toilet Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            }
        }

        if (currentActivity == Activities.Attic)
        {
            if (timeDifference.Seconds > 9)
            {
                GameObject.Find("Attic Door").GetComponent<DoorController>().state = DoorController.DoorStates.open;
            }
            else if (timeDifference.Seconds > 0.5f)
            {
                GameObject.Find("Attic Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            }
        }

        if (currentActivity == Activities.Typewriter && !hasStarted)
        {
            int r = UnityEngine.Random.Range(1, 5);
            string t = "";

            switch(r)
            {
                case 1: t = "Dear Friend, |I wanted to tell you how much I love my new home.  |It's a really cool place to live.  |Thanks!|Bob|";
                    break;
                case 2: t = "Dear Friend, |Living alone is kind of dull.  But at least I have you to keep me entertained.  And honestly... its 2017. Why do I still|use a typewriter?|Bob|";
                    break;
                case 3: t = "Dear Friend, |I realize Im a remake of an older computer game. But being in 3D, |I feel a little bit fatter than I used to. Do you think| Ive put on weight since the 80s?|Bob|";
                    break;
                case 4: t = "Dear Friend, |I used to have a a dog.  I miss him.  What ever happened to him?  Maybe|you will program me a dog someday? Or a girlfriend?|Bob|";
                    break;
                case 5: t = "Dear Friend, |You may think I have endless supplies of paper to type on, |but I really dont.  Nor do I have endless food and water.|Lets get on it, mmkay?|Bob|";
                    break;
            }

            GameObject.Find("GameObject").GetComponent<TypewriterController>().enabled = true;
            GameObject.Find("GameObject").GetComponent<TypewriterController>().characterLetter = t;
        }

        if (currentActivity == Activities.Typewriter && GameObject.Find("GameObject").GetComponent<TypewriterController>().enabled == false && hasStarted)
        {
            state = CharacterStates.finishedactivity;
        }

        hasStarted = true;
    }

    void FinishedActivity(Activities activity)
    {
        state = CharacterStates.idle;

        StopAnimations();

        if (activity == Activities.Sleep)
        {
            transform.position = new Vector3(7.22f, 5.43f, 2.62f);
            transform.forward = new Vector3(0f, 0f, 1f);

            GameObject.Find("Bed_Sheet").GetComponent<MeshRenderer>().enabled = false;
        }

        if (activity == Activities.TV)
        {
            if (GameObject.Find("TVScreen").GetComponent<TVController>().state == TVController.States.off)
            {
                GameObject.Find("TVScreen").GetComponent<TVController>().state = TVController.States.on;
                PlaySound("tv1", false);
                activityQueue.Push(Activities.TVChair);
            }
            else
            {
                GameObject.Find("TVScreen").GetComponent<TVController>().state = TVController.States.off;
            }
        }

        if (activity == Activities.Cupboard)
        {
            activityQueue.Push(Activities.Refrigerator);
        }

        if (activity == Activities.Stove)
        {
            activityQueue.Push(Activities.KitchenTable);
        }

        if (activity == Activities.LeaveHouse)
        {
            GameObject.Find("FrontDoor").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            transform.position = new Vector3(-7.83f, 2.16f, 1.15f);
        }

        if (activity == Activities.Toilet)
        {
            GameObject.Find("Toilet Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            transform.position = new Vector3(-1.11f, 5.53f, 1.43f);
            activityQueue.Push(Activities.BathroomSink);
        }

        if (activity == Activities.Attic)
        {
            GameObject.Find("Attic Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            transform.position = new Vector3(-0.79f, 8.66f, 1.15f);
        }

        if (activity == Activities.Closet)
        {
            GameObject.Find("Closet Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
        }

        if (activity == Activities.Refrigerator)
        {
            GameObject.Find("Refrigerator Door").GetComponent<DoorController>().state = DoorController.DoorStates.closed;
            activityQueue.Push(Activities.Stove);
        }

        if (activity == Activities.BuildFire)
        {
            GameObject.Find("Fire").GetComponent<ParticleSystem>().Play();
        }

        if (currentActivity == Activities.Sofa)
        {
            GameObject.Find("Book").GetComponent<MeshRenderer>().enabled = false;
        }

        if (currentActivity == Activities.Typewriter)
        {
            GameObject.Find("GameObject").GetComponent<TypewriterController>().enabled = false;
        }

        if (currentActivity == Activities.Phone)
        {
            GameObject.Find("Phone_Handset").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("Phone").GetComponent<MeshRenderer>().enabled = false;
            characterModel.GetComponent<Animator>().SetBool("isUsingPhone", false);
        }

        hasStarted = false;
    }

    private void StopAnimations()
    {
        characterModel.GetComponent<Animator>().SetBool("isSitting", false);
        characterModel.GetComponent<Animator>().SetBool("isBending", false);
        characterModel.GetComponent<Animator>().SetBool("isDoing", false);
        characterModel.GetComponent<Animator>().SetBool("isEating", false);
        characterModel.GetComponent<Animator>().SetBool("isExercising", false);
        characterModel.GetComponent<Animator>().SetBool("isReading", false);
        characterModel.GetComponent<Animator>().SetBool("isRelaxing", false);

        GameObject.Find("Shower").GetComponent<ParticleSystem>().Stop();
        GetComponent<AudioSource>().Stop();
    }

    Activities GetNextActivity()
    {
        if (activityQueue.Count > 0)
            return activityQueue.Pop();

        TimeSpan start = new TimeSpan(23, 00, 0);
        TimeSpan end = new TimeSpan(6, 00, 0); 
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now > start) && (now < end))
        {
            return Activities.Sleep;
        }

        int nextActivity = UnityEngine.Random.Range(1, 25);

        switch (nextActivity)
        {
            case 1: return Activities.BathroomSink;
            case 2: return Activities.ComputerDesk;
            case 3: return Activities.ComputerRoom;
            case 4: return Activities.FrontDoor;
            case 5: return Activities.Kitchen;
            case 6: return Activities.KitchenTable;
            case 7: return Activities.Shower;
            case 8: return Activities.Stove;
            case 9: return Activities.TV;
            case 10: return Activities.TVChair;
            case 11: return Activities.Typewriter;
            case 12: return Activities.Piano;
            case 13: return Activities.Sofa;
            case 14: return Activities.Sleep;
            case 15: return Activities.LeaveHouse;
            case 16: return Activities.BuildFire;
            case 17: return Activities.Cupboard;
            case 18: return Activities.Toilet;
            case 19: return Activities.ReadBook;
            case 20: return Activities.Exercise;
            case 21: return Activities.Phone;
            case 22: return Activities.Attic;
            case 23: return Activities.Closet;
            case 24: return Activities.WashDishes;
        }

        return Activities.FrontDoor;
    }

    void StartWalk(int floor, Vector3 location, Path.FacingDirection facing, int timeToStay, bool isCarrying = false)
    {
        List<Vector3> waypoints = GetPath(currentFloor, floor);
        waypoints.Add(location);
        path = new Path(waypoints, facing, floor, timeToStay);
        StartCoroutine(WalkCoroutine(path, isCarrying));
    }

    List<Vector3> GetPath(int startFloor, int endFloor)
    {
        List<Vector3> waypoints = new List<Vector3>();

        if (startFloor == 1 && endFloor == 2)
        {
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
        }

        if (startFloor == 1 && endFloor == 3)
        {
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f)); // 1 floor stairs
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f)); // 2ns floor stairs
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f)); // 3rd floor stairs
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
        }

        if (startFloor == 2 && endFloor == 3)
        {
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
        }

        if (startFloor == 3 && endFloor == 2)
        {
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
        }

        if (startFloor == 3 && endFloor == 1)
        {
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f)); // 3rd floor stairs
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f)); // 2ns floor stairs
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f)); // 1 floor stairs
        }

        if (startFloor == 2 && endFloor == 1)
        {
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f));
        }

        return waypoints;
    }
    
    IEnumerator WalkCoroutine(Path path, bool isCarrying)
    {
        StartAltSound("walk", true);
        state = CharacterStates.walking;

        if (isCarrying)
            characterModel.GetComponent<Animator>().SetBool("isCarrying", true);
        else
            characterModel.GetComponent<Animator>().SetBool("isWalking", true);

        foreach (Vector3 position in path.waypoints)
        {
            while (Vector3.Distance(transform.position, position) > .0001)
            {
                var lookPos = position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }

            if (position == new Vector3(0.35f, 2.12f, 2.62f))
                GameObject.Find("Main Camera").GetComponent<CameraController>().floor = 1;
            if (position == new Vector3(2.62f, 5.3f, 2.4f))
                GameObject.Find("Main Camera").GetComponent<CameraController>().floor = 2;
            if (position == new Vector3(0.21f, 8.58f, 2.37f))
                GameObject.Find("Main Camera").GetComponent<CameraController>().floor = 3;

        }

        characterModel.GetComponent<Animator>().SetBool("isCarrying", false);
        characterModel.GetComponent<Animator>().SetBool("isWalking", false);

        if (path.finalFacing == Path.FacingDirection.forward)
                transform.forward = new Vector3(0f, 0f, 1f);
        else if (path.finalFacing == Path.FacingDirection.backward)
            transform.forward = new Vector3(0f, 0f, -1f);
        else if (path.finalFacing == Path.FacingDirection.right)
            transform.forward = new Vector3(1f, 0f, 0f);
        else if (path.finalFacing == Path.FacingDirection.left)
            transform.forward = new Vector3(-1f, 0f, 0f);
        else if (path.finalFacing == Path.FacingDirection.up)
            transform.forward = new Vector3(0f, 1f, 0f);

        currentFloor = path.floor;
        state = CharacterStates.startactivity;
        arrivalTime = System.DateTime.Now;

        
    }

    void PlaySound(string sound, bool loop)
    {
        if (sound != lastSound)
        {
            GetComponent<AudioSource>().Stop();

            AudioClip clip = Resources.Load<AudioClip>(sound);
            GetComponent<AudioSource>().loop = loop;
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();

            lastSound = sound;
        }

    }

    void StopSound()
    {
        GetComponent<AudioSource>().Stop();
    }

    void StartAltSound(string sound, bool loop)
    {
        AudioSource go = GameObject.Find("GameObject").GetComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>(sound);
        go.loop = loop;
        go.clip = clip;
        go.Play();
    }

    void StopAltSound()
    {
        AudioSource go = GameObject.Find("GameObject").GetComponent<AudioSource>();
        go.Stop();
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            int r = UnityEngine.Random.Range(1, 3);

            yield return new WaitForSeconds(r);

            tempVec1 = GameObject.Find("REyeball").transform.localScale;
            tempVec2 = GameObject.Find("LEyeball").transform.localScale;

            //Turn My game object that is set to false(off) to True(on).
            GameObject.Find("REyeball").transform.localScale = new Vector3(0, 0, -1);
            GameObject.Find("LEyeball").transform.localScale = new Vector3(0, 0, -1);

            //Turn the Game Oject back off after 1 sec.
            yield return new WaitForSeconds(0.1f);

            //Game object will turn off
            GameObject.Find("REyeball").transform.localScale = tempVec1;
            GameObject.Find("LEyeball").transform.localScale = tempVec2;
        }

    }

}
