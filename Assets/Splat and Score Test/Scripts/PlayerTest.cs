using System.Collections;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    Vector4 channelMask = new Vector4(1, 0, 0, 0);

    int splatsX = 1;
    int splatsY = 1;

    public float splatScale = 1.0f;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private MeshRenderer playerMeshRend;
    [SerializeField]
    private Material[] colourMaterials;


    private void Start()
    {
        playerMeshRend.material = colourMaterials[0];
    }

    private void Update()
    {

        Splat();
    }

    private void Splat()
    {
        //Get how many splats are in the splat atlas
        splatsX = SplatManagerSystem.instance.splatsX;
        splatsY = SplatManagerSystem.instance.splatsY;

        if (Input.GetKeyDown(KeyCode.Alpha1)) //orange/yellow
        {
            channelMask = new Vector4(1, 0, 0, 0);
            playerMeshRend.material = colourMaterials[0];
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //red
        {
            channelMask = new Vector4(0, 1, 0, 0);
            playerMeshRend.material = colourMaterials[1];
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //green
        {
            channelMask = new Vector4(0, 0, 1, 0);
            playerMeshRend.material = colourMaterials[2];
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //blue
        {
            channelMask = new Vector4(0, 0, 0, 1);
            playerMeshRend.material = colourMaterials[3];
        }

        //Splat when player presses Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray splatRay = new Ray(playerTransform.position, -transform.up);
            RaycastHit hitInfo;
            if (Physics.Raycast(splatRay, out hitInfo, 10000))
            {

                Vector3 leftVec = Vector3.Cross(hitInfo.normal, Vector3.up);
                float randScale = Random.Range(0.5f, 1.5f);

                GameObject newSplatObject = new GameObject();
                newSplatObject.transform.position = hitInfo.point;
                if (leftVec.magnitude > 0.001f)
                {
                    newSplatObject.transform.rotation = Quaternion.LookRotation(leftVec, hitInfo.normal);
                }
                newSplatObject.transform.RotateAround(hitInfo.point, hitInfo.normal, Random.Range(-180, 180));
                newSplatObject.transform.localScale = new Vector3(randScale, randScale * 0.5f, randScale) * splatScale;

                Splat newSplat;
                newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
                newSplat.channelMask = channelMask;

                float splatscaleX = 1.0f / splatsX;
                float splatscaleY = 1.0f / splatsY;
                float splatsBiasX = Mathf.Floor(Random.Range(0, splatsX * 0.99f)) / splatsX;
                float splatsBiasY = Mathf.Floor(Random.Range(0, splatsY * 0.99f)) / splatsY;

                newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);

                SplatManagerSystem.instance.AddSplat(newSplat);

                GameObject.Destroy(newSplatObject);
            }
        }
    }
}
