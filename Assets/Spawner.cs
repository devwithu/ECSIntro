using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;

    [SerializeField] private Material unitMaterial;

    [SerializeField] private GameObject gameObjectPrefab;

    [SerializeField] private int xSize = 10;
    [SerializeField] private int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] private float spacing = 1f;
    
    private Entity entityPrefab;

    private World defaultWorld;

    private EntityManager entityManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //MakeEntity();
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);
        
        //InstantiateEntity(new float3(4f, 0f, 4f));
        InstantiateEntityGrid(xSize,ySize,spacing);
        
    }

    private void InstantiateEntity(float3 positioin)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = positioin
        });
    }

    private void InstantiateEntityGrid(int dimX, int dimY, float spacing = 1f)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f)  );
                
            }
        }
    }
    private void MakeEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Transform),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );
        Entity myEntity =  entityManager.CreateEntity(archetype);
        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = new float3(2f, 0f, 4f)
        });
        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });
    }
}
