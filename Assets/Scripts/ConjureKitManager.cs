using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Auki.ConjureKit;
using UnityEngine.UI;
using System.Numerics;
using Auki.ConjureKit.Manna;

public class ConjureKitManager : MonoBehaviour
{
    private IConjureKit _conjureKit;
    public Camera arCamera;

    [SerializeField] private Text sessionState;
    [SerializeField] private Text sessionID;


    [SerializeField] private GameObject cube;
    [SerializeField] private Button spawnButton;

    private Manna _manna;

    [SerializeField] bool qrCodeBool;
    [SerializeField] Button qrCodeButton;
    // Start is called before the first frame update
    void Start()
    {
        _conjureKit = new ConjureKit(
            ConjureKitConfiguration.Get(), arCamera.transform,
            "b5344f6c-185a-45cc-a02e-c04a6d825dbe",
            "a8afab64-fd40-42a3-b4fe-1ef8f78492154a1a3cfa-1879-4ab7-a6a1-739570c48674");
        _manna = new Manna(_conjureKit);
        _conjureKit.OnStateChanged += state =>
        {
            sessionState.text = state.ToString();
        };

        _conjureKit.OnJoined += session =>
        {
            sessionID.text = session.Id;
        };

        _conjureKit.OnLeft += () =>
        {
            sessionID.text = "";
        };

        _conjureKit.OnEntityAdded += CreateCube;

        _conjureKit.Connect();
    }
    private void ToggleControlsState(bool interactable)
    {
        if (spawnButton) spawnButton.interactable = interactable;
        if (qrCodeButton) qrCodeButton.interactable = interactable;
    }

    public void ToggleLighthouse()
    {
        qrCodeBool = !qrCodeBool;
        _manna.SetLighthouseVisible(qrCodeBool);

    }

    public void CreateCubeEntity()
    {

        if (_conjureKit.GetState() != State.Calibrated)
            return;

        UnityEngine.Vector3 position = arCamera.transform.position + arCamera.transform.forward * 0.5f;
        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(0, arCamera.transform.eulerAngles.y, 0);

        Pose entityPos = new Pose(position, rotation);

        _conjureKit.AddEntity(
            entityPos,
            onComplete: entity => CreateCube(entity),
            onError: error => Debug.Log(error));
    }

    private void CreateCube(Entity entity)
    {
        if (entity.Flag == EntityFlag.EntityFlagParticipantEntity) return;

        Instantiate(cube, entity.Pose.position, entity.Pose.rotation);
    }
}
