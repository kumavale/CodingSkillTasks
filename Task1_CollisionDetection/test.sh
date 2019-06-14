#!/bin/bash

gcc kadai1.c

./a.out << EOF
4 4 4 4
3
1 1 8 8
1 10 7 7
6 5 6 9
EOF

./a.out << EOF
10 10 7 10
3
5 6 10 5
1 12 10 5
16 11 7 7
EOF
