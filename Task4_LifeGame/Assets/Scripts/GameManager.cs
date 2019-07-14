using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject cell;
    [SerializeField] Text switch_text;
    [SerializeField] Slider slider_speed;

    GameObject parent;
    float timeOut = 0.1f;
    float timeElapsed = 0f;

    Color none = new Color(235/255f, 237/255f, 240/255f);
    Color[] greens = {
        new Color(198/255f, 228/255f, 139/255f),
        new Color(123/255f, 201/255f, 111/255f),
        new Color( 35/255f, 154/255f,  59/255f),
        new Color( 25/255f,  97/255f,  39/255f)
    };

    enum State {
        DIES,
        LIVES,
        NEXT,
    }

    struct Cell {
        public GameObject obj;
        public int state;
    }

    Cell[,] cells = new Cell[36, 39];
    State[,] cells_buf = new State[36, 39];

	// Use this for initialization
	void Start () {
        Screen.SetResolution(512, 512, false, 60);

        parent = GameObject.Find("cells");

        for(int y = 16; y >= -19; --y) {
            for(int x = -19; x <= 19; ++x) {
                cells[y+19, x+19].obj = (GameObject)Instantiate(cell, new Vector3(x, y, 0), Quaternion.identity);
                cells[y+19, x+19].obj.transform.parent = parent.transform;
            }
        }

        init();
        Time.timeScale = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > timeOut) {
            check_cells();
            timeElapsed = 0f;
        }
	}

    public void init() {
        for(int y = 16; y >= -19; --y) {
            for(int x = -19; x <= 19; ++x) {
                // 25%
                if(Random.Range(0, 100) < 25) {
                    cells[y+19, x+19].state = (int)State.LIVES;
                    cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = greens[0];
                } else {
                    cells[y+19, x+19].state = (int)State.DIES;
                    cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = none;
                }
            }
        }
    }

    int check_neighbor(int _x, int _y) {
        int cnt = 0;
        for(int y=-1; y<=1; ++y) {
            for(int x=-1; x<=1; ++x) {
                if((_x+x < -19) || (19 < _x+x) || (_y+y < -19) || (16 < _y+y))
                    continue;
                if(cells[_y+y+19, _x+x+19].state > 0)
                    ++cnt;
            }
        }
        return cnt;
    }

    void check_cells() {
        for(int y = 16; y >= -19; --y) {
            for(int x = -19; x <= 19; ++x) {
                if(cells[y+19, x+19].state > 0) {
                    switch(check_neighbor(x, y)) {
                        case 2:
                        case 3:
                            cells_buf[y+19, x+19] = State.NEXT;
                            break;
                        default:
                            cells_buf[y+19, x+19] = State.DIES;
                            break;
                    }
                } else {
                    if(check_neighbor(x, y) == 3) {
                        cells_buf[y+19, x+19] = State.LIVES;
                    }
                }
            }
        }

        // Apply color and state
        for(int y = 16; y >= -19; --y) {
            for(int x = -19; x <= 19; ++x) {
                if(cells_buf[y+19, x+19] == State.DIES) {
                    cells[y+19, x+19].state -= 1;
                    if (cells[y+19, x+19].state < 0) cells[y+19, x+19].state = (int)State.DIES;
                } else if(cells_buf[y+19, x+19] == State.LIVES) {
                    cells[y+19, x+19].state = (int)State.LIVES;
                } else {
                    cells[y+19, x+19].state += 1;
                    switch(cells[y+19, x+19].state) {
                        case 2:
                            // 100%
                            break;
                        case 3:
                            // 50%
                            if(Random.Range(0, 2) == 0)
                                cells[y+19, x+19].state = (int)State.DIES;
                            break;
                        case 4:
                            // 25%
                            if(Random.Range(0, 4) < 3)
                                cells[y+19, x+19].state = (int)State.DIES;
                            break;
                        default:
                            // 0%
                            cells[y+19, x+19].state = (int)State.DIES;
                            break;
                    }
                }

                switch(cells[y+19, x+19].state) {
                    case 0: cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = none; break;
                    case 1: cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = greens[0]; break;
                    case 2: cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = greens[1]; break;
                    case 3: cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = greens[2]; break;
                    case 4: cells[y+19, x+19].obj.GetComponent<SpriteRenderer>().material.color = greens[3]; break;
                }
            }
        }
    }

    public void on_click_init() {
        Time.timeScale = 0f;
        init();
    }

    public void on_click_switch() {
        // Stop to Start
        if (Time.timeScale == 0f) {
            switch_text.text = "Stop";
            Time.timeScale = 1f;

        // Start to Stop
        } else {
            switch_text.text = "Start";
            Time.timeScale = 0f;
        }
    }

    public void on_slide_speed() {
        timeOut = slider_speed.value;
    }
}
