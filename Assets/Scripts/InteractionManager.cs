using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(InteractionManager))]
public class EditorButtonScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //this is my script
        //from object being Inspected :
        InteractionManager myscript = (InteractionManager)target;
        if (GUILayout.Button("ButtonFunction"))
        {
            myscript.ButtonFunction();
        }
       
    }
}
#endif

public class InteractionManager : MonoBehaviour
{
    public enum PlayerInteractions { Sword, Boots, Shield, Helmet }
    public PlayerInteractions RequiredInteraction;
    public static InteractionManager Instance;
    [System.Serializable]
    public struct ColorForInteraction
    {
        public PlayerInteractions InteractionForColor;
        public Color Color;
    }
    
    public ColorForInteraction[] InteractionColors = new ColorForInteraction[4];

    public PlayerButton[] PlayerButtons;
    [Header("button things")]
    public float TimeBetweenWaves = 0.2f;
    public float TimeBetweenPixels=0.000001f;
    public int PixelsPerRow = 2;
    [Header("Player progress")]
    public float CurrentAllowedTime = 1;
    public float MaxTime=3, MinTime = 0.5f;
    private float _currentTime = 0;
    public float DifferencePerScore = 0.1f;

    private int _iScore = 0;
    public UnityEngine.UI.Text Score;
    public UnityEngine.UI.Image TimeDisplay;
    public bool Waiting = true;

    private void Awake()
    {
        ButtonFunction();
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Score.text = "0";
        ResetProgress();
    }


    public Color GetColorForInteration(PlayerInteractions interaction)
    {
        return InteractionColors[(int)interaction].Color;
    }

   

    public void ButtonPress(PlayerInteractions interaction)
    {
        Waiting = false;
        if (RequiredInteraction == interaction)
        {
            //pass
            //get new target interaction
            RequiredInteraction = (PlayerInteractions)Random.Range(0,4);
            Camera.main.backgroundColor = GetColorForInteration(RequiredInteraction);
            _iScore += 1;
            Score.text = _iScore.ToString();
            //update timer
            _currentTime = 0;
            CurrentAllowedTime -= DifferencePerScore;
            if (CurrentAllowedTime < MinTime)
            {
                CurrentAllowedTime = MinTime;
            }
        }
        else
        {
            //gameover?
            ResetProgress();

           
        }
    }

    private void ResetProgress()
    {
        Waiting = true;
        CurrentAllowedTime = MaxTime;
        _iScore = 0;
    }

    public void ButtonFunction()
    {
        for (int i = 0; i < PlayerButtons.Length; i++)
        {
            PlayerButtons[i].SetTextureColor(GetColorForInteration(PlayerButtons[i].Interaction));
        }
    }
    private void Update()
    {
        if (!Waiting)
        {
            _currentTime += Time.deltaTime;
            TimeDisplay.fillAmount = 1 - (_currentTime / CurrentAllowedTime);
            if (_currentTime >= CurrentAllowedTime)
            {
                ResetProgress();
            }
        }
    }
}
