using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrowManager))]
public class GameController : MonoBehaviour
{
    //Monster Information
    [SerializeField] private MonsterController monsterController;
    [SerializeField] private CaptureDisplay captureDisplay;
    [SerializeField] private List<GameObject> monsterPrefabs;


    //Player Information
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject playerPrefab;

    //Game Mechanisms
    [SerializeField] private UI_SphereController sphereControllerUI;
    [SerializeField] private ThrowManager throwManager;

    //Variables
    public int numSpheres = 5;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize things
        GameEvents.current.onCaptureMonster += CaptureMonster;
        //
        throwManager.SetNumberOfSpheres(numSpheres);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Z))
        {
            sphereControllerUI.ClearList();
            HardResetNumSpheres(numSpheres);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetSphereIcons(numSpheres);
        
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            sphereControllerUI.DecreaseSphereIconCountByOne();
            throwManager.ThrowSphere();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            sphereControllerUI.ClearList();
        }


        if(Input.GetKeyDown(KeyCode.A))
        {
            throwManager.AutoThrow();
        }
        
    }

    private void CaptureMonster()
    {
        Debug.Log("Monster Captured");
        monsterController.DestroyMonster();

        //Add captured monster to displayed inventory
        AddMonsterToDisplay();

        //Cue cut scene

        //Spawn a new monster this is causing huge bugs, not sure why
        monsterController.InitializeMonster();

        //Reset Throws
        ResetSphereIcons(numSpheres);
    
    }


    private void OnDestroy()
    {
        GameEvents.current.onCaptureMonster -= CaptureMonster;
    }

    private void AddMonsterToDisplay()
    {
        captureDisplay.AddMonsterIcon(monsterController.currentMonsterIcon);
    }

    public void SetNumberOfSpheres(int num)
    {
        numSpheres = num;
    }

    public void HardResetNumSpheres(int num)
    {
        sphereControllerUI.ResetNumberOfSphereIcons(num);
        throwManager.ResetNumberOfSpheres(num);
    }

    public void ResetSphereIcons(int num)
    {
        sphereControllerUI.ResetSphereIcons();
        throwManager.ResetNumberOfSpheres(num);
    }

    

}
