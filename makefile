SRC = $(wildcard src/**/*.c) $(wildcard src/*.c) $(wildcard src/**/**/*.c) $(wildcard src/**/**/**/*.c)\
	  $(wildcard src/**/*.h) $(wildcard src/*.h) $(wildcard src/**/**/*.h) $(wildcard src/**/**/**/*.h)
FOLDER = $(lastword $(notdir $(CURDIR)))

CFLAGS = -Iexternal
LDFLAGS = external/libtermcolor/libtermcolor.a

all:
	gcc -g ${SRC} -o bin\\${FOLDER}.exe -Wall ${CFLAGS} ${LDFLAGS}