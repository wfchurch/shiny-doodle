using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CodeSimState { waiting, busy, done };

public class CodeSimInst : MonoBehaviour, IComparable<CodeSimInst> {

    public int id;
    public CodeSim codeSim = null;
    public CodeSimInst opCodeInst = null;

    public float taskProgress = 0f;
    private float taskTime = 0f;
    public CodeSimState codeSimState = CodeSimState.waiting;

    private TaskDoneDelegate taskDoneDelegate;
    public delegate void TaskDoneDelegate(CodeSimInst codeSimInst);

    public int CompareTo(CodeSimInst compareInst)
    {
        return codeSim.model.CompareTo(compareInst.codeSim.model);
    }
    
    public float DoCyclesTask(float mHz, TaskDoneDelegate newDelegate)
    {
        //Debug.Log("DoCyclesTask - " + this.ToString());
        float time = mHz * codeSim.cycles;
        StartTask(time, newDelegate);
        return time;
    }

    public float DoReadTask(float mHz, TaskDoneDelegate newDelegate)
    {
        //Debug.Log("DoReadTask - " + this.ToString());
        float time = mHz * codeSim.size;
        StartTask(time, newDelegate);
        return time;
    }

    public void DoTimeTask( float time, TaskDoneDelegate newDelegate)
    {
        StartTask(time, newDelegate);
    }

    public float GetProgress()
    {
        return taskProgress / taskTime;
    }

    public void ResetTask()
    {
        codeSimState = CodeSimState.waiting;
        taskProgress = 0f;
    }

    public float AddTaskTime(int cycles, float mHz)
    {
        taskTime += cycles * mHz;
        return taskTime;
    }

    private void StartTask(float time, TaskDoneDelegate newDelegate)
    {
        taskTime = time;
        taskProgress = 0f;
        taskDoneDelegate = newDelegate;
        StartCoroutine(TaskTimer());
    }

    IEnumerator TaskTimer()
    {
        //Debug.Log("TaskTimer - " + this.ToString());
        codeSimState = CodeSimState.busy;
        while (taskProgress < taskTime)
        {
            if (codeSimState == CodeSimState.waiting)
            {
                taskProgress = 0f;
                break;
            }
            if (codeSimState == CodeSimState.done)
            {
                DoTaskDone();
                break;
            }
            taskProgress += Time.deltaTime;
            yield return null;
        }
        DoTaskDone();
    }

    private void DoTaskDone()
    {
        if(codeSimState == CodeSimState.busy) codeSimState = CodeSimState.done;
        if (taskDoneDelegate != null)
        {
            taskDoneDelegate.Invoke(this);
            taskDoneDelegate = null;
        }
    }
}
