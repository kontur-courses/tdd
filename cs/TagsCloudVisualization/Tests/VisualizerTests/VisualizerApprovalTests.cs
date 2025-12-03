using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

[TestFixture]
public class VisualizerApprovalTests : VisualizerTestsBase
{
    private int imageWidth { get;  set; }
    private int imageHeight { get;  set; }
    private Color TextColor { get;  set; }
    private Color BackgroundColor { get; set; }
    private Font Font { get;  set; }
    
    [SetUp]
    public void SetUp()
    {
        imageWidth = 1100;
        imageHeight = 900;
        TextColor = Color.FromArgb(255, 102, 0);
        BackgroundColor = Color.FromArgb(0, 28, 39);
        Font = new Font("Times New Roman", 16);
    }
    
    [Test]
    public void GenerateWordCloudImage()
    {
        var words = new[]
        {
            "Forest", "River", "Mountain", "Ocean", "Sea",
            "Lake", "Valley", "Desert", "Island", "Cliff",
            "Meadow", "Field", "Prairie", "Steppe", "Canyon",
            "Waterfall", "Glacier", "Volcano", "Geyser", "Lagoon",
            "Tree", "Bush", "Grass", "Flower", "Moss",
            "Leaf", "Roots", "Branch", "Seed", "Bloom",
            "Sunrise", "Sunset", "Rain", "Snow", "Storm",
            "Thunder", "Lightning", "Clouds", "Wind", "Fog",
            "Wildlife", "Birds", "Animals", "Insects", "Fish",
            "Ecosystem", "Nature", "Landscape", "Horizon", "Sky"
        };
        SetVisualizationData(imageWidth,  imageHeight, words, "wordCloudNature_failed.png", Font, TextColor, BackgroundColor);

        var visualizer = new Visualizer(new CircularCloudLayouter(new Point(imageWidth / 2, imageHeight / 2)));

        visualizer.DrawCloud(
            words, 
            "wordCloud.png",
            imageWidth,
            imageHeight,
            TextColor,
            BackgroundColor,
            Font);
    }
        
    [Test]
    public void GenerateSportsWordCloudImage_Failed()
    {
        var words = new[]
        {
            "Football", "Soccer", "Basketball", "Baseball", "Hockey",
            "Tennis", "Volleyball", "Rugby", "Cricket", "Golf",
            "Boxing", "MMA", "Wrestling", "Cycling", "Running",
            "Marathon", "Sprint", "Swimming", "Diving", "Rowing",
            "Skating", "Skiing", "Snowboarding", "Surfing", "Skateboard",
            "Climbing", "Bouldering", "Gymnastics", "Karate", "Judo",
            "Taekwondo", "Badminton", "TableTennis", "Handball", "WaterPolo",
            "Lacrosse", "Softball", "AmericanFootball", "Fencing", "Archery",
            "Triathlon", "Biathlon", "Decathlon", "Heptathlon", "Crossfit",
            "Powerlifting", "Weightlifting", "Bodybuilding", "Yoga", "Pilates",
            "Aerobics", "Zumba", "Parkour", "Freerun", "Motorsport",
            "FormulaOne", "Rally", "Karting", "Esports", "Chess",
            "Darts", "Bowling", "Snooker", "Billiards", "Polo",
            "Kayaking", "Canoeing", "Windsurfing", "Kitesurfing", "Paragliding",
            "Mountaineering", "TrailRunning", "Ultramarathon", "NordicWalking", "Orienteering",
            "Stadium", "Arena", "Coach", "Referee", "Captain",
            "Team", "League", "Tournament", "Championship", "Playoffs",
            "Fitness", "Training", "Warmup", "Cooldown", "Stretching",
            "Goal", "Assist", "Penalty", "Offense", "Defense",
            "SprintFinish", "Record", "Medal", "Victory", "FairPlay"
        };
        SetVisualizationData(imageWidth,  imageHeight, words, "wordCloudSports_failed.png", Font, TextColor, BackgroundColor);

        var visualizer = new Visualizer(new CircularCloudLayouter(new Point(imageWidth / 2, imageHeight / 2)));

        visualizer.DrawCloud(
            words, 
            "wordCloudSports.png",
            imageWidth,
            imageHeight,
            TextColor,
            BackgroundColor,
            Font);
        1.Should().Be(2);
    }

    [Test]
    public void GenerateMedicalWordCloudImage()
    {
        var words = new[]
        { 
            "Medicine", "Health", "Doctor", "Nurse", "Surgeon", 
            "Therapist", "Cardiologist", "Neurologist", "Pediatrician", "Dentist", 
            "Oncologist", "Dermatologist", "Psychiatrist", "Radiologist", "Anesthesiologist",
            "Pharmacist", "Paramedic", "Clinician", "Internist", "Endocrinologist",
            "Hospital", "Clinic", "Ward", "ICU", "Emergency",
            "Ambulance", "OperatingRoom", "Laboratory", "Pharmacy", "Reception",
            "WaitingRoom", "RecoveryRoom", "TraumaCenter", "Outpatient", "Inpatient",
            "Diagnosis", "Treatment", "Therapy", "Surgery", "Operation",
            "Rehabilitation", "Consultation", "Checkup", "Vaccination", "Screening",
            "Prevention", "Monitoring", "FollowUp", "Referral", "Telemedicine",
            "Heart", "Lungs", "Brain", "Liver", "Kidneys",
            "Stomach", "Intestines", "Pancreas", "Skin", "Bones",
            "Muscles", "Joints", "Spine", "Blood", "ImmuneSystem",
            "Infection", "Virus", "Bacteria", "Inflammation", "Tumor",
            "Cancer", "Diabetes", "Hypertension", "Stroke", "Allergy",
            "Asthma", "Depression", "Anxiety", "Insomnia", "Obesity",
            "Symptom", "Fever", "Pain", "Cough", "Fatigue",
            "Nausea", "Headache", "Dizziness", "Bleeding", "Swelling",
            "Rash", "ShortnessOfBreath", "Palpitations", "Vomiting", "Diarrhea",
            "Vaccine", "Antibiotic", "Analgesic", "Antiseptic", "Anesthetic",
            "Hormone", "Insulin", "Sedative", "Antiviral", "Steroid",
            "Vitamin", "Supplement", "Infusion", "Injection", "Tablet",
            "Stethoscope", "Syringe", "Scalpel", "Thermometer", "Glucometer",
            "ECG", "XRay", "MRI", "CTScan", "Ultrasound",
            "Defibrillator", "Ventilator", "Wheelchair", "Bandage", "Mask",
            "Gloves", "Disinfectant", "Microscope", "TestTube", "Monitor",
            "Wellness", "Nutrition", "Hydration", "Exercise", "Hygiene",
            "Immunity", "Recovery", "FirstAid", "EmergencyCare", "PalliativeCare",
            "PublicHealth", "Epidemiology", "ClinicalTrial", "Research", "EvidenceBased"
                
        };
        SetVisualizationData(imageWidth,  imageHeight, words, "wordCloudMedicine_failed.png", Font, TextColor, BackgroundColor);

        var visualizer = new Visualizer(new CircularCloudLayouter(new Point(imageWidth / 2, imageHeight / 2)));

        visualizer.DrawCloud(
            words, 
            "wordCloudMedicine.png",
            imageWidth,
            imageHeight,
            TextColor,
            BackgroundColor,
            Font);
    }
}
