using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldMap : MonoBehaviour
{
    public PlayerInputInfo inputInfo;
    public StagesVector3Dictionary stagePositions;

    public Stages currentStage;
    Vector3 vel = Vector3.one;
    float velMag;
    public float transitionTime = 0.2f;

    public OnAnimationFinished selectDaggerAnim;
    public Vector3 cursorPos = Vector3.up;

    public UnityEvent<string> worldMapDaggerSelected;

    public PlayerWinInfo winInfo;

    private void OnEnable()
    {
        transform.localPosition = stagePositions[currentStage];

        selectDaggerAnim = Instantiate(selectDaggerAnim.gameObject, transform.localPosition, Quaternion.identity, transform.parent).GetComponent<OnAnimationFinished>();
        selectDaggerAnim.transform.localPosition = cursorPos;
        selectDaggerAnim.onAnimationFinished += DaggerSelected;

        winInfo.OnLeaveLevel();
    }

    public void MoveMap()
    {
        currentStage++;

        if (currentStage == Stages.StageUnset)
            currentStage = Stages.Spain;

        velMag = 1;
    }

    private void Update()
    {
        if (velMag != 0)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, stagePositions[currentStage], ref vel, transitionTime);
            velMag = Mathf.Round(vel.magnitude);
        }
    }

    public void OnPlayerJoined()
    {
        int index = inputInfo.players.Count - 1;


        inputInfo.players[index].onUpButtonPressed += MoveMap;
    }

    public void OnPlayersSelection()
    {
        selectDaggerAnim.animator.SetTrigger("Selected");
    }

    public void OnPlayersSelectionCanceled()
    {
        if (selectDaggerAnim.animator.GetCurrentAnimatorStateInfo(0).IsName("dagger_select"))
            selectDaggerAnim.animator.SetTrigger("Deselect");
    }

    public void DaggerSelected()
    {
        winInfo.OnSelectionLocked();
        worldMapDaggerSelected.Invoke(currentStage.ToString());
    }
}

public enum Stages
{
    //order is important 
    Spain,
    Rome,
    Germany,
    Turkey,
    India,
    China,
    Japan,


    StageUnset
}

[System.Serializable]
public class StagesVector3Dictionary : SerializableDictionary<Stages, Vector3> { }
