using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public enum JobFields
{
    NONE,
    Assistant_Nurse,
    Assistant_Midwife,
    Assistant_Medtech,
    Assistant_Radtech,
    Assistant_Pharmacist,
    Assistant_Researchlab,
    Office_Assistant,
    HR_Assistant,
    IT_Assistant,
    Assistant_Engineer,
    Assistant_Architect,
    Assistant_Teacher,
    Assistant_Manager,
    Bookkeeper,
    Assistant_Programmer,
    Assistant_Data_Analyst,
    Data_Analyst,
    Assistant_Salesrep,
    Assistant_Mechanic,
    Service_Staff,
    Call_Center_Agent
}


public class JobApplicationManager : MonoBehaviour
{
    [SerializeField] private GameObject jobSystemOverlay;
    [SerializeField] private GameObject jobPosListOverlay;
    [SerializeField] private Transform availableJobPositionsHolder;
    [SerializeField] private GameObject jobDetailedViewOverlay;
    [SerializeField] private GameObject jobPositionPrefab;
    [SerializeField] private Prompts notQualifiedPrompt;
    [SerializeField] private GameObject qualifiedMsgOverlay;
    [SerializeField] private TextMeshProUGUI congratulatoryMsg;
    [SerializeField] private List<JobPositions> hospitalJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> cityHallJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> universityJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> fireStationJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> cafeteriaJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> policeStationJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> wasteMngmntFacilityJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> waterDistrictJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> bankJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> cafeJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> foodXpressJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> beeUsolutionsJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> autoDealerJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> convenienceStoreJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> factoryJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> callCenterJobPositions = new List<JobPositions>();
    [SerializeField] private List<JobPositions> electricCompanyJobPositions = new List<JobPositions>();
    private Player currentPlayer;
    private bool isHalfQuali;
    private List<JobPositions> jobPositionsList = new List<JobPositions>();

    public static JobApplicationManager Instance {private set; get;}


    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }


    public void ShowAvailablePositons(Building currentBuilding)
    {
        jobSystemOverlay.SetActive(true);
        jobPosListOverlay.SetActive(true);

        if (availableJobPositionsHolder.childCount == 0)
        {
            jobPositionsList = JobPositionsList(currentBuilding.buildingEnumName);

            foreach(JobPositions jobPosition in jobPositionsList)
            {
                GameObject newJobPositionObj = Instantiate(jobPositionPrefab, Vector3.zero, Quaternion.identity, availableJobPositionsHolder);
                JobPositionObj newJobPosition = newJobPositionObj.GetComponent<JobPositionObj>();

                if (jobPosition == Player.Instance.CurrentPlayerJob)
                {
                    newJobPosition.PrepareJobDets(jobPosition, "Current Job");
                    newJobPositionObj.GetComponent<Button>().enabled = false;
                    
                    Color newCol1 = Color.grey;
                    Color newCol2;
                    if (ColorUtility.TryParseHtmlString("C5C5C5", out newCol2))
                    {
                        newCol1 = newCol2;
                    }
                    newJobPositionObj.GetComponent<Image>().color = newCol1;
                }
                else
                {
                    newJobPosition.PrepareJobDets(jobPosition);
                }
            }
        }
    }


    public void ShowSelectedJobPosition(JobPositions selectedJobPosition)
    {
        jobDetailedViewOverlay.GetComponent<JobDetailedViewObj>().PrepareDetailedJobDets(selectedJobPosition);
        jobDetailedViewOverlay.SetActive(true);
    }


    public void ApplyJob(JobPositions newJobData)
    {
        StartCoroutine(JobAppProcessingAnim(2f, newJobData));
    }


    private void JobApplicationProcessDone(JobPositions newJobData)
    {
        if (IsPlayerQualified(newJobData))
        {
            qualifiedMsgOverlay.SetActive(true);
            jobDetailedViewOverlay.SetActive(false);
            jobPosListOverlay.SetActive(false);

            congratulatoryMsg.text = ("Congratulations on applying for the " + newJobData.jobPosName + " position at " + 
                                        BuildingManager.Instance.CurrentSelectedBuilding.buildingStringName + "! Wishing you the best of luck in this exciting opportunity.");

            for (int i = 0; i < availableJobPositionsHolder.childCount; i++)
            {
                Object.Destroy(availableJobPositionsHolder.GetChild(i).gameObject);
            }

            JobManager.Instance.ArrangeJobAcceptance(newJobData);
            //game save
        }
        else
        {
            PromptManager.Instance.ShowPrompt(notQualifiedPrompt);
        }
    }


    private IEnumerator JobAppProcessingAnim(float waitingTime, JobPositions newJobData)
    {
        AnimOverlayManager.Instance.StartAnim(Animations.JOBAPPLICATIONPROCESSING);
        yield return new WaitForSeconds(waitingTime);
        AnimOverlayManager.Instance.StopAnim();
        JobApplicationProcessDone(newJobData);
        yield return null;
    }


    private bool IsPlayerQualified(JobPositions jobData)
    {
        currentPlayer = Player.Instance;
        isHalfQuali = false;

        if (jobData.reqCourse.Count > 0)
        {
            if (currentPlayer.PlayerEnrolledCourse == UniversityCourses.NONE && jobData.reqStudyField == StudyFields.NONE)
            {
                return false; //if player is not yet enrolled to any course
            }

            if (!jobData.reqCourse.Contains(UniversityCourses.ANY))
            {
                if (!jobData.reqCourse.Contains(currentPlayer.PlayerEnrolledCourse) && jobData.reqStudyField == StudyFields.NONE)
                {
                    return false; //if player's course doesn't meet the job's required course 
                }
            }
            isHalfQuali = true;
        }

        if (!isHalfQuali && jobData.reqStudyField != StudyFields.NONE)
        {
            if (jobData.reqStudyField != currentPlayer.PlayerEnrolledStudyField)
            {
                return false; //if player's study field doesn't meet the job's required study field 
            }
        }

        if (jobData.reqWorkHrs > 0f)
        {
            if (jobData.reqWorkField != JobFields.NONE)
            {
                if (currentPlayer.CurrentPlayerJob.workField == jobData.reqWorkField)
                {
                    if (jobData.reqWorkHrs > currentPlayer.CurrentWorkHours) 
                    {
                        return false; //if player doesn't have "enough" experience of the required work field
                    }
                }
                else
                {
                    if (currentPlayer.PlayerWorkFieldHistory.ContainsKey(jobData.reqWorkField))
                    {
                        if (jobData.reqWorkHrs > currentPlayer.PlayerWorkFieldHistory[jobData.reqWorkField])
                        {
                            return false; //if player doesn't have "enough" experience of the required work field
                        }
                    }
                    else
                    {
                        return false; //if player doesn't have "any" experience of the required work field
                    }
                }
            }
            else
            {
                if (jobData.reqWorkHrs > currentPlayer.GetTotalWorkHours())
                {
                    return false;
                }
            }
        }

        return true;
    }


    private List<JobPositions> JobPositionsList(Buildings currentBuildingName)
    {
        switch (currentBuildingName)
        {
            case Buildings.HOSPITAL:
                return hospitalJobPositions;
            case Buildings.CITYHALL:
                return cityHallJobPositions;
            case Buildings.UNIVERSITY:
                return universityJobPositions;
            case Buildings.FIRESTATION:
                return fireStationJobPositions;
            case Buildings.CAFETERIA:
                return cafeteriaJobPositions;
            case Buildings.POLICESTATION:
                return policeStationJobPositions;
            case Buildings.WASTEFACILITY:
                return wasteMngmntFacilityJobPositions;
            case Buildings.WATERDISTRICT:
                return waterDistrictJobPositions;
            case Buildings.BANK:
                return bankJobPositions;
            case Buildings.CAFE:
                return cafeJobPositions;
            case Buildings.FOODXPRESS:
                return foodXpressJobPositions;
            case Buildings.BEEUSOLUTIONS:
                return beeUsolutionsJobPositions;
            case Buildings.AUTODEALER:
                return autoDealerJobPositions;
            case Buildings.CONVENIENCESTORE:
                return convenienceStoreJobPositions;
            case Buildings.FACTORY:
                return factoryJobPositions;
            case Buildings.CALLCENTER:
                return callCenterJobPositions;
            case Buildings.ELECTRICCOOP:
                return electricCompanyJobPositions;
            default:
                return new List<JobPositions>();
        }
    }
}
