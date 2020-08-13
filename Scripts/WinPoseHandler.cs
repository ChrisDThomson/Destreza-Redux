using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoseHandler : MonoBehaviour
{
    public PlayerWinInfo playerWinInfo;
    public WinPose prefab;

    public float maximumOffset = 6;

    List<WinPose> winPoses;
    // Start is called before the first frame update
    void OnEnable()
    {
        List<WinInfo> winners = playerWinInfo.GetWiningPlayerList();

        if (winPoses == null)
        {
            winPoses = new List<WinPose>();
        }

        for (int i = 0; i < winners.Count; i++)
        {
            WinPose w = Instantiate(prefab, transform.position, Quaternion.identity, transform);

            w.SetCharacterWinPoseArt(winners[i].winArt);
            w.SetWinFlowerParticle(winners[i].winFlower);

            Vector3 v = Vector3.zero;
            v.x = (maximumOffset / winners.Count) * i;
            w.gameObject.transform.localPosition += v;

            winPoses.Add(w);
        }
    }

    private void OnDisable()
    {
        if (winPoses != null)
        {
            foreach (WinPose w in winPoses)
            {
                Destroy(w.gameObject);
            }

            winPoses.Clear();
        }

        //playerWinInfo.OnLeaveLevel();
    }
}
