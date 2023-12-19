


using System.Collections;
using System.Collections.Generic;

public partial class MusicBeatState : Node
{
	public static int curSection = 0;
    private int stepsToDo = 0;
    
    public static int curStep = 0;
    public static int curBeat = 0;

    private float curDecStep = 0;
    private float curDecBeat = 0;

	public static float timePassedOnState = 0;

	public override void _Process(double delta)
	{
		int oldStep = curStep;
        timePassedOnState += 1.0f/(float)delta;
        
        updateCurStep();
        updateBeat();

        if (oldStep != curStep)
        {
            if (curStep > 0)
            {
                stepHit();
            }
            
            if (oldStep < curStep)
                updateSection();
        }
	}

	    private void updateSection()
    {
        if(stepsToDo < 1) stepsToDo = Mathf.RoundToInt(getBeatsOnSection() * 4);
        while(curStep >= stepsToDo)
        {
            curSection++;
            float beats = getBeatsOnSection();
            stepsToDo += Mathf.RoundToInt(beats * 4);
            sectionHit();
        }
    }

    public virtual void sectionHit() {}
    
    public virtual void stepHit()
    {
        if (curStep % 4 == 0)
            beatHit();
    }

    private void updateCurStep()
    {
        BPMChangeEvent lastChange = Conductor.getBPMFromSeconds(Conductor.songPosition);
        float shit = ((Conductor.songPosition - 0) - lastChange.songTime) / lastChange.stepCrochet;
        curDecStep = lastChange.stepTime + shit;
        curStep = lastChange.stepTime + Mathf.FloorToInt(shit);
    }

    float getBeatsOnSection()
    {
        float val = 4;
        //if(PlayState.instance.SONG != null && PlayState.instance.SONG.notes[curSection] != null) val = PlayState.instance.SONG.notes[curSection].sectionBeats;
        return val;
    }
    
    private void updateBeat()
    {
        curBeat = Mathf.FloorToInt(curStep / 4);
        curDecBeat = curDecStep / 4;
    }
    
    public virtual void beatHit() {}
}
