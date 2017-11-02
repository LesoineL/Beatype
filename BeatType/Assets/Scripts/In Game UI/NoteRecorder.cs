using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteRecorder : MonoBehaviour {

    StreamWriter writer; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public  void InitializeFileWriter()
    {
        writer = new StreamWriter("Assets/Resources/notes.txt", true);
    }
    public void CloseFileWriter()
    {
        writer.Close(); 
    }
    public void writeNote(float time)
    {
        writer.WriteLine("addNote(, " + time + "f);");
        Debug.Log("Note Recorded");
    }
}
