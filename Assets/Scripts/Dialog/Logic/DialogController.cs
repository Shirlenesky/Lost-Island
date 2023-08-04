using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogData_SO dialogEmpty;
    public DialogData_SO dialogFinish;

    private Stack<string> dialogEmptyStack;
    private Stack<string> dialogFinishStack;

    private bool isTalking;

    private void Awake()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        FillDialogStack();
    }



    private void FillDialogStack()
    {
        dialogEmptyStack = new Stack<string>();
        dialogFinishStack = new Stack<string>();

        for(int i = dialogEmpty.dialogList.Count - 1;i >= 0;i--)
        {
            dialogEmptyStack.Push(dialogEmpty.dialogList[i]);
        }

        for (int i = dialogFinish.dialogList.Count - 1; i >= 0; i--)
        {
            dialogFinishStack.Push(dialogFinish.dialogList[i]);
        }
    }

    public void ShowDialogEmpty()
    {
        if(!isTalking)
        {
            StartCoroutine(DialogRoutine(dialogEmptyStack));
        }
    }
    public void ShowDialogFinish()
    {
        if (!isTalking)
        {
            StartCoroutine(DialogRoutine(dialogFinishStack));
        }
    }

    private IEnumerator DialogRoutine(Stack<string> data)
    {
        isTalking = true;
        if(data.Count > 0)
        {
            string res = data.Peek();
            data.Pop();
            EventHandler.CallShowDialogEvent(res);
            yield return null;
            isTalking = false;
            EventHandler.CallGameStateChangeEvent(GameState.Pause);
        }
        else
        {
            EventHandler.CallShowDialogEvent(string.Empty);
            FillDialogStack();
            isTalking = false;
            EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        }
    }
}
