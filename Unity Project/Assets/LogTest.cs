using CSharpLogSystem;
using UnityEngine;

public class LogTest : MonoBehaviour , IDebugerTag
{
    public string Log_Tag
    {
        get { return "<Log_Test_Tag>"; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debuger.Init( true, true, true, true,
            Application.dataPath + "/../LogTest", 
            new UnityDebugerConsole());

        InvokeRepeating("LogRepeating", 1f,1f);
    }

    int count = 0;

    // Update is called once per frame
    void LogRepeating()
    {
        DebugerExtension.Log(this, "LogRepeating: n = {0}...", ++count);
    }
}
