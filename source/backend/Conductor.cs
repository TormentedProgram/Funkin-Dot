using Godot;

using System.Collections.Generic;
using System;

public class BPMChangeEvent
{
    public int stepTime;
    public float songTime;
    public float bpm;
	public float stepCrochet;
}

public class SwagNote
{
    public float strumTime;
    public int noteData;
    public float sustainLength;
    public string noteType;
}

public class SwagSection
{
    public List<SwagNote> sectionNotes = new List<SwagNote>();
    public float sectionBeats;
    public int lengthInSteps;
    public int typeOfSection;
    public bool mustHitSection;
    public float bpm;
    public bool changeBPM;
    public bool altAnim;
}

public class SwagSong
{
    public string song;
    public List<SwagSection> notes = new List<SwagSection>();
    public float bpm;
    public bool needsVoices;
    public float speed;
    
    public string player1;
    public string player2;
    public bool validScore;
}

public partial class Conductor : Node
{
	public static float bpm = 100;
    public static float crochet = ((60 / bpm) * 1000);
    public static float stepCrochet = crochet / 4;
    public static float songPosition = 0;
    public static float lastSongPos;
    public static float offset = 0;

	public static float safeZoneOffset = 0;
    public static List<BPMChangeEvent> bpmChangeMap = new List<BPMChangeEvent>();

    public static BPMChangeEvent getBPMFromSeconds(float time)
    {
        BPMChangeEvent lastChange = new BPMChangeEvent
        {
            stepTime = 0,
            songTime = 0,
            bpm = bpm,
            
            stepCrochet = stepCrochet
        };
        foreach (BPMChangeEvent bpmEvent in bpmChangeMap)
        {
            if (time >= bpmEvent.songTime)
                lastChange = bpmEvent;
        }
        return lastChange;
    }
    
    public static BPMChangeEvent getBPMFromStep(float step)
    {
        BPMChangeEvent lastChange = new BPMChangeEvent
        {
            stepTime = 0,
            songTime = 0,
            bpm = bpm,
            stepCrochet = stepCrochet
        };
        foreach (BPMChangeEvent bpmEvent in bpmChangeMap)
        {
            if (bpmEvent.stepTime <= step)
                lastChange = bpmEvent;
        }
        return lastChange;
    }
    
    public static float beatToSeconds(float beat)
    {
        float step = beat * 4;
        BPMChangeEvent lastChange = getBPMFromStep(step);
        return lastChange.songTime + ((step - lastChange.stepTime) / (lastChange.bpm / 60)/4) * 1000;
    }
    
    public static float getStep(float time)
    {
        BPMChangeEvent lastChange = getBPMFromSeconds(time);
        return lastChange.stepTime + (time - lastChange.songTime) / lastChange.stepCrochet;
    }

    public static float getStepRounded(float time)
    {
        BPMChangeEvent lastChange = getBPMFromSeconds(time);
        return lastChange.stepTime + Mathf.FloorToInt(time - lastChange.songTime) / lastChange.stepCrochet;
    }

    public static float getBeat(float time)
    {
        return getStep(time) / 4;
    }
    
    public static float getBeatRounded(float time)
    {
        return Mathf.FloorToInt(getStepRounded(time) / 4);
    }
    
    public static float CalculateCrochet(float bpm)
    {
        return (60f / bpm) * 1000f;
    }

    public static float GetSectionBeats(SwagSong song, int section)
    {
        float? val = null;

        if (song.notes[section] != null)
        {
            val = song.notes[section].sectionBeats;
        }

        return val.HasValue ? val.Value : 4f;
    }

    public static void mapBPMChanges(SwagSong song)
    {
        bpmChangeMap.Clear();

        float curBPM = song.bpm;
        int totalSteps = 0;
        float totalPos = 0;

        for (int i = 0; i < song.notes.Count; i++)
        {
            if (song.notes[i].changeBPM && song.notes[i].bpm != curBPM)
            {
                curBPM = song.notes[i].bpm;

                BPMChangeEvent bpmEvent = new BPMChangeEvent
                {
                    stepTime = totalSteps,
                    songTime = totalPos,
                    bpm = curBPM,
                    stepCrochet = CalculateCrochet(curBPM) / 4
                };

                bpmChangeMap.Add(bpmEvent);
            }
            int deltaSteps = Mathf.RoundToInt(GetSectionBeats(song, i) * 4);
            totalSteps += deltaSteps;
            totalPos += ((60f / curBPM) * 1000f / 4f) * deltaSteps;
        }
    }

    public static float set_bpm(float newBPM)
    {
        bpm = newBPM;
        crochet = CalculateCrochet(bpm);
        stepCrochet = crochet / 4;
        return bpm = newBPM;
    }
}
