using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct RoomState
{
    public bool bCanUseRoom;

    public int lastCheckinTime;
    public int lastCheckoutTime;
    public int ghostFrameInvokeTime;

    public EBedType currnetBedType;
    public bool bGhostFrame;
    public bool bIsClearTrashcan;
    public bool bIsOpenBlind;

    public bool bIsOpenToilet;

    public bool bIsClearShowerRoom;
    public bool bIsOpenLeftDrawer;
    public bool bIsOpenRightDrawer;
    

    public void Initialize()
    {
        bCanUseRoom = true;

        lastCheckinTime = -1;
        currnetBedType = EBedType.clear;
        bIsOpenToilet = true;
        bIsOpenLeftDrawer = false;
        bIsOpenRightDrawer = false;
        bIsClearTrashcan = true;
        bIsClearShowerRoom = true;
        bIsOpenBlind = true;
        bGhostFrame = false;
    }
    public bool IsClear()
    {
        return currnetBedType == EBedType.clear && bIsClearTrashcan && bIsClearShowerRoom;
    }
    public bool IsGameOverBed()
    {
        return currnetBedType == EBedType.oneDeadBody || currnetBedType == EBedType.twoDeadBody
            || currnetBedType == EBedType.blood || currnetBedType == EBedType.liquid || currnetBedType == EBedType.liquid2;
    }
}
public struct CorriderState
{
    public ECorriderState currentState;
    public int lastEventInvokeTime;

    public void Initialize()
    {
        currentState = ECorriderState.clear;
        lastEventInvokeTime = -1;
    }

}

public enum ECorriderState
{
    clear,
    blood,
    liquid,
    liquid2,
    babyGhost,

    max
}

public enum EItemType
{
    red,
    blue,
    black,
    yellow,
    purple,
    valve,

    max
}
//이벤트들 실행되고있는지 열거형으로 처리(나중 인덱스 알아보기 쉽게 처리하려고)
// 특정 씬에서만 이루어지는 이벤트(다른맵으로 이동할 경우 종료)이벤트는 제외
// 복도 바닥의 수상한 액체 이벤트는 CorriderState에서 관리하므로 제외
// 침대 관련 이벤트는 RoomState의 BedType으로 관리하므로 제외
public enum EEventType
{
    checkin,
    checkout,

    bloodReflux,
    //끝 알 수 있게(초기화시!)
    max
}
// 무조건 이벤트들
public enum ENoConditionEventType
{
    bloodReflux,
    corriderLiquid,
    ghostFrame,
    max
}

// 침대 상태
public enum EBedType
{
    clear,
    dirty,
    blood,
    liquid,
    liquid2,
    oneDeadBody,
    twoDeadBody,

    max
}

public class GameEventManager : MonoBehaviour
{
    //실제시간에서 게임시간으로 바꿔서 저장하기위한 비율
    public float ratioGameTime = 30;//하루에 24분 * 5일이라고 가정

    public int currentDay = 1;
    public int currentFloor = 0;

    public float gameStartTime;
    public int curGameTime;
    public bool bStartGame = false;
    public bool bIsClickedDetergent = false;
    public int deadline = 30;//게임시간으로 30분(실제시간으로는 2분정도)
    public AudioSource mainAudioSource;
    public bool bShowStoryBoard;

    public int[] checkinTime =
    {
        0,20,15,15,10,10
    };
    public int[] checkinDeadline =
    {
        0,20,15,15,10,10
    };
    public int[] checkoutTime =
    {
        0,40,30,30,20,20
    };
    public int[] checkoutDeadline =
    {
        0,20,15,15,10,10
    };

    public int[] noconditionEventTime =
    {
        0,20,15,15,10,10
    };

    public int[] bloodRefluxDeadline =
    {
        0,20,15,15,10,10
    };

    public int[] corriderBloodDeadline =
    {
        0,20,15,15,10,10
    };

    public int[] corriderLiquidDeadline =
    {
        0,30,30,30,30,30
    };

    public int[] bedBloodDeadLine =
    {
        0,20,15,15,10,10
    };
    public int[] bedLiquidDeadLine =
    {
        0,30,30,30,30,30
    };
    public int[] bedDeadBodyDeadLine =
    {
        0,30,30,30,30,30
    };
    public int[] percentageBedLiquid =
    {
        0,10,10,20,20,30
    };
    public int[] ghostFrameDeadLine =
    {
        0,20,15,15,10,10
    };
    public EEventType[] noconditionEvents =
    {
        EEventType.bloodReflux,



    };

    public int noconditionEventCheckTime;
    public int checkinCheckTime;
    public int checkoutCheckTime;

    public int floor;
    public int number;

    public List<KeyValuePair<int, int>> checkoutKeys = new List< KeyValuePair<int,int >> ();


    public int[] lastEventInvokeTime = new int[(int)EEventType.max];
    //이벤트들 상태, 아이템, 문서
    public bool[] beventState = new bool[(int)EEventType.max];
    public bool[] bitemState = new bool[(int)EItemType.max];


    //이벤트를 관리하는 관리자***
    public static GameEventManager instance = null;

    //true면 사용가능 false면 사용불가능
    public RoomState[,] roomStates = new RoomState[4, 5];
    public CorriderState[] corriderStates = new CorriderState[4];

    //경고 몇 번 받았는지 저장할 변수
    public int warning;
    // Start is called before the first frame update
    //Awake는 Start보다 먼저 호출되는 함수
    void Awake()
    {
        //만약에 생성되었는데 관리하는 관리자가 없으면 자기자신을 관리자로 임명
        if (instance == null)
        {
            instance = this;//this는 스크립트말하는거
        }
        //만약에 생성되었는데 관리하는 관리자가 있으면 오브젝트 자살
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //게임씬이 변경되어도 죽지않게 하는 코드
        DontDestroyOnLoad(gameObject);
    }
    public void GameStart()
    {
        bStartGame = true;
        gameStartTime = Time.realtimeSinceStartup;
        checkinCheckTime = checkinTime[currentDay];
        checkoutCheckTime = checkoutTime[currentDay];
        noconditionEventCheckTime = noconditionEventTime[currentDay];
        bShowStoryBoard = true;
        mainAudioSource.Play();

    }
    private void Start()
    {
        //방 상태 초기화
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                roomStates[i, j] = new RoomState();
                roomStates[i, j].Initialize();
            }
        }

        // 이벤트 상태 초기화
        for (int i = 0; i < (int)EEventType.max; i++)
        {
            beventState[i] = false;
        }
        //아이템 상태 초기화
        for (int i = 0; i  < (int)EItemType.max; i++)
        {
            bitemState[i] = false;
        }
        // 복도 상태 초기화
        for (int i=1; i<=3; i++)
        {
            corriderStates[i].Initialize();
        }

        //경고 초기화
        warning = 0;
        noconditionEventCheckTime = noconditionEventTime[currentDay];
    }
    public bool IsEventInvoked(EEventType eventType)
    {
        return beventState[(int)eventType];
    }
    public void AddWarning(int amount)
    {
        Debug.Log(amount);
        warning += amount;
        if(warning >=4)
            GameOver();
    }
    public void GetItem(int item)
    {
        //bItem[item] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bStartGame)
        {

            //게임시작시간부터 현재까지의 실제시간 담고있는 변수
            float curRealTime = Time.realtimeSinceStartup - gameStartTime;
            //현재게임(내)시간
            curGameTime = (int)(curRealTime * ratioGameTime) / 60;
            if (curGameTime >= 60 * 6 && bStartGame)
            {
                ToNextDay();
                return;
            }


            if (beventState[(int)EEventType.checkin] &&
                lastEventInvokeTime[(int)EEventType.checkin] - curGameTime >= checkinDeadline[currentDay])
            {
                AddWarning(1);
                beventState[(int)EEventType.checkin] = false;
            }
            if (checkoutKeys.Count >= 1 && lastEventInvokeTime[(int)EEventType.checkout] - curGameTime >= checkoutDeadline[currentDay])
            {
                AddWarning(1);
                lastEventInvokeTime[(int)EEventType.checkout] = curGameTime;
            }

            if (curGameTime == checkinCheckTime && curGameTime!=0)
            {
                checkinCheckTime += checkinTime[currentDay];
                CheckCondition_checkin();
            }
            if (curGameTime == checkoutCheckTime && curGameTime != 0)
            {
                checkoutCheckTime += checkoutTime[currentDay];
                CheckCondition_checkout();
            }

            CheckNoConditionEvents();

            CheckBeds();

            CheckCorriders();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ToNextDay()
    {
        currentDay += 1;
        noconditionEventCheckTime = noconditionEventTime[currentDay];
        checkinCheckTime = checkinTime[currentDay];
        checkoutCheckTime = checkoutTime[currentDay];

        Debug.Log("Day " + currentDay);
        gameStartTime = Time.realtimeSinceStartup;
        bShowStoryBoard = true;
        SceneManager.LoadScene("LobbyInformation");
    }


    #region 로비 관련 함수 시작
    public void CheckCondition_checkin()
    {
        if (beventState[(int)EEventType.checkin])
            return;
        //체크인이벤트발생조건검사
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                if (i == 3 && j == 2)
                {
                    continue;
                }
                if (roomStates[i, j].bCanUseRoom == true)
                {
                    if (beventState[(int)EEventType.checkin] == false)
                    {
                        if (Random.value > 0.5f)
                        {
                            //체크인이벤트발생
                            Invoke_checkin();
                        }
                        return;
                    }
                }
            }
        }
    }
    public void Invoke_checkin()
    {
        //마지막 체크인 시간을 현재 게임시간으로 초기화
        //        lastCheckinTime = curGameTime;
        beventState[(int)EEventType.checkin] = true;
        SoundFXManager.instance.PlaySound(ESound.ECheckin);
    }
    public void CheckCondition_checkout()
    {
        List<KeyValuePair<int, int>> checkoutRooms = new List<KeyValuePair<int, int>>();
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (roomStates[i, j].bCanUseRoom == false)
                {
                    int elapsedTime = curGameTime - roomStates[i, j].lastCheckinTime;
                    if (elapsedTime >= 30)
                    {
                        // 이미 체크아웃 발생한 방인지 검사
                        KeyValuePair<int, int> checkoutRoom = new KeyValuePair<int, int>(i, j);
                        bool bAlreadyCheckout=false;
                        for(int k=0; k<checkoutRooms.Count; k++)
                        {
                            if(checkoutRooms[k].Equals(checkoutRoom))
                            {
                                bAlreadyCheckout = true;
                                break;
                            }
                        }
                        // 이미 체크아웃 한 방이라면 넘어가기
                        if (bAlreadyCheckout)
                            continue;
                        // 아니라면 체크아웃 대상으로 추가
                        checkoutRooms.Add(checkoutRoom);
                    }
                }
            }
        }

        if (checkoutRooms.Count >= 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                int checkoutRoomIdx = Random.Range(0, checkoutRooms.Count);
                Invoke_checkout(checkoutRooms[checkoutRoomIdx]);
            }
        }
    }
    public void Invoke_checkout(KeyValuePair<int,int> checkoutRoom)
    {
        if(checkoutKeys.Count<2)
        {
            checkoutKeys.Add(checkoutRoom);
        }
    }
    public int GetCheckoutKeyCount()
    {
        return checkoutKeys.Count;
    }
    public KeyValuePair<int,int> GetCheckoutRoom(int keyNumber)
    {
        return checkoutKeys[keyNumber];
    }
    public void Checkout(int keyNumber)
    {
        KeyValuePair<int, int> CheckoutRoom = checkoutKeys[keyNumber];
        checkoutKeys.RemoveAt(keyNumber);
        int floor = CheckoutRoom.Key;
        int number = CheckoutRoom.Value;
        roomStates[floor, number].bCanUseRoom = true;


        if(Random.Range(0,100) < percentageBedLiquid[currentDay] )
        {
            if(Random.Range(0,100)<80)
            {
                roomStates[floor, number].currnetBedType = EBedType.blood;
            }
            else
            {
                roomStates[floor, number].currnetBedType = EBedType.liquid;
            }
        }
        else
        {
            roomStates[floor, number].currnetBedType = EBedType.dirty;
        }

        roomStates[floor, number].bIsClearTrashcan = false;
        roomStates[floor,number].bIsClearShowerRoom = false;

        roomStates[floor, number].lastCheckoutTime = curGameTime;
    }

    #endregion


    #region 방 관련 함수 시작
    // ShowerRoom 함수 시작
    public bool IsOpenToilet()
    {
        return roomStates[floor, number].bIsOpenToilet;
    }
    public void OnClickedRoomToilet()
    {
        roomStates[floor, number].bIsOpenToilet = !roomStates[floor, number].bIsOpenToilet;
    }
    public bool IsClearShowerRoom()
    {
        return roomStates[floor, number].bIsClearShowerRoom;
    }
    public void OnClickShowerRoom()
    {
        roomStates[floor, number].bIsClearShowerRoom = true;
    }
    // Shower Room 함수 끝

    // DressingTable 함수 시작
    public bool IsLeftDrawerOpen()
    {
        return roomStates[floor, number].bIsOpenLeftDrawer;
    }
    public bool IsRightDrawerOpen()
    {
        return roomStates[floor, number].bIsOpenRightDrawer;
    }

    public void OnClickedLeftDrawer()
    {
        roomStates[floor, number].bIsOpenLeftDrawer = !roomStates[floor, number].bIsOpenLeftDrawer;
    }
    public void OnClickedRightDrawer()
    {
        roomStates[floor, number].bIsOpenRightDrawer = !roomStates[floor, number].bIsOpenRightDrawer;
    }
    public bool IsGhostFrame()
    {
        return roomStates[floor, number].bGhostFrame;
    }
    public void OnClickedGhostFrame()
    {
        roomStates[floor, number].bGhostFrame = false;
        SoundFXManager.instance.PlaySound(ESound.EGhostFrameClick);
    }
    // DressingTable 함수 끝

    // Bed 함수 시작
    public EBedType GetBedType()
    {
        return roomStates[floor, number].currnetBedType;
    }

    public void OnClickedBed()
    {
        EBedType currentBedType = roomStates[floor, number].currnetBedType;
        switch (currentBedType)
        {
            case EBedType.clear:
                return;
            case EBedType.blood:
            case EBedType.dirty:
                roomStates[floor, number].currnetBedType = EBedType.clear;
                break;
            case EBedType.liquid:
                roomStates[floor, number].currnetBedType = EBedType.liquid2;
                break;
            case EBedType.liquid2:
            case EBedType.oneDeadBody:
            case EBedType.twoDeadBody:
                GameOver();
                break;
        }
    }
    public void CheckBeds()
    {
        for(int i=1; i<=3; i++)
        {
            for(int j=1; j<4; j++)
            {
                EBedType bedType = roomStates[i, j].currnetBedType;
                #region 침대 검사
                if (bedType == EBedType.clear)
                    continue;
                int elapsedTime = roomStates[i, j].lastCheckinTime - curGameTime;
                if (bedType == EBedType.liquid || bedType == EBedType.liquid2)
                {
                    if(elapsedTime >= bedLiquidDeadLine[currentDay])
                    {
                        roomStates[i, j].currnetBedType = EBedType.clear;
                    }
                }

                else if(bedType == EBedType.blood)
                {
                    if(elapsedTime >= bedBloodDeadLine[currentDay])
                    {
                        roomStates[i, j].currnetBedType = EBedType.clear;
                        AddWarning(1);
                    }
                }
                else if(bedType == EBedType.oneDeadBody || bedType == EBedType.twoDeadBody)
                {
                    if(elapsedTime >= bedDeadBodyDeadLine[currentDay])
                    {
                        roomStates[i, j].currnetBedType = EBedType.clear;
                    }
                }
                #endregion
                #region 귀신 액자 검사
                if (roomStates[i,j].bGhostFrame)
                {
                    elapsedTime = curGameTime - roomStates[i, j].ghostFrameInvokeTime;
                    if(elapsedTime>=ghostFrameDeadLine[currentDay])
                    {
                        AddWarning(1);
                    }
                }
                #endregion
            }
        }
    }
    public bool IsOpenBlind()
    {
        return roomStates[floor, number].bIsOpenBlind;
    }

    public void OnClickedBlind()
    {
        roomStates[floor, number].bIsOpenBlind = !roomStates[floor, number].bIsOpenBlind;
    }
    public bool isClearTrashCan()
    {
        return roomStates[floor, number].bIsClearTrashcan;
    }
    public void OnClickedTrashCan()
    {
        roomStates[floor, number].bIsClearTrashcan = true;
    }

    // Bed 함수 끝


    public void OnClickedFrontDoor()
    {
        switch(floor)
        {
            case 1:
                SceneManager.LoadScene("CorriderFloor1");
                break;
            case 2:
                SceneManager.LoadScene("CorriderFloor2");
                break;
            case 3:
                SceneManager.LoadScene("CorriderFloor3");
                break;
            default :
                break;
        }
    }
    public void MoveFloor(int cFloor)
    {
        currentFloor = cFloor;
    }
    public int GetCurrentFloor()
    {
        return currentFloor;
    }
    #endregion


    #region 무조건 이벤트관련 함수들

    public void CheckNoConditionEvents()
    {
        if (curGameTime == noconditionEventCheckTime && curGameTime != 0)
        {
            noconditionEventCheckTime += noconditionEventTime[currentDay];
            ChoiceNoConditionEvent();
        }
        for (int i = 0; i < noconditionEvents.Length; i++)
        {
            EEventType eventType = (EEventType)noconditionEvents[i];
            if (IsEventInvoked(eventType))
            {
                int elapsedTime = curGameTime - lastEventInvokeTime[(int)eventType];

                switch (eventType)
                {
                    case EEventType.bloodReflux:
                        if (elapsedTime >= bloodRefluxDeadline[currentDay])
                        {
                            AddWarning(1);
                            beventState[(int)eventType] = false;
                        }
                        break;
                }
            }
        }
    }
    public void ChoiceNoConditionEvent()
    {
        while (true)
        {
            int targetEvent = Random.Range(0, (int)ENoConditionEventType.max);
            Debug.Log((ENoConditionEventType)targetEvent);
            switch ((ENoConditionEventType)targetEvent)
            {
                case ENoConditionEventType.bloodReflux:
                    if (IsEventInvoked(EEventType.bloodReflux))
                        continue;
                    Invoke_bloodReflux();
                    break;
                case ENoConditionEventType.corriderLiquid:
                    if (corriderStates[1].currentState != ECorriderState.clear &&
                        corriderStates[2].currentState != ECorriderState.clear &&
                        corriderStates[3].currentState != ECorriderState.clear)
                        continue;
                    Invoke_corriderLiquid();
                    break;
                case ENoConditionEventType.ghostFrame:
                    if (roomStates[1, 2].bCanUseRoom || roomStates[2, 2].bCanUseRoom || 
                        roomStates[2, 4].bCanUseRoom || roomStates[3, 1].bCanUseRoom)
                    {
                        Invoke_ghostFrame();
                    }
                    else
                        continue;
                    break;
            }

            break;
        }
    }
    public void Invoke_ghostFrame()
    {
        int x;
        int floor, number;
        KeyValuePair<int, int>[] rooms =
        {
            new KeyValuePair<int,int>(1,2),
            new KeyValuePair<int,int>(2,2),
            new KeyValuePair<int,int>(2,4),
            new KeyValuePair<int,int>(3,1)

        };
        /*        while (true)
                {
                    x = Random.Range(0, 4);
                    floor = rooms[x].Key;
                    number = rooms[x].Value;
                    if (roomStates[floor, number].bCanUseRoom)
                        break;
                }
        */
        x = Random.Range(0, 4);
        floor = rooms[x].Key;
        number = rooms[x].Value;
        roomStates[floor, number].bGhostFrame = true;
        roomStates[floor, number].ghostFrameInvokeTime = curGameTime;
    }
    public void Invoke_bloodReflux()
    {
        beventState[(int)EEventType.bloodReflux] = true;
        lastEventInvokeTime[(int)EEventType.bloodReflux] = curGameTime;
    }
    public void End_bloodReflux()
    {
        SoundFXManager.instance.PlaySound(ESound.EBubble);
        beventState[(int)EEventType.bloodReflux] = false;
    }

    public void Invoke_corriderLiquid()
    {
        while (true)
        {
            int targetFloor = Random.Range(1, 4);

            if (corriderStates[targetFloor].currentState != ECorriderState.clear)
                continue;

//            if (Random.Range(0, 10) <= 1)
//            {
                corriderStates[targetFloor].currentState = ECorriderState.liquid;
 //           }
   //         else
     //       {
         //       corriderStates[targetFloor].currentState = ECorriderState.blood;
       //     }
            corriderStates[targetFloor].lastEventInvokeTime = curGameTime;

            break;

        }
    }
    public void End_corriderBlood(int targetFloor)
    {
        corriderStates[targetFloor].currentState = ECorriderState.clear;
    }
    #endregion

    #region 복도 관련 함수들
    void CheckCorriders()
    {
        for(int i=1; i<=3; i++)
        {
            if (corriderStates[i].currentState == ECorriderState.clear)
                continue;
            int elapsedTime = curGameTime - corriderStates[i].lastEventInvokeTime;
            Debug.Log(corriderStates[i].currentState);
            Debug.Log(elapsedTime);
            if(corriderStates[i].currentState == ECorriderState.blood && elapsedTime>=corriderBloodDeadline[currentDay])
            {
                corriderStates[i].currentState = ECorriderState.clear;
                AddWarning(1);
            }
            if ((corriderStates[i].currentState == ECorriderState.liquid || corriderStates[i].currentState == ECorriderState.liquid2)
                && elapsedTime >= corriderLiquidDeadline[currentDay])
            {
                corriderStates[i].currentState = ECorriderState.clear;
            }
        }
    }
    public CorriderState GetCorriderState(int floor)
    {
        return corriderStates[floor];
    }
    public void OnClickedCorriderLiquid(int floor)
    {
        if(corriderStates[floor].currentState == ECorriderState.liquid)
        {
            corriderStates[floor].currentState = ECorriderState.liquid2;
        }
        else if(corriderStates[floor].currentState == ECorriderState.liquid2)
        {
            GameOver();
        }
    }
    public void OnClickedCorriderBlood(int floor)
    {
        corriderStates[floor].currentState = ECorriderState.clear;
    }
    #endregion


    //배경음악 on
    public void ClickBgmOn()
    {
        mainAudioSource.Play();
    }
    //배경음악 off
    public void ClickBgmOff()
    {

        mainAudioSource.Stop();
    }

    public void GameOver()
    {
        mainAudioSource.Stop();
        bStartGame = false;
        SceneManager.LoadScene("GameOver");
    }
}