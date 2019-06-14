#include <stdio.h>
#include <stdlib.h>
#include <stdarg.h>
#include <string.h>
#include <ctype.h>
#include <time.h>

#include "aa.c"

#define VAL_MAX 13   // 1-10JQK
#define TEHUDA_MAX 5 // 手札の枚数

// スートの定義
enum SUIT {
    SPADES,
    CLUBS,
    DIAMONDS,
    HEARTS,
    SUIT_MAX,
};

// 役の定義
enum HAND {
    ROYAL_STRAIGHT_FLUSH,
    STRAIGHT_FLUSH,
    FOUR_OF_A_KIND,
    FULL_HOUSE,
    FLUSH,
    STRAIGHT,
    THREE_OF_A_KIND,
    TWO_PAIR,
    ONE_PAIR,
    HIGH_CARD,
    HAND_MAX,
};

// 手札のカードの種類を定義
typedef struct {
    int suit;
    int val;
} Tehuda;

void error(const char *fmt, ... ) {
    va_list ap;
    va_start(ap, fmt);
    vfprintf(stderr, fmt, ap);
    fprintf(stderr, "\n");
    exit(1);
}

void tokenize(char *p, Tehuda t[], int i) {
    int j = 0;
    while(*p) {
        if(isspace(*p)) {
            p++;
            continue;
        }

        if(isdigit(*p)) {
            if(0 == j) {
                int s = strtol(p, &p, 10);
                if(s < 0 || SUIT_MAX < s)
                    error("Invalid suit: %d", s);
                t[i].suit = s;
                j++;
                continue;
            }

            if(1 == j) {
                int v = strtol(p, &p, 10);
                if(v < 1 || VAL_MAX < v)
                    error("Invalid value: %d", v);
                t[i].val = v;
                return;
            }
        }

        error("Failured tokenize: %s", p);
    }
    error("Insufficient input value", p);
}

int duplication(Tehuda t[]) {
    for(int i=0; i<TEHUDA_MAX-1; i++) {
        for(int j=i+1; j<TEHUDA_MAX; j++) {
            if(i==j) continue;
            if((t[i].val == t[j].val) && (t[i].suit == t[j].suit))
                return 1;
        }
    }
    return 0;
}

// 昇順 バブルソート
Tehuda *sort(Tehuda t[]) {
    while(1) {
      int swap = 0;
      for(int i=0; i<TEHUDA_MAX-1; i++) {
          if(t[i].val > t[i+1].val) {
              swap = 1;
              Tehuda tmp = t[i];
              t[i]       = t[i+1];
              t[i+1]     = tmp;
          }
      }
      if(!swap)
          break;
    }
    return t;
}

int analysis(Tehuda t[]) {

    // 同じスートか否かフラグ
    int same_suit = 1;
    // 同じ数字を数えるための定義 最大2ペア
    struct {int val; int maisu;} sames[2] = {{0,1}, {0,1}};

    // 同じスートか
    for(int i=0; i<TEHUDA_MAX-1; i++)
        if(t[i].suit != t[i+1].suit)
            same_suit = 0;

    // ロイヤルストレートフラッシュ, ストレートフラッシュ, フラッシュ
    if(same_suit) {
        int royal = 0;
        for(int i=0; i<TEHUDA_MAX; i++)
            switch(t[i].val) {
                case 1:  royal |= 1; break;
                case 10: royal |= 2; break;
                case 11: royal |= 4; break;
                case 12: royal |= 8; break;
                case 13: royal |= 16; break;
                default: /* Do nothing */ break;
            }
        if(royal == 31)
            return ROYAL_STRAIGHT_FLUSH;
        else {
            int str = 0;
            for(int i=0; i<TEHUDA_MAX-1; i++)
                if(((t[i].val + 1) % VAL_MAX) == t[i+1].val)
                    str++;
            if(str == 4)
                return STRAIGHT_FLUSH;
            else
                return FLUSH;
        }
    }

    // 同じ数字を数える
    for(int i=0; i<TEHUDA_MAX-1; i++) {
        if((t[i].val) == t[i+1].val) {
            if(sames[0].val == 0 || sames[0].val == t[i].val) {
                sames[0].val = t[i].val;
                sames[0].maisu++;
            } else {
                sames[1].val = t[i].val;
                sames[1].maisu++;
            }
        }
    }

    // フォーカード
    if(sames[0].maisu == 4)
        return FOUR_OF_A_KIND;

    // フルハウス, スリーカード
    if(sames[0].maisu == 3 || sames[1].maisu == 3) {
        if(sames[0].maisu == 2 || sames[1].maisu == 2)
            return FULL_HOUSE;
        else
            return THREE_OF_A_KIND;
    }

    // ストレート
    int str = 0;
    for(int i=0; i<TEHUDA_MAX-1; i++)
        if(((t[i].val + 1) % VAL_MAX) == t[i+1].val)
            str++;
    if(str == 4)
        return STRAIGHT;

    // ツーペア, ワンペア
    if(sames[0].maisu == 2) {
        if(sames[1].maisu == 2)
            return TWO_PAIR;
        else
            return ONE_PAIR;
    }

    // 上記以外
    return HIGH_CARD;
}

void draw(Tehuda t[], int hand) {
    // 手札の表示
    for(int i=0; i<TEHUDA_MAX; i++) {
        int s = t[i].suit;
        printf(s<1?" S":s<2?" C":s<3?" D":" H");
        switch(t[i].val) {
            case 1:  printf("A"); break;
            case 11: printf("J"); break;
            case 12: printf("Q"); break;
            case 13: printf("K"); break;
            default: printf("%d", t[i].val); break;
        }
    }
    printf("\n");

    // AA  ※ MSゴシックでの表示のみ確認済
    const char **val;
    for(int i=0; i<AA_HEIGHT; i++) {
        for(int j=0; j<TEHUDA_MAX; j++) {
            printf(" \033[48;5;15m");
            char suit[3+1];
            switch(t[j].suit) {
                case SPADES:   strcpy(suit,"♠"); printf("\033[38;5;16m"); break;
                case CLUBS:    strcpy(suit,"♣"); printf("\033[38;5;16m"); break;
                case DIAMONDS: strcpy(suit,"♦");  printf("\033[38;5;1m"); break;
                case HEARTS:   strcpy(suit,"♥"); printf("\033[38;5;1m"); break;
            }
            switch(t[j].val) {
                case 1:  val = ace;   break;
                case 2:  val = two;   break;
                case 3:  val = three; break;
                case 4:  val = four;  break;
                case 5:  val = five;  break;
                case 6:  val = six;   break;
                case 7:  val = seven; break;
                case 8:  val = eight; break;
                case 9:  val = nine;  break;
                case 10: val = ten;   break;
                case 11: val = jack;  break;
                case 12: val = queen; break;
                case 13: val = king;  break;
            }
            for(int k=0; k<AA_WIDTH; k++) {
                if(val[i][k] == '#')
                    printf("%s", suit);
                else
                    printf("%c", val[i][k]);
            }
            printf("\033[0m");
        }
        printf("\n");
    }

    // 役の表示
    switch(hand) {
        case ROYAL_STRAIGHT_FLUSH: printf(" ROYAL STRAIGHT FLUSH\n"); break;
        case STRAIGHT_FLUSH: printf(" STRAIGHT FLUSH\n"); break;
        case FOUR_OF_A_KIND: printf(" FOUR OF A KIND\n"); break;
        case FULL_HOUSE: printf(" FULL HOUSE\n"); break;
        case FLUSH: printf(" FLUSH\n"); break;
        case STRAIGHT: printf(" STRAIGHT\n"); break;
        case THREE_OF_A_KIND: printf(" THREE OF A KIND\n"); break;
        case TWO_PAIR: printf(" TWO PAIR\n"); break;
        case ONE_PAIR: printf(" ONE PAIR\n"); break;
        case HIGH_CARD: printf(" HIGH CARD\n"); break;
        default: error(" Invalid hand: %d", hand);
    }
}

int main() {

    srand((unsigned int)time(NULL));

    Tehuda tehudas[TEHUDA_MAX];

    // 課題2-1
    /*
    printf("Input hands: \n");
    for(int i=0; i<TEHUDA_MAX; i++) {
        char buffer[256];
        scanf("%255[^\n]%*[^\n]", buffer);
        tokenize(buffer, tehudas, i);
        scanf("%*c");
    }
    // 重複検査
    if(duplication(tehudas))
        error("Duplication");
    //*/


    // 課題2-2
    // for i in {0..10}; do ./kadai2; echo ""; sleep 1; done
    while(1) {
        for(int i=0; i<TEHUDA_MAX; i++) {
            tehudas[i].val  = rand() % VAL_MAX + 1;
            tehudas[i].suit = rand() % SUIT_MAX;
        }
        if(!duplication(tehudas))
            break;
    }

    int res = analysis(sort(tehudas));
    draw(tehudas, res);


    return 0;
}
