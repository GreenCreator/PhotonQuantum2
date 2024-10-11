using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class InsurerUI : MonoBehaviourPun
{
    [SerializeField] private Text _points;


    private int point;
    public static InsurerUI UI { get; private set; }

    private void Awake()
    {
        UI = this;
    }
    public void SetPoint(int point)
    {
        this.point += point;
        _points.text = this.point.ToString();
    }
}
