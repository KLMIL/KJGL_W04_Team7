using UnityEngine;

public class CubeArrayController : MonoBehaviour
{
    [System.Serializable]
    public class CubeData
    {
        public GameObject cube;
        public int index;
        public bool isMaterial1 = false;
        public int specialMaterialState = 0; // 0: material2, 1: material3, 2: material4, 3: material5
    }

    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;

    public CubeData[] cubes = new CubeData[51];

    void Start()
    {
        if (material1 == null || material2 == null || material3 == null || material4 == null || material5 == null)
        {
            Debug.LogError("Material 중 하나가 설정되지 않았습니다.");
            return;
        }

        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i].cube == null)
            {
                Debug.LogError($"인덱스 {i}의 큐브가 설정되지 않았습니다.");
                continue;
            }

            Renderer renderer = cubes[i].cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                UpdateCubeMaterial(cubes[i]);
            }
            else
            {
                Debug.LogWarning($"인덱스 {i}의 큐브에 Renderer가 없습니다.");
            }
        }
    }

    public void ToggleCubes(int[] indicesToTurnOn) // 이름 간소화
    {
        foreach (int index in indicesToTurnOn)
        {
            if (index >= 0 && index < cubes.Length)
            {
                CubeData cubeData = cubes[index];
                if (index >= 27) // 27~51
                {
                    cubeData.specialMaterialState = (cubeData.specialMaterialState + 1) % 5;
                }
                else if (index >= 24) // 24~26
                {
                    cubeData.specialMaterialState = (cubeData.specialMaterialState + 1) % 4;
                }
                else // 0~23
                {
                    cubeData.isMaterial1 = !cubeData.isMaterial1;
                }
                UpdateCubeMaterial(cubeData);
                Debug.Log($"인덱스 {index} 상태 변경: specialMaterialState = {cubeData.specialMaterialState}, isMaterial1 = {cubeData.isMaterial1}");
            }
            else
            {
                Debug.LogWarning($"유효하지 않은 인덱스: {index}");
            }
        }
    }

    private void UpdateCubeMaterial(CubeData cubeData)
    {
        Renderer renderer = cubeData.cube.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError($"인덱스 {cubeData.index}의 큐브에 Renderer가 없습니다!");
            return;
        }

        if (cubeData.index >= 24 && cubeData.index <= 26) // 특수 큐브
        {
            Material newMaterial = null;
            switch (cubeData.specialMaterialState)
            {
                case 0: newMaterial = material2; break;
                case 1: newMaterial = material3; break;
                case 2: newMaterial = material4; break;
                case 3: newMaterial = material5; break;
            }

            if (newMaterial == null)
            {
                Debug.LogError($"인덱스 {cubeData.index}에 적용할 머터리얼이 null입니다! (state: {cubeData.specialMaterialState})");
            }
            else
            {
                renderer.material = newMaterial;
                Debug.Log($"인덱스 {cubeData.index}에 머터리얼 적용: {newMaterial.name}");
            }
        }
        else if (cubeData.index >= 27) // 특수 큐브
        {
            Material newMaterial = null;
            switch (cubeData.specialMaterialState)
            {
                case 0: newMaterial = material2; break;
                case 1: newMaterial = material3; break;
                case 2: newMaterial = material4; break;
                case 3: newMaterial = material5; break;
                case 4: newMaterial = material6; break;
            }

            if (newMaterial == null)
            {
                Debug.LogError($"인덱스 {cubeData.index}에 적용할 머터리얼이 null입니다! (state: {cubeData.specialMaterialState})");
            }
            else
            {
                renderer.material = newMaterial;
                Debug.Log($"인덱스 {cubeData.index}에 머터리얼 적용: {newMaterial.name}");
            }
        }
        else // 기본 큐브
        {
            renderer.material = cubeData.isMaterial1 ? material1 : material2;
            Debug.Log($"인덱스 {cubeData.index}에 기본 머터리얼 적용: {(cubeData.isMaterial1 ? material1.name : material2.name)}");   
        }
    }

    public void ResetAllToFalse()
    {
        foreach (CubeData cubeData in cubes)
        {
            cubeData.isMaterial1 = false;
            cubeData.specialMaterialState = 0;
            UpdateCubeMaterial(cubeData);
        }
    }
}