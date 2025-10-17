using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : SerializedScriptableObject
{
    public GameObject slotTemplate;
    public Dictionary<ViewType, GameObject> viewDic;
    /// <summary>
    /// 任务的配置
    /// </summary>
    public Dictionary<string, TaskConfig> TaskConfigs;
    /// <summary>
    /// 职位类型的配置
    /// </summary>
    public Dictionary<string,JobCardData> JobCardDatas;
    /// <summary>
    /// Npc的配置
    /// </summary>
    public Dictionary<string, NpcCardData> NpcCardDatas;
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
            {CardEnum.JobCard,new JobCardData()},
        };
    }

    public TaskConfig GetTask(string name)
    {
        TaskConfig res = null;
        TaskConfigs.TryGetValue(name,out res);
        return res;
    }

    [Button]
    public void CreateNpcData(string name)
    {
        var npc = new NpcCardData();
        npc.NpcName = name;
        npc.age = 20;
        NpcCardDatas[name] = npc;
        npc.npcInfo[CardEnum.strength.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.constitution.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.size.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.dexterity.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.appearance.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.intelligence.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.power.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.education.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.luck.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.sanity.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.health.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.spotHidden.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.listen.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.psychology.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.occult.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.cthulhuMythos.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.archaeology.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.history.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.creditRating.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.firstAid.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.medicine.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.mechanicalRepair.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.electricalRepair.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.electronics.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.drive.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.dodge.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.persuade.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.stealth.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.brawl.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.firearms.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.fastTalk.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.locksmith.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.linguistics.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.disguise.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.animalTraining.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.performance.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.astronomy.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.charm.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.climb.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.fineArt.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.intimidate.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.libraryUse.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.psychoanalysis.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.track.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[CardEnum.throwing.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Courage.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Composure.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Impulsiveness.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Aggressiveness.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Greed.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Curiosity.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Piety.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Skepticism.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Morality.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Authority.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Loyalty.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.Sociability.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.PhobiaTrigger.ToString()] = UnityEngine.Random.Range(0, 1000);
        npc.npcInfo[NpcMind.ForbiddenKnowledgeAcceptance.ToString()] = UnityEngine.Random.Range(0, 1000);
    }
}
