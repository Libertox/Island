using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Island.Item;

namespace Island.InteractObject
{

    public class Plant : MonoBehaviour, Interactable, IWriteable
    {

        [SerializeField] private SpriteRenderer plant;
        [SerializeField] private float lifeTimeOfHeap;

        public SeedSO Seed { get; private set; }
        public float TimeGrowing { get; private set; }
        public GrowState GrowState { get; private set; } = GrowState.Stalk;

        private Tree newPlant;

        private void Update()
        {
            if (Seed)
            {
                TimeGrowing += Time.deltaTime;
                if (TimeGrowing > Seed.growingCooldown)
                {
                    TimeGrowing -= Seed.growingCooldown;
                    switch (GrowState)
                    {
                        case GrowState.Stalk:
                            plant.sprite = Seed.stalk;
                            GrowState = GrowState.SmallPlant;
                            break;
                        case GrowState.SmallPlant:
                            newPlant = Instantiate(Seed.smallPlant, plant.transform.position, Quaternion.identity);
                            newPlant.SetTreeSeed(this);
                            plant.gameObject.SetActive(false);
                            GetComponent<BoxCollider2D>().enabled = false;
                            GrowState = GrowState.FinalPlant;
                            break;
                        case GrowState.FinalPlant:
                            Destroy(newPlant.gameObject);
                            Instantiate(Seed.finalPlant, plant.transform.position, Quaternion.identity);
                            Destroy(gameObject);
                            break;
                    }
                }
            }
            else
            {
                lifeTimeOfHeap -= Time.deltaTime;
                if (lifeTimeOfHeap < 0)
                    Destroy(gameObject);
            }
        }
        public void Interact(PlayerController player) { }

        public void SetSeed(SeedSO seedSO)
        {
            if (!Seed)
            {
                Seed = seedSO;
                plant.gameObject.SetActive(true);
            }
        }

        public void Initialize(PlantSerializable data)
        {
            transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
            TimeGrowing = data.timeOfGrowing;
            Seed = (SeedSO)(SaveManager.Instance.FindItemSO(data.seed.itemSOName));
            GrowState = data.growState;
            if (Seed != null)
            {
                ChangeGrowingStateAfterLoad();
                TimeGrowing += TimeManager.Instance.GetExtraTime();
            }

        }

        private void ChangeGrowingStateAfterLoad()
        {
            switch (GrowState)
            {
                case GrowState.Stalk:
                    plant.gameObject.SetActive(true);
                    break;
                case GrowState.SmallPlant:
                    plant.gameObject.SetActive(true);
                    plant.sprite = Seed.stalk;
                    break;
                case GrowState.FinalPlant:
                    newPlant = Instantiate(Seed.smallPlant, plant.transform.position, Quaternion.identity);
                    newPlant.SetTreeSeed(this);
                    GetComponent<BoxCollider2D>().enabled = false;
                    plant.gameObject.SetActive(false);
                    break;
            }
        }

        public ObjectSerializable CreateItemData() => new PlantSerializable(this);

    }

    public enum GrowState
    {
        Stalk,
        SmallPlant,
        FinalPlant,
    }

    [System.Serializable]
    public class PlantSerializable : ObjectSerializable
    {
        public float timeOfGrowing;
        public ItemInstance seed;
        public GrowState growState;

        public PlantSerializable(Plant plant) : base(plant.transform.position)
        {
            timeOfGrowing = plant.TimeGrowing;
            seed = new ItemInstance(plant.Seed, 0);
            growState = plant.GrowState;
        }

        public override Object CreateObject(SaveManager saveSystem)
        {
            Plant plant = Object.Instantiate(saveSystem.PrefabList.plantPrefab);
            plant.Initialize(this);
            return plant;
        }

    }

}

