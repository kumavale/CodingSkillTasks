using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;

/*
   +--------+
   | 0| 1| 2|        1
   | 3| 4| 5|        0
   | 6| 7| 8|    8 3 # 1 2
   | 9|10|11|        2
   |12|13|14|        4
   +--------+
 */

public class CreateWall : MonoBehaviour
{
    [SerializeField] Material[] white;
	
    // Start is called before the first frame update
    void Start() {
        // 0_2
        CreateLine(new Vector2(-1.125f, 0.8125f), new Vector2(-0.8125f, 0.8125f), new Vector2(-0.8125f, -0.8125f), new Vector2(-1.125f, -0.8125f), "0_2");

        // 1_2
        CreateLine(new Vector2(-0.8125f, 0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(0.8125f, -0.8125f), new Vector2(-0.8125f, -0.8125f), "1_2");

        // 2_2
        CreateLine(new Vector2(1.125f, -0.8125f), new Vector2(0.8125f, -0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(1.125f, 0.8125f), "2_2");

        // 3_0
        CreateLine(new Vector2(-1.125f, 0.8125f), new Vector2(-0.8125f, 0.8125f), new Vector2(-0.8125f, -0.8125f), new Vector2(-1.125f, -0.8125f), "3_0");
        // 3_1
        CreateLine(new Vector2(-1.125f, 1.125f), new Vector2(-0.8125f, 0.8125f), new Vector2(-0.8125f, -0.8125f), new Vector2(-1.125f, -1.125f), "3_1");
        // 3_2
        CreateLine(new Vector2(-1.75f, 1.125f), new Vector2(-1.125f, 1.125f), new Vector2(-1.125f, -1.125f), new Vector2(-1.75f, -1.125f), "3_2");

        // 4_0
        CreateLine(new Vector2(-0.8125f, 0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(0.8125f, -0.8125f), new Vector2(-0.8125f, -0.8125f), "4_0");
        // 4_1
        CreateLine(new Vector2(1.125f, -1.125f), new Vector2(0.8125f, -0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(1.125f, 1.125f), "4_1");
        // 4_2
        CreateLine(new Vector2(-1.125f, 1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.125f, -1.125f), new Vector2(-1.125f, -1.125f), "4_2");
        // 4_3
        CreateLine(new Vector2(-1.125f, 1.125f), new Vector2(-0.8125f, 0.8125f), new Vector2(-0.8125f, -0.8125f), new Vector2(-1.125f, -1.125f), "4_3");

        // 5_0
        CreateLine(new Vector2(1.125f, -0.8125f), new Vector2(0.8125f, -0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(1.125f, 0.8125f), "5_0");
        // 5_2
        CreateLine(new Vector2(1.75f, -1.125f), new Vector2(1.125f, -1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.75f, 1.125f), "5_2");
        // 5_3
        CreateLine(new Vector2(1.125f, -1.125f), new Vector2(0.8125f, -0.8125f), new Vector2(0.8125f, 0.8125f), new Vector2(1.125f, 1.125f), "5_3");

        // 6_0
        CreateLine(new Vector2(-1.75f, 1.125f), new Vector2(-1.125f, 1.125f), new Vector2(-1.125f, -1.125f), new Vector2(-1.75f, -1.125f), "6_0");
        // 6_1
        CreateLine(new Vector2(-1.75f, 1.75f), new Vector2(-1.125f, 1.125f), new Vector2(-1.125f, -1.125f), new Vector2(-1.75f, -1.75f), "6_1");
        // 6_2
        CreateLine(new Vector2(-3f, 1.75f), new Vector2(-1.75f, 1.75f), new Vector2(-1.75f, -1.75f), new Vector2(-3f, -1.75f), "6_2");

        // 7_0
        CreateLine(new Vector2(-1.125f, 1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.125f, -1.125f), new Vector2(-1.125f, -1.125f), "7_0");
        // 7_1
        CreateLine(new Vector2(1.75f, -1.75f), new Vector2(1.125f, -1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.75f, 1.75f), "7_1");
        // 7_2
        CreateLine(new Vector2(-1.75f, 1.75f), new Vector2(1.75f, 1.75f), new Vector2(1.75f, -1.75f), new Vector2(-1.75f, -1.75f), "7_2");
        // 7_3
        CreateLine(new Vector2(-1.75f, 1.75f), new Vector2(-1.125f, 1.125f), new Vector2(-1.125f, -1.125f), new Vector2(-1.75f, -1.75f), "7_3");

        // 8_0
        CreateLine(new Vector2(1.75f, -1.125f), new Vector2(1.125f, -1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.75f, 1.125f), "8_0");
        // 8_2
        CreateLine(new Vector2(3f, -1.75f), new Vector2(1.75f, -1.75f), new Vector2(1.75f, 1.75f), new Vector2(3f, 1.75f), "8_2");
        // 8_3
        CreateLine(new Vector2(1.75f, -1.75f), new Vector2(1.125f, -1.125f), new Vector2(1.125f, 1.125f), new Vector2(1.75f, 1.75f), "8_3");

        // 9_0
        CreateLine(new Vector2(-3f, 1.75f), new Vector2(-1.75f, 1.75f), new Vector2(-1.75f, -1.75f), new Vector2(-3f, -1.75f), "9_0");
        // 9_1
        CreateLine(new Vector2(-3f, 3f), new Vector2(-1.75f, 1.75f), new Vector2(-1.75f, -1.75f), new Vector2(-3f, -3f), "9_1");
        // 9_2
        CreateLine(new Vector2(-5f, 3f), new Vector2(-3f, 3f), new Vector2(-3f, -3f), new Vector2(-5f, -3f), "9_2");

        // 10_0
        CreateLine(new Vector2(-1.75f, 1.75f), new Vector2(1.75f, 1.75f), new Vector2(1.75f, -1.75f), new Vector2(-1.75f, -1.75f), "10_0");
        // 10_1
        CreateLine(new Vector2(3f, -3f), new Vector2(1.75f, -1.75f), new Vector2(1.75f, 1.75f), new Vector2(3f, 3f), "10_1");
        // 10_2
        CreateLine(new Vector2(-3f, 3f), new Vector2(3f, 3f), new Vector2(3f, -3f), new Vector2(-3f, -3f), "10_2");
        // 10_3
        CreateLine(new Vector2(-3f, 3f), new Vector2(-1.75f, 1.75f), new Vector2(-1.75f, -1.75f), new Vector2(-3f, -3f), "10_3");

        // 11_0
        CreateLine(new Vector2(3f, -1.75f), new Vector2(1.75f, -1.75f), new Vector2(1.75f, 1.75f), new Vector2(3f, 1.75f), "11_0");
        // 11_2
        CreateLine(new Vector2(5f, -3f), new Vector2(3f, -3f), new Vector2(3f, 3f), new Vector2(5f, 3f), "11_2");
        // 11_3
        CreateLine(new Vector2(3f, -3f), new Vector2(1.75f, -1.75f), new Vector2(1.75f, 1.75f), new Vector2(3f, 3f), "11_3");

        // 12_0
        CreateLine(new Vector2(-5f, 3f), new Vector2(-3f, 3f), new Vector2(-3f, -3f), new Vector2(-5f, -3f), "12_0");
        // 12_1
        CreateLine(new Vector2(-5f, 5f), new Vector2(-3f, 3f), new Vector2(-3f, -3f), new Vector2(-5f, -5f), "12_1");

        // 13_0
        CreateLine(new Vector2(-3f, 3f), new Vector2(3f, 3f), new Vector2(3f, -3f), new Vector2(-3f, -3f), "13_0");
        // 13_1
        CreateLine(new Vector2(5f, -5f), new Vector2(3f, -3f), new Vector2(3f, 3f), new Vector2(5f, 5f), "13_1");
        // 13_3
        CreateLine(new Vector2(-5f, 5f), new Vector2(-3f, 3f), new Vector2(-3f, -3f), new Vector2(-5f, -5f), "13_3");

        // 14_0
        CreateLine(new Vector2(5f, -3f), new Vector2(3f, -3f), new Vector2(3f, 3f), new Vector2(5f, 3f), "14_0");
        // 14_3
        CreateLine(new Vector2(5f, -5f), new Vector2(3f, -3f), new Vector2(3f, 3f), new Vector2(5f, 5f), "14_3");
    }

    private void CreateLine(Vector2 begin, Vector2 middle1, Vector2 middle2, Vector2 end, string name) {
        GameObject obj = new GameObject();
        obj.name = name;
        obj.SetActive(false);

        obj.transform.parent = transform;

        Regex re = new Regex(@"_[0-9]");
        string w = re.Replace(name, "");


        //LineRenderer line = new LineRenderer();
        //line = obj.AddComponent<LineRenderer>();

        //float weight = 0.01f;
        //line.startWidth = weight;
        //line.endWidth   = weight;

        ////line.loop = true;  // 一部表示されないバグ

        //line.sharedMaterial = white[int.Parse(w)/3];

        //line.positionCount = 5;

        //line.SetPosition(0, (Vector3)begin);
        //line.SetPosition(1, (Vector3)middle1);
        //line.SetPosition(2, (Vector3)middle2);
        //line.SetPosition(3, (Vector3)end);
        //line.SetPosition(4, (Vector3)begin);


        //Vector3[] vertices = { begin, middle1, middle2, end };
        Vector3[] vertices = {
            new Vector3(begin.x,   begin.y,   0),
            new Vector3(middle1.x, middle1.y, 1),
            new Vector3(middle2.x, middle2.y, 2),
            new Vector3(end.x,     end.y,     3),
        };
        int[] triangles = { 0, 1, 2, 0, 2, 3 };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterial = white[int.Parse(w)/3];
    }
}
