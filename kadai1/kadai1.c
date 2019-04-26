#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>

// 座標の定義
enum {
    UPPER_LEFT_X,  // 左上位置x
    UPPER_LEFT_Y,  // 左上位置y
    RECT_WIDTH,    // 幅
    RECT_HEIGHT,   // 高さ
    DATA_SIZE,
};

// xのy乗を返す
int _pow(int x, int y) {
    if(y==0) return 1;
    int z = x;
    for(int _i=1; _i<y; _i++)
        z*=x;
    return z;
}

int draw(int rect[]) {
    static int height_max = 0;
    if(height_max < rect[UPPER_LEFT_Y]+rect[RECT_HEIGHT])
        height_max = rect[UPPER_LEFT_Y]+rect[RECT_HEIGHT];

    printf("\033[%d;%dH", rect[UPPER_LEFT_Y]+1, rect[UPPER_LEFT_X]*2+1);
    for(int y=0; y<rect[RECT_HEIGHT]; y++) {
        for(int x=0; x<rect[RECT_WIDTH]; x++) {
            if((y==0 && x==0) || (y==0 && x==rect[RECT_WIDTH]-1)
              || ((y==rect[RECT_HEIGHT]-1 && x==0)
                  || (y==rect[RECT_HEIGHT]-1 && x==rect[RECT_WIDTH]-1)))
                printf("＋");
            else if(y==0 || y==rect[RECT_HEIGHT]-1)
                printf("－");
            else if(x==0 || x==rect[RECT_WIDTH]-1)
                printf("｜");
            else
                printf("\033[2C");
        }
        printf("\033[%d;%dH", rect[UPPER_LEFT_Y]+y+2, rect[UPPER_LEFT_X]*2+1);
    }

    return height_max;
}

void tokenize(char *p, int ary[]) {
    int i = 0;
    while(*p) {
        // 空白文字をスキップ
        if(isspace(*p)) {
            p++;
            continue;
        }

        if(*p == '\n')
            break;

        if(isdigit(*p) && i<DATA_SIZE) {
            ary[i] = strtol(p, &p, 10);
            i++;
            continue;
        }

        fprintf(stderr, "Failured tokenize: %s\n", p);
        exit(1);
    }

    if(DATA_SIZE != i) {
        fprintf(stderr, "Insufficient number of elements\n");
        exit(1);
    }
}

int main() {
    int collisions = 0;

    int player[DATA_SIZE];
    {
        printf("自機のデータを入力: ");
        char buffer[32];
        scanf("%31[^\n]%*[^\n]", buffer);
        tokenize(buffer, player);
    }

    int enemy_count;
    printf("敵機の数を入力: ");
    scanf("%d%*[^\n]", &enemy_count);
    scanf("%*c");

    int enemys[3][DATA_SIZE];
    printf("敵機のデータを入力:\n");
    for(int i=0; i<enemy_count; i++) {
        char buffer[32];
        scanf("%31[^\n]%*[^\n]", buffer);
        tokenize(buffer, enemys[i]);
        scanf("%*c");
    }

    // 可視化  ※環境依存
    // 入力値の検査が面倒なのでterminalのサイズ内であることが前提
    ///*
    printf("\033[2J");
    // Enemy
    for(int i=0; i<enemy_count; i++)
        draw(enemys[i]);
    // Player
    printf("\033[31m"); // Red
    printf("\033[%d;1H\033[0m", draw(player)+2);
    //*/


    for(int y=player[UPPER_LEFT_Y]; y<player[UPPER_LEFT_Y]+player[RECT_HEIGHT]; y++) {
        for(int x=player[UPPER_LEFT_X]; x<player[UPPER_LEFT_X]+player[RECT_WIDTH]; x++) {
            for(int i=0; i<enemy_count; i++) {
                if(
                         (y >= enemys[i][UPPER_LEFT_Y])
                      && (y <= enemys[i][UPPER_LEFT_Y]+enemys[i][RECT_HEIGHT]-1)
                      && (x >= enemys[i][UPPER_LEFT_X])
                      && (x <= enemys[i][UPPER_LEFT_X]+enemys[i][RECT_WIDTH]-1)
                  )
                    // 当たったビットを立てる
                    collisions |= _pow(2, i);
            }
        }
    }

    for(int i=0; i<enemy_count; i++) {
        if((collisions>>i)&1)
            printf("敵機 %d が当たり\n", i+1);
    }

    return 0;
}
