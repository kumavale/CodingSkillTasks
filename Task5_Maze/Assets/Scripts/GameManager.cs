using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum DIR {
        NORTH = 0,
        EAST,
        SOUTH,
        WEST,
        DIR_MAX
    };

    const int LOCATION_MAX = 15;

    GameObject walls;

    [SerializeField] int FIELD_WIDTH  = 6;
    [SerializeField] int FIELD_HEIGHT = 6;

    int player_dir = (int)DIR.NORTH;

    Vector2Int[,] locations = {
        // DIRECTION NORTH
        {
            new Vector2Int(-1, -4), new Vector2Int( 1, -4), new Vector2Int( 0, -4),
            new Vector2Int(-1, -3), new Vector2Int( 1, -3), new Vector2Int( 0, -3),
            new Vector2Int(-1, -2), new Vector2Int( 1, -2), new Vector2Int( 0, -2),
            new Vector2Int(-1, -1), new Vector2Int( 1, -1), new Vector2Int( 0, -1),
            new Vector2Int(-1,  0), new Vector2Int( 1,  0), new Vector2Int( 0,  0)
        },
        // DIRECTION EAST
        {
            new Vector2Int( 4, -1), new Vector2Int( 4,  1), new Vector2Int( 4,  0),
            new Vector2Int( 3, -1), new Vector2Int( 3,  1), new Vector2Int( 3,  0),
            new Vector2Int( 2, -1), new Vector2Int( 2,  1), new Vector2Int( 2,  0),
            new Vector2Int( 1, -1), new Vector2Int( 1,  1), new Vector2Int( 1,  0),
            new Vector2Int( 0, -1), new Vector2Int( 0,  1), new Vector2Int( 0,  0)
        },
        // DIRECTION SOUTH
        {
            new Vector2Int( 1,  4), new Vector2Int(-1,  4), new Vector2Int( 0,  4),
            new Vector2Int( 1,  3), new Vector2Int(-1,  3), new Vector2Int( 0,  3),
            new Vector2Int( 1,  2), new Vector2Int(-1,  2), new Vector2Int( 0,  2),
            new Vector2Int( 1,  1), new Vector2Int(-1,  1), new Vector2Int( 0,  1),
            new Vector2Int( 1,  0), new Vector2Int(-1,  0), new Vector2Int( 0,  0)
        },
        // DIRECTION WEST
        {
            new Vector2Int(-4,  1), new Vector2Int(-4, -1), new Vector2Int(-4,  0),
            new Vector2Int(-3,  1), new Vector2Int(-3, -1), new Vector2Int(-3,  0),
            new Vector2Int(-2,  1), new Vector2Int(-2, -1), new Vector2Int(-2,  0),
            new Vector2Int(-1,  1), new Vector2Int(-1, -1), new Vector2Int(-1,  0),
            new Vector2Int( 0,  1), new Vector2Int( 0, -1), new Vector2Int( 0,  0)
        }
    };

    int[,] maze;
    int[,] field;

    GameObject[,] wall_objs = new GameObject[LOCATION_MAX, (int)DIR.DIR_MAX];

    Vector2Int player_pos;

    // Start is called before the first frame update
    void Start() {
        walls = GameObject.Find("Walls");
        
        define_walls();

        maze  = new int[FIELD_HEIGHT, FIELD_WIDTH];
        field = new int[FIELD_HEIGHT, FIELD_WIDTH];

        create_field();

        //for(int i=0; i<FIELD_HEIGHT; ++i) {
        //    string str = "";
        //    for(int j=0; j<FIELD_WIDTH; ++j) {
        //        str += "[" + field[i, j].ToString() + "]";
        //    }
        //    Debug.Log(str);
        //}

        player_pos.x = 1;  // ランダムにする
        player_pos.y = 4;
    }

    // Update is called once per frame
    void Update() {

        foreach(Transform wall in walls.transform) {
            wall.gameObject.SetActive(false);
        }

        //int[] location_order = {0,2,1,3,5,4,6,8,7,9,11,10,12,14,13};
        //foreach(int i in location_order) {
        for(int i=0; i<LOCATION_MAX; ++i) {
            int x = player_pos.x + locations[player_dir, i].x;
            int y = player_pos.y + locations[player_dir, i].y;
            if(y < 0 || FIELD_HEIGHT <= y || x < 0 || FIELD_WIDTH <= x)
                continue;

            for(int j=0; j<(int)DIR.DIR_MAX; ++j) {
                if(wall_objs[i, j] != null) {
                    int dir = (player_dir + j) % (int)DIR.DIR_MAX;
                    if((field[y, x]>>dir & 1) == 1)
                        wall_objs[i, j].SetActive(true);
                }
            }
        }

        // Advance
        if(Input.GetKeyDown(KeyCode.K)) {
            Vector2Int[] v = {new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1,0)};
            if(maze[player_pos.y+v[player_dir].y, player_pos.x+v[player_dir].x] == 0) {
                player_pos.x = player_pos.x + v[player_dir].x;
                player_pos.y = player_pos.y + v[player_dir].y;
            }
        }
        // Reverse
        else if(Input.GetKeyDown(KeyCode.J)) {
            Vector2Int[] v = {new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1,0)};
            if(maze[player_pos.y-v[player_dir].y, player_pos.x-v[player_dir].x] == 0) {
                player_pos.x = player_pos.x - v[player_dir].x;
                player_pos.y = player_pos.y - v[player_dir].y;
            }
        }
        // Turn Left
        else if(Input.GetKeyDown(KeyCode.H)) {
            player_dir = (player_dir+1) % (int)DIR.DIR_MAX;
        }
        // Turn Right
        else if(Input.GetKeyDown(KeyCode.L)) {
            player_dir = ((int)DIR.DIR_MAX+player_dir-1) % (int)DIR.DIR_MAX;
        }

    }

    private void define_walls() {
        wall_objs[ 0,2] = walls.transform.Find("0_2").gameObject;
        wall_objs[ 1,2] = walls.transform.Find("1_2").gameObject;
        wall_objs[ 2,2] = walls.transform.Find("2_2").gameObject;
        wall_objs[ 3,0] = walls.transform.Find("3_0").gameObject;
        wall_objs[ 3,1] = walls.transform.Find("3_1").gameObject;
        wall_objs[ 3,2] = walls.transform.Find("3_2").gameObject;
        wall_objs[ 4,0] = walls.transform.Find("4_0").gameObject;
        wall_objs[ 4,1] = walls.transform.Find("4_1").gameObject;
        wall_objs[ 4,2] = walls.transform.Find("4_2").gameObject;
        wall_objs[ 4,3] = walls.transform.Find("4_3").gameObject;
        wall_objs[ 5,0] = walls.transform.Find("5_0").gameObject;
        wall_objs[ 5,2] = walls.transform.Find("5_2").gameObject;
        wall_objs[ 5,3] = walls.transform.Find("5_3").gameObject;
        wall_objs[ 6,0] = walls.transform.Find("6_0").gameObject;
        wall_objs[ 6,1] = walls.transform.Find("6_1").gameObject;
        wall_objs[ 6,2] = walls.transform.Find("6_2").gameObject;
        wall_objs[ 7,0] = walls.transform.Find("7_0").gameObject;
        wall_objs[ 7,1] = walls.transform.Find("7_1").gameObject;
        wall_objs[ 7,2] = walls.transform.Find("7_2").gameObject;
        wall_objs[ 7,3] = walls.transform.Find("7_3").gameObject;
        wall_objs[ 8,0] = walls.transform.Find("8_0").gameObject;
        wall_objs[ 8,2] = walls.transform.Find("8_2").gameObject;
        wall_objs[ 8,3] = walls.transform.Find("8_3").gameObject;
        wall_objs[ 9,0] = walls.transform.Find("9_0").gameObject;
        wall_objs[ 9,1] = walls.transform.Find("9_1").gameObject;
        wall_objs[ 9,2] = walls.transform.Find("9_2").gameObject;
        wall_objs[10,0] = walls.transform.Find("10_0").gameObject;
        wall_objs[10,1] = walls.transform.Find("10_1").gameObject;
        wall_objs[10,2] = walls.transform.Find("10_2").gameObject;
        wall_objs[10,3] = walls.transform.Find("10_3").gameObject;
        wall_objs[11,0] = walls.transform.Find("11_0").gameObject;
        wall_objs[11,2] = walls.transform.Find("11_2").gameObject;
        wall_objs[11,3] = walls.transform.Find("11_3").gameObject;
        wall_objs[12,0] = walls.transform.Find("12_0").gameObject;
        wall_objs[12,1] = walls.transform.Find("12_1").gameObject;
        wall_objs[13,0] = walls.transform.Find("13_0").gameObject;
        wall_objs[13,1] = walls.transform.Find("13_1").gameObject;
        wall_objs[13,3] = walls.transform.Find("13_3").gameObject;
        wall_objs[14,0] = walls.transform.Find("14_0").gameObject;
        wall_objs[14,3] = walls.transform.Find("14_3").gameObject;
    }

    private void create_field() {
        generate_maze();

        Vector2Int[] dir = {new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0)};

        //for(int y=0; y<FIELD_HEIGHT; ++y) {
        //    for(int x=0; x<FIELD_WIDTH; ++x) {
        //        if(maze[y,x] == 0)
        //            continue;
        //        for(int d=0; d<dir.Length; ++d) {
        //            if(y+dir[d].y < 0 || FIELD_HEIGHT <= y+dir[d].y
        //             || x+dir[d].x < 0 || FIELD_WIDTH <= x+dir[d].x)
        //                continue;
        //            if(maze[y+dir[d].y, x+dir[d].x] == 0)
        //                field[y, x] |= 1<<d;
        //        }
        //    }
        //}
        for(int y=0; y<FIELD_HEIGHT; ++y) {
            for(int x=0; x<FIELD_WIDTH; ++x) {
                for(int d=0; d<dir.Length; ++d) {
                    if(y+dir[d].y < 0 || FIELD_HEIGHT <= y+dir[d].y
                     || x+dir[d].x < 0 || FIELD_WIDTH <= x+dir[d].x)
                        continue;
                    if((maze[y,x]==0 && maze[y+dir[d].y, x+dir[d].x]==1)
                     || (maze[y,x]==1 && maze[y+dir[d].y, x+dir[d].x]==0))
                        field[y, x] |= 1<<d;
                }
            }
        }
    }

    private void generate_maze() {
        // For test
        // ######
        // #    #
        // # ## #
        // # ## #
        // # ## #
        // ######
        maze[0, 0] = maze[0, 1] = maze[0, 2] = maze[0, 3] = maze[0, 4] = maze[0, 5] =
        maze[1, 0] =                                                     maze[1, 5] =
        maze[2, 0] =              maze[2, 2] = maze[2, 3] =              maze[2, 5] =
        maze[3, 0] =              maze[3, 2] = maze[3, 3] =              maze[3, 5] =
        maze[4, 0] =              maze[4, 2] = maze[4, 3] =              maze[4, 5] =
        maze[5, 0] = maze[5, 1] = maze[5, 2] = maze[5, 3] = maze[5, 4] = maze[5, 5] = 1;

        // 棒倒しか穴掘り
        //for(int y=0; y<FIELD_HEIGHT; ++y) {
        //    for(int x=0; x<FIELD_WIDTH; ++x) {
        //        field[y, x]
        //    }
        //}
    }
}
