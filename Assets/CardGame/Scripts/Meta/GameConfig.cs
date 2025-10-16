using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : SerializedScriptableObject
{
    public GameObject slotTemplate;
    public Dictionary<ViewType, GameObject> viewDic;
    public Dictionary<string, TaskConfig> TaskConfigs;
    public Dictionary<string,JobCardData> JobCardDatas;
    public GameObject taskTemplate;
    public GameObject kvItemUI;
    public GameObject buttonUI;
    public GameObject tableItemUI;
    public GameObject lineTemplate;
    public SaveData saveData;

    public Dictionary<CardEnum, CardData> CardMap = new Dictionary<CardEnum, CardData>();
    [Button]
    public void CardMapInit()
    {
        CardMap = new Dictionary<CardEnum, CardData>()
        {
            {CardEnum.FogforestTown,new MainStreetSceneCard()},
            {CardEnum.NResidentArea,new NResidentArea() },
            {CardEnum.SResidentArea,new SResidentArea() },
            {CardEnum.WResidentArea,new WResidentArea() },
            {CardEnum.EResidentArea,new EResidentArea() },
            {CardEnum.CentralHospital,new CentralHospital() },
            {CardEnum.MUniversity,new MUniversity() },
            {CardEnum.Museum,new Museum() },
            {CardEnum.Market,new Market() },
            {CardEnum.WoodLand,new WoodLand() },
            {CardEnum.Pool,new Pool() },
            {CardEnum.Muniversity_ArtBuilding,new Muniversity_ArtBuilding()},
            {CardEnum.Muniversity_LabBuilding,new Muniversity_LabBuilding()},
            {CardEnum.Muniversity_Library,new Muniversity_Library()},
            {CardEnum.Muniversity_Dorm,new Muniversity_Dorm()},
            {CardEnum.Muniversity_Gym,new Muniversity_Gym()},
            {CardEnum.MUniversity_TeachingBuilding,new MUniversity_TeachingBuilding()},
            {CardEnum.Muniversity_Hospital,new Muniversity_Hospital()},
            {CardEnum.money,new MoneyCardData()},
            {CardEnum.strength,new StrengthCardData()},
            {CardEnum.constitution,new ConstitutionCardData()},
            {CardEnum.size,new SizeCardData()},
            {CardEnum.dexterity,new DexterityCardData()},
            {CardEnum.appearance,new AppearanceCardData()},
            {CardEnum.intelligence,new IntelligenceCardData()},
            {CardEnum.power,new PowerCardData()},
            {CardEnum.education,new EducationCardData()},
            {CardEnum.luck,new LuckCardData()},
            {CardEnum.sanity,new SanityCardData()},
            {CardEnum.health,new HealthCardData()},
            {CardEnum.spotHidden,new SpotHiddenCardData()},
            {CardEnum.listen,new ListenCardData()},
            {CardEnum.psychology,new PsychologyCardData()},
            {CardEnum.occult,new OccultCardData()},
            {CardEnum.cthulhuMythos,new CthulhuMythosCardData()},
            {CardEnum.archaeology,new ArchaeologyCardData()},
            {CardEnum.history,new HistoryCardData()},
            {CardEnum.creditRating,new CreditRatingCardData()},
            {CardEnum.firstAid,new FirstAidCardData()},
            {CardEnum.medicine,new MedicineCardData()},
            {CardEnum.mechanicalRepair,new MechanicalRepairCardData()},
            {CardEnum.electricalRepair,new ElectricalRepairCardData()},
            {CardEnum.electronics,new ElectronicsCardData()},
            {CardEnum.dodge,new DodgeCardData()},
            {CardEnum.persuade,new PersuadeCardData()},
            {CardEnum.stealth,new StealthCardData()},
            {CardEnum.brawl,new BrawlCardData()},
            {CardEnum.firearms,new FirearmsCardData()},
            {CardEnum.fastTalk,new FastTalkCardData()},
            {CardEnum.locksmith,new LocksmithCardData()},
            {CardEnum.linguistics,new LinguisticsCardData()},
            {CardEnum.disguise,new DisguiseCardData()},
            {CardEnum.animalTraining,new AnimalTrainingCardData()},
            {CardEnum.performance,new PerformanceCardData()},
            {CardEnum.astronomy,new AstronomyCardData()},
            {CardEnum.charm,new CharmCardData()},
            {CardEnum.climb,new ClimbCardData()},
            {CardEnum.fineArt,new FineArtCardData()},
            {CardEnum.intimidate,new IntimidateCardData()},
            {CardEnum.libraryUse,new LibraryUseCardData()},
            {CardEnum.psychoanalysis,new PsychoanalysisCardData()},
            {CardEnum.track,new TrackCardData()},
            {CardEnum.throwing,new ThrowCardData()},
            {CardEnum.mathbook,new MathBookCardData()},
            {CardEnum.JobRecord,new JobRecordCardData()},
        };
    }

    public TaskConfig GetTask(string name)
    {
        TaskConfig res = null;
        TaskConfigs.TryGetValue(name,out res);
        return res;
    }
}
